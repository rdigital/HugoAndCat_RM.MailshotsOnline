// viewmodel to handle my images data
define(['knockout', 'view-models/notification'],
    function(ko, notificationViewModel) {

        function uploadImageViewModel() {
            this.src = ko.observable();
            console.log(notificationViewModel);
            this.fileData = ko.observable({
                dataURL: ko.observable()
            });
            this.fileData.subscribe(this.loadFile, this);
        }

        uploadImageViewModel.prototype.loadFile = function loadFile(fileData){
            this.src(fileData);         
        };

        /**
         * Event handler for drag over during file drag and drop
         */
        uploadImageViewModel.prototype.dragOver = function dragOver(obj, e) {
            e.preventDefault();
        };

        /**
         * drop even handler. Prevents default then loads the image dropped by the user
         */
        uploadImageViewModel.prototype.drop = function drop(obj, e) {
            e.preventDefault();
            obj.loadImage(e.originalEvent.dataTransfer.files[0], e.target);
        };

        /**
         * load dropped image file from user's storage
         * @param  {String} src    image src
         * @param  {Element} target canvas HTML element to render to
         */
        uploadImageViewModel.prototype.loadImage = function(src) {
            // only allow images to be loaded
            if(!src.type.match(/image.*/)){
                return;
            }
            if (typeof(FileReader) === 'undefined') {
                return;
            }
            var reader = new FileReader();
            reader.onload = function(e){
                this.src(e.target.result);
                //this.render(e.target.result, target);
            }.bind(this);
            reader.readAsDataURL(src);
        };


        return {
            viewModel: uploadImageViewModel,
            template: { require: 'text!/scripts/src/templates/upload-image.html' }
        };
    }
);