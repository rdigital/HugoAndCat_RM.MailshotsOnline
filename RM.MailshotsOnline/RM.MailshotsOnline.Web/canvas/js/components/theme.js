define(['knockout', 'view_models/format', 'view_models/theme', 'view_models/user', 'view_models/state', 'view_models/history'],

    function(ko, formatViewModel, themeViewModel, userViewModel, stateViewModel, historyViewModel) {

        // ViewModel
        function themeComponentViewModel(params) {
            this.face = formatViewModel.allFaces()[0];
            this.themes = themeViewModel.objects;
            this.element = ko.observable();
            this.scale = ko.observable();
            this.ms_scale = ko.observable();
            this.top = ko.observable(0);
            this.translate = ko.observable('translate(0, 0)');
            this.name_top = ko.observable(0);
            this.container_width = ko.observable(2000);
            this.doScale();
        }

        /**
         * resets any user defined styles, before changing the selected theme
         * @param  {themeViewModel} theme theme view model instance
         */
        themeComponentViewModel.prototype.selectTheme = function selectTheme(theme) {
            userViewModel.resetUserStyles();
            if (theme.id == userViewModel.objects.themeID()) {
                userViewModel.objects.themeID(0);
            }
            userViewModel.objects.themeID(theme.id);
            stateViewModel.toggleThemePicker();
            historyViewModel.pushToHistory();
        };

        themeComponentViewModel.prototype.doScale = function doScale() {
            var el_width = this.face.width,
                el_height = this.face.height,
                height = 200,
                width = 220,
                full_width = 280,
                width_scale = width / el_width,
                height_scale = height / el_height
                scale = Math.min(width_scale, height_scale);
            this.scale('scale(' + scale + ')');
            this.ms_scale('scale(' + scale + ', ' + scale + ')');

            if (height_scale < width_scale) {
                //center align the scaled preview
                var adjusted_width = height_scale * el_width;
                this.translate('translate(' + ((width - adjusted_width) / 2) + 'px, 0)');
            }

            var adjusted_height = scale * el_height;
            this.top(((height - adjusted_height) / 2) + 20);
            this.name_top(this.top() + adjusted_height);
            this.container_width(full_width * this.themes().length);
        };

        /**
         * toggle the theme picker modal
         */
        themeComponentViewModel.prototype.toggleThemePicker = function toggleThemePicker() {
            stateViewModel.toggleThemePicker();
        };

        return {
            viewModel: themeComponentViewModel,
            template: { require: 'text!/canvas/templates/theme.html' }
        };
});