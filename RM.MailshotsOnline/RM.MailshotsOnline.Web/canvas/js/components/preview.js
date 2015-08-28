define(['knockout', 'view_models/format'], 

    function(ko, faceComponent, formatViewModel) {

        // ViewModel
        function previewViewModel(params) {
            this.faces = formatViewModel.allFaces;
            this.sides = ['front', 'back'];
        }

        return {
            viewModel: previewViewModel,
            template: { require: 'text!/canvas/templates/preview.html' }
        }
});