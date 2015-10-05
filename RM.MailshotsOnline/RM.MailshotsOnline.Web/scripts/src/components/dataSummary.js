define(['knockout', 'view-models/state'],

    function(ko, stateViewModel) {

        function dataSummaryComponentViewModel() {
            console.log('dataSummary.js');
        }

        return {
            viewModel: dataSummaryComponentViewModel,
            template: { require: 'text!/scripts/src/templates/data-summary.html' }
        };
});