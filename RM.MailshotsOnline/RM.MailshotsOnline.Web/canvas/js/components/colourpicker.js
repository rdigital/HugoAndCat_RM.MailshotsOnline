define(['knockout', 'kospectrum'],

    function(ko) {
        // ViewModel
        function colourPickerViewModel(params) {
            this.options = params.options;
            this.action = params.action;
            this.selectedObs = params.selected;
            this.focusCallback = params.focusCallback;
            this.anchorClass = params.anchorClass;
            this.open = ko.observable(false);
            this.customShown = ko.observable(false);
            this.element = ko.observable();

            this.greys = ['#000000', '#434343', '#666666', '#CCCCCC', '#D9D9D9', '#FFFFFF'];
            this.colours = ['#940A00', '#F71700', '#FA9F1E', '#FBFF3C', '#39FF3D', '#47FFFF',
                            '#577CEC', '#3500F9', '#9A00FA', '#FA00FA', '#E3B9AD', '#F2CCCB',
                            '#F9E6CB', '#FEF4CA', '#DAECD2', '#D1E0E3', '#CBD7F9', '#D2E1F5',
                            '#D8D0EA', '#E8CFDC', '#D88168', '#E79B98', '#F5CE9A', '#FDE893',
                            '#B7D9A6', '#A6C4C9', '#A7BFF6', '#A4C2EA', '#B6A4DA', '#D2A5BE',
                            '#C7451B', '#DA6965', '#F1B563', '#FBDE5B', '#95C879', '#7AA4B0',
                            '#7596EE', '#78A3DF', '#8F76C6', '#BF79A0', '#A02100', '#C71000',
                            '#E1952A', '#ECC72B', '#6CAB48', '#4B808F', '#4A6EDC', '#4A7FC9',
                            '#6843A9', '#A34A7A', '#591101', '#640400', '#754207', '#7D6210',
                            '#2A500D', '#11333D', '#263E8A', '#153366', '#23054F', '#4A0F31'
                        ];

            this.hideOnDocClick = this.hideOnDocClick.bind(this);
            this.showCustom = this.showCustom.bind(this);
            this.hideCustom = this.hideCustom.bind(this);

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
            }, this);
        };

        /**
         * returns read / write computed to get / set currently selected colour
         * @return {ko.pureComputed} 
         */
        colourPickerViewModel.prototype.getSelectedComputed = function getSelectedComputed() {
            return ko.pureComputed({
                read: function() {
                    return this.selectedObs();
                },
                write: function(colour) {
                    this.selectedObs(colour);
                    //this.hide();
                }
            }, this);
        };

        /**
         * hides the colourpicker on a click anywhere outside of the colourpicker
         */
        colourPickerViewModel.prototype.hideOnDocClick = function hideOnDocClick(e) {
            if ($.contains(this.element()[0], e.target) ) {
                return;
            }
            this.hide();
        };

        /**
         * toggles the colourpicker and set the focus back to the selected element
         */
        colourPickerViewModel.prototype.toggle = function toggle() {
            this.open(!this.open());
            if (this.focusCallback) {
                this.focusCallback();
            }
        };

        /**
         * hides the colourpicker and set the focus back to the selected element
         */
        colourPickerViewModel.prototype.hide = function hide() {
            this.open(false);
            if (this.focusCallback) {
                this.focusCallback();
            }
        };

        colourPickerViewModel.prototype.showCustom = function showCustom() {
            if (this.focusCallback) {
                this.focusCallback();
            }
            this.customShown(true);
        };

        colourPickerViewModel.prototype.hideCustom = function hideCustom() {
            this.customShown(false);
        };

        return {
            viewModel: colourPickerViewModel,
            template: { require: 'text!/canvas/templates/colourpicker.html' }
        };
});