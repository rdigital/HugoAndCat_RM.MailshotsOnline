define(['knockout', 'jquery', 'koeditable', 'koelement', 'view_models/element', 'view_models/user', 'view_models/history'],

    function(ko, $, koeditable, koelement, elementViewModel, userViewModel, historyViewModel) {

        function inputViewModel(params) {
            // tracking for whether element is selected
            this.isSelected = ko.observable(false);
            this.isSelectedSetOnly = this.getIsSelectedSetOnlyComputed();
            this.subscriptions = [];

            // instance variables
            this.element = ko.observable();
            this.data = params.data;
            this.preview = params.preview;
            this.colorOnly = params.colorOnly;
            this.isBold = ko.observable(false);
            this.isUnderline = ko.observable(false);
            this.isItalic = ko.observable(false);
            this.currentFontSize = ko.observable(1);
            this.fallbackBackground = ko.observable(null);

            // if theme override passed in, process it
            this.override_theme = params.override_theme;
            this.override_template = params.override_template;
            this.handleOverrides();

            this.userData = userViewModel.getOrSetElementByName(this.data.name);
            this.handleSubscriptions();

            this.flatStyles = this.getFlatStyles();

            // scroll visible computed
            this.isScrollVisible = this.getScrollVisibleComputed();
            this.message = this.getMessageComputed();
            this.title = this.getTitleComputed();

            // bound functions
            this.dispose = this.dispose.bind(this);
            this.sizeAdjust = this.sizeAdjust.bind(this);
            this.sizeAdjustDelayed = this.sizeAdjustDelayed.bind(this);
            this.sizeAdjustPreview = this.sizeAdjustPreview.bind(this);
            this.verticalAlignMiddle = this.verticalAlignMiddle.bind(this);
        }

        // extend the element view model
        inputViewModel.prototype = Object.create(elementViewModel.prototype);
        inputViewModel.prototype.constructor = inputViewModel;

        inputViewModel.prototype.select = function select() {
            this.isSelected(true);
        }

        /**
         * sets focus to the element
         */
        inputViewModel.prototype.setFocus = function setFocus() {
            if (this.element()) {
                this.element().focus();
                this.sizeAdjust();
            }
        };

        /**
         * call from knockout click binding to set focus to contenteditable div from parent container
         * this is useful when the contenteditable div is smaller than its container
         */
        inputViewModel.prototype.clickSetFocus = function clickSetFocus(data, e) {
            this.fallbackBackground(null);
            var target = $(e.target);
            if (target.hasClass('dynamic-field-content') || target.hasClass('dynamic-field-nohighlight')) {
                // the personalization elements are inserted outside of the framework, so cannot have bound functions
                // on them. Simplest way to handle the click is by firing an event and listening for it in the tools component
                $(window).trigger('personalization', [target, target.attr('data-field'), target.attr('data-fallback')]);
            } else {
                $(window).trigger('closeEditPersonalization');
            }

            this.getBackgroundColour(e);

            this.setFocus();
            this.getState();
        };

        /**
         * executes bold command on the document
         */
        inputViewModel.prototype.bold = function bold() {
            document.execCommand("Bold", false, null);
            this.getState();
            this.sizeAdjust();
        };

        /**
         * executes underline command on the document
         */
        inputViewModel.prototype.underline = function underline() {
            document.execCommand("Underline", false, null);
            this.getState();
        };

        /**
         * executes italic command on the document
         */
        inputViewModel.prototype.italic = function italic() {
            document.execCommand("Italic", false, null);
            this.getState();
        };

        inputViewModel.prototype.orderedList = function orderedList() {
            document.execCommand("insertOrderedList", false, null);
            this.sizeAdjust();
        };

        inputViewModel.prototype.unorderedList = function unorderedList() {
            document.execCommand("insertUnorderedList", false, null);
            this.sizeAdjust();
        };

        /**
         * checks the current state at the caret and triggers automatic font resizing on key up
         */
        inputViewModel.prototype.handleKeyup = function handleKeyup() {
            this.getState();
            this.sizeAdjust();
        };

        /**
         * gets the current inline styles (bold, italic, underline) applied at the caret / selection
         * @return {Boolean} return true to continue event propagation
         */
        inputViewModel.prototype.getState = function getState() {
            var bold = document.queryCommandState("bold"),
                italic = document.queryCommandState("italic"),
                underline = document.queryCommandState("underline");
            this.isBold(bold);
            this.isUnderline(underline);
            this.isItalic(italic);
            return true;
        };

        /**
         * Call to automatically adjust font size downward if the element has overflown
         */
        inputViewModel.prototype.sizeAdjust = function sizeAdjust() {
            console.log('adjust')
            if (this.getVerticalMiddle()) {
                this.setStyle('padding-top', '0');
            }
            var scrollVisible = this.scrollVisible();
            if (scrollVisible) {
                while (scrollVisible) {
                    scrollVisible = this.decreaseFontSize();
                }
            }
            if (this.getVerticalMiddle()) {
                this.verticalAlignMiddle();
            }
        };

        /**
         * this is kinda lame, but not much else we can do
         * after rendering the template, call size adjust a few times on different delays
         * to help account for fonts loading in etc
         */
        inputViewModel.prototype.sizeAdjustDelayed = function sizeAdjustDelayed() {
            this.sizeAdjust();
            //this.decreaseFontSize();
            setTimeout(this.sizeAdjust, 500);
            setTimeout(this.sizeAdjust, 1000);
            setTimeout(this.sizeAdjust, 2000);
            //setTimeout(this.sizeAdjust, 4000);
        }

        /**
         * use padding to vertically align element
         * this will only work if the theme has no padding on this element
         * @return {[type]} [description]
         */
        inputViewModel.prototype.verticalAlignMiddle = function verticalAlignMiddle() {
            var el = this.element(),
                el_height = el.innerHeight(),
                container_height = el.closest('.component').children().innerHeight(),
                margin = (container_height - el_height) / 2;
            if (el_height) {
                this.setStyle('padding-top', margin + 'px');
            }
        }

        /**
         * inline version of verical align function, used for previews
         * does not modify view model
         */
        inputViewModel.prototype.verticalAlignMiddleInline = function verticalAlignMiddleInline() {
            var el = this.element(),
                el_height = el.innerHeight(),
                container_height = el.closest('.component').children().innerHeight(),
                margin = (container_height - el_height) / 2;
            this.element().closest('.editable').css('padding-top', margin + 'px');
        }

        inputViewModel.prototype.fitFontSize = function fitFontSize() {
            var font_sizes = this.getFontSizes(),
                idx = font_sizes.length-1;
            this.setStyle('font-size', font_sizes[idx]);

            var scrollVisible = this.scrollVisible();
            while (scrollVisible) {
                scrollVisible = this.decreaseFontSize();
            }
            if (this.getVerticalMiddle()) {
                this.verticalAlignMiddle();
            }
        };

        /**
         * if available, bumps the font size up to the next available size for the theme applied to this element
         * @return {Boolean} will be false when no more sizes are available (max size reached), otherwise true
         */
        inputViewModel.prototype.increaseFontSize = function increaseFontSize() {
            var font_sizes = this.getFontSizes(),
                font_size = this.getStyle('font-size'),
                index = font_sizes.indexOf(font_size || 0);

            if (index < font_sizes.length-1) {
                this.setStyle('font-size', font_sizes[index+1]);
                if (this.getVerticalMiddle()) {
                    this.verticalAlignMiddle();
                }
                return true;
            }
            //console.log('no more sizes');
            return false;
        };

        /**
         * if available, drops the font size down to the previous available size for the theme applied to this element
         * @return {Boolean} will be false when no more sizes are available (min size reached), otherwise true
         */
        inputViewModel.prototype.decreaseFontSize = function decreaseFontSize() {
            var font_sizes = this.getFontSizes(),
                font_size = this.getStyle('font-size'),
                index = font_sizes.indexOf(font_size || 0);
            if (index > 0) {
                this.setStyle('font-size', font_sizes[index-1]);
                if (this.getVerticalMiddle()) {
                    this.verticalAlignMiddle();
                }
                return this.scrollVisible();
            }
            //console.log('no more sizes');
            return false;
        };

        /**
         * Check whether the current text area has overflown
         * @return {Boolean} true if text area has overflow, otherwise false
         */
        inputViewModel.prototype.scrollVisible = function scrollVisible() {
            var el = this.element();
            if (el) {
                el = el[0];
                if (el.scrollHeight > el.clientHeight) {
                    return true;
                }
            }
            return false;
        };

        /**
         * version of sizeAdjustPreview for template / theme previews. Does not effect the underlying user viewmodel
         * instead works directly on the element
         * Automatically downsizes the font if text has overflown. Small timeout used as it seems the browser needs time after
         * render event is triggered to actually fully render the text
         */
        inputViewModel.prototype.sizeAdjustPreview = function sizeAdjustPreview() {
            setTimeout( function() {
                var scrollVisible = this.scrollVisible();
                if (scrollVisible) {
                    while (scrollVisible) {
                        scrollVisible = this.decreaseFontSizeInline();
                    }
                }
                if (this.getVerticalMiddle()) {
                    this.verticalAlignMiddleInline();
                }
            }.bind(this), 100);
        };

        /**
         * version of decreaseFontSize for template / theme previews. Does not effect the underlying user viewmodel
         * instead works directly on the element
         * if available, drops the font size down to the previous available size for the theme applied to this element
         * @return {Boolean} will be false when no more sizes are available (min size reached), otherwise true
         */
        inputViewModel.prototype.decreaseFontSizeInline = function decreaseFontSizeInline() {
            var font_sizes = this.getFontSizes(),
                font_size = this.element().css('font-size') || this.getStyle('font-size'),
                index = font_sizes.indexOf(font_size || 0);
            if (index > 0) {
                this.element().css('font-size', font_sizes[index-1]);
                return this.scrollVisible();
            }
            //console.log('no more sizes');
            return false;
        };

        /**
         * returns a computed which evaluates to a boolean. True if element is overflowing
         * @return {ko.pureComputed}
         */
        inputViewModel.prototype.getScrollVisibleComputed = function getScrollVisibleComputed() {
            return ko.pureComputed(function() {
                // subscribe to changes on content, font size and font family
                var content = this.userData.content(),
                    font_size = userViewModel.getStyleObs('font-size', this.data.name),
                    font_family = userViewModel.getStyleObs('font-family', this.data.name);
                if (font_size) {
                    font_size();
                }
                if (font_family) {
                    font_family();
                }
                return this.scrollVisible();
            }, this).extend({throttle: 600});
        };

        /**
         * returns a computed that only allows setting the value of isSelected to true. This allows us to maintain
         * focus on an element when clicking controls whilst still using knockout's hasFocus binding
         * @return {ko.pureComputed}
         */
        inputViewModel.prototype.getIsSelectedSetOnlyComputed = function getIsSelectedSetOnlyComputed() {
            return ko.pureComputed({
                read: function() {
                    return this.isSelected();
                },
                write: function(val) {
                    if (val) {
                        this.isSelected(val);
                    }
                }
            }, this);
        };

        /**
         * hook onto paste events in the content editable div. This cancels the paste event and inserts the text
         * representation of the clipboard contents instead
         */
        inputViewModel.prototype.plainTextPaste = function plainTextPaste(data, e) {
            e.preventDefault();

            var text = e.originalEvent.clipboardData.getData("text/plain");
            document.execCommand("insertHTML", false, text);
            this.handleKeyup();
        };

        //return
        return {
            viewModel: inputViewModel,
            template: { require: 'text!/canvas/templates/input.html' }
        };
    }
);