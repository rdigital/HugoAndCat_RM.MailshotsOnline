define(['jquery', 'knockout', 'view-models/state'],

    function($, ko, stateViewModel) {

        // ViewModel
        function matchDataComponentViewModel() {
            this.currentList = stateViewModel.currentList;
        }

        return {
            viewModel: matchDataComponentViewModel,
            template: { require: 'text!/scripts/src/templates/match-data.html' }
        };
});