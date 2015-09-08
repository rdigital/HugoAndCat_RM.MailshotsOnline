define(['knockout', 'view_models/format', 'view_models/state'], 

    function(ko, formatViewModel, stateViewModel) {

        // ViewModel
        function previewViewModel(params) {
            this.faces = formatViewModel.allFaces;
            this.sides = ['front', 'back'];
            this.showPreview = stateViewModel.showPreview;
            this.container = ko.observable();
            this.scale = ko.observable();

            this.sideFaces = this.sideFaces.bind(this);
            this.doScale = this.doScale.bind(this);
            this.togglePreview = stateViewModel.togglePreview.bind(this);

            $(window).resize(this.doScale);
        }

        previewViewModel.prototype.sideFaces = function sideFaces(side) {
            ret = ko.utils.arrayFilter(this.faces(), function(face) {
                return face.side == side
            })
            return ret
        }

        previewViewModel.prototype.doScale = function doScale() {
            var side = this.sides[0],
                faces = this.sideFaces(side),
                width = 0,
                height = 0;

            ko.utils.arrayForEach(faces, function(face) {
                height += face.height + 50;
                width = Math.max(face.width, width);
            })

            var v_scale = (this.container().height() - 100) / height,
                h_scale = (this.container().width() - 40) / width,
                scale = Math.min(v_scale, h_scale);

            this.scale('scale(' + scale + ')');
        }

        return {
            viewModel: previewViewModel,
            template: { require: 'text!/canvas/templates/preview.html' }
        }
});