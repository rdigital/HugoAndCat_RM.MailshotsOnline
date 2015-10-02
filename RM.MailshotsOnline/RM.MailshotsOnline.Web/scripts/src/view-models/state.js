define(['knockout'],

    function(ko) {

        // state view model. describes the state of the app at any given time
        function stateViewModel() {

            this.lists = ko.observableArray();
            this.selectedLists = ko.observableArray();
        }

        window.appState = new stateViewModel();

        return window.appState;
});