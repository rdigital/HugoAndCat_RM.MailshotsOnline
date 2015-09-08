define(['knockout', 'komapping', 'components/side', 'components/tools', 'components/backgroundtools', 'components/options', 'components/upload', 'components/zoom', 'components/preview', 'components/sidepicker', 'view_models/format', 'view_models/state', 'view_models/history'],

    function(ko, mapping, sideComponent, toolsComponent, backgroundToolsComponent, optionsComponent, uploadComponent, zoomComponent, previewComponent, sidePickerComponent, formatViewModel, stateViewModel, historyViewModel) {
        // register required components
        ko.components.register('tools-component', toolsComponent);
        ko.components.register('backgroundtools-component', backgroundToolsComponent);
        ko.components.register('options-component', optionsComponent);
        ko.components.register('side-component', sideComponent);
        ko.components.register('sidepicker-component', sidePickerComponent);
        ko.components.register('upload-component', uploadComponent);
        ko.components.register('zoom-component', zoomComponent);
        ko.components.register('preview-component', previewComponent);

        // ViewModel
        function editorViewModel(params) {
            this.faces = formatViewModel.allFaces;
            this.sides = ['front', 'back']
            this.viewingSide = stateViewModel.viewingSide;
            this.viewingFace = stateViewModel.viewingFace;
            this.historyRerender = stateViewModel.historyRerender;
            this.showPreview = stateViewModel.showPreview;
            this.showImageUpload = stateViewModel.showImageUpload;
            this.showThemePicker = stateViewModel.showThemePicker;
            this.showTemplatePicker = stateViewModel.showTemplatePicker;
            this.isBackgroundSelected = stateViewModel.backgroundSelected;

            //testing
            this.output = stateViewModel.output;
            this.clearOutput = this.clearOutput.bind(this);

            this.unfocusConditional = this.unfocusConditional.bind(this);

            this.elementFocused = this.getHiddenComputed();
            this.message = this.getMessageComputed();

            // subscriptions
            this.handleSubscriptions();
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
         * unfocus any currently selected element if clicking directly on the
         * editor container
         */
        editorViewModel.prototype.unfocusConditional = function unfocusConditional(data, e) {
            if ($(e.originalEvent.target).hasClass('canvas-container')) {
                this.unfocus();
            }
        }

        editorViewModel.prototype.getHiddenComputed = function getHiddenComputed() {
            return ko.pureComputed(function() {
                if (stateViewModel.selectedElement() || stateViewModel.backgroundSelected()) {
                    return true
                }
                return false
            }, this)
        }

        editorViewModel.prototype.getMessageComputed = function getMessageComputed() {
            return ko.pureComputed(function() {
                if (stateViewModel.selectedElement()) {
                    return stateViewModel.selectedElement().message();
                }
                return null
            }, this)
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