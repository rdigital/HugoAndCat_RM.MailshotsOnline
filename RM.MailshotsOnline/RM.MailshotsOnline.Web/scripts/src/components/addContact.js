define(['jquery', 'knockout', 'view-models/state', 'koMapping'],

    function($, ko, stateViewModel, koMapping) {

        // ViewModel
        function addContactComponentViewModel() {
            this.showEditModal = stateViewModel.showEditModal;
            this.contact = stateViewModel.currentContact;
            this.currentList = stateViewModel.currentList;
            this.loading = ko.observable(false);

            $('html').addClass('modal-open');
        }

        addContactComponentViewModel.prototype.closeModal = function closeModal() {
            this.contact({});
            this.showEditModal(false);
        };

        addContactComponentViewModel.prototype.updateSingleContact = function help() {
            console.log(this.contact());

            /*POST: /Umbraco/API/DistributionList/PostAddContactsToList
            {
              "ListName":"List 006",
              "DistributionListId":"0c2aad27-1a6e-457d-8450-c5d9e2b1d1a8",
              "Contacts":[{
                "Title":"Dr",
                "FirstName":"Name Here",
                "Surname":"",
                "FlatId":"4",
                "HouseName":"Flat House",
                "HouseNumber":"7",
                "Address1":"Corrected with an address 1",
                "Address2":"No Name or Address",
                "Address3":"",
                "Address4":"",
                "PostCode":"TE5 7NG",
                "ContactId":"5e76a445-4b3b-4bb3-93fb-26eac36d5eef"
              },{…}]
            }*/

            var self = this,
                data = {
                    "ListName": this.currentList.ListName(),
                    "DistributionListId": this.currentList.DistributionListId(),
                    "Contacts": [koMapping.toJS(this.contact())]
                };

            this.loading(true);

            console.log(data);

            $.ajax({
                url: '/Umbraco/API/DistributionList/PostAddContactsToList',
                data: data,
                method: "POST",
                contentType: "application/json",
                success: function(result) {
                    console.log(result);
                    //koMapping.fromJS(result, self.currentList);
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

        addContactComponentViewModel.prototype.dispose = function dispose() {
            $('html').removeClass('modal-open');
        };

        return {
            viewModel: addContactComponentViewModel,
            template: { require: 'text!/scripts/src/templates/add-contact.html' }
        };
});