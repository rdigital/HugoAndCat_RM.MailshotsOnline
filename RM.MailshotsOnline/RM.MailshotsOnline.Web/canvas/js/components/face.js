define(['knockout', 'components/input', 'components/image', 'view_models/template', 'view_models/theme', 'view_models/user', 'view_models/state'],

    function(ko, inputComponent, imageComponent, templateViewModel, themeViewModel, userViewModel, stateViewModel) {
        // register required components
        ko.components.register('input-component', inputComponent);
        ko.components.register('image-component', imageComponent);

        // ViewModel
        function faceViewModel(params) {
            this.data = params.data;
            this.preview = params.preview;
            this.backgroundSelected = stateViewModel.backgroundSelected;
            this.backgroundToolsTop = ko.observable(0);
            this.backgroundToolsLeft = ko.observable(0);
            this.selectedElement = stateViewModel.selectedElement;
            this.current_theme = ko.observable();
            this.scale = function() {
                stateViewModel.scaleElement.valueHasMutated();
            };

            // if theme / template override passed in, process it
            this.override_theme = params.override_theme;
            this.handleThemeOverride();
            this.override_template = params.override_template;
            this.handleTemplateOverride();

            // computeds
            this.isBackgroundSelected = this.getBackgroundSelectedComputed();
            this.elements = this.getElementsComputed();
            this.flatStyles = this.getFlatStyles();
            this.overlayVisible = this.getOverlayVisibleComputed();

            // subscriptions
            this.handleSubscriptions();
        }

        /**
         * Set up subscriptions which deselect element when BG is selected and vice versa
         */
        faceViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.selectedElement.subscribe(function(element) {
                if (element) {
                    this.backgroundSelected(false);
                }
            }, this);
            this.backgroundSelected.subscribe(function(selected) {
                if (selected) {
                    stateViewModel.selectElement(null);
                }
            }, this);
        };

        /**
         * returns a computed which evaluates to whether the background of this face is currently selected
         * @return {ko.pureComputed}
         */
        faceViewModel.prototype.getBackgroundSelectedComputed = function getBackgroundSelectedComputed() {
            return ko.pureComputed(function() {
                return this.backgroundSelected() == this;
            }, this);
        };

        /**
         * returns a computed which evaluates to whether the greyed out overlay should be displayed
         * @return {ko.pureComputed}
         */
        faceViewModel.prototype.getOverlayVisibleComputed = function getOverlayVisibleComputed() {
            return ko.pureComputed(function() {
                return (stateViewModel.selectedElement()) ? true : false;
            });
        };

        /**
         * returns a computed which evaluates to an array of elements from the template which are on this face
         * @return {ko.pureComputed}
         */
        faceViewModel.prototype.getElementsComputed = function getElementsComputed() {
            return ko.pureComputed(function(){
                var face_name = this.data.name;
                if (!(userViewModel.ready() && themeViewModel.objects())) {
                    return {};
                }

                // bit hacky
                // force redraw of everything when theme changes too, not just when template changes
                if (this.current_theme() != this.themeViewModel.selected()) {
                    setTimeout( function() {
                        this.current_theme(this.themeViewModel.selected());
                    }.bind(this), 10);
                    return [];
                }
                return this.templateViewModel.getElementsByFace(face_name);
            }, this);
        };

        /**
         * get recommended theme colours for this element
         * @return {Array} array of css colour values (hex or rgb)
         */
        faceViewModel.prototype.getColours = function getColours() {
            return this.themeViewModel.getColours();
        };

        /**
         * get css style value by property name, check for user defined first then fall back to theme
         * @param  {String} property property name to check for
         * @return {String}          value of provided property
         */
        faceViewModel.prototype.getStyle = function getStyle(property) {
            var userStyle = userViewModel.getFaceStyle(property, this.data.name);
            if (userStyle) {
                return userStyle;
            }
            var themeStyles = this.themeViewModel.getFaceStylesByName(this.data.name);
            return themeStyles[property];
        };

        /**
         * Set user defined style for this face
         * @param {String} property css property to apply
         * @param {String} value    value of css property to apply
         */
        faceViewModel.prototype.setStyle = function setStyle(property, value) {
            userViewModel.setFaceStyle(property, value, this.data.name);
        };

        /**
         * returns a computed which evaluates to an object of property value pairs for the current face styles
         * @return {ko.pureComputed}
         */
        faceViewModel.prototype.getFlatStyles = function getFlatStyles(){
            // if we're overriding the theme, ignore userstyles
            if (this.override_theme) {
                var styles = {},
                    themeStyles = this.themeViewModel.getFaceStylesByName(this.data.name);
                ko.utils.extend(styles, themeStyles);
                styles.width = this.data.width + 'px';
                styles.height = this.data.height + 'px';
                return ko.observable(styles);
            }
            return ko.pureComputed(function(){
                if (!(userViewModel.ready() && themeViewModel.objects())) {
                    return {};
                }
                var styles = {},
                    themeStyles = this.themeViewModel.getFaceStylesByName(this.data.name),
                    userStyles = userViewModel.getFaceStylesByName(this.data.name);

                // remove font size from user styles if overrideing the template (template change preview)
                if (this.override_template) {
                    delete userStyles['font-size'];
                }
                // merge theme styles and user defined styles
                ko.utils.extend(styles, themeStyles);
                ko.utils.extend(styles, userStyles);
                styles.width = this.data.width + 'px';
                styles.height = this.data.height + 'px';
                return styles;
            }, this);
        };

        /**
         * Returns true if the provided name is the name of the currently selected element
         * Used to set a class on the component container when the component is selected
         * @param  {String} name name of child element
         * @return {Boolean}     true if child is selected
         */
        faceViewModel.prototype.childSelected = function childSelected(name) {
            var selectedEl = this.selectedElement();
            return (selectedEl && selectedEl.data.name == name) ? true : false;
        };

        /**
         * returns property / value pairs for the layout style properties for an element on the face
         * @param  {Object} element element
         * @return {Object}         property / value pairs of styles
         */
        faceViewModel.prototype.getElementLayout = function getElementLayout(element){
            var styles = {};
            ko.utils.arrayForEach(element.layout, function(style) {
                styles[style.property] = style.value;
            });
            return styles;
        };

        /**
         * if we are overriding the theme, clone the current themeViewModel instance and
         * set the selected ID to be the that of the theme we want to override the current theme with
         */
        faceViewModel.prototype.handleThemeOverride = function handleThemeOverride() {
            if (this.override_theme) {
                this.themeViewModel = $.extend(true, Object.create(Object.getPrototypeOf(themeViewModel)), themeViewModel);
                this.themeViewModel.selected = ko.observable(this.override_theme);
                this.themeViewModel.selectedID = ko.observable(this.override_theme.id);
                this.override_theme = this.themeViewModel;
            } else {
                this.themeViewModel = themeViewModel;
            }
        };

        /**
         * if we are overriding the template, clone the current templateViewModel instance and
         * set the selected ID to be the that of the theme we want to override the current template with
         */
        faceViewModel.prototype.handleTemplateOverride = function handleTemplateOverride() {
            if (this.override_template) {
                this.templateViewModel = $.extend(true, Object.create(Object.getPrototypeOf(templateViewModel)), templateViewModel);
                this.templateViewModel.selected = ko.observable(this.override_template);
                this.templateViewModel.selectedID = ko.observable(this.override_template.id);
                this.override_template = this.templateViewModel;
            } else {
                this.templateViewModel = templateViewModel;
            }
        };

        /**
         * select the background of this face
         */
        faceViewModel.prototype.selectBackground = function selectBackground(data, e) {
            this.backgroundToolsLeft(e.screenX);
            this.backgroundToolsTop(Math.max(e.screenY, 160));
            this.backgroundSelected(this);
        };

        return {
            viewModel: faceViewModel,
            template: { require: 'text!/canvas/templates/face.html' }
        };
});