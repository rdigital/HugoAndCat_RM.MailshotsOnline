// viewmodel to handle my images data
define(['knockout', 'view_models/data', 'view_models/notification', 'view_models/auth'],
    function(ko, dataViewModel, notificationViewModel, authViewModel) {

        function myImagesViewModel() {
            // this.objects contains the data returned from the server
            this.objects = ko.observableArray([]);
            // use selection by ID (instead of direct ref to object)
            // in case we want to use routing to describe selections
            this.selected = ko.observable();
            this.loading = ko.observable(true);

            this.select = this.select.bind(this);
            this.add = this.add.bind(this);
            this.remove = this.remove.bind(this);

            // fetch data if / when authenticated
            this.fetch();
            authViewModel.isAuthenticated.subscribe(this.fetch, this);
        }

        // extend the element model
        myImagesViewModel.prototype = Object.create(dataViewModel.prototype);
        myImagesViewModel.prototype.constructor = myImagesViewModel;

        myImagesViewModel.prototype.fetch = function fetch() {
            if (!authViewModel.isAuthenticated()) {
                return
            }
            $.get('/Umbraco/Api/ImageLibrary/GetMyImages', function(data) {
                this.loading(false);
                ko.utils.arrayForEach(data, function(obj) {
                    obj.deleting = ko.observable(false);
                });
                this.objects(data);
            }.bind(this)).fail(function() {
                console.log('There was an error fetching my images');
            });
        };

        myImagesViewModel.prototype.add = function add(image) {
            this.objects.unshift(image);
        };

        myImagesViewModel.prototype.select = function select(image) {
            this.selected(image);
        };

        myImagesViewModel.prototype.remove = function remove(image) {
            notificationViewModel.show('Deleting image', 'message');
            if (image.MailshotUses) {
                notificationViewModel.show('This image is currently in use on a campaign. It cannot be deleted.', 'error');
                return;
            }
            image.deleting(true);
            $.post('/Umbraco/Api/ImageLibrary/ProcessDeleteImage/' + image.ImageId, {}, function(data) {
                notificationViewModel.hideWithMessage('Image deleted', 'message');
                this.objects.remove(image);
            }.bind(this)).fail(function() {
                notificationViewModel.hideWithMessage('Error deleting image', 'error');
                image.deleting(false);
            });
            return false;
        };

        var myImages = new myImagesViewModel();


        // for testing
        window.myImages = myImages;

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return myImages;
    }
);