define(['knockout', 'jquery', 'components/face', 'view_models/format', 'view_models/state'], 

    function(ko, $, faceComponent, formatViewModel, stateViewModel) {
        // register required components
        ko.components.register('face-component', faceComponent);

        // ViewModel
        function sideViewModel(params) {
            this.name = params.side;
            this.scale = params.scale;
            this.override_theme = params.override_theme;
            this.override_template = params.override_template;
            this.preview = (this.override_theme || this.override_template) ? true : params.preview;

            this.faces = ko.pureComputed(function() {
                return formatViewModel.getFacesBySide(this.name);
            }, this)

            if (!this.preview) {
                window.side = this;
                this.element = stateViewModel.scaleElement;
            } else {
                this.element = ko.observable();
            }
            this.zoom = stateViewModel.getZoom;
            this.scale = ko.pureComputed(function() {
                if (!this.preview) {
                    return 'scale(' + this.zoom() + ')'
                }
            }, this);

            this.resize = ko.observable();
            $(window).resize(function() {
                this.resize.valueHasMutated();
            }.bind(this));
            this.coordinates = this.getCoordinatesComputed();
            this.canvas_container = $('.canvas-container');
            
        }

        sideViewModel.prototype.getCoordinatesComputed = function getCoordinatesComputed() {
            return ko.pureComputed(function(){
                if (this.preview) {
                    return 'auto'
                }
                this.zoom();
                this.resize();
                var el = this.element();
                if (!el) {
                    return 0
                }
                el = el[0];
                var diff = this.canvas_container.width() - el.getBoundingClientRect().width;
                if (stateViewModel.selectedElement() || stateViewModel.backgroundSelected()) {
                    diff -= 150;
                }
                if (diff > 100) {
                    return diff/2 + 'px'
                } else {
                    return '50px'
                }
            }, this)
        }

        return {
            viewModel: sideViewModel,
            template: { require: 'text!/canvas/templates/side.html' }
        }
});