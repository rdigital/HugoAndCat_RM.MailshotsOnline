define(['knockout', 'kospectrum'],

    function(ko) {
        // ViewModel
        function colourPickerViewModel(params) {
            this.options = params.options
            this.action = params.action
            this.selectedObs = params.selected
            this.focusCallback = params.focusCallback
            this.anchorClass = params.anchorClass
            this.open = ko.observable(false);
            this.element = ko.observable();

            this.hideOnDocClick = this.hideOnDocClick.bind(this);

            // computeds
            this.selected = this.getSelectedComputed();

            // subscriptions
            this.handleSubscriptions();
        }

        /**
         * set up subscription which adds hide function to document click when colour picker is open
         */
        colourPickerViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.open.subscribe(function(open) {
                if (open) {
                    $(document).on('mousedown', this.hideOnDocClick);
                } else {
                    $(document).off('mousedown', this.hideOnDocClick);
                }
            }, this)
        }

        /**
         * returns read / write computed to get / set currently selected colour
         * @return {ko.pureComputed} 
         */
        colourPickerViewModel.prototype.getSelectedComputed = function getSelectedComputed() {
            return ko.pureComputed({
                read: function() {
                    return this.selectedObs()
                },
                write: function(colour) {
                    this.selectedObs(colour);
                    //this.hide();
                }
            }, this)
        }

        /**
         * hides the colourpicker on a click anywhere outside of the colourpicker
         */
        colourPickerViewModel.prototype.hideOnDocClick = function hideOnDocClick(e) {
            if ($.contains(this.element()[0], e.target) ) {
                return
            }
            this.hide();
        }

        /**
         * toggles the colourpicker and set the focus back to the selected element
         */
        colourPickerViewModel.prototype.toggle = function toggle() {
            this.open(!this.open());
            if (this.focusCallback) {
                this.focusCallback();
            }
        }

        /**
         * hides the colourpicker and set the focus back to the selected element
         */
        colourPickerViewModel.prototype.hide = function hide() {
            this.open(false);
            if (this.focusCallback) {
                this.focusCallback();
            }
        }

        return {
            viewModel: colourPickerViewModel,
            template: { require: 'text!/canvas/templates/colourpicker.html' }
        }
});