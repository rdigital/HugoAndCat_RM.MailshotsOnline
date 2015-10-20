// viewmodel to handle format data
define(['knockout', 'view_models/data', 'view_models/user', 'temp/data'],
    function(ko, dataViewModel, userViewModel, tempData) {

        function themeViewModel() {
            this.objects = ko.observableArray([]);
            this.selected = this.getSelectedComputed();
            this.fetchURL = '/umbraco/api/MailshotSettings/GetThemes';
            this.selectedID = ko.pureComputed(function () {
                if (userViewModel.ready()) {
                    return userViewModel.objects.themeID();
                }
            }, this);

            // fetch data
            this.fetch();
        }

        // extends dataViewModel
        themeViewModel.prototype = Object.create(dataViewModel.prototype);
        themeViewModel.prototype.constructor = themeViewModel;

        /* TEMP XX DELETE */
        /*themeViewModel.prototype.fetch = function fetch() {
            //console.log('fetching data from ' + this.fetchURL);
            this.objects(tempData.themeData);
        };*/

        /**
         * try to get style info from themeData by class name
         * @param  {String} name name of class to look for
         * @return {Object}      style info from themeData
         */
        themeViewModel.prototype.getClassByName = function getClassByName(name) {
            var selected = this.selected(),
                class_obj = null;
            if (selected) {
                class_obj = ko.utils.arrayFirst(selected.classes, function(theme_class) {
                    return theme_class.name == name;
                });
            }
            return class_obj;
        };

        /**
         * get styles from the current theme by class name
         * @param  {String} name the class name to search for
         * @return {Object}      key value pairs of css properties to values
         */
        themeViewModel.prototype.getStylesByName = function getStylesByName(name) {
            var themeData = this.getClassByName(name),
                styles = {};
            if (themeData && themeData.styles) {
                ko.utils.arrayForEach(themeData.styles, function(style) {
                    styles[style.property] = style.value;
                });
            }
            return styles;
        };

        /**
         * get specific styles from the current theme by class name and property name
         * @param  {String} property the property name to search for
         * @param  {String} name     the class name to search for
         * @return {String}          value of css property
         */
        themeViewModel.prototype.getStyle = function getStyle(property, name) {
            var styles = this.getStylesByName(name);
            return styles[property];
        };

        /**
         * get face object from theme's array of faces by face name
         * @param  {String} name face name
         * @return {Object}      face object from theme data
         */
        themeViewModel.prototype.getFaceByName = function getFaceByName(name) {
            var selected = this.selected(),
                face_obj = [];
            if (selected) {
                face_obj = ko.utils.arrayFirst(selected.faces || [], function(face) {
                    return face.name == name;
                });
            }
            return face_obj;
        };

        themeViewModel.prototype.getFaceStyle = function getFaceStyle(property, name) {
            var styles = this.getFaceStylesByName(name);
            return ko.utils.unwrapObservable(styles[property]);
        };

        /**
         * get face specific styles from theme by face name
         * @param  {String} name face name
         * @return {Object}      key value pairs of css properties to values
         */
        themeViewModel.prototype.getFaceStylesByName = function getFaceStylesByName(name) {
            var faceData = this.getFaceByName(name),
                styles = {};
            if (faceData && faceData.styles) {
                ko.utils.arrayForEach(faceData.styles, function(style) {
                    styles[style.property] = style.value;
                });
            }
            return styles;
        };

        /**
         * get the image source, position and scale for this class name
         * @param  {String} name class name to look up
         * @return {Object}      source position and scale of the image within the canvas
         */
        themeViewModel.prototype.getImageByName = function getImageByName(name) {
            var themeData = this.getClassByName(name),
                image = {};
            if (themeData) {
                image.src = themeData.src;
                image.img_position = themeData.img_position || { top: 0, left: 0 };
                image.scale = themeData.scale;
            }
            return image;
        };

        /**
         * get the available font sizes for a class by name
         * @param  {String} name class name to search for
         * @return {Array}       Array of available font sizes
         */
        themeViewModel.prototype.getFontSizes = function getFontSizes(name) {
            var classObj = this.getClassByName(name);
            return (classObj) ? classObj.font_sizes : [];
        };

        themeViewModel.prototype.getVerticalMiddle = function getVerticalMiddle(name) {
            var classObj = this.getClassByName(name);
            return (classObj) ? (classObj.vertical_middle || false) : false;
        };

        /**
         * get the available fonts for a theme
         * @return {Object} name value pairs for available fonts
         */
        themeViewModel.prototype.getFonts = function getFonts() {
            var selected = this.selected();
            if (selected) {
                return selected.fonts;
            }
            return [];
        };

        /**
         * get the available colours for this theme
         * @return {[String]} array of values for available colours (hex or rgb)
         */
        themeViewModel.prototype.getColours = function getColours() {
            var selected = this.selected();
            if (selected) {
                return selected.colours;
            }
            return [];
        };

        // for testing
        window.theme = new themeViewModel();

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return window.theme;
    }
);