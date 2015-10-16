define(['jquery', 'knockout', 'perfectScrollbar', 'koelement', 'view-models/state'],

    function($, ko, perfectScrollbar, koelement, stateViewModel) {

        // ViewModel
        function batchTrayComponentViewModel() {
            var self = this;

            this.campaignId = window.campaignId;
            this.selectedLists = stateViewModel.selectedLists;
            this.listsContainer = ko.observable();
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

            this.selectedListCount.subscribe(this.updateScroll, this);

            this.initScroll = this.initScroll.bind(this);
            this.updateScroll = this.updateScroll.bind(this);
            this.remove = this.remove.bind(this);
        }

        batchTrayComponentViewModel.prototype.expandTray = function expandTray() {
            this.isOpen(!this.isOpen());
        };

        batchTrayComponentViewModel.prototype.initScroll = function initScroll() {
            this.listsContainer().perfectScrollbar();
            this.listsContainer().perfectScrollbar('update');
        };

        batchTrayComponentViewModel.prototype.updateScroll = function updateScroll() {
            this.listsContainer().perfectScrollbar('update');    
        };

        batchTrayComponentViewModel.prototype.remove = function remove(list) {
            var pos = this.selectedLists().map(function(e) { return e.DistributionListId; }).indexOf(list.DistributionListId);
            this.selectedLists.splice(pos, 1);
            list.selected(false);
        };

        // add data to campaign functionality to be added here
        batchTrayComponentViewModel.prototype.addDataToCampaign = function addDataToCampaign() {
            console.log('add data to campaign', this.campaignId);
            // add API call here and redirect back to campaign page on success
            // campaign ID is stored in this.campaignId
        };

        return {
            viewModel: batchTrayComponentViewModel,
            template: { require: 'text!/scripts/src/templates/batch-tray.html' }
        };
});