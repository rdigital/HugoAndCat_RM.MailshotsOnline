define(['knockout', 'view_models/state', 'view_models/theme', 'view_models/user'],
    function(ko, stateViewModel, themeViewModel, userViewModel) {

        // ViewModel
        function elementViewModel(params) {
            // generic viewmodel to be extended for any input fields
            // abstract class with empty constructor
        }

        /**
         * set up subscriptions which react to changes to the selected element
         * on the stateViewModel and also push changes to it upon element selection
         */
        elementViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.subscriptions.push(
                this.isSelected.subscribe(function(selected) {
                    if (selected) {
                        this.getBackgroundColour();
                        stateViewModel.selectedElement(this);
                    } else if (stateViewModel.selectedElement() == this) {
                        stateViewModel.selectedElement(null);
                    }
                }.bind(this))
            );

            this.subscriptions.push(
                this.isSelected.subscribe(this.getCoords.bind(this))
            );

            this.subscriptions.push(
                stateViewModel.selectedElement.subscribe(function(selected) {
                    this.isSelected(selected == this);
                }.bind(this))
            );
        };

        /**
         * called to dispose of subscriptions on destroy
         */
        elementViewModel.prototype.dispose = function dispose() {
            ko.utils.arrayForEach(this.subscriptions, function(sub) {
                sub.dispose();
            });
        };

        /**
         * get the coordinates of the top, left, right and bottom edges of the element
         * @return {Object} element coordinates
         */
        elementViewModel.prototype.getCoords = function getCoords() {
            var el;
            if (this.element()) {
                el = this.element().closest('.component')[0];
            } else if (this.canvas && this.canvas()) {
                el = this.canvas()[0];
            }
            var coords = el.getBoundingClientRect();

            return coords;
        };

        /**
         * returns a computed which flattens the styles from theme and user data into
         * something useable directly on the template in a knockout binding
         * @return {ko.pureComputed} key / value pairs of styles
         */
        elementViewModel.prototype.getFlatStyles = function getFlatStyles() {
            if (this.override_theme) {
                return ko.observable(this.themeViewModel.getStylesByName(this.data.theme_class));
            }
            return ko.pureComputed(function(){
                // apply theme, then user styles before flattening styles array to be used in binding
                var styles = {},
                    themeStyles = this.themeViewModel.getStylesByName(this.data.theme_class),
                    userStyles = userViewModel.getStylesByName(this.data.name);

                // ignore user defined font-size if we are overriding the template
                if (this.override_template) {
                    delete userStyles['font-size'];
                }

                // merge theme and user defined styles
                ko.utils.extend(styles, themeStyles);
                ko.utils.extend(styles, userStyles);

                // if no explicit bg colour set on element, inherit the face's bg colour when selected
                if (!(styles['background-color'] || styles['background'])) {
                    if (this.isSelected()) {
                        var bg = this.fallbackBackground();
                        if (bg) {
                            styles['background'] = bg;
                        }
                    } else {
                        styles['background'] = 'transparent';
                    }
                }
                return styles;
            }, this);
        };

        elementViewModel.prototype.getBackgroundColour = function getBackgroundColour() {
            // try to get the required background colour for this input to grey out other inputs
            var component = this.element().closest('.component'),
                coords = component[0].getBoundingClientRect();
            component.hide();
            var behind_el = document.elementFromPoint(coords.left, coords.top),
                face = $(behind_el).closest('.face, .colour-box'),
                bg_colour = face.css('background-color');
            this.fallbackBackground(bg_colour);
            component.show();
        } 

        /**
         * Set user defined style for this element
         * @param {String} property css property to apply
         * @param {String} value    value of css property to apply
         */
        elementViewModel.prototype.setStyle = function setStyle(property, value) {
            userViewModel.setStyle(property, value, this.data.name);
        };

        /**
         * get css style value by property name, check for user defined first then fall back to theme
         * @param  {String} property property name to check for
         * @return {String}          value of provided property
         */
        elementViewModel.prototype.getStyle = function getStyle(property) {
            var userStyle = userViewModel.getStyle(property, this.data.name);
            if (userStyle) {
                return userStyle;
            }
            var themeStyle = this.themeViewModel.getStyle(property, this.data.theme_class);
            return themeStyle;
        };

        /**
         * get the available font sizes for this element from the theme
         * @return {Array} array of pixel font sizes
         */
        elementViewModel.prototype.getFontSizes = function getFontSizes() {
            return this.themeViewModel.getFontSizes(this.data.theme_class);
        };

        elementViewModel.prototype.getVerticalMiddle = function getVerticalMiddle() {
            return this.themeViewModel.getVerticalMiddle(this.data.theme_class);
        };

        /**
         * get available font families for this element from the theme
         * @return {Array} array of name, value pairs for fonts
         */
        elementViewModel.prototype.getFonts = function getFonts() {
            return this.themeViewModel.getFonts();
        };

        /**
         * get recommended theme colours for this element
         * @return {Array} array of css colour values (hex or rgb)
         */
        elementViewModel.prototype.getColours = function getColours() {
            return this.themeViewModel.getColours();
        };

        /**
         * sets this.themeViewModel to be the override theme if one is provided
         * (used to facilitate theme selector with previews)
         */
        elementViewModel.prototype.handleOverrides = function handleOverrides() {
            if (this.override_theme) {
                this.themeViewModel = this.override_theme;
            } else {
                this.themeViewModel = themeViewModel;
            }
        };

        elementViewModel.prototype.getMessageComputed = function getMessageComputed() {
            return ko.pureComputed(function() {
                if (this.isScrollVisible && this.isScrollVisible()) {
                    return {
                        type: 'error',
                        message: "You've run out of room in this text field. Try reducing the amount of text, or use a smaller font size if available."
                    };
                }
                if (this.data.message) {
                    return {
                        type: 'message',
                        message: this.data.message
                    };
                }
                return null;
            }, this);
        };

        elementViewModel.prototype.getTitleComputed = function getTitleComputed() {
            return ko.pureComputed(function() {
                if (this.isScrollVisible && this.isScrollVisible()) {
                    return "<span class='help-error icon-alert'></span>";
                }
                return (this.data.message) ? this.data.title : '';
            }, this);
        };

        //return
        return elementViewModel;
    }
);