define(['knockout', 'view-models/state', 'components/uploadData'],

    function(ko, stateViewModel, uploadDataComponent) {

        ko.components.register('upload-data-component', uploadDataComponent);

        function createListComponentViewModel() {
            this.listTitle = stateViewModel.listTitle;
            this.oldTitle = this.listTitle();
            this.titleEdit = ko.observable(true);

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
                    stateViewModel.errorMessage('You already have a list with this name, please choose another.')
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