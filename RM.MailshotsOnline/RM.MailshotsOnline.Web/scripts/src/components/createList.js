define(['knockout', 'view-models/state', 'components/uploadData', 'components/matchData', 'components/dataSummary'],

    function(ko, stateViewModel, uploadDataComponent, matchDataComponent, dataSummaryComponent) {

        ko.components.register('upload-data-component', uploadDataComponent);
        ko.components.register('match-data-component', matchDataComponent);
        ko.components.register('data-summary-component', dataSummaryComponent);

        function createListComponentViewModel() {
            this.listTitle = stateViewModel.listTitle;
            this.oldTitle = this.listTitle();
            this.titleEdit = ko.observable(true);
            this.step = stateViewModel.createListStep;
        }

        createListComponentViewModel.prototype.editTitle = function editTitle() {
            this.titleEdit(!this.titleEdit());
        }

        createListComponentViewModel.prototype.checkTitle = function checkTitle() {
            var self = this;

            if (this.listTitle() !== this.oldTitle) {
                $.get('/Umbraco/Api/DistributionList/GetCheckListNameIsUnique?listName='+this.listTitle(), function(data) {
                    self.titleEdit(false);
                }).error(function(error){
                    self.titleEdit(true);
                    stateViewModel.showError(true);
                    stateViewModel.errorTitle(error.responseJSON.error);
                    stateViewModel.errorMessage('You already have a list with this name, please choose another.');
                });
                this.oldTitle = this.listTitle();
            } else {
                this.titleEdit(false);
            }
        }

        return {
            viewModel: createListComponentViewModel,
            template: { require: 'text!/scripts/src/templates/create-list.html' }
        };
});