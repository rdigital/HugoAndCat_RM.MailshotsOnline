define(['knockout', 'jquery', 'view_models/state'], 

    function(ko, $, stateViewModel) {

        // ViewModel
        function zoomComponentViewModel() {
            this.zoom = stateViewModel.zoom;
            this.scaleElement = stateViewModel.scaleElement;
            this.availableZooms = [0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2];

            // bound functions
            this.handleScale = this.handleScale.bind(this);

            // subscriptions
            this.handleSubscriptions();

            setTimeout(function() {
                this.handleScale();
            }.bind(this), 100)
        }

        zoomComponentViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.scaleElement.subscribe(this.handleScale, this)
            $(window).resize(this.handleScale);
        }

        zoomComponentViewModel.prototype.handleScale = function handleScale() {
            var el = this.scaleElement();
            if (!el) {
                return
            }
            var window_height = $(window).height() - 150,
                window_width = $(window).width() - 150,
                width_factor = (Math.floor((window_width / el.width())*4))/4,
                height_factor = (Math.floor((window_height / el.height())*4))/4;

            this.zoom(Math.min(width_factor, height_factor));
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