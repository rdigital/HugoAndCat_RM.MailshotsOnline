define([
        'knockout',
        'view-models/state',
        'components/uploadData',
        'components/matchData',
        'components/dataSummary',
        'components/addContact'
    ],

    function(ko, stateViewModel, uploadDataComponent, matchDataComponent, dataSummaryComponent, addContactComponent) {

        ko.components.register('upload-data-component', uploadDataComponent);
        ko.components.register('match-data-component', matchDataComponent);
        ko.components.register('data-summary-component', dataSummaryComponent);
        ko.components.register('add-contact-component', addContactComponent);

        function createListComponentViewModel() {
            this.listTitle = stateViewModel.currentList.ListName;
            this.currentList = stateViewModel.currentList;
            this.oldTitle = this.listTitle();
            this.titleEdit = ko.observable(true);
            this.step = stateViewModel.createListStep;
            this.showEditModal = stateViewModel.showEditModal;
            this.backUrl = window.backUrl;
            this.backText = window.backText;
        }

        createListComponentViewModel.prototype.editTitle = function editTitle() {
            this.titleEdit(!this.titleEdit());
        };

        createListComponentViewModel.prototype.checkTitle = function checkTitle() {
            var self = this;

            if (this.listTitle() !== this.oldTitle) {
                $.get('/Umbraco/Api/DistributionList/GetCheckListNameIsUnique?listName='+this.listTitle(), function() {
                    self.currentList.ListTitle = self.listTitle();
                    self.titleEdit(false);
                }).error(function(error){
                    self.titleEdit(true);
                    stateViewModel.showError(true);
                    stateViewModel.errorTitle('Oops!');
                    stateViewModel.errorMessage(error.responseJSON.error);
                });
                this.oldTitle = this.listTitle();
            } else {
                this.titleEdit(false);
            }
        };

        createListComponentViewModel.prototype.cancel = function cancel() {
            var data = {
                    "DistributionListId": this.currentList.DistributionListId() || "",
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

        return {
            viewModel: createListComponentViewModel,
            template: { require: 'text!/scripts/src/templates/create-list.html' }
        };
});