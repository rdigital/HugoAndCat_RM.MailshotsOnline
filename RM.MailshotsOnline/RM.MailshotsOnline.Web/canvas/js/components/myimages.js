// viewmodel to handle my images data
define(['knockout', 'view_models/data', 'view_models/format', 'view_models/user', 'temp/data'],
    function(ko, dataViewModel, userViewModel, tempData) {

        function myImagesViewModel() {
            // this.objects contains the data returned from the server
            this.objects = ko.observableArray([]);
            // use selection by ID (instead of direct ref to object)
            // in case we want to use routing to describe selections
            this.selected = ko.pureComputed(function(){
                return this.getObjByID(this.objects, this.selectedID);
            }, this);
            this.myImagesLoading = ko.observable(true);
            this.myImages = ko.observableArray();

            this.getVars();

            // fetch data
            this.fetch();
        }

        // extend the element model
        myImagesViewModel.prototype = Object.create(dataViewModel.prototype);
        myImagesViewModel.prototype.constructor = myImagesViewModel;

        myImagesViewModel.prototype.getVars = function getVars() {
            this.fetchURL = '/Umbraco/Api/ImageLibrary/GetImages';
            this.testData = tempData.templateData;
        }

        myImagesViewModel.prototype.fetch = function fetch() {
            $.get('/Umbraco/Api/ImageLibrary/GetMyImages', function(data) {
                this.myImagesLoading(false);
                this.myImages(data);
            }.bind(this)).fail(function() {
                console.log('There was an error fetching my images');
            })
        }

        myImagesViewModel.prototype.selectMyImage = function selectMyImage(image) {
            this.myImage(image);
        }

        myImagesViewModel.prototype.deleteMyImage = function deleteMyImage(image) {
            $.get('/Umbraco/Api/ImageLibrary/GetMyImages', function(data) {
                this.myImagesLoading(false);
                this.myImages(data);
            }.bind(this)).fail(function() {
                console.log('There was an error deleting this image');
            })
        }

        var myImages = new myImagesViewModel();


        // for testing
        window.myImages = myImages;

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return myImages;
    }
)