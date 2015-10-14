define(['jquery', 'knockout', 'koMapping', 'view-models/state'],

    function($, ko, koMapping, stateViewModel) {

        // ViewModel
        function campaignListComponentViewModel() {
        	var self = this;
        	this.campaignList = ko.observableArray([]);
        	this.displayedList = ko.observableArray([]);

        	$.ajax({
        	    url: '/Umbraco/Api/Campaign/GetAll',
        	    type: 'GET',
        	    success: function (data) {
        	        self.campaignList(data);
        	    },
        	    error: function(error) {
                    stateViewModel.showError(true);
                    if (error) {
                        stateViewModel.errorTitle("Oops!");
                        stateViewModel.errorMessage(error.responseJSON.error);
                    } else {
                        stateViewModel.errorTitle("Oops!");
                        stateViewModel.errorMessage("Looks like something went wrong, please try again");
                    }
                }
        	});
        }

        return {
            viewModel: campaignListComponentViewModel,
            template: { require: 'text!/scripts/src/templates/campaign-list.html' }
        };
});