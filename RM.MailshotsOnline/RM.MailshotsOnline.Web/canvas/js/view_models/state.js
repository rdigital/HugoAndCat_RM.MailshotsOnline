// view model which describes the state of the app at any given time
// and handles fecthing / persisting of data
define(['knockout', 'view_models/auth'],
    function(ko, authViewModel) {

        function stateViewModel() {
            this.mailshotID = ko.observable(this.getUrlVars()['mailshotId']);
            this.campaignID = window.campaignId || '';
            this.formatID = this.getUrlVars()['formatId'];
            this.selectedElement = ko.observable();
            this.scaleElement = ko.observable();
            this.backgroundSelected = ko.observable();
            this.backgroundToolsTop = ko.observable(0);
            this.backgroundToolsLeft = ko.observable(0);
            this.historyRerender = ko.observable(true);
            this.imageTab = ko.observable();
            this.showAuth = ko.observable(false);
            this.showPreview = ko.observable(false);
            this.showImageUpload = ko.observable(false);
            this.showThemePicker = ko.observable(false);
            this.showTemplatePicker = ko.observable(false);
            this.rerender = ko.observable(false);
            this.zoom = ko.observable(1);
            this.overrideZoom = ko.observable();
            this.fitToWidth = ko.observable(false);
            this.getZoom = this.getZoomComputed();
            this.viewingSide = ko.observable('front');
            this.viewingFace = ko.observable();
            this.ready = ko.observable(false);
            this.saving = ko.observable(false);
            this.uploadingImages = ko.observableArray();

            // subscriptions
            this.mailshotID.subscribe(this.pushIDtoURL, this);

            // bound methods
            this.toggleImage = this.toggleImage.bind(this);
            this.toggleAuth = this.toggleAuth.bind(this);
            this.displayAuth = this.displayAuth.bind(this);

            // set to true to make images rescale to default when the components render
            this.repositionImages = false;
            // testing
            this.output = ko.observable();
            this.selectedElement.subscribe(function() {
                this.showImageUpload(false);
            }, this);
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
        };

        stateViewModel.prototype.pushIDtoURL = function pushIDtoURL() {
            if (typeof(history) == "undefined") {
                return
            }
            history.replaceState(null, null, '/create-canvas/?formatId=' + this.formatID + '&mailshotId=' + this.mailshotID());
        }

        /**
         * Set a particular element as the currently selected one
         * @param  {elementViewModel} element element view model instance which has been selected
         */
        stateViewModel.prototype.selectElement = function selectElement(element) {
            // set the selected element
            this.selectedElement(element);
            if (!element) {
                window.getSelection().removeAllRanges();
            }
        };

        stateViewModel.prototype.toggleImageUpload = function toggleImageUpload() {
            if (!this.showImageUpload() && !authViewModel.isAuthenticated()) {
                this.displayAuth();
                return
            }
            this.imageTab('upload');
            // also check in the background if we still think the user is logged in
            // but they have actually logged out very recently in another tab
            if (!this.showImageUpload()) {
                authViewModel.getAuthenticated(function() {
                    this.displayAuth();
                    this.toggleImage();
                }.bind(this));
            }
            this.toggleImage();
        };

        stateViewModel.prototype.toggleImageLibrary = function toggleImageLibrary() {
            authViewModel.getAuthenticated();
            this.imageTab('library');
            this.toggleImage();
        };

        stateViewModel.prototype.toggleMyImages = function toggleMyImages() {
            authViewModel.getAuthenticated();
            this.imageTab('my_images');
            this.toggleImage();
        };

        /**
         * toggle the image upload modal
         */
        stateViewModel.prototype.toggleImage = function toggleImage() {
            this.showImageUpload(!this.showImageUpload());
            this.overrideZoom(null);
            setTimeout(function(){
                this.scaleElement.valueHasMutated();
            }.bind(this), 0);
        };

        /**
         * toggle the theme picker modal
         */
        stateViewModel.prototype.toggleThemePicker = function toggleThemePicker() {
            this.showThemePicker(!this.showThemePicker());
            this.showTemplatePicker(false);
            this.selectElement(null);
        };

        /**
         * toggle the template picker modal
         */
        stateViewModel.prototype.toggleTemplatePicker = function toggleTemplatePicker() {
            this.showTemplatePicker(!this.showTemplatePicker());
            this.showThemePicker(false);
            this.selectElement(null);
        };

        stateViewModel.prototype.displayAuth = function displayAuth() {
            this.showAuth(true);
        }

        stateViewModel.prototype.toggleAuth = function toggleAuth() {
            this.showAuth(!this.showAuth());
        }

        stateViewModel.prototype.togglePreview = function togglePreview() {
            this.showPreview(!this.showPreview());
        };

        stateViewModel.prototype.getZoomComputed = function getZoomComputed() {
            return ko.computed(function() {
                return this.overrideZoom() || this.zoom();
            }, this);
        };

        // testing
        window.state = new stateViewModel();
        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return window.state;
    }
);