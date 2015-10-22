define(['jquery', 'knockout', 'koMapping', 'view-models/state'],

    function($, ko, koMapping, stateViewModel) {

        // ViewModel
        function listDetailComponentViewModel() {
            this.listID = window.listId;
            stateViewModel.currentList.DistributionListId(this.listID);
            this.count = ko.observable();
            this.contacts = ko.observableArray([]);
            this.displayedList = ko.observableArray([]);
            this.searchTerm = ko.observable();
            this.selectedContacts = ko.observableArray([]);
            this.showEditModal = stateViewModel.showEditModal;
            this.list = ko.observable();
            this.backUrl = window.backUrl;
            this.backText = window.backText;

            // computeds
            this.filteredContacts = this.getFilterComputed();

            // bound methods
            this.toggleSelect = this.toggleSelect.bind(this);

            this.fetch()
        }

        listDetailComponentViewModel.prototype.fetch = function fetch() {
            $.get('/Umbraco/API/DistributionList/GetMyListDetails?distributionListId='+ this.listID, function(data) {
                stateViewModel.currentList.ListName(data.Name);
                data.Contacts.forEach(function(contact){
                    contact.selected = false;
                });
                this.count(data.List.RecordCount);
                this.list(data.List);
                koMapping.fromJS(data.Contacts, [], this.contacts);
            }.bind(this)).fail(function(error) {
                stateViewModel.showError(true);
                stateViewModel.errorTitle('Oops!');
                stateViewModel.errorMessage(error.responseJSON.error);
            });
        }

        listDetailComponentViewModel.prototype.getFilterComputed = function getFilterComputed() {
            return ko.pureComputed(function() {
                if (!this.searchTerm()) {
                    return this.contacts();
                } else {
                    return ko.utils.arrayFilter(this.contacts(), function(contact) {
                        for(var i in contact) {
                            var attr = ko.utils.unwrapObservable(contact[i]);
                            if (typeof(attr) === 'string' && attr.toLowerCase().indexOf(this.searchTerm().toLowerCase()) >= 0) {
                                return true;
                            }
                        }
                    }.bind(this));
                }
            }, this);
        }

        listDetailComponentViewModel.prototype.submitSearch = function submitSearch(e) {
            // annoying workaround to allow the search to submit on pressing enter in IE
            return;
        };

        // toggle the selected state for lists and add/remove from the selectedLists array
        listDetailComponentViewModel.prototype.toggleSelect = function toggleSelect(contact) {
            if (contact.selected() === false) {
                contact.selected(true);
                this.selectedContacts.push(contact.ContactId);
            } else {
                contact.selected(false);
                var pos = this.selectedContacts().indexOf(contact.ContactId);
                this.selectedContacts.splice(pos, 1);
            }
        };

        listDetailComponentViewModel.prototype.deleteList = function deleteList() {

            $.ajax({
                url: '/Umbraco/Api/DistributionList/DeleteMyList/?DistributionListId='+this.listID,
                method: "DELETE",
                success: function() {
                    window.location = this.backUrl;
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
            var data = {
                    "DistributionListId": this.listID,
                    "ContactIds": this.selectedContacts()
                },
                self = this;

            $.post('/Umbraco/Api/DistributionList/PostDeleteContactsFromList/', data, function(data) {
                if (data === null) {
                    window.location = this.backUrl;
                } else {
                    data.Contacts.forEach(function(contact){
                        contact.selected = ko.observable(false);
                    });
                    self.count(data.List.RecordCount);
                    self.list(data.List);
                    self.contacts(data.Contacts);
                    self.selectedContacts([]);
                }
            }).fail(function(error) {
                stateViewModel.showError(true);
                if (error.reponseJSON) {
                    stateViewModel.errorTitle(error.responseJSON.error);
                } else {
                    stateViewModel.errorTitle("Oops!");
                    stateViewModel.errorMessage("Looks like something went wrong, please try again");
                }
            });
        };

        listDetailComponentViewModel.prototype.edit = function edit(data) {
            stateViewModel.currentContact(data);
            stateViewModel.showEditModal(true);
        };

        return {
            viewModel: listDetailComponentViewModel,
            template: { require: 'text!/scripts/src/templates/list-detail.html' }
        };
});