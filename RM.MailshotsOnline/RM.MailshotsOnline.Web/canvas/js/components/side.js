define(['knockout', 'components/face', 'view_models/format', 'view_models/state'], 

    function(ko, faceComponent, formatViewModel, stateViewModel) {
        // register required components
        ko.components.register('face-component', faceComponent);

        // ViewModel
        function sideViewModel(params) {
            this.name = params.side;
            this.override_theme = params.override_theme;
            this.override_template = params.override_template;
            this.preview = (this.override_theme || this.override_template) ? true : params.preview;
            
            this.faces = ko.pureComputed(function() {
                return formatViewModel.getFacesBySide(this.name);
            }, this)
            
        }

        return {
            viewModel: sideViewModel,
            template: { require: 'text!/canvas/templates/side.html' }
        }
});