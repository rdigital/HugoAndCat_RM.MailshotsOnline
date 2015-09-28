define(['jquery', 'knockout'],

    function($, ko) {

        // ViewModel
        function listsComponentViewModel() {

            this.lists = ko.observableArray();
            this.totalListCount = ko.observable();
            this.selectedLists = ko.observableArray();
            this.selectedCount = 0;

            // binding stuff
            this.toggleSelect = this.toggleSelect.bind(this);

            $.get('/Umbraco/Api/DistributionList/GetMyLists', function(data) {
                data.forEach(function(list){
                    list.selected = ko.observable(false);
                });
                this.lists(data);
                this.totalListCount(data.length);
            }.bind(this)).fail(function() {
                console.log('There was an error fetching your lists');
            });
        }

        // toggle the selected state for lists and add/remove from the selectedLists array
        listsComponentViewModel.prototype.toggleSelect = function toggleSelect(list) {
            if (list.selected() === false) {
                list.selected(true);
                this.selectedLists.push(list);
                this.selectedCount = this.selectedLists().length;
            } else {
                list.selected(false);
                var pos = this.selectedLists().map(function(e) { return e.DistributionListId; }).indexOf(list.DistributionListId);
                this.selectedLists().splice(pos, 1);
                this.selectedCount = this.selectedLists().length;
            }
        }

        return {
            viewModel: listsComponentViewModel,
            template: { require: 'text!/scripts/src/templates/lists.html' }
        };
});