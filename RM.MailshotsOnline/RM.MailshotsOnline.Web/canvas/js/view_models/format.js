// viewmodel to handle format data
define(['knockout', 'jquery', 'view_models/state', 'temp/data'],
    function(ko, $, stateViewModel, tempData) {

        function formatViewModel() {
            // this.objects contains the data returned from the server
            this.objects = ko.observableArray([]);
            this.selected = ko.observable();

            this.selectedID = stateViewModel.formatID;
            this.selected.subscribe(function() {
                stateViewModel.viewingFace(this.getDefaultFace());
            }, this);

            // fetch data
            this.fetch();
        }

        formatViewModel.prototype.fetch = function fetch() {
            // fetch data from server using fetchURL
            /* TESTING ONLY */
            /*this.selected(tempData.formatData[0]);
            return*/
            var fetchURL = "/Umbraco/Api/MailshotSettings/GetFormat/" + this.selectedID; 
            //console.log('fetching data from ' + fetchURL);
            $.getJSON(fetchURL, function(data) {
                this.selected(data);
            }.bind(this));
        };

        formatViewModel.prototype.getDefaultFace = function getDefaultFace() {
            var faces = this.getFaces(),
                default_face = ko.utils.arrayFirst(faces, function(face) {
                    return face.default_face == true;
                });
            return default_face || faces[0];
        };

        formatViewModel.prototype.getFaces = function getFaces() {
        	var selectedFormat = this.selected();
        	if (selectedFormat) {
        		return selectedFormat.faces;
        	}
        	return [];
        };

        formatViewModel.prototype.getFacesBySide = function getFacesBySide(side) {
    		return ko.utils.arrayFilter(this.allFaces(), function(face) {
                return face.side == side;
            });
        };

        var format = new formatViewModel();

        formatViewModel.prototype.allFaces = ko.pureComputed(function() {
        	return this.getFaces();
        }, format);

        // for testing
        window.format = format;

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return format;
    }
);