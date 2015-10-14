// viewmodel to handle my images data
define(['knockout', 'view-models/notification', 'components/uploadImage'],
    function(ko, notificationViewModel, uploadImageComponent) {
        ko.components.register('upload-image-component', uploadImageComponent);

        function myImagesViewModel() {
            // this.objects contains the data returned from the server
            this.objects = ko.observableArray([]);
            this.displayedObjects = ko.observableArray([]);
            // use selection by ID (instead of direct ref to object)
            // in case we want to use routing to describe selections
            this.selected = ko.observableArray();
            this.loading = ko.observable(true);
            this.deleteImages = ko.observable(false);
            this.uploadImage = ko.observable(false);
            this.previewingImage = ko.observable();

            this.add = this.add.bind(this);
            this.remove = this.remove.bind(this);
            this.previewImage = this.previewImage.bind(this);
            this.clearPreviewingImage = this.clearPreviewingImage.bind(this);
            this.selectImage = this.selectImage.bind(this);
            this.deleteImage = this.deleteImage.bind(this);
            this.clearDeleteImage = this.clearDeleteImage.bind(this);
            this.toggleUploadImage = this.toggleUploadImage.bind(this);

            // fetch data if / when authenticated
            this.fetch();
        }

        myImagesViewModel.prototype.previewImage = function previewImage(image) {
            this.previewingImage(image);
        };

        myImagesViewModel.prototype.clearPreviewingImage = function clearPreviewingImage() {
            this.previewingImage(null);
        };

        myImagesViewModel.prototype.fetch = function fetch() {
            $.get('/Umbraco/Api/ImageLibrary/GetMyImages', function(data) {
                this.loading(false);
                this.objects(data);
            }.bind(this)).fail(function() {
                console.log('There was an error fetching my images');
            });
        };

        myImagesViewModel.prototype.add = function add(image) {
            this.objects.push(image);
        };

        myImagesViewModel.prototype.selectImage = function selectImage(image) {
            if (this.selected().indexOf(image) > -1) {
                this.selected.remove(image);
                return;
            }
            this.selected.push(image);
        };

        myImagesViewModel.prototype.deleteImage = function deleteImage(image) {
            if (image.MailshotUses) {
                notificationViewModel.show('This image is currently in use on a campaign. It cannot be deleted.', 'error');
                return;
            }
            this.deleteImages([image]);
        };

        myImagesViewModel.prototype.deleteSelected = function deleteSelected() {
            this.deleteImages(this.selected());
        };

        myImagesViewModel.prototype.clearDeleteImage = function clearDeleteImage() {
            this.deleteImages([]);
        };

        myImagesViewModel.prototype.remove = function remove() {
            var images = this.deleteImages();
            this.objects.removeAll(images);
            /*$.post('/Umbraco/Api/ImageLibrary/ProcessDeleteImage/' + image.ImageId, {}, function() {
                notificationViewModel.hideWithMessage('Image deleted', 'message');
            }.bind(this)).fail(function() {
                notificationViewModel.show('This image could not be deleted', 'error');
            });*/
            return false;
        };

        myImagesViewModel.prototype.toggleUploadImage = function toggleUploadImage() {
            this.uploadImage(!this.uploadImage());
        };

        return {
            viewModel: myImagesViewModel,
            template: { require: 'text!/scripts/src/templates/my-images.html' }
        };
    }
);