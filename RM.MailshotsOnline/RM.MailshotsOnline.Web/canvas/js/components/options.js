define(['knockout', 'components/dropdown', 'view_models/user', 'view_models/format', 'view_models/template', 'view_models/theme', 'view_models/state'], 

    function(ko, dropdownComponent, userViewModel, formatViewModel, templateViewModel, themeViewModel, stateViewModel) {
        // register required components

        // ViewModel
        function optionsViewModel(params) {}

        /**
         * unfocus the current element and toggle the template picker
         */
        optionsViewModel.prototype.toggleTemplatePicker = function toggleTemplatePicker() {
            this.unfocus();
            stateViewModel.toggleTemplatePicker();
        }

        /**
         * unfocus the current element and toggle the theme picker
         */
        optionsViewModel.prototype.toggleThemePicker = function toggleThemePicker() {
            this.unfocus();
            stateViewModel.toggleThemePicker();
        }

        /**
         * unfocus the current element and show the user output json (TESTING)
         */
        optionsViewModel.prototype.output = function output() {
            this.unfocus();
            stateViewModel.output(userViewModel.toJSON());
        }

        /**
         * unfocus the current element
         */
        optionsViewModel.prototype.unfocus = function unfocus() {
            stateViewModel.selectElement(null);
            stateViewModel.backgroundSelected(null);
        }

        /**
         * clear the user output json (TESTING)
         */
        optionsViewModel.prototype.clearOutput = function clearOutput() {
            stateViewModel.output(null);
        }

        return {
            viewModel: optionsViewModel,
            template: { require: 'text!/canvas/templates/options.html' }
        }
});