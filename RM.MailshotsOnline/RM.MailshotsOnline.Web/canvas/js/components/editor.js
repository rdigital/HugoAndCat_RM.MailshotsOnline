define(['knockout', 'komapping', 'components/side', 'components/tools', 'components/backgroundtools', 'components/options', 'components/upload', 'components/zoom', 'view_models/format', 'view_models/state'], 

    function(ko, mapping, sideComponent, toolsComponent, backgroundToolsComponent, optionsComponent, uploadComponent, zoomComponent, formatViewModel, stateViewModel) {
        // register required components
        ko.components.register('tools-component', toolsComponent);
        ko.components.register('backgroundtools-component', backgroundToolsComponent);
        ko.components.register('options-component', optionsComponent);
        ko.components.register('side-component', sideComponent);
        ko.components.register('upload-component', uploadComponent);
        ko.components.register('zoom-component', zoomComponent);

        // ViewModel
        function editorViewModel(params) {
            this.faces = formatViewModel.allFaces;
            this.sides = ['front', 'back']
            this.viewingSide = stateViewModel.viewingSide;
            this.showImageUpload = stateViewModel.showImageUpload;
            this.showThemePicker = stateViewModel.showThemePicker;
            this.showTemplatePicker = stateViewModel.showTemplatePicker;

            //testing
            this.output = stateViewModel.output;
            this.clearOutput = this.clearOutput.bind(this);

            // subscriptions
            this.handleSubscriptions();

            this.zoom = stateViewModel.zoom;
            this.scale = ko.pureComputed(function() {
                if (!this.preview) {
                    return 'scale(' + this.zoom() + ')'
                }
            }, this)
        }

        /**
         * set up subscription (testing only) to auto highlight output json
         */
        editorViewModel.prototype.handleSubscriptions = function handleSubscriptions() {
            this.output.subscribe(function () {
                setTimeout(function () {
                    $('.json-output').select();
                }, 100)
            })
        }

        /**
         * switch the currently viewing side
         */
        editorViewModel.prototype.toggleSide = function toggleSide() {
            var side = stateViewModel.viewingSide();
            stateViewModel.viewingSide( (side == 'front') ? 'back' : 'front' )
            this.unfocus();
        }

        /**
         * unfocus any currently selected element
         */
        editorViewModel.prototype.unfocus = function unfocus() {
            stateViewModel.selectElement(null);
            stateViewModel.backgroundSelected(null);
        }

        /**
         * testing only - clear the json output
         */
        editorViewModel.prototype.clearOutput = function clearOutput() {
            this.output(null)
        }

        return {
            viewModel: editorViewModel,
            template: { require: 'text!/canvas/templates/editor.html' }
        }
});