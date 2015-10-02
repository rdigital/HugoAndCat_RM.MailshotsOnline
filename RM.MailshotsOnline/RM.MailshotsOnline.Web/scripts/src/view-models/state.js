define(['knockout'],

    function(ko) {

        // state view model. describes the state of the app at any given time
        function stateViewModel() {

            //general stuff
            this.showError = ko.observable(false);
            this.errorTitle = ko.observable();
            this.errorMessage = ko.observable();

            // data stuff
            this.lists = ko.observableArray();
            this.selectedLists = ko.observableArray();
            this.listTitle = ko.observable('');
            this.currentList = ko.observable();
        }

        window.appState = new stateViewModel();

        return window.appState;
});