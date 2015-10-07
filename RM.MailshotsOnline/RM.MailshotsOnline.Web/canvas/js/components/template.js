define(['knockout', 'view_models/format', 'view_models/template', 'view_models/user', 'view_models/state', 'view_models/history'],

    function(ko, formatViewModel, templateViewModel, userViewModel, stateViewModel, historyViewModel) {

        // ViewModel
        function templateComponentViewModel(params) {
            this.faces = formatViewModel.allFaces;
            this.sides = [];
            ko.utils.arrayForEach(this.faces(), function(face) {
                if (this.sides.indexOf(face.side) < 0) {
                    this.sides.push(face.side);
                }
            }.bind(this));
            this.templates = templateViewModel.objects;
            this.currentTemplate = userViewModel.objects.templateID;
            this.previewingTemplate = ko.observable(this.currentTemplate());

            this.container = ko.observable();
            this.scale = ko.observable();
            this.ms_scale = ko.observable();

            this.width = ko.observable();
            this.height = ko.observable();
            this.margin = ko.observable(0);

            this.sideFaces = this.sideFaces.bind(this);
            this.doScale = this.doScale.bind(this);
            this.previewTemplate = this.previewTemplate.bind(this);

            $(window).resize(this.doScale);
        }

        /**
         * resets any user defined styles, before changing the selected template
         * if the template has changed, set reposition images to tru on the state view model
         * which tells the image components to recalculate the initial image positions on canvases
         * @param  {templateViewModel} template template view model instance
         */
        templateComponentViewModel.prototype.selectTemplate = function selectTemplate(template) {
            userViewModel.resetUserFontSizes();
            if (userViewModel.objects.templateID() != template.id) {
                stateViewModel.repositionImages = true;
                console.log(stateViewModel.repositionImages);
                userViewModel.objects.templateID(template.id);
            }
            stateViewModel.toggleTemplatePicker();
            historyViewModel.pushToHistory();
        };

        templateComponentViewModel.prototype.previewTemplate = function previewTemplate(template) {
            this.previewingTemplate(template.id);
        };

        /**
         * toggle the template picker modal
         */
        templateComponentViewModel.prototype.toggleTemplatePicker = function toggleTemplatePicker() {
            stateViewModel.toggleTemplatePicker();
        };

        templateComponentViewModel.prototype.sideFaces = function sideFaces(side) {
            var ret = ko.utils.arrayFilter(this.faces(), function(face) {
                return face.side == side;
            });
            return ret;
        };

        templateComponentViewModel.prototype.doScale = function doScale() {
            var side = this.sides[0],
                faces = this.sideFaces(side),
                width = 0,
                height = 0,
                container_height = this.container().height(),
                container_width = this.container().width() - 40;

            ko.utils.arrayForEach(faces, function(face) {
                height += face.height + 60;
                width = Math.max(face.width, width);
            });

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
            this.ms_scale('scale(' + scale + ', ' + scale + ')');
        };

        return {
            viewModel: templateComponentViewModel,
            template: { require: 'text!/canvas/templates/template.html' }
        };
});