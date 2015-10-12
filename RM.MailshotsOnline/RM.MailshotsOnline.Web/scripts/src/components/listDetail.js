define(['jquery', 'knockout', 'view-models/state'],

    function($, ko, stateViewModel) {

        // ViewModel
        function listDetailComponentViewModel() {
            this.listID = window.listId;
            this.count = ko.observable();
            this.contacts = ko.observableArray([]);
            this.displayedList = ko.observableArray([]);
            this.searchTerm = ko.observable();
            this.selectedContacts = ko.observableArray([]);
            this.list = ko.observable();

            this.filteredContacts = ko.pureComputed(function() {
                var self = this;

                if (!this.searchTerm()) {
                    return this.contacts();
                } else {
                    return ko.utils.arrayFilter(this.contacts(), function(contact) {
                        for(var i in contact) {
                            if (contact[i] && contact[i].toLowerCase().indexOf(self.searchTerm().toLowerCase()) >= 0) {
                                return true;
                            }
                        }
                    });
                }
            }, this);

            this.toggleSelect = this.toggleSelect.bind(this);

            $.get('/Umbraco/API/DistributionList/GetMyListDetails?distributionListId='+ this.listID, function(data) {
                data.Contacts.forEach(function(contact){
                    contact.selected = ko.observable(false);
                });
                this.count(data.List.RecordCount);
                this.list(data.List);
                this.contacts(data.Contacts);
            }.bind(this)).fail(function(error) {
                stateViewModel.showError(true);
                stateViewModel.errorTitle('Oops!');
                stateViewModel.errorMessage(error.responseJSON.error);
            });
        }

        // toggle the selected state for lists and add/remove from the selectedLists array
        listDetailComponentViewModel.prototype.toggleSelect = function toggleSelect(contact) {
            if (contact.selected() === false) {
                contact.selected(true);
                this.selectedContacts.push(contact);
            } else {
                contact.selected(false);
                var pos = this.selectedContacts().map(function(e) { return e.ContactId; }).indexOf(contact.ContactId);
                this.selectedContacts.splice(pos, 1);
            }
        };

        listDetailComponentViewModel.prototype.deleteList = function deleteList() {

            $.ajax({
                url: '/Umbraco/Api/DistributionList/DeleteMyList/?DistributionListId='+this.listID,
                method: "DELETE",
                success: function() {
                    window.location = '/lists';
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

        listDetailComponentViewModel.prototype.deleteContacts = function deleteContacts() {
            console.log('deleteContacts');
        };

        return {
            viewModel: listDetailComponentViewModel,
            template: { require: 'text!/scripts/src/templates/list-detail.html' }
        };
});