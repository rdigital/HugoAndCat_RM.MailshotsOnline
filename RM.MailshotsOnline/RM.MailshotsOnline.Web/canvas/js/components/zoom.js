define(['knockout', 'jquery', 'view_models/state'],

    function(ko, $, stateViewModel) {

        // ViewModel
        function zoomComponentViewModel() {
            this.zoom = stateViewModel.zoom;
            this.overrideZoom = stateViewModel.overrideZoom;
            this.scaleElement = stateViewModel.scaleElement;
            this.viewingSide = stateViewModel.viewingSide;
            this.availableZooms = [0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5];

            // bound functions
            this.handleScale = this.handleScale.bind(this);
            this.toggleZoom = this.toggleZoom.bind(this);

            // subscriptions
            this.handleSubscriptions();

            setTimeout(function() {
                this.handleScale();
            }.bind(this), 100)
        }

        zoomComponentViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.scaleElement.subscribe(this.handleScale, this);
            $(window).resize(this.handleScale);
            // chrome blurry font rendering big hack
            this.zoom.subscribe(function() {
                $('canvas').hide();
                setTimeout(function() {
                    $('canvas').show();
                }, 0)
            })
            this.overrideZoom.subscribe(function() {
                $('canvas').hide();
                setTimeout(function() {
                    $('canvas').show();
                }, 0)
            })
        }

        zoomComponentViewModel.prototype.handleScale = function handleScale() {
            var el = this.scaleElement();
            if (!el) {
                return
            }
            setTimeout(function () {
                var window_height = $('.canvas-container').height() - 100,
                window_width = $('.canvas-container').width() - 100,
                width_factor = (Math.floor((window_width / el.width())*4))/4,
                height_factor = (Math.floor((window_height / el.height())*4))/4;

                this.zoom(Math.min(width_factor, height_factor));
            }.bind(this), 100)
            
        }

        zoomComponentViewModel.prototype.toggleZoom = function toggleZoom() {
            if (this.overrideZoom()) {
                this.overrideZoom(null);
            } else {
                this.overrideZoom(this.availableZooms[this.availableZooms.length-1])
            }
        }

        zoomComponentViewModel.prototype.getZoomAvailableComputed = function getZoomAvailableComputed() {
            return this.zoom() != this.availableZooms[this.availableZooms.length-1]
        }

        zoomComponentViewModel.prototype.increaseZoom = function increaseZoom() {
            var idx = this.availableZooms.indexOf(this.zoom());
            if (idx < this.availableZooms.length -1) {
                this.zoom(this.availableZooms[idx+1]);
                return true
            }
            return false
        }

        zoomComponentViewModel.prototype.decreaseZoom = function decreaseZoom() {
            var idx = this.availableZooms.indexOf(this.zoom());
            if (idx >0) {
                this.zoom(this.availableZooms[idx-1]);
                return true
            }
            return false
        }

        return {
            viewModel: zoomComponentViewModel,
            template: { require: 'text!/canvas/templates/zoom.html' }
        }
});