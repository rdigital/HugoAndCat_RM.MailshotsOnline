define(['knockout', 'komapping', 'view_models/format', 'view_models/template', 'view_models/user', 'view_models/state', 'view_models/history'],

    function(ko, mapping, formatViewModel, templateViewModel, userViewModel, stateViewModel, historyViewModel) {

        // ViewModel
        function templateComponentViewModel(params) {
            this.faces = formatViewModel.allFaces;
            this.templates = templateViewModel.availableTemplates;
            this.currentTemplate = userViewModel.objects.templateID;
        }

        /**
         * resets any user defined styles, before changing the selected template
         * if the template has changed, set reposition images to tru on the state view model
         * which tells the image components to recalculate the initial image positions on canvases
         * @param  {templateViewModel} template template view model instance
         */
        templateComponentViewModel.prototype.selectTemplate = function selectTemplate(template) {
            userViewModel.resetUserFontSizes();
            if (userViewModel.objects.templateID() != template.id) {
                stateViewModel.repositionImages = true;
                userViewModel.objects.templateID(template.id);
            }
            stateViewModel.toggleTemplatePicker();
            historyViewModel.pushToHistory();
        }

        /**
         * toggle the template picker modal
         */
        templateComponentViewModel.prototype.toggleTemplatePicker = function toggleTemplatePicker() {
            stateViewModel.toggleTemplatePicker();
        }

        return {
            viewModel: templateComponentViewModel,
            template: { require: 'text!/canvas/templates/template.html' }
        }
});