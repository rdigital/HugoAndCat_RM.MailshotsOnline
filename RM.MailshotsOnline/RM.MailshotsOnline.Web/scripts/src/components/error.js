define(['jquery', 'knockout', 'view-models/state'],

    function($, ko, stateViewModel) {

        // ViewModel
        function errorComponentViewModel() {
            this.showError = stateViewModel.showError;
            this.errorTitle = stateViewModel.errorTitle;
            this.errorMessage = stateViewModel.errorMessage;
        }

        errorComponentViewModel.prototype.closeError = function closeError() {
            this.showError(false);
        }

        return {
            viewModel: errorComponentViewModel,
            template: { require: 'text!/scripts/src/templates/error.html' }
        };
});