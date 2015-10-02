define(['knockout', 'view-models/state', 'components/uploadData'],

    function(ko, stateViewModel, uploadDataComponent) {

        ko.components.register('upload-data-component', uploadDataComponent);

        function createListComponentViewModel() {
            this.listTitle = ko.observable('New List');
            this.oldTitle = this.listTitle();
            this.titleEdit = ko.observable(false);
        }

        createListComponentViewModel.prototype.editTitle = function editTitle() {
            if (this.listTitle() !== this.oldTitle) {
                console.log('title changed - send to server');
                this.oldTitle = this.listTitle();
            }
            this.titleEdit(!this.titleEdit());
        }

        return {
            viewModel: createListComponentViewModel,
            template: { require: 'text!/scripts/src/templates/create-list.html' }
        };
});