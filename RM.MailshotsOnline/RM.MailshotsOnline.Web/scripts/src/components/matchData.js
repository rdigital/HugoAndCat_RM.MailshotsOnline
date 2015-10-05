define(['jquery', 'knockout', 'koMapping', 'select2', 'view-models/state'],

    function($, ko, koMapping, select2, stateViewModel) {

        // ViewModel
        function matchDataComponentViewModel() {
            this.currentList = stateViewModel.currentList;
            this.mappings = ko.observableArray([]);
            this.loading = ko.observable(false);
        }

        matchDataComponentViewModel.prototype.createMappings = function createMappings() {
            var self = this;

            this.loading(true);

            this.mappings([]);

            $('select').each(function(){
                self.mappings.push($(this).val());
            });
            this.currentList().Mappings(this.mappings());

            var data = koMapping.toJSON(this.currentList());
            console.log(data);

           $.post('/Umbraco/Api/DistributionList/PostConfirmFields', data, function(result) {
                console.log(result);
                stateViewModel.createListStep('summary');
                self.loading(false);
            }).fail(function(error) {
                stateViewModel.showError(true);
                if (error) {
                    stateViewModel.errorTitle("Oops!");
                    stateViewModel.errorMessage(error.responseJSON.error);
                } else {
                    stateViewModel.errorTitle("Oops!");
                    stateViewModel.errorMessage("Looks like something went wrong, please try again");
                }
                self.loading(false);
            });
        }

        matchDataComponentViewModel.prototype.initSelect = function initSelect(array, data) {
            $('select').select2();
        }

        return {
            viewModel: matchDataComponentViewModel,
            template: { require: 'text!/scripts/src/templates/match-data.html' }
        };
});