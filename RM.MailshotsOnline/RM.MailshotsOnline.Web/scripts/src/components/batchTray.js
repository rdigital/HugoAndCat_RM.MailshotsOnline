define(['jquery', 'knockout', 'view-models/state'],

    function($, ko, stateViewModel) {

        // ViewModel
        function batchTrayComponentViewModel() {
            var self = this;

            this.selectedLists = stateViewModel.selectedLists;
            this.isOpen = ko.observable(false);
            this.totalCount = ko.computed(function(){
                var count = 0;
                self.selectedLists().forEach(function(list) {
                    count += list.RecordCount;
                });
                return count;
            });

            this.selectedListCount = ko.pureComputed(function() {
                return stateViewModel.selectedLists().length;
            });
        }

        batchTrayComponentViewModel.prototype.expandTray = function expandTray(data, e) {
            this.isOpen(!this.isOpen());
        }

        return {
            viewModel: batchTrayComponentViewModel,
            template: { require: 'text!/scripts/src/templates/batch-tray.html' }
        };
});