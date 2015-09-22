define(['knockout', 'jquery', 'view_models/state'],

    function(ko, $, stateViewModel) {

        // ViewModel
        function zoomComponentViewModel() {
            this.zoom = stateViewModel.zoom;
            this.overrideZoom = stateViewModel.overrideZoom;
            this.fitToWidth = stateViewModel.fitToWidth;
            this.scaleElement = stateViewModel.scaleElement;
            this.viewingSide = stateViewModel.viewingSide;
            this.availableZooms = [0.125, 0.25, 0.375, 
                                   0.5, 0.625, 0.75, 
                                   0.875, 1, 1.125, 
                                   1.25, 1.375, 1.5, 
                                   1.625, 1.75, 1.875, 
                                   2, 2.125, 2.25,
                                   2.375, 2.5];

            // bound functions
            this.handleScale = this.handleScale.bind(this);
            this.toggleZoom = this.toggleZoom.bind(this);

            // subscriptions
            this.handleSubscriptions();
        }

        zoomComponentViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.scaleElement.subscribe(this.handleScale, this);
            this.fitToWidth.subscribe(this.handleScale, this);
            $(window).resize(this.handleScale);
            // chrome blurry font rendering big hack
            this.zoom.subscribe(function() {
                $('.canvas-container canvas').hide();
                setTimeout(function() {
                    $('.canvas-container canvas').show();
                }, 20);
            });
            this.overrideZoom.subscribe(function() {
                $('.canvas-container canvas').hide();
                setTimeout(function() {
                    $('.canvas-container canvas').show();
                }, 20);
            });
        };

        zoomComponentViewModel.prototype.handleScale = function handleScale() {
            var el = this.scaleElement();
            if (!el) {
                return;
            }

            var window_height = $('.canvas-container').height() - 120,
                window_width = $('.canvas-container').width() - 150;

            if (stateViewModel.selectedElement() || stateViewModel.backgroundSelected()) {
                window_height -= 50;
                window_width -= 150;
            }

            var width_factor = (Math.floor((window_width / stateViewModel.viewingFace().width)*16))/16,
                height_factor = (Math.floor((window_height / stateViewModel.viewingFace().height)*16))/16;
            
            if (this.fitToWidth()) {
                this.zoom(width_factor);
            } else {
                this.zoom(Math.min(width_factor, height_factor));
            }
            
        };

        zoomComponentViewModel.prototype.toggleZoom = function toggleZoom() {
            if (this.overrideZoom()) {
                this.overrideZoom(null);
            } else {
                this.overrideZoom(this.availableZooms[this.availableZooms.length-1]);
            }
        };

        zoomComponentViewModel.prototype.getZoomAvailableComputed = function getZoomAvailableComputed() {
            return this.zoom() != this.availableZooms[this.availableZooms.length-1];
        };

        zoomComponentViewModel.prototype.increaseZoom = function increaseZoom() {
            var idx = this.availableZooms.indexOf(this.zoom());
            if (idx < this.availableZooms.length -1) {
                this.zoom(this.availableZooms[idx+1]);
                return true;
            }
            return false;
        };

        zoomComponentViewModel.prototype.decreaseZoom = function decreaseZoom() {
            var idx = this.availableZooms.indexOf(this.zoom());
            if (idx >0) {
                this.zoom(this.availableZooms[idx-1]);
                return true;
            }
            return false;
        };

        return {
            viewModel: zoomComponentViewModel,
            template: { require: 'text!/canvas/templates/zoom.html' }
        };
});