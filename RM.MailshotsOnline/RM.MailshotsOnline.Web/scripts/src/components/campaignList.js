define(['jquery', 'knockout', 'koMapping', 'view-models/state', 'view-models/notification'],

    function($, ko, koMapping, stateViewModel, notificationViewModel) {

        // ViewModel
        function campaignListComponentViewModel() {
        	this.campaignList = ko.observableArray([]);
        	this.displayedList = ko.observableArray([]);
        	this.loading = ko.observable(true);

        	this.deleteCampaign = this.deleteCampaign.bind(this);
        	this.copy = this.copy.bind(this);

        	this.getCampaigns();
        }

        campaignListComponentViewModel.prototype.getCampaigns = function getCampaigns() {
        	var self = this;
        	this.loading(true);

    		$.ajax({
    		    url: '/Umbraco/Api/Campaign/GetAll',
    		    type: 'GET',
    		    success: function (data) {
    		        self.campaignList(data);
    		        self.loading(false);
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
    	            self.loading(false);
    	        }
    		});
        };

        campaignListComponentViewModel.prototype.deleteCampaign = function deleteCampaign(data) {
        	var self = this;

    	    $.ajax({
    	        url: '/Umbraco/Api/Campaign/Delete/' + data.CampaignId,
    	        type: 'DELETE',
    	        success: function () {
    	            self.getCampaigns();
    	            notificationViewModel.hideWithMessage("Campaign deleted", 'message');
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
        };

        campaignListComponentViewModel.prototype.copy = function copy(data) {
        	var self = this;

        	$.ajax({
        	    url: '/Umbraco/Api/Campaign/GetCopy/' + data.CampaignId,
        	    type: 'GET',
        	    success: function () {
        	        self.getCampaigns();
        	        notificationViewModel.hideWithMessage("Campign copied", 'addContact');
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
        };

        campaignListComponentViewModel.prototype.createNew = function createNew() {
        	console.log('createNew');
        };

        return {
            viewModel: campaignListComponentViewModel,
            template: { require: 'text!/scripts/src/templates/campaign-list.html' }
        };
});