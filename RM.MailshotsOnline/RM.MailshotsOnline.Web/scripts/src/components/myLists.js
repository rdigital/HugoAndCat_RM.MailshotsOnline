define(['jquery', 'knockout', 'view-models/state', 'view-models/notification'],

    function($, ko, stateViewModel, notificationViewModel) {

        // ViewModel
        function myListsComponentViewModel() {

            this.lists = stateViewModel.lists;
            this.displayedList = ko.observableArray();
            this.deleteLists = ko.observableArray();
            this.totalListCount = ko.pureComputed(function() {
                return this.lists().length;
            }, this);
            this.selectedLists = stateViewModel.selectedLists;
            this.createUrl = ko.observable(window.createUrl);
            this.listDetailBaseUrl = window.listDetailBaseUrl;

            // bound methods
            this.toggleSelect = this.toggleSelect.bind(this);
            this.clearDeleteLists = this.clearDeleteLists.bind(this);
            this.deleteLists = this.deleteLists.bind(this);
            this.remove = this.remove.bind(this);

            $.get('/Umbraco/Api/DistributionList/GetMyLists', function(data) {
                data.forEach(function(list){
                    list.selected = ko.observable(false);
                });
                this.lists(data);
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

        myListsComponentViewModel.prototype.deleteSelected = function deleteSelected() {
            this.deleteLists(this.selectedLists());
        }

        myListsComponentViewModel.prototype.clearDeleteLists = function clearDeleteLists() {
            this.deleteLists([]);
        }

        myListsComponentViewModel.prototype.remove = function remove() {
            var lists = this.deleteLists(),
                ids = [];
            ko.utils.arrayForEach(lists, function(list) {
                ids.push(list.DistributionListId);
            });

            notificationViewModel.show('Deleting lists', 'message');

            $.ajax({
                url: '/Umbraco/Api/DistributionList/DeleteMultipleLists',
                method: 'DELETE',
                data: {
                    Ids: ids
                },
                success: function(result) {
                    // Do something with the result
                    if (result.failure.length) {
                        notificationViewModel.hideWithMessage('Error deleting ' + result.failure.length +' lists', 'error');
                    } else {
                        notificationViewModel.hideWithMessage('Lists deleted', 'message');
                    }
                    this.lists.remove(function(list) {
                        return result.success.indexOf(list.DistributionListId) > -1;
                    });
                }.bind(this),
                error: function() {
                    notificationViewModel.show('There was an error deleting lists', 'error');
                }.bind(this)
            }).always(function() {
                this.selectedLists([]);
                this.deleteLists([]);
            }.bind(this));
            return false;
        }

        return {
            viewModel: myListsComponentViewModel,
            template: { require: 'text!/scripts/src/templates/' + ((window.manageLists) ? 'manage-my-lists.html' : 'my-lists.html') }
        };
});