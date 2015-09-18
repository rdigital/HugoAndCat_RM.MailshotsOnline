define(['knockout', 'view_models/element', 'view_models/theme', 'view_models/user', 'view_models/state'],

    function(ko, elementViewModel, themeViewModel, userViewModel, stateViewModel) {

        function imageViewModel(params) {

            this.data = params.data;
            this.preview = params.preview;
            this.isSelected = ko.observable(false);
            this.element = ko.observable();
            this.canvas = ko.observable();
            this.imageObj = this.getImageObj();
            this.subscriptions = [];

            // if theme override passed in, process it
            this.override_theme = params.override_theme;
            this.override_template = params.override_template;
            this.handleOverrides();

            // image positioning variables
            this.dragging = false;
            this.offsetX = 0;
            this.offsetY = 0;
            this.image = null;
            this.old_scale = 0;
            this.adjusted_width = 0;
            this.adjusted_height = 0;

            // computeds
            this.flatStyles = this.getFlatStyles();
            this.message = this.getMessageComputed();
            this.isEmpty = this.getIsEmptyComputed();

            // subscriptions
            this.handleSubscriptions();

            // bound functions
            this.dispose = this.dispose.bind(this);
            this.setup = this.setup.bind(this);
        }

        // extend the element model
        imageViewModel.prototype = Object.create(elementViewModel.prototype);
        imageViewModel.prototype.constructor = imageViewModel;

        /**
         * setup image element specific subscriptions, called after render
         */
        imageViewModel.prototype.setup = function setup() {
            this.imageObj = this.getImageObj();
            this.subscriptions.push(
                this.imageObj.scale.subscribe(this.setOldScale, this, 'beforeChange')
            );
            this.subscriptions.push(
                this.imageObj.scale.subscribe(this.redrawScale, this)
            );

            // if there is no image element created yet, but we have a src url
            // trigger a render
            if (!this.image && this.imageObj.src) {
                this.rerender();
                stateViewModel.repositionImages = false;
            }

            // on deselect, export the canvas to base64 string and store on the imageObj
            this.subscriptions.push(
                this.isSelected.subscribe(function(selected) {
                    if (!selected && this.image && this.image.src) {
                        this.setContent();
                    }
                }, this)
            )

            if (this.preview) {
                this.subscriptions.push(
                    this.imageObj.src.subscribe(function(src) {
                        this.render(src, false);
                    }, this)
                );
                this.subscriptions.push(
                    this.imageObj.img_position.left.subscribe(function() {
                        this.rerender();
                    }, this)
                );
                this.subscriptions.push(
                    this.imageObj.img_position.top.subscribe(function() {
                        this.rerender();
                    }, this)
                );
                this.subscriptions.push(
                    this.imageObj.scale.subscribe(function() {
                        this.rerender();
                    }, this)
                );
            }
        }

        /**
         * select the image element
         */
        imageViewModel.prototype.select = function select() {
            this.isSelected(true);
        }

        /**
         * returns a computed which contains image data (source, position, scale) from
         * theme and user data combined.
         * @return {ko.pureComputed} [contains image data in object]
         */
        imageViewModel.prototype.getImageObj = function getImageObj() {
            var image = { img_position: {} },
                themeImage = themeViewModel.getImageByName(this.data.theme_class),
                userImage = userViewModel.getOrSetImageByName(this.data.name);

            // content is only defined on the user data
            image.content = userImage.content;

            // if we are overriding the template, return new observables containing the values from
            // userdata, so as to not be updating the user view model
            if (this.override_template) {
                image.src = ko.observable(ko.utils.unwrapObservable(userImage.src));
                image.scale = ko.observable(ko.utils.unwrapObservable(userImage.scale));
                image.img_position.top = ko.observable(ko.utils.unwrapObservable(userImage.img_position.top));
                image.img_position.left = ko.observable(ko.utils.unwrapObservable(userImage.img_position.left));
                return image
            }

            // set the required properties on the image based on a combination of
            // theme and user data
            image.src = this.getSrcComputed(themeImage, userImage)
            image.scale = this.getScaleComputed(themeImage, userImage)
            image.img_position.top = this.getTopComputed(themeImage, userImage)
            image.img_position.left = this.getLeftComputed(themeImage, userImage)

            // forcibly reset image position / scale to that of the theme if user
            // has not provided their own image
            var src = ko.utils.unwrapObservable(userImage.src)
            if (!src) {
                image.scale(themeImage.scale || 100)
                image.img_position.top(themeImage.img_position.top || 0)
                image.img_position.left(themeImage.img_position.left || 0)
            }

            return image;
        }

        imageViewModel.prototype.getSrcComputed = function getSrcComputed(themeImage, userImage) {
            return ko.pureComputed({
                read: function() {
                    var src = ko.utils.unwrapObservable(userImage.src);
                    return (src === null) ? (themeImage.src || '') : src
                },
                write: function(new_val) {
                    userImage.src(new_val)
                }
            })
        }

        imageViewModel.prototype.getScaleComputed = function getScaleComputed(themeImage, userImage) {
            return ko.pureComputed({
                read: function() {
                    var scale = ko.utils.unwrapObservable(userImage.scale);
                    return (scale === null) ? (themeImage.scale || 100) : scale
                },
                write: function(new_val) {
                    userImage.scale(new_val)
                }
            })
        }

        imageViewModel.prototype.getTopComputed = function getTopComputed(themeImage, userImage) {
            return ko.pureComputed({
                read: function() {
                    var top = ko.utils.unwrapObservable(userImage.img_position.top);
                    return (top === null) ? (themeImage.img_position.top || 0) : top
                },
                write: function(new_val) {
                    userImage.img_position.top(new_val)
                }
            })
        }

        imageViewModel.prototype.getLeftComputed = function getLeftComputed(themeImage, userImage) {
            return ko.pureComputed({
                read: function() {
                    var left = ko.utils.unwrapObservable(userImage.img_position.left);
                    return (left === null) ? (themeImage.img_position.left || 0) : left
                },
                write: function(new_val) {
                    userImage.img_position.left(new_val)
                }
            })
        }

        /**
         * this is the factor by which to scale the canvas compared to the size at which it is
         * displayed. This results in higher resolution exports.
         * @type {Number}
         */
        imageViewModel.prototype.scaleFactor = 3;

        /**
         * set the old_scale instance variable before changing the scale
         * @param {Integer} old_scale [the previous scale value ]
         */
        imageViewModel.prototype.setOldScale = function setOldScale(old_scale) {
            this.old_scale = old_scale;
        }

        /**
         * redraw the image at the provided scale
         * @param  {Integer} new_scale [new scale (0 - 400) to render at]
         */
        imageViewModel.prototype.redrawScale = function redrawScale(new_scale) {
            // temp, fix subscription issue
            if (!this.isSelected()) {
                return
            };

            var scale_diff = (new_scale - this.old_scale) / 100;

            // nudge coordinates of top left by half of the adjustment made on each 
            // axis on the image (resizes around center instead of top left)

            var left_diff = (scale_diff * this.adjusted_width / 2) / this.scaleFactor;
            this.imageObj.img_position.left(this.imageObj.img_position.left() - left_diff );

            var top_diff = (scale_diff * this.adjusted_height / 2) / this.scaleFactor;
            this.imageObj.img_position.top(this.imageObj.img_position.top() - top_diff );
            this.rerender();
        }

        /**
         * drag event handler, moves the image in line with mouse movement
         */
        imageViewModel.prototype.dragMove = function dragMove(data, e) {
            if (!this.dragging) {
                return
            }
            var diffX = e.offsetX - this.offsetX,
                diffY = e.offsetY - this.offsetY;
            this.offsetX = e.offsetX;
            this.offsetY = e.offsetY;

            this.imageObj.img_position.left(this.imageObj.img_position.left() + diffX);
            this.imageObj.img_position.top(this.imageObj.img_position.top() + diffY);
            this.rerender()
        }

        /**
         * sets dragging to true on initial mousedown
         */
        imageViewModel.prototype.dragStart = function dragStart(data, e) {
            if (!this.image) {
                return
            }
            this.offsetX = e.offsetX;
            this.offsetY = e.offsetY;
            this.dragging = true;
        }

        /**
         * sets dragging to false on mouseout or mouseup
         */
        imageViewModel.prototype.dragEnd = function dragEnd() {
            this.dragging = false;
        }

        /**
         * render an image to the canvas
         * @param  {String} src    [image src]
         * @param  {Element} canvas [canvas HTML element]
         */
        imageViewModel.prototype.render = function render(src, new_upload) {
            this.image = new Image()
            //this.image.crossOrigin = "Anonymous";
            var canvas = this.canvas();

            this.image.onload = function(){
                if (new_upload) {
                    var canvas_width = canvas.width(),
                        canvas_height = canvas.height();

                    // work out initial alignment for image
                    var width_factor = canvas_width / this.image.width,
                        height_factor = canvas_height / this.image.height,
                        base_scale = Math.max(width_factor, height_factor),
                        user_scale = (this.imageObj.scale() || 100) / 100,
                        width_offset = (canvas_width - (this.image.width * base_scale * user_scale)) / 2,
                        height_offset = (canvas_height - (this.image.height * base_scale * user_scale)) / 2;

                    this.imageObj.img_position.left(width_offset);
                    this.imageObj.img_position.top(height_offset);
                }
                this.rerender()
            }.bind(this);
            
            this.image.src = src;
            if (new_upload) {
                this.imageObj.src(src);
                this.imageObj.scale(100);
            }
        };

        /**
         * rerender an image which has already been drawn to the canvas
         */
        imageViewModel.prototype.rerender = function rerender() {
            if (this.imageObj.src && this.imageObj.src()) {

                if (!this.image) {
                    if (this.override_template || stateViewModel.repositionImages) {
                        return this.render(this.imageObj.src(), true);
                    }
                    return this.render(this.imageObj.src(), false);
                }
                var canvas = this.canvas(),
                    ctx = canvas[0].getContext("2d"),
                    canvas_width = canvas.width(),
                    canvas_height = canvas.height()

                // clear the canvas
                //ctx.clearRect(0, 0, canvas_width * this.scaleFactor, canvas_height * this.scaleFactor);
                ctx.fillStyle = "white";
                ctx.fillRect(0, 0, canvas_width * this.scaleFactor, canvas_height * this.scaleFactor);

                // work out scale to apply to image
                var width_factor = canvas_width / this.image.width,
                    height_factor = canvas_height / this.image.height,
                    base_scale = Math.max(width_factor, height_factor),
                    user_scale = (this.imageObj.scale() || 100) / 100,
                    width = this.image.width * base_scale * user_scale * this.scaleFactor,
                    height = this.image.height * base_scale * user_scale * this.scaleFactor

                this.adjusted_width = this.image.width * base_scale * this.scaleFactor;
                this.adjusted_height = this.image.height * base_scale * this.scaleFactor;

                // draw image to the canvas
                ctx.drawImage(
                    this.image,
                    0,
                    0,
                    this.image.width,
                    this.image.height,
                    this.imageObj.img_position.left() * this.scaleFactor,
                    this.imageObj.img_position.top() * this.scaleFactor,
                    width,
                    height
                );

            }
        }

        /**
         * set image object content observable to equal base64 dump of canvas
         */
        imageViewModel.prototype.setContent = function setContent() {
            if (this.imageObj.src) {
                this.imageObj.content(this.exportCanvas());
            }
        }

        /**
         * export the canvas to png
         * @return {String} base 64 encoded canvas export
         */
        imageViewModel.prototype.exportCanvas = function() {
            return this.canvas() ? this.canvas()[0].toDataURL(): '';
        }

        /**
         * get X and Y dimensions of the canvas.
         * @return {Object} width and height
         */
        imageViewModel.prototype.getDimensions = function getDimensions() {
            var width = ko.utils.arrayFirst(this.data.layout, function(style) {
                return style.property == 'width';
            });
            var height = ko.utils.arrayFirst(this.data.layout, function(style) {
                return style.property == 'height';
            });
            return {
                width: (width) ? width.value : '100%',
                height: (height) ? height.value : '100%'
            }
        }

        /**
         * get X and Y dimensions of the canvas, then multiply by a scale
         * factor to render canvas exports at higher res, whilst still displaying
         * them at the same size in the editor.
         * @return {Object} width and height
         */
        imageViewModel.prototype.getScaledDimensions = function getScaledDimensions() {
            var dimensions = this.getDimensions();

            var width = parseInt(dimensions.width, 10),
                widthScaled = width * this.scaleFactor,
                widthUnits = dimensions.width.replace(width, ''),
                widthOutput = widthScaled + widthUnits;

            var height = parseInt(dimensions.height, 10),
                heightScaled = height * this.scaleFactor,
                heightUnits = dimensions.height.replace(height, ''),
                heightOutput = heightScaled + heightUnits;

            return {
                width: widthOutput,
                height: heightOutput
            }
        }

        /**
         * Returns flattened CSS styles with layout (width / height) also included
         * we include dimensions of layout in css, so that canvas size can be larger than it's
         * display size. This allows canvases to be exported at higher resolutions
         * @return {ko.pureComputed} computed of styles with dimensions
         */
        imageViewModel.prototype.flatStylesWithDimensions = function flatStylesWithDimensions() {
            //
            //
            return ko.pureComputed( function() {
                var styles = this.flatStyles();
                ko.utils.extend(styles, this.getDimensions())
                return styles
            }, this)
        }

        imageViewModel.prototype.getMessageComputed = function getMessageComputed() {
            return ko.pureComputed(function() {
                if (this.data.message) {
                    return {
                        type: 'message',
                        message: this.data.message
                    }
                } else {
                    return null
                }
            }, this)
        }

        imageViewModel.prototype.getIsEmptyComputed = function getIsEmptyComputed() {
            return ko.pureComputed(function() {
                return (this.imageObj && this.imageObj.src) ? !(this.imageObj.src()) : false
            }, this).extend({throttle: 100})
        }

        return {
            viewModel: imageViewModel,
            template: { require: 'text!/canvas/templates/image.html' }
        }
    }
);