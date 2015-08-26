// viewmodel to handle format data
define(['knockout', 'view_models/data', 'view_models/user', 'temp/data'],
    function(ko, dataViewModel, userViewModel, tempData) {

        // extends dataViewModel
        var formatViewModel = dataViewModel;

        formatViewModel.prototype.getVars = function getVars() {
            this.fetchURL = '/formats';
            this.testData = tempData.formatData;
            this.selectedID = userViewModel.objects.formatID;
        }

        formatViewModel.prototype.getFaces = function getFaces() {
        	var selectedFormat = this.selected();
        	if (selectedFormat) {
        		return selectedFormat.faces
        	}
        	return []
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
)