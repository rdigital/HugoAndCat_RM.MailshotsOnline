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
            this.window_width = ko.observable(0);
            this.window_height = ko.observable(0);

            this.personalizing = ko.observable(false);
            this.peronalizationOptions = ko.observableArray([
                {name: 'First Name', value: 'FirstName'},
                {name: 'Last Name', value: 'LastName'},
                {name: 'Title', value: 'Title'}
            ]);
            this.personalizationField = ko.observable('FirstName');
            this.personalizationFallback = ko.observable('');
            this.personalizationEl = ko.observable(null);
            this.caretPosition = 0;

            // computeds
            this.isVisible = this.getIsVisibleComputed();
            this.elementType = this.getElementTypeComputed();
            this.showScale = this.getScaleComputed();
            this.attachment = this.getAttachmentComputed();
            this.alignment = this.getAlignmentComputed();
            this.fonts = this.getFontsComputed();
            this.colours = this.getColoursComputed();
            this.colour = this.getStyleComputed('color');
            this.font = this.getStyleComputed('font-family');

            // bound methods
            this.focusInput = this.focusInput.bind(this);
            this.focusToolsInput = this.focusToolsInput.bind(this)
            this.handleResize = this.handleResize.bind(this);
            this.setCaretPosition = this.setCaretPosition.bind(this);
            this.showPersonalization = this.showPersonalization.bind(this);
            this.closeEditPersonalization = this.closeEditPersonalization.bind(this);

            // resize handlers
            $(window).resize(this.handleResize);
            $('.canvas-container').on('scroll', this.handleResize)

            // subscriptions
            this.selectedElement.subscribe(this.closePersonalization, this);
            stateViewModel.zoom.subscribe(this.handleResize, this);
            stateViewModel.overrideZoom.subscribe(this.handleResize, this);
            this.handleResize();

            // handle event triggered by clicking a personalization element
            // (can't be handled by framework unfortunately)
            $(window).on('personalization', this.showPersonalization);
            $(window).on('closeEditPersonalization', this.closeEditPersonalization);
        }

        toolsViewModel.prototype.getIsVisibleComputed = function getIsVisibleComputed() {
            return ko.pureComputed(function() {
                if (this.selectedElement()) {
                    return true
                }
                this.personalizing(false);
                return false
            }, this).extend({throttle: 50})
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
                this.window_width();
                this.window_height();
                if (this.selectedElement()) {
                    return this.calcAttachment();
                }
                return {}
            }, this).extend({throttle: 50})
        }

        toolsViewModel.prototype.handleResize = function handleResize() {
            //this.window_width($('.canvas-container')[0].scrollWidth)
            //this.window_height($('.canvas-container')[0].scrollHeight)
            this.window_width($(window).width());
            this.window_height($(window).height());
            this.window_width.valueHasMutated();
        }

        toolsViewModel.prototype.calcAttachment = function calcAttachment() {
            var coords = this.selectedElement().getCoords(),
                right_margin = this.window_width() - coords.right,
                bottom_margin = this.window_height() - coords.bottom,
                attachment = {},
                container = $('.canvas-container'),
                scrollHeight = container[0].scrollHeight,
                scrollWidth = container[0].scrollWidth,
                scrollTop = container.scrollTop(),
                scrollLeft = container.scrollLeft();

            // work out where the largest space is
            var maxV = Math.max(coords.top, bottom_margin),
                maxH = Math.max(coords.left, right_margin),
                max = Math.max(maxV, maxH),
                tools_height = $('.tools').height() + 20,
                tools_width = $('.tools').width() + 20;


            if (maxV < tools_height && maxH < tools_width) {
                // not enough room for the tools, just place them in the top left
                return {
                    left: 50,
                    top: 100,
                    right: 'auto',
                    bottom: 'auto'
                }
            }

            // deal with horizontal case
            if (maxH > tools_width) {
                // have enough horizontal room
                if (maxH == coords.left) {
                    // attach to the left
                    attachment.right = this.window_width() - coords.left - scrollLeft;
                    attachment.left = 'auto';
                } else {
                    // attach to the right
                    attachment.left = coords.right + scrollLeft;
                    attachment.right = 'auto';
                }
                if (maxV > tools_height) {
                    if (maxV == coords.top) {
                        attachment.bottom = Math.min((this.window_height() - coords.bottom)-scrollTop, scrollHeight - 100);
                        attachment.top = 'auto';
                    } else {
                        attachment.top = Math.max(coords.top + scrollTop, scrollTop + 100);
                        attachment.bottom = 'auto'
                    }
                } else {
                    attachment.top = Math.max(coords.top + scrollTop, scrollTop + 100);
                    attachment.bottom = 'auto';
                }
                return attachment
            } else {
                // attach the left of the tools to either the left of the element or the screen edge
                attachment.left = Math.max(coords.left + scrollLeft, scrollLeft + 10);
                attachment.right = 'auto';
            }

            if (maxV > tools_height) {
                // have enough vertical room
                if (maxV == coords.top) {
                    // attach to the top
                    attachment.bottom = (this.window_height() - coords.top) - scrollTop  + 10;
                    attachment.top = 'auto';
                } else {
                    // attach to the bottom
                    attachment.top = coords.bottom + 10 + scrollTop;
                    attachment.bottom = 'auto';
                }
                return attachment
            } else {
                // attach the top to the top of the element or the edge of the screen
                attachment.top = Math.max((this.window_height() - coords.top) - scrollTop + 10, scrollTop + 100);
                attachment.bottom = 'auto'
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
        toolsViewModel.prototype.fitFontSize = function fitFontSize() {
            if (this.selectedElement()) {
                this.selectedElement().fitFontSize();
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

        toolsViewModel.prototype.orderedList = function orderedList() {
            if (this.selectedElement()) {
                this.selectedElement().orderedList();
            }
        }

        toolsViewModel.prototype.unorderedList = function unorderedList() {
            if (this.selectedElement()) {
                this.selectedElement().unorderedList();
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

        toolsViewModel.prototype.toggleImageLibrary = function toggleImageLibrary() {
            stateViewModel.toggleImageLibrary();
        }

        toolsViewModel.prototype.togglePersonalization = function togglePersonalization() {
            this.personalizing(!this.personalizing());
        }

        toolsViewModel.prototype.closePersonalization = function closePersonalization() {
            this.personalizing(false);
            this.personalizationField('FirstName');
            this.personalizationFallback('');
            this.personalizationEl(null);
        }

        toolsViewModel.prototype.closeEditPersonalization = function closeEditPersonalization() {
            if (this.personalizationEl()) {
                this.closePersonalization();
            }
        }

        toolsViewModel.prototype.showPersonalization = function showPersonalization(e, target, field, fallback) {
            this.personalizing(true);
            this.personalizationField(field);
            this.personalizationFallback(fallback);
            this.personalizationEl(target);
        }

        toolsViewModel.prototype.getCaretPosition = function getCaretPosition(element) {
            var caretOffset = 0,
                range = window.getSelection().getRangeAt(0),
                preCaretRange = range.cloneRange();
            preCaretRange.selectNodeContents(element);
            preCaretRange.setEnd(range.endContainer, range.endOffset);
            caretOffset = preCaretRange.toString().length;
            return caretOffset;
        }

        toolsViewModel.prototype.getTextNodes = function getTextNodes(node) {
            var textNodes = [];
            if (node.nodeType == 3) {
                textNodes.push(node);
            } else {
                var childNodes = node.childNodes;
                for (var i = 0, len = childNodes.length; i < len; ++i) {
                    textNodes.push.apply(textNodes, getTextNodes(childNodes[i]));
                }
            }
            return textNodes;
        }

        toolsViewModel.prototype.setCaretPosition = function setCaretPosition(data, e) {
            var el = stateViewModel.selectedElement().element(),
                relatedTarget = e.relatedTarget;
            if (!el) {
                return
            }
            el = el[0];
            if (relatedTarget && el != relatedTarget && relatedTarget.hasClass('editable')) {
                return
            }
            var range = document.createRange(),
                sel = window.getSelection(),
                childNodes = this.getTextNodes(el),
                nodeIdx = 0,
                caretPosition = this.caretPosition;
            
            while (nodeIdx < childNodes.length && caretPosition > $(childNodes[nodeIdx]).text().length) {
                caretPosition -= $(childNodes[nodeIdx]).text().length;
                nodeIdx += 1;
            }

            var node = childNodes[nodeIdx];
            range.setStart(node, caretPosition);
            range.collapse(true);
            sel.removeAllRanges();
            sel.addRange(range);
            el.focus();
            this.caretPosition = 0;
        }

        toolsViewModel.prototype.focusToolsInput = function focusToolsInput(data, e) {
            var el = stateViewModel.selectedElement().element();
            if (el) {
                var caretPosition = this.getCaretPosition(el[0]);
                if (caretPosition) {
                    this.caretPosition = caretPosition;
                }
            }
            e.target.focus();
        }

        toolsViewModel.prototype.insertPersonalization = function insertPersonalization() {
            setTimeout(function() {
                var field = this.personalizationField(),
                    fallback = this.personalizationFallback(),
                    isIE = this.isIE(),
                    content = (fallback != '') ? '['+ field + '/' + fallback +']' : '['+ field + ']';
                if (isIE){
                    var innerHTML = '<span contenteditable="false" class="dynamic-field-content" data-content="'+ content +'" data-field="'+ field +'" data-fallback="'+ fallback +'"></span>',
                        html = '<span contenteditable="false" class="dynamic-field" class="editable">' + innerHTML + '</span>&#8202;'
                } else {
                    var innerHTML = '<span class="dynamic-field-content" data-content="'+ content +'" data-field="'+ field +'" data-fallback="'+ fallback +'"></span>',
                        html = '<span contenteditable="true" class="dynamic-field" class="editable">' + innerHTML + '</span>&#8202;'
                }

                if (this.personalizationEl()) {
                    $(this.personalizationEl()).replaceWith(innerHTML);
                } else {
                    if (document.queryCommandSupported('insertHTML')) {
                        document.execCommand('insertHTML', false, html);
                    } else {
                        var sel = window.getSelection();
                        if (sel.getRangeAt && sel.rangeCount) {
                            range = sel.getRangeAt(0);
                            range.deleteContents();

                            var el = document.createElement("div");
                            el.innerHTML = html;
                            var frag = document.createDocumentFragment(), node, lastNode;
                            while ( (node = el.firstChild) ) {
                                lastNode = frag.appendChild(node);
                            }
                            var firstNode = frag.firstChild;
                            range.insertNode(frag);

                            if (lastNode) {
                                range = range.cloneRange();
                                range.setStartAfter(lastNode);
                                range.collapse(true);
                                sel.removeAllRanges();
                                sel.addRange(range);
                            }
                        }
                    }
                }

                var el = this.selectedElement();
                el.sizeAdjust();
                el.userData.content(el.element().html());
                this.closePersonalization();
            }.bind(this), 0)
        }

        toolsViewModel.prototype.deletePersonalization = function deletePersonalization() {
            if (this.personalizationEl()) {
                $(this.personalizationEl()).remove();
                this.closePersonalization();
            }
        }

        toolsViewModel.prototype.isIE = function isIE() {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
                return true
            } else {
                return false
            }

            return false;
        }

        return {
            viewModel: toolsViewModel,
            template: { require: 'text!/canvas/templates/tools.html' }
        }
});