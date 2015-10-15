// viewmodel to handle template data
define(['knockout', 'view_models/data', 'view_models/user', 'view_models/state', 'temp/data'],
    function(ko, dataViewModel, userViewModel, stateViewModel, tempData) {

        function templateViewModel() {
            this.objects = ko.observableArray([]);
            this.selected = this.getSelectedComputed();
            this.formatID = stateViewModel.formatID;
            this.fetchURL = '/Umbraco/Api/MailshotSettings/GetTemplatesForFormat/' + this.formatID;
            this.selectedID = ko.pureComputed(function () {
                if (userViewModel.ready()) {
                    return userViewModel.objects.templateID();
                }
            }, this);

            // fetch data
            this.fetch();
        }

        // extends dataViewModel
        templateViewModel.prototype = Object.create(dataViewModel.prototype);
        templateViewModel.prototype.constructor = templateViewModel;

        /* TEMP XX DELETE */
        templateViewModel.prototype.fetch = function fetch() {
            //console.log('fetching data from ' + this.fetchURL);
            this.objects(tempData.templateData);
        };

        /**
         * get the elements for a particular face on the selected template
         * @param  {String} face_name [the name of the face to get the elements for]
         * @return {[Object]}           [array of objects which represent each element]
         */
        templateViewModel.prototype.getElementsByFace = function getElementsByFace(face_name) {
            var template = this.selected(),
                elements = ko.utils.arrayFilter(template.elements, function(element) {
                    return element.face == face_name;
                });
            return (elements.length) ? elements : [];
        };

        window.template = new templateViewModel();

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return window.template
    }
);