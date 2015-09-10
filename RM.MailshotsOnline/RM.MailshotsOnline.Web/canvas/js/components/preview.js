define(['knockout', 'view_models/format', 'view_models/state'], 

    function(ko, formatViewModel, stateViewModel) {

        // ViewModel
        function previewViewModel(params) {
            this.faces = formatViewModel.allFaces;
            this.sides = ['front', 'back'];
            this.showPreview = stateViewModel.showPreview;
            this.container = ko.observable();
            this.scale = ko.observable();

            this.width = ko.observable();
            this.height = ko.observable();
            this.margin = ko.observable(0);

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
                height = 0,
                container_height = this.container().height(),
                container_width = this.container().width() - 40;

            ko.utils.arrayForEach(faces, function(face) {
                height += face.height + 60;
                width = Math.max(face.width, width);
            })

            this.width(width + 'px');
            this.height(height + 'px');

            var v_scale = container_height / height,
                h_scale = container_width / width,
                scale = Math.min(v_scale, h_scale),
                scaled_height = scale * height;

            if (scaled_height < container_height) {
                this.margin(((container_height - scaled_height) / 2) + 'px');
            } else {
                this.margin(0);
            }

            this.scale('scale(' + scale + ')');
        }

        return {
            viewModel: previewViewModel,
            template: { require: 'text!/canvas/templates/preview.html' }
        }
});