// viewmodel to handle template data
define(['knockout', 'view_models/data', 'view_models/format', 'view_models/user', 'temp/data'],
    function(ko, dataViewModel, formatViewModel, userViewModel, tempData) {

        // extends dataViewModel
        var templateViewModel = dataViewModel;

        templateViewModel.prototype.getElementsByFace = function getElementsByFace(face_name) {
            var template = this.selected();
                elements = ko.utils.arrayFilter(template.elements, function(element) {
                    return element.face == face_name;
                });
            return (elements.length) ? elements : []
        }

        templateViewModel.prototype.getVars = function getVars() {
            this.fetchURL = '/templates';
            this.testData = tempData.templateData;
            this.selectedID = userViewModel.objects.templateID;
        }

        var template = new templateViewModel();

        templateViewModel.prototype.availableTemplates = ko.pureComputed(function() {
            // filter available templates by their format ID
            var selectedFormatID = formatViewModel.selectedID(),
                selectedID = this.selectedID(),
                objects = this.objects(),
                out = ko.utils.arrayFilter(objects, function(obj) {
                    return obj.format_id == selectedFormatID;
                });
            if (selectedFormatID && objects.length) {
                var match = ko.utils.arrayFirst(out, function(object) {
                    return object.id == selectedID;
                });
                if (!match) {
                    this.selectedID(out[0].id)
                }
            }
            return out
        }, template)

        // for testing
        window.template = template;

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return template;
    }
)