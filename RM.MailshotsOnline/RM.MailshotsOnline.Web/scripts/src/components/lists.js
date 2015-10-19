define(['knockout', 'view-models/state', 'components/myLists', 'components/batchTray', 'components/rentList'],

    function(ko, stateViewModel, myListsComponent, batchTrayComponent, rentListComponent) {

        ko.components.register('my-lists-component', myListsComponent);
        ko.components.register('batch-tray-component', batchTrayComponent);
        ko.components.register('rent-list-component', rentListComponent);

        function listsComponentViewModel() {
            this.backUrl = ko.observable(window.backUrl);
            this.backText = ko.observable(window.backText);
        }

        return {
            viewModel: listsComponentViewModel,
            template: { require: 'text!/scripts/src/templates/lists.html' }
        };
});