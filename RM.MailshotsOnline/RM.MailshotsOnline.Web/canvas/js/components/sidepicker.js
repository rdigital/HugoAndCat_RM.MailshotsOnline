define(['knockout', 'koelement', 'view_models/state'],

    function(ko, koelement, stateViewModel) {
        // register required components

        // ViewModel
        function sidePickerViewModel(params) {
            this.face = params.face;
            this.unfocus = params.unfocus;
            this.viewingFace = stateViewModel.viewingFace;
            this.container = ko.observable();
            this.element = ko.observable();
            this.scale = ko.observable();
            this.ms_scale = ko.observable();
            this.opacity = ko.observable(0);
            this.height = ko.observable();

            this.selectFace = this.selectFace.bind(this);
            this.doScale = this.doScale.bind(this);
            this.width = 75;
        }

        sidePickerViewModel.prototype.selectFace = function selectFace() {
            if (this.face != this.viewingFace()) {
                this.viewingFace(this.face);
            }
            this.unfocus();
            return false;
        };

        sidePickerViewModel.prototype.doScale = function doScale() {
            var el_width = this.face.width,
                el_height = this.face.height,
                scale = this.width / el_width;
            this.scale('scale(' + scale + ')');
            this.ms_scale('scale(' + scale + ', ' + scale + ')');
            this.height(((el_height * scale) + 60) + 'px');
            this.opacity(1);
        };


        return {
            viewModel: sidePickerViewModel,
            template: { require: 'text!/canvas/templates/sidepicker.html' }
        };
});