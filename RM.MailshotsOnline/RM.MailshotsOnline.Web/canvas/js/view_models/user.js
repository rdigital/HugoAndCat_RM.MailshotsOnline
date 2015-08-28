// user data view model, manages the user-defined data (content and styles) as
// well as the format / template / theme IDs
define(['knockout', 'komapping', 'jquery', 'temp/data', 'view_models/history', 'view_models/state'],
    function(ko, komapping, $, tempData, historyViewModel, stateViewModel) {

        function userViewModel() {
            // initialize this.objects
            this.objects = komapping.fromJS({});

            this.fetch();
            window.user = this;
        }

        userViewModel.prototype.cancelChanges = function cancelChanges() {
            historyViewModel.cancelChanges();
        }

        userViewModel.prototype.applyChanges = function applyChanges() {
            historyViewModel.pushToHistory();
        }

        userViewModel.prototype.fromJSON = function fromJSON(data) {
            komapping.fromJSON(data, this.objects);
            // force rerender
            stateViewModel.viewingSide.valueHasMutated()
        }

        userViewModel.prototype.toJSON = function toJSON() {
            return komapping.toJSON(this.objects, {'ignore': ["src"]})
        }

        userViewModel.prototype.toHistoryJSON = function toHistoryJSON() {
            return komapping.toJSON(this.objects)
        }

        /**
         * fetch userData JSON from server and store it in this.objects
         */
        userViewModel.prototype.fetch = function fetch() {
            // XXX TEMP XXX
            komapping.fromJS(tempData.userData, this.objects);
            setTimeout(this.applyChanges.bind(this),1000)
            this.objects.themeID.subscribe(this.resetUserStyles.bind(this));
            return
            $.getJSON('/user_data', function(data) {
                komapping.fromJS(data, this.objects);
            }.bind(this))
        }

        userViewModel.prototype.resetUserFontSizes = function resetUserFontSizes() {
            ko.utils.arrayForEach(this.get('elements') || [], function(element) {
                element.styles.remove(function(style) {
                    return style.property() == 'font-size'
                })
            })
        }

        userViewModel.prototype.resetUserStyles = function resetUserStyles() {
            ko.utils.arrayForEach(this.get('elements') || [], function(element) {
                element.styles([])
            })
            ko.utils.arrayForEach(this.get('faces') || [], function(face) {
                face.styles([])
            })
        }


        /**
         * get value by key within object of observables
         * @param  {String} key the key to search by
         * @return {?}      value for this key
         */
        userViewModel.prototype.get = function get(key) {
        	var val = this.objects[key];
        	return (val) ? ko.utils.unwrapObservable(val) : null
        }

        /**
         * try to get element info from userData by name
         * @param  {String} name name of element to look for
         * @return {Object}      element info from userData
         */
        userViewModel.prototype.getElementByName = function getElementByName(name) {
            var elem_array = this.get('elements') || [],
                element = ko.utils.arrayFirst(elem_array, function(element) {
                    return element.name() == name;
                });
            return element || null
        }

        /**
         * try to get element info from userData by name
         * if it does not exist, create an object of observables for the provided name
         * in the elements array on the userData object
         * @param  {String} name name of element to look for
         * @return {Object}      element info from userData
         */
        userViewModel.prototype.getOrSetElementByName = function getOrSetElementByName(name) {
            var elem = this.getElementByName(name);
            if (!elem) {
                var new_elem = {
                        name: name,
                        content: '',
                        styles: []
                    },
                    elem = komapping.fromJS(new_elem);
                this.objects.elements.push(elem);
            }
            return elem
        }

        /**
         * get user defined styles for an element by element name
         * @param  {String} name the class name to search for
         * @return {Object}      key value pairs of css properties to values
         */
        userViewModel.prototype.getStylesByName = function getStylesByName(name) {
            var elem = this.getOrSetElementByName(name),
                styles = {};
            if (elem.styles) {
                ko.utils.arrayForEach(elem.styles(), function(style) {
                    styles[style.property()] = style.value;
                });
            } else {
                elem.styles = ko.observableArray();
            }
            return styles;
        }

        /**
         * Find element by name, then set the css property to be value by updating userData
         * @param {String} property name of the css property to set
         * @param {String} value    value to set css property to
         * @param {String} name     name of element to update
         */
        userViewModel.prototype.setStyle = function setStyle(property, value, name) {
            var elem = this.getOrSetElementByName(name),
                match = false;
            if (elem.styles) {
                ko.utils.arrayForEach(elem.styles(), function(style) {
                    if (style.property() == property) {
                        style.value(value);
                        match = true;
                    }
                });
            }
            // if no matching style in the userData, push a new style
            if (!match) {
                var new_style = komapping.fromJS({
                    property: property,
                    value: value
                });
                elem.styles.push(new_style);
            }
        }

        userViewModel.prototype.getStyle = function getStyle(property, name) {
            var styles = this.getStylesByName(name);
            return ko.utils.unwrapObservable(styles[property]);
        }

        userViewModel.prototype.getStyleObs = function getStyleObs(property, name) {
            var styles = this.getStylesByName(name);
            return styles[property];
        }

        userViewModel.prototype.getOrSetImageByName = function getOrSetImageByName(name) {
            var elem = this.getElementByName(name);
            if (!elem) {
                var new_elem = {
                    name: name,
                    content: '',
                    styles: [],
                    src: null,
                    scale: null,
                    img_position: {
                        top: null,
                        left: null
                    }
                },
                elem = komapping.fromJS(new_elem);
                this.objects.elements.push(elem);
            }
            return elem
        }

        /**
         * get the image source, position and scale for an element by name
         * @param  {String} name element name to look up
         * @return {Object}      source position and scale of the image within the canvas
         */
        userViewModel.prototype.getImageByName = function getImageByName(name) {
            var elem = this.getOrSetImageByName(name),
                image = {};

            if (elem) {
                image.src = elem.src;
                image.img_position = elem.img_position;
                image.scale = elem.scale;
                image.content = elem.content;
            }
            return image;
        }

        userViewModel.prototype.getFaceByName = function getFaceByName(name) {
            var face_array = this.get('faces') || [],
                face = ko.utils.arrayFirst(face_array, function(face) {
                    return face.name() == name;
                });
            return face || null
        }

        userViewModel.prototype.getOrSetFaceByName = function getOrSetFaceByName(name) {
            var face = this.getFaceByName(name);
            if (!face) {
                var new_face = {
                        name: name,
                        styles: []
                    },
                    face = komapping.fromJS(new_face);
                this.objects.faces.push(face);
            }
            return face
        }

        userViewModel.prototype.setFaceStyle = function setFaceStyle(property, value, name) {
            var face = this.getOrSetFaceByName(name),
                match = false;
            if (face.styles) {
                ko.utils.arrayForEach(face.styles(), function(style) {
                    if (style.property() == property) {
                        style.value(value);
                        match = true;
                    }
                });
            }
            // if no matching style in the userData, push a new style
            if (!match) {
                var new_style = komapping.fromJS({
                    property: property,
                    value: value
                });
                face.styles.push(new_style);
            }
        }

        userViewModel.prototype.getFaceStyle = function getFaceStyle(property, name) {
            var styles = this.getFaceStylesByName(name);
            return ko.utils.unwrapObservable(styles[property]);
        }

        userViewModel.prototype.getFaceStylesByName = function getFaceStylesByName(name) {
            var face = this.getOrSetFaceByName(name),
                styles = {};
            if (face.styles) {
                ko.utils.arrayForEach(face.styles(), function(style) {
                    styles[style.property()] = style.value;
                });
            } else {
                face.styles = ko.observableArray();
            }
            return styles;
        }

        return new userViewModel();
    }
)