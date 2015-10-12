define(['jquery', 'knockout', 'koMapping', 'select2', 'view-models/state'],

    function($, ko, koMapping, select2, stateViewModel) {

        // ViewModel
        function matchDataComponentViewModel() {
            this.currentList = stateViewModel.currentList;
            this.mappings = ko.observableArray([]);
            this.loading = ko.observable(false);
            this.step = stateViewModel.createListStep;
        }

        matchDataComponentViewModel.prototype.createMappings = function createMappings() {
            var self = this;

            this.loading(true);

            this.mappings([]);

            $('select').each(function(){
                self.mappings.push($(this).val());
            });
            
            this.currentList.Mappings(this.mappings());

            this.postData();
        };

        matchDataComponentViewModel.prototype.postData = function postData() {
            var self = this,
                data = koMapping.toJSON(this.currentList);

            $.ajax({
                url: '/Umbraco/Api/DistributionList/PostConfirmFields',
                data: data,
                method: "POST",
                contentType: "application/json",
                success: function(result) {
                    koMapping.fromJS(result, self.currentList);
                    self.step('summary');
                    self.loading(false);
                },
                error: function(error) {
                    stateViewModel.showError(true);
                    if (error) {
                        stateViewModel.errorTitle("Oops!");
                        stateViewModel.errorMessage(error.responseJSON.error);
                    } else {
                        stateViewModel.errorTitle("Oops!");
                        stateViewModel.errorMessage("Looks like something went wrong, please try again");
                    }
                    self.loading(false);
                }
            });
        };

        matchDataComponentViewModel.prototype.initSelect = function initSelect() {
            $('select').select2();
        };

        return {
            viewModel: matchDataComponentViewModel,
            template: { require: 'text!/scripts/src/templates/match-data.html' }
        };
});