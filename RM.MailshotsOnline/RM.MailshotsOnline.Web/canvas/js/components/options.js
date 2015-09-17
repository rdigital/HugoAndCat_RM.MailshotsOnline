define(['knockout', 'components/dropdown', 'view_models/user', 'view_models/format', 'view_models/template', 'view_models/theme', 'view_models/state', 'view_models/history'],

    function(ko, dropdownComponent, userViewModel, formatViewModel, templateViewModel, themeViewModel, stateViewModel, historyViewModel) {

        // ViewModel
        function optionsViewModel(params) {
            this.redoAvailable = historyViewModel.redoAvailable;
            this.undoAvailable = historyViewModel.undoAvailable;
            this.showPreview = stateViewModel.showPreview;

            // computeds
            this.hidden = this.getHiddenComputed();

            // bound methods
            this.togglePreview = stateViewModel.togglePreview.bind(this);
        }

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

        optionsViewModel.prototype.redo = function redo() {
            historyViewModel.redo();
        }

        optionsViewModel.prototype.undo = function undo() {
            historyViewModel.undo();
        }

        optionsViewModel.prototype.reset = function reset() {
            historyViewModel.reset();
        }

        optionsViewModel.prototype.save = function save() {
            userViewModel.save();
        }

        optionsViewModel.prototype.getHiddenComputed = function regetHiddenComputedset() {
            return ko.pureComputed(function() {
                if (stateViewModel.selectedElement() || stateViewModel.backgroundSelected()) {
                    return true
                }
                return false
            }, this)
        }

        return {
            viewModel: optionsViewModel,
            template: { require: 'text!/canvas/templates/options.html' }
        }
});