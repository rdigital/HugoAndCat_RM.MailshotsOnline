// view model which describes the state of the app at any given time
// and handles fecthing / persisting of data
define(['knockout', 'jquery'],
    function(ko, $) {

        function stateViewModel() {
            this.selectedElement = ko.observable();
            this.scaleElement = ko.observable();
            this.backgroundSelected = ko.observable();
            this.showImageUpload = ko.observable(false);
            this.showThemePicker = ko.observable(false);
            this.showTemplatePicker = ko.observable(false);
            this.rerender = ko.observable(false);
            this.zoom = ko.observable(1);
            this.overrideZoom = ko.observable();
            this.getZoom = this.getZoomComputed();
            this.viewingSide = ko.observable('front');
            // set to true to make images rescale to default when the components render
            this.repositionImages = false;
            // testing
            this.output = ko.observable();
        }

        /**
         * Set a particular element as the currently selected one
         * @param  {elementViewModel} element element view model instance which has been selected
         */
        stateViewModel.prototype.selectElement = function selectElement(element) {
            // set the selected element
            this.selectedElement(element);
            if (!element) {
                window.getSelection().removeAllRanges()
            }
        }

        /**
         * toggle the image upload modal
         */
        stateViewModel.prototype.toggleImageUpload = function toggleImageUpload() {
            this.showImageUpload(!this.showImageUpload());
        }

        /**
         * toggle the theme picker modal
         */
        stateViewModel.prototype.toggleThemePicker = function toggleThemePicker() {
            this.showThemePicker(!this.showThemePicker());
            this.selectElement(null);
        }

        /**
         * toggle the template picker modal
         */
        stateViewModel.prototype.toggleTemplatePicker = function toggleTemplatePicker() {
            this.showTemplatePicker(!this.showTemplatePicker());
            this.selectElement(null);
        }

        stateViewModel.prototype.getZoomComputed = function getZoomComputed() {
            return ko.computed(function() {
                return this.overrideZoom() || this.zoom();
            }, this)
        }

        // testing
        window.state = new stateViewModel()
        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return window.state;
    }
)