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
                scale = Math.min((width / el_width), (height / el_height));
            this.scale('scale(' + scale + ')');
            this.ms_scale('scale(' + scale + ', ' + scale + ')');

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