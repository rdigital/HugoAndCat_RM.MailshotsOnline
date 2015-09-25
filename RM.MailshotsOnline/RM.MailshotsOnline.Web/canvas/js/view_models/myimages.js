// viewmodel to handle my images data
define(['knockout', 'view_models/data', 'view_models/format', 'view_models/user'],
    function(ko, dataViewModel, userViewModel) {

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

            // fetch data
            this.fetch();
        }

        // extend the element model
        myImagesViewModel.prototype = Object.create(dataViewModel.prototype);
        myImagesViewModel.prototype.constructor = myImagesViewModel;

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

        myImagesViewModel.prototype.select = function select(image) {
            this.selected(image);
        };

        myImagesViewModel.prototype.remove = function remove(image) {
            this.objects.remove(image);
            $.post('/Umbraco/Api/ImageLibrary/ProcessDeleteImage/' + image.ImageId, {}, function(data) {
                console.log('Image deleted');
            }.bind(this)).fail(function() {
                console.log('There was an error deleting this image');
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