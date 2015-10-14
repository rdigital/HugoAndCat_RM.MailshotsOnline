define(['knockout', 'view-models/state', 'koelement', 'kofile', 'koMapping'],

    function(ko, stateViewModel, koelement, kofile, koMapping) {

        function uploadDataComponentViewModel() {
            this.uploadEl = ko.observable();
            this.fileData = ko.observable();
            this.fileString = ko.observable();
            this.loading = ko.observable(false);
            this.currentList = stateViewModel.currentList;

            this.fileData.subscribe(this.postData, this);
        }

        uploadDataComponentViewModel.prototype.clickUpload = function clickUpload(){
            this.uploadEl().val('');
            this.fileData('');
            this.uploadEl().click();
        };

        uploadDataComponentViewModel.prototype.dragOver = function dragOver(obj, e) {
            e.preventDefault();
        };

        uploadDataComponentViewModel.prototype.drop = function drop(obj, e) {
            e.preventDefault();
            obj.readFile(e.originalEvent.dataTransfer.files[0]);
        };

        uploadDataComponentViewModel.prototype.readFile = function readFile(src) {
        	// only allow CSVs to be loaded
            if(!src.type.match(/application\/vnd.ms-excel/)){
                return;
            }
            if (typeof(FileReader) === 'undefined') {
                return;
            }
            var reader = new FileReader();
           	reader.onloadend = function(){
                this.postData(reader.result);
            }.bind(this);
            reader.readAsDataURL(src);
        };

        uploadDataComponentViewModel.prototype.addContact = function addContact() {
            stateViewModel.currentContact({});
            stateViewModel.showEditModal(true);
            stateViewModel.addNewContact(true);
        };

        uploadDataComponentViewModel.prototype.postData = function postData(fileData) {
        	var self = this;
        	this.loading(true); 

            if (this.fileData() === '') {
                return;
            }           

        	var data = {
        		"DistributionListId": "",
        		"ListName": stateViewModel.currentList.ListName(),
        		"CsvString": fileData
        	};

        	$.post('/Umbraco/Api/DistributionList/PostUploadCsv', data, function(result) {
        	    koMapping.fromJS(result, self.currentList);
        	    stateViewModel.createListStep('match');
        	    self.loading(false);
        	}).fail(function(error) {
        	    stateViewModel.showError(true);
        	    if (error.reponseJSON) {
                    stateViewModel.errorTitle(error.responseJSON.error);
                } else {
                	stateViewModel.errorTitle("Oops!");
                	stateViewModel.errorMessage("Looks like something went wrong, please try again");
                }
        	    self.loading(false);
        	});
        };

        return {
            viewModel: uploadDataComponentViewModel,
            template: { require: 'text!/scripts/src/templates/upload-data.html' }
        };
});