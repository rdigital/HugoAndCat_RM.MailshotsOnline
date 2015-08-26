define(['knockout', 'components/dropdown', 'components/slider', 'components/colourpicker', 'view_models/state'],

    function(ko, dropdownComponent, sliderComponent, colourpickerComponent, stateViewModel) {
        // register required components
        ko.components.register('dropdown-component', dropdownComponent);
        ko.components.register('colourpicker-component', colourpickerComponent);
        ko.components.register('slider-component', sliderComponent);

        // ViewModel
        function toolsViewModel(params) {
            this.element = ko.observable();
            this.selectedElement = stateViewModel.selectedElement;
            this.window_width = ko.observable(0)
            this.window_height = ko.observable(0)

            this.isVisible = ko.pureComputed(function() {
                return this.selectedElement() ? true : false;
            }, this).extend({throttle: 10})

            this.elementType = this.getElementTypeComputed();
            this.showScale = this.getScaleComputed();
            this.attachment = this.getAttachmentComputed();
            this.alignment = this.getAlignmentComputed();
            this.fonts = this.getFontsComputed();
            this.colours = this.getColoursComputed();

            this.focusInput = this.focusInput.bind(this)

            this.colour = this.getStyleComputed('color');
            this.font = this.getStyleComputed('font-family');
            $(window).resize(this.handleResize.bind(this));
        }

        /**
         * set the focus to the editable div within this component
         */
        toolsViewModel.prototype.focusInput = function focusInput() {
            if (this.selectedElement() && this.selectedElement().setFocus) {
                this.selectedElement().setFocus();
            }
        }

        /**
         * get computed which evaluates the currently selected element type
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.getElementTypeComputed = function getElementTypeComputed() {
            return ko.pureComputed(function() {
                var el = this.selectedElement();
                return el ? el.data.type : '';
            }, this)
        }

        /**
         * get computed which evaluates whether to show the scale slider
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.getScaleComputed = function getScaleComputed() {
            return ko.pureComputed(function() {
                if (this.elementType() == 'image') {
                    if (this.selectedElement().imageObj.src && this.selectedElement().imageObj.src()) {
                        return true
                    }
                }
                return false
            }, this)
        }

        /**
         * get computed which evaluates the available fonts for the selected element
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.getFontsComputed = function getFontsComputed() {
            return ko.pureComputed(function() {
                if (this.selectedElement()) {
                    return this.selectedElement().getFonts();
                }
                return []
            }, this)
        }

        /**
         * get computed which evaluates the available colours for the selected element
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.getColoursComputed = function getColoursComputed() {
            return ko.pureComputed(function() {
                if (this.selectedElement()) {
                    return this.selectedElement().getColours();
                }
                return []
            }, this)
        }

        /**
         * get computed which evaluates the best available attachment position for the tools menu
         * such that it appears next to the selected element
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.getAttachmentComputed = function getAttachmentComputed() {
            return ko.pureComputed(function() {
                if (this.selectedElement()) {
                    return this.calcAttachment();
                }
                return {}
            }, this)
        }

        toolsViewModel.prototype.handleResize = function handleResize() {
            this.window_width($(window).width())
            this.window_height($(window).height())
        }

        toolsViewModel.prototype.calcAttachment = function calcAttachment() {
            var coords = this.selectedElement().getCoords(),
                right_margin = this.window_width() - coords.right,
                bottom_margin = this.window_height() - coords.bottom,
                attachment = {};

            // not enough horizontal room, try placing tools above or below
            if (coords.left < coords.width && right_margin < coords.width) {
                attachment.left = coords.left - 10;
                attachment.right = 'auto';
                if (coords.top > bottom_margin && coords.top > coords.height) {
                    attachment.bottom = $(window).height() - coords.top + 10;
                    attachment.top = 'auto';
                } else {
                    attachment.top = coords.bottom + 10;
                    attachment.bottom = 'auto';
                }
                return attachment
            }

            // horizontal attachment
            if (coords.left > right_margin) {
                attachment.right = $(window).width() - coords.left;
                attachment.left = 'auto';
            } else {
                attachment.left = coords.right;
                attachment.right = 'auto';
            }

            // vertical attachment
            if (coords.top > bottom_margin) {
                attachment.bottom = bottom_margin;
                attachment.top = 'auto';
            } else {
                attachment.top = coords.top;
                attachment.bottom = 'auto';
            }
            return attachment
        }

        /**
         * set a user defined style on the current element
         * @param {String} property property name to apply style to 
         * @param {String} value    value of style property to apply
         */
        toolsViewModel.prototype.setStyle = function setStyle(property, value) {
            if (this.selectedElement()) {
                this.selectedElement().setStyle(property, value);
            }
        }

        /**
         * trigger an increase of font size for this element
         */
        toolsViewModel.prototype.increaseFontSize = function increaseFontSize() {
            if (this.selectedElement()) {
                this.selectedElement().increaseFontSize();
            }
        }

        /**
         * trigger an decrease of font size for this element
         */
        toolsViewModel.prototype.decreaseFontSize = function decreaseFontSize() {
            if (this.selectedElement()) {
                this.selectedElement().decreaseFontSize();
            }
        }

        /**
         * get computed which evaluates whether text is bold in the current context
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.isBold = function isBold() {
            return ko.pureComputed( function() {
                if (this.selectedElement()) {
                    return this.selectedElement().isBold();
                }
                return false
            }, this)
        }

        /**
         * set text to bold at caret / selection
         */
        toolsViewModel.prototype.bold = function bold() {
            var el = this.selectedElement()
            if (el) {
                setTimeout(el.bold.bind(el),0);
            }
        }

        /**
         * get computed which evaluates whether text is italic in the current context
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.isItalic = function isItalic() {
            return ko.pureComputed( function() {
                if (this.selectedElement()) {
                    return this.selectedElement().isItalic();
                }
                return false
            }, this)
        }

        /**
         * set text to italic at caret / selection
         */
        toolsViewModel.prototype.italic = function italic() {
            var el = this.selectedElement()
            if (el) {
                setTimeout(el.italic.bind(el),0);
            }
        }

        /**
         * get computed which evaluates whether text is underlined in the current context
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.isUnderline = function isUnderline() {
            return ko.pureComputed( function() {
                if (this.selectedElement()) {
                    return this.selectedElement().isUnderline();
                }
                return false
            }, this)
        }

        /**
         * set text to underlined at caret / selection
         */
        toolsViewModel.prototype.underline = function underline() {
            var el = this.selectedElement()
            if (el) {
                setTimeout(el.underline.bind(el),0);
            }
        }

        /**
         * get computed which evaluates the current text aligment of the element
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.getAlignmentComputed = function getAlignmentComputed() {
            return ko.pureComputed(function() {
                if (this.selectedElement()) {
                    return this.selectedElement().getStyle('text-align');
                }
                return
            }, this)
        }

        /**
         * computed generator for styles. Provide a style property name and the returned computed
         * will provide a read / write computed which evaluates to that property's current value
         * @return {ko.pureComputed} 
         */
        toolsViewModel.prototype.getStyleComputed = function getStyleComputed(property) {
            return ko.pureComputed( {
                read: function() {
                    var el = this.selectedElement()
                    if (el) {
                        return el.getStyle(property);
                    }
                    return
                },
                write: function(val) {
                    var el = this.selectedElement();
                    if (el) {
                        el.setStyle(property, val);
                        el.setFocus();
                    }
                }
            }, this)
        }

        /**
         * toggle the image upload modal
         */
        toolsViewModel.prototype.toggleImageUpload = function toggleImageUpload() {
            stateViewModel.toggleImageUpload();
        }

        return {
            viewModel: toolsViewModel,
            template: { require: 'text!/canvas/templates/tools.html' }
        }
});