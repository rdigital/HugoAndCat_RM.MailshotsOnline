// view model which describes the state of the app at any given time
// and handles fecthing / persisting of data
define(['knockout', 'jquery'],
    function(ko, $) {

        function stateViewModel() {
            this.mailshotID = this.getUrlVars()['mailshotId'];
            this.formatID = this.getUrlVars()['formatId'];
            this.selectedElement = ko.observable();
            this.scaleElement = ko.observable();
            this.backgroundSelected = ko.observable();
            this.historyRerender = ko.observable(true);
            this.imageTab = ko.observable();
            this.showPreview = ko.observable(false);
            this.showImageUpload = ko.observable(false);
            this.showThemePicker = ko.observable(false);
            this.showTemplatePicker = ko.observable(false);
            this.rerender = ko.observable(false);
            this.zoom = ko.observable(1);
            this.overrideZoom = ko.observable();
            this.getZoom = this.getZoomComputed();
            this.viewingSide = ko.observable('front');
            this.viewingFace = ko.observable();
            // set to true to make images rescale to default when the components render
            this.repositionImages = false;
            // testing
            this.output = ko.observable();
            this.selectedElement.subscribe(function() {
                this.showImageUpload(false);
            }, this)
        }

        /**
         * get variables from the URL
         * @return {Object} [key value pairs for the URL variables] 
         */
        stateViewModel.prototype.getUrlVars = function getUrlVars() {
            var vars = {},
                parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
                    vars[key] = value;
                });
            return vars;
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

        stateViewModel.prototype.toggleImageUpload = function toggleImageUpload() {
            this.imageTab('upload');
            this.toggleImage();
        }

        stateViewModel.prototype.toggleImageLibrary = function toggleImageLibrary() {
            this.imageTab('library');
            this.toggleImage();
        }

        /**
         * toggle the image upload modal
         */
        stateViewModel.prototype.toggleImage = function toggleImage() {
            this.showImageUpload(!this.showImageUpload());
            this.overrideZoom(null)
            setTimeout(function(){
                this.scaleElement.valueHasMutated();
            }.bind(this), 0) 
        }

        /**
         * toggle the theme picker modal
         */
        stateViewModel.prototype.toggleThemePicker = function toggleThemePicker() {
            this.showThemePicker(!this.showThemePicker());
            this.showTemplatePicker(false);
            this.selectElement(null);
        }

        /**
         * toggle the template picker modal
         */
        stateViewModel.prototype.toggleTemplatePicker = function toggleTemplatePicker() {
            this.showTemplatePicker(!this.showTemplatePicker());
            this.showThemePicker(false);
            this.selectElement(null);
        }

        stateViewModel.prototype.togglePreview = function togglePreview() {
            this.showPreview(!this.showPreview());
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