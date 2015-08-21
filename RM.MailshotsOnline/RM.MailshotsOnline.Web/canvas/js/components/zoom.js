define(['knockout', 'jquery', 'view_models/state'], 

    function(ko, $, stateViewModel) {

        // ViewModel
        function zoomComponentViewModel() {
            this.zoom = stateViewModel.zoom;
            this.scaleElement = stateViewModel.scaleElement;
            this.availableZooms = [0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2];

            // bound functions
            this.handleScale = this.handleScale.bind(this);
            this.handleScale();

            // subscriptions
            this.handleSubscriptions();
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
            var coords = el[0].getBoundingClientRect();
            if (coords.bottom > $(window).height()) {
                var resized = this.decreaseZoom();
                if (resized) {
                    this.handleScale();
                }
            }
            console.log(coords.right);
            console.log($(window).width() - 150)
            if (coords.right >= $(window).width() - 150) {
                var resized = this.decreaseZoom();
                if (resized) {
                    this.handleScale();
                }
            }
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