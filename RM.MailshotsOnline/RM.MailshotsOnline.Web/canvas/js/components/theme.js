define(['knockout', 'komapping', 'view_models/format', 'view_models/theme', 'view_models/user', 'view_models/state', 'view_models/history'],

    function(ko, mapping, formatViewModel, themeViewModel, userViewModel, stateViewModel, historyViewModel) {

        // ViewModel
        function themeComponentViewModel(params) {
            this.faces = formatViewModel.allFaces;
            this.themes = themeViewModel.objects;
        }

        /**
         * resets any user defined styles, before changing the selected theme
         * @param  {themeViewModel} theme theme view model instance
         */
        themeComponentViewModel.prototype.selectTheme = function selectTheme(theme) {
            userViewModel.resetUserStyles();
            userViewModel.objects.themeID(theme.id);
            stateViewModel.toggleThemePicker();
            historyViewModel.pushToHistory();
        }

        /**
         * toggle the theme picker modal
         */
        themeComponentViewModel.prototype.toggleThemePicker = function toggleThemePicker() {
            stateViewModel.toggleThemePicker();
        }

        return {
            viewModel: themeComponentViewModel,
            template: { require: 'text!/canvas/templates/theme.html' }
        }
});