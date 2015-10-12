define(['jquery', 'knockout', 'view-models/state'],

    function($, ko, stateViewModel) {

        // ViewModel
        function myListsComponentViewModel() {

            this.lists = stateViewModel.lists;
            this.displayedList = ko.observableArray([]);
            this.totalListCount = ko.observable();
            this.selectedLists = stateViewModel.selectedLists;

            // binding stuff
            this.toggleSelect = this.toggleSelect.bind(this);

            $.get('/Umbraco/Api/DistributionList/GetMyLists', function(data) {
                data.forEach(function(list){
                    list.selected = ko.observable(false);
                });
                this.lists(data);
                this.totalListCount(data.length);
            }.bind(this)).fail(function(error) {
                stateViewModel.showError(true);
                stateViewModel.errorTitle('Oops!');
                stateViewModel.errorMessage(error.responseJSON.error);
            });
        }

        // toggle the selected state for lists and add/remove from the selectedLists array
        myListsComponentViewModel.prototype.toggleSelect = function toggleSelect(list) {
            if (list.selected() === false) {
                list.selected(true);
                this.selectedLists.push(list);
            } else {
                list.selected(false);
                var pos = this.selectedLists().map(function(e) { return e.DistributionListId; }).indexOf(list.DistributionListId);
                this.selectedLists.splice(pos, 1);
            }
        };

        return {
            viewModel: myListsComponentViewModel,
            template: { require: 'text!/scripts/src/templates/my-lists.html' }
        };
});