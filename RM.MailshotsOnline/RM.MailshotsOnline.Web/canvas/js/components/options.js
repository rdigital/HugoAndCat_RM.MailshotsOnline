define(['knockout', 'jquery', 'components/dropdown', 'view_models/user', 'view_models/format', 'view_models/template', 'view_models/theme', 'view_models/state', 'view_models/history', 'view_models/auth'],

    function(ko, $, dropdownComponent, userViewModel, formatViewModel, templateViewModel, themeViewModel, stateViewModel, historyViewModel, authViewModel) {

        // ViewModel
        function optionsViewModel(params) {
            this.redoAvailable = historyViewModel.redoAvailable;
            this.undoAvailable = historyViewModel.undoAvailable;
            this.showPreview = stateViewModel.showPreview;
            this.showRestart = ko.observable(false);
            this.showFormat = ko.observable(false);
            this.menuVisible = ko.observable(false);
            this.uploadingImages = stateViewModel.uploadingImages;
            this.saving = stateViewModel.saving;
            this.showThemePicker = stateViewModel.showThemePicker;
            this.showTemplatePicker = stateViewModel.showTemplatePicker;
            this.toggleAuth = stateViewModel.toggleAuth;
            this.isAuthenticated = authViewModel.isAuthenticated;

            // computeds
            this.hidden = this.getHiddenComputed();

            // bound methods
            this.togglePreview = stateViewModel.togglePreview.bind(this);
            this.hideMenu = this.hideMenu.bind(this);

            this.handleSubscriptions();
        }

        optionsViewModel.prototype.showMenu = function showMenu() {
            this.menuVisible(true);
        }

        optionsViewModel.prototype.hideMenu = function hideMenu() {
            setTimeout( function() {
                this.menuVisible(false);
            }.bind(this), 0);
        }

        optionsViewModel.prototype.toggleRestartModal = function toggleRestartModal() {
            this.showRestart(!this.showRestart());
        }

        optionsViewModel.prototype.restart = function restart() {
            this.showRestart(false);
        }

        optionsViewModel.prototype.toggleFormatModal = function toggleFormatModal() {
            this.showFormat(!this.showFormat());
        }

        optionsViewModel.prototype.changeFormat = function changeFormat() {
            this.showFormat(false);
        }

        optionsViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.menuVisible.subscribe(function(visible) {
                if (visible) {
                    $(document).on('mouseup', this.hideMenu);
                } else {
                    $(document).off('mouseup', this.hideMenu);
                }
            }, this);
        };

        /**
         * unfocus the current element and toggle the template picker
         */
        optionsViewModel.prototype.toggleTemplatePicker = function toggleTemplatePicker() {
            this.unfocus();
            stateViewModel.toggleTemplatePicker();
        };

        /**
         * unfocus the current element and toggle the theme picker
         */
        optionsViewModel.prototype.toggleThemePicker = function toggleThemePicker() {
            this.unfocus();
            stateViewModel.toggleThemePicker();
        };

        /**
         * unfocus the current element and show the user output json (TESTING)
         */
        optionsViewModel.prototype.output = function output() {
            this.unfocus();
            stateViewModel.output(userViewModel.toJSON());
        };

        /**
         * unfocus the current element
         */
        optionsViewModel.prototype.unfocus = function unfocus() {
            stateViewModel.selectElement(null);
            stateViewModel.backgroundSelected(null);
            if (this.showThemePicker()) {
                stateViewModel.toggleThemePicker();
            }
            if (this.showTemplatePicker()) {
                stateViewModel.toggleTemplatePicker();
            }
            if (this.showPreview()) {
                this.togglePreview();
            }
        };

        /**
         * clear the user output json (TESTING)
         */
        optionsViewModel.prototype.clearOutput = function clearOutput() {
            stateViewModel.output(null);
        };

        optionsViewModel.prototype.redo = function redo() {
            historyViewModel.redo();
        };

        optionsViewModel.prototype.undo = function undo() {
            historyViewModel.undo();
        };

        optionsViewModel.prototype.reset = function reset() {
            historyViewModel.reset();
        };

        optionsViewModel.prototype.save = function save() {
            if (this.isAuthenticated()) {
                if (this.saving() || this.uploadingImages().length) {
                    return;
                }
                userViewModel.save();
            } else {
                this.toggleAuth();
            }
        };

        optionsViewModel.prototype.getHiddenComputed = function regetHiddenComputedset() {
            return ko.pureComputed(function() {
                if (!stateViewModel.ready() || stateViewModel.selectedElement() || stateViewModel.backgroundSelected()) {
                    return true;
                }
                return false;
            }, this);
        };

        return {
            viewModel: optionsViewModel,
            template: { require: 'text!/canvas/templates/options.html' }
        };
});