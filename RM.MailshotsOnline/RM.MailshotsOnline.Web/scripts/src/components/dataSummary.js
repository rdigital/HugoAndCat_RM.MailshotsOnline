define(['knockout', 'view-models/state'],

    function(ko, stateViewModel) {

        function dataSummaryComponentViewModel() {
            this.currentList = stateViewModel.currentList;
            this.invalidList = ko.pureComputed(function() {
                return (this.currentList.InvalidContacts) ? this.currentList.InvalidContacts() : [];
            }, this);
            this.duplicateList = ko.pureComputed(function() {
                return (this.currentList.DuplicateContacts) ? this.currentList.DuplicateContacts() : [];
            }, this);
            this.step = stateViewModel.createListStep;
            this.invalidDisplayedList = ko.observableArray([]);
            this.duplicateDisplayedList = ko.observableArray([]);
            this.hasInvalid = ko.pureComputed(function() {
                return this.invalidList().length > 0;
            }, this);
            this.hasDuplicates = ko.pureComputed(function() {
                return this.duplicateList().length > 0;
            }, this);
        }

        dataSummaryComponentViewModel.prototype.cancel = function cancel() {
            var data = {
                    "DistributionListId": this.currentList.DistributionListId(),
                    "command": "cancel"
                },
                self = this;

            $.post('/Umbraco/API/DistributionList/PostFinishList', data, function() {
                self.step('create');
            }).error(function(error){
                stateViewModel.showError(true);
                stateViewModel.errorMessage(error.responseJSON.error);
            });
        };

        dataSummaryComponentViewModel.prototype.finish = function finish() {
            var data = {
                    "DistributionListId": this.currentList.DistributionListId(),
                    "ListName": this.currentList.ListName(),
                    "command": "finish"
                },
                self = this;

            $.post('/Umbraco/API/DistributionList/PostFinishList', data, function() {
                self.step('summary');
            }).error(function(error){
                stateViewModel.showError(true);
                stateViewModel.errorMessage(error.responseJSON.error);
            });
        };

        return {
            viewModel: dataSummaryComponentViewModel,
            template: { require: 'text!/scripts/src/templates/data-summary.html' }
        };
});