define(['knockout', 'jquery'],

    function(ko, $) {
        // register required components

        // ViewModel
        function dropdownViewModel(params) {
            this.options = params.options;
            this.action = params.action;
            this.selectedObs = params.selected;
            this.placeholder = params.placeholder || '';
            this.autohide = params.autohide;
            this.full = params.full;
            this.focusCallback = params.focusCallback;
            this.anchorClass = params.anchorClass;
            this.open = ko.observable(false);
            this.element = ko.observable();

            this.hideOnDocClick = this.hideOnDocClick.bind(this);

            // computeds
            this.selected = this.getSelectedComputed();

            // subscriptions
            this.handleSubscriptions();
        }

        /**
         * subscription to add hide function to document click when dropdown is open
         */
        dropdownViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.open.subscribe(function(open) {
                if (open) {
                    $(document).on('mousedown', this.hideOnDocClick);
                } else {
                    $(document).off('mousedown', this.hideOnDocClick);
                }
            }, this);
        };

        /**
         * returns read / write computed to get / set currently selected value
         * @return {ko.pureComputed} 
         */
        dropdownViewModel.prototype.getSelectedComputed = function getSelectedComputed() {
            return ko.pureComputed({
                read: function() {
                    var val = this.selectedObs(),
                        obj = ko.utils.arrayFirst(this.options(), function(option) {
                            return option.value == val;
                        });
                    return obj ? obj.name : '';
                },
                write: function(data) {
                    this.selectedObs(data.value);
                    if (this.autohide) {
                        this.hide();
                    }
                }
            }, this);
        };

        /**
         * hides the dropdown on a click anywhere outside of the dropdown
         */
        dropdownViewModel.prototype.hideOnDocClick = function hideOnDocClick(e) {
            if ($.contains(this.element()[0], e.target) ) {
                return;
            }
            this.hide();
        };

        /**
         * toggles the dropdown and set the focus back to the selected element
         */
        dropdownViewModel.prototype.toggle = function toggle() {
            this.open(!this.open());
            if (this.focusCallback) {
                this.focusCallback();
            }
        };

        /**
         * hides the dropdown and set the focus back to the selected element
         */
        dropdownViewModel.prototype.hide = function hide() {
            this.open(false);
            if (this.focusCallback) {
                this.focusCallback();
            }
        };

        return {
            viewModel: dropdownViewModel,
            template: { require: 'text!/canvas/templates/dropdown.html' }
        };
});