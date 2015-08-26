define(['knockout', 'view_models/state'],

    function(ko, stateViewModel) {

        function imageUploadViewModel() {
            this.src = ko.observable();
        }

        /**
         * Event handler for drag over during file drag and drop
         */
        imageUploadViewModel.prototype.dragOver = function dragOver(obj, e) {
            e.preventDefault();
        }

        /**
         * drop even handler. Prevents default then loads the image dropped by the user
         */
        imageUploadViewModel.prototype.drop = function drop(obj, e) {
            e.preventDefault();
            obj.loadImage(e.originalEvent.dataTransfer.files[0], e.target);
        }

        /**
         * load dropped image file from user's storage
         * @param  {String} src    image src
         * @param  {Element} target canvas HTML element to render to
         */
        imageUploadViewModel.prototype.loadImage = function(src, target) {
            // only allow images to be loaded
            if(!src.type.match(/image.*/)){
                return;
            }

            var reader = new FileReader();
            reader.onload = function(e){
                this.src(e.target.result);
                //this.render(e.target.result, target);
            }.bind(this);
            reader.readAsDataURL(src);
        }

        /**
         * Call to accept the dropped image and close the modal
         */
        imageUploadViewModel.prototype.confirm = function confirm() {
            var src = this.src();
            if (!src) { return }
            stateViewModel.selectedElement().render(this.src(), true);
            this.toggleImageUpload();
        }

        /**
         * toggles the image upload modal
         */
        imageUploadViewModel.prototype.toggleImageUpload = function toggleImageUpload() {
            stateViewModel.toggleImageUpload()
        }

        return {
            viewModel: imageUploadViewModel,
            template: { require: 'text!/canvas/templates/upload.html' }
        }
    }
);