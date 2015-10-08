define(['knockout', 'koMapping'],

    function(ko, koMapping) {

        // state view model. describes the state of the app at any given time
        function stateViewModel() {

            //general stuff
            this.showError = ko.observable(false);
            this.errorTitle = ko.observable();
            this.errorMessage = ko.observable();

            // data stuff
            this.lists = ko.observableArray();
            this.selectedLists = ko.observableArray();
            this.currentList = koMapping.fromJS({
                "DistributionListId": "",
                "ListName": ''
            });
            this.createListStep = ko.observable('create');
            this.showEditModal = ko.observable(false);
            this.currentContact = ko.observable();
        }

        window.appState = new stateViewModel();

        return window.appState;
});