define(['jquery', 'knockout', 'view-models/state', 'view-models/notification', 'koMapping', 'koValidation'],

    function($, ko, stateViewModel, notificationViewModel, koMapping, koValidation) {

        // ViewModel
        function addContactComponentViewModel() {
            this.showEditModal = stateViewModel.showEditModal;
            this.contact = stateViewModel.currentContact();
            this.currentList = stateViewModel.currentList;
            this.addNewContact = stateViewModel.addNewContact;
            this.loading = ko.observable(false);

            //validation stuff
            this.contact.FirstName = this.contact.FirstName || ko.observable();
            this.contact.Surname = this.contact.Surname || ko.observable();
            this.contact.Address1 = this.contact.Address1 || ko.observable();
            this.contact.PostCode = this.contact.PostCode || ko.observable();

            this.contact.FirstName.extend({
                required: {
                    message: 'Please provide a First Name'
                }
            });

            this.contact.Surname.extend({
                required: {
                    message: 'Please provide a Surname'
                }
            });

            this.contact.Address1.extend({
                required: {
                    message: 'Please provide at least one line of the address'
                }
            });

            this.contact.PostCode.extend({
                required: {
                    message: 'Please provide a postcode'
                },
                pattern: {
                    message: 'Please provide a valid UK postcode (all caps, with a space)',
                    params: '^([A-PR-UWYZ0-9][A-HK-Y0-9][AEHMNPRTVXY0-9]?[ABEHMNPRVWXY0-9]? {1,2}[0-9][ABD-HJLN-UW-Z]{2}|GIR 0AA)$'
                }
            });


            this.isValid = this.isValid.bind(this);
            this.resetContact = this.resetContact.bind(this);

            $('html').addClass('modal-open');

            this.errors = koValidation.group(this, {deep: true});
        }

        addContactComponentViewModel.prototype.resetContact = function resetContact() {

            for(var i in this.contact) {
               if(ko.isObservable(this.contact[i])) {
                  this.contact[i]('');
               }
            }

            this.errors.showAllMessages(false);
            
        };      

        addContactComponentViewModel.prototype.isValid = function isValid() {
            if (this.errors().length === 0) {
               return true;
            }
            else {
                this.errors.showAllMessages();
                return false;
            }
        };

        addContactComponentViewModel.prototype.closeModal = function closeModal() {
            this.resetContact();
            this.showEditModal(false);
            this.addNewContact(false);
        };

        addContactComponentViewModel.prototype.updateSingleContact = function updateSingleContact() {

            if (!this.isValid()) {
                return false;
            }

            var self = this,
                data = {
                    "ListName": this.currentList.ListName(),
                    "DistributionListId": this.currentList.DistributionListId(),
                    "Contacts": [koMapping.toJS(this.contact)]
                };

            this.loading(true);

            $.ajax({
                url: '/Umbraco/API/DistributionList/PostAddContactsToList',
                data: data,
                method: "POST",
                success: function(result) {
                    koMapping.fromJS(result, self.currentList);
                    self.loading(false);
                    self.showEditModal(false);
                    notificationViewModel.hideWithMessage("Contact updated", 'addContact');
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

        addContactComponentViewModel.prototype.addSingleContact = function addSingleContact() {

            if (!this.isValid()) {
                return false;
            }

            var self = this,
                data = {
                    "ListName": this.currentList.ListName(),
                    "DistributionListId": this.currentList.DistributionListId() || "",
                    "Contacts": [koMapping.toJS(this.contact)]
                };

            this.loading(true);

            $.ajax({
                url: '/Umbraco/API/DistributionList/PostAddContactsToList',
                data: data,
                method: "POST",
                success: function(result) {
                    koMapping.fromJS(result, self.currentList);
                    self.resetContact();
                    self.loading(false);
                    self.addNewContact(false);
                    self.showEditModal(false);
                    notificationViewModel.hideWithMessage("Contact added to your list", 'addContact');
                    // go to finished list view
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

        addContactComponentViewModel.prototype.addMultipleContacts =  function addMultipleContacts() {

            if (!this.isValid()) {
                return false;
            }

            var self = this,
                data = {
                    "ListName": this.currentList.ListName(),
                    "DistributionListId": this.currentList.DistributionListId() || "",
                    "Contacts": [koMapping.toJS(this.contact)]
                };

            this.loading(true);

            $.ajax({
                url: '/Umbraco/API/DistributionList/PostAddContactsToList',
                data: data,
                method: "POST",
                success: function(result) {
                    koMapping.fromJS(result, self.currentList);

                    if (result.InvalidContactsAdded === 1) {
                        notificationViewModel.hideWithMessage("There was a problem saving this contact.", 'error');
                    } else if (result.DuplicateContactsAdded === 1) {
                        notificationViewModel.hideWithMessage("This is a duplicate record. Contact not added.", 'error');
                    } else {
                        notificationViewModel.hideWithMessage("Contact added to your list", 'addContact');
                    }

                    self.resetContact();
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