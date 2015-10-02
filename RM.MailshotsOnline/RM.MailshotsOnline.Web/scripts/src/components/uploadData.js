define(['knockout', 'view-models/state', 'koelement', 'kofile'],

    function(ko, stateViewModel, koelement, kofile) {

        function uploadDataComponentViewModel() {
            this.uploadEl = ko.observable();
            this.fileData = ko.observable();
            this.fileString = ko.observable();

            this.fileData.subscribe(this.postData, this);
        }

        /*uploadDataComponentViewModel.prototype.loadFile = function loadFile(fileData) {
        	this.fileString(fileData)
        	console.log('click upload', this.fileString());
        }*/

        uploadDataComponentViewModel.prototype.clickUpload = function clickUpload(){
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
            if (typeof(FileReader) == 'undefined') {
                return
            }
            var reader = new FileReader();
           	reader.onloadend = function(){
                //this.fileString(reader.result);
                //console.log('drag upload', this.fileString());
                this.postData(reader.result);
            }.bind(this);
            reader.readAsDataURL(src);
        };

        uploadDataComponentViewModel.prototype.postData = function postData(fileData) {
        	var data = {
        		"DistributionListId": "",
        		"ListName": stateViewModel.listTitle(),
        		"CsvString": fileData
        	}

        	console.log(data);


        	$.post('/Umbraco/Api/DistributionList/PostUploadCsv', data, function(result) {
        	    console.log(result);
        	}).fail(function(error) {
        	    console.log('There was an error uploading your file', error);
        	});
        }

        return {
            viewModel: uploadDataComponentViewModel,
            template: { require: 'text!/scripts/src/templates/upload-data.html' }
        };
});