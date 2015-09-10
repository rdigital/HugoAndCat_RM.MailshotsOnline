define(['knockout', 'jquery', 'kofile', 'view_models/state'],

    function(ko, $, kofile, stateViewModel) {

        function imageUploadViewModel(params) {
            this.src = ko.observable();
            this.libraryImage = ko.observable();
            this.fileData = ko.observable({
                dataURL: ko.observable()
            })
            this.selectedTab = ko.observable(stateViewModel.imageTab() || 'upload');
            this.libraryImages = ko.observableArray();
            this.tag = ko.observable();


            this.tags = this.getTagsComputed();
            this.libraryImagesFiltered = this.getLibraryComputed();


            this.fileData.subscribe(this.loadFile, this);
            this.src.subscribe(this.render, this);
            this.libraryImage.subscribe(this.renderLibraryImage, this);

            this.selectLibraryImage = this.selectLibraryImage.bind(this);
            this.selectTab = this.selectTab.bind(this);

            this.fetchLibrary();
        }

        imageUploadViewModel.prototype.getTagsComputed = function getTagsComputed() {
            return ko.pureComputed( function() {
                var tags = [];
                ko.utils.arrayForEach(this.libraryImages(), function(image) {
                    ko.utils.arrayForEach(image.Tags, function(tag) {
                        var match = ko.utils.arrayFirst(tags, function(existing) {
                            return existing.name == tag
                        })
                        if (!match) {
                            tags.push({
                                name: tag,
                                value: tag
                            });
                        }
                    })
                });
                return tags.sort(function (a, b) {
                    return a.name.localeCompare(b.name, 'en', {'sensitivity': 'base'})
                });
            }, this)
        }

        imageUploadViewModel.prototype.getLibraryComputed = function getLibraryComputed() {
            return ko.pureComputed( function() {
                var tag = this.tag();
                if (tag) {
                    return ko.utils.arrayFilter(this.libraryImages(), function(image) {
                        return image.Tags.indexOf(tag) >= 0
                    })
                }
                return this.libraryImages()
            }, this)
        }

        // XXX move this into it's own data model
        imageUploadViewModel.prototype.fetchLibrary = function fetchLibrary() {
            $.get('/Umbraco/Api/ImageLibrary/GetImages', function(data) {
                this.libraryImages(data);
            }.bind(this)).fail(function() {
                console.log('There was an error fetching the image library');
            })
        }

        imageUploadViewModel.prototype.selectLibraryImage = function selectLibraryImage(image) {
            this.libraryImage(image);
            this.src(null);
        }

        imageUploadViewModel.prototype.renderLibraryImage = function renderLibraryImage() {
            stateViewModel.selectedElement().render(this.libraryImage().Src, true);
        }


        imageUploadViewModel.prototype.selectTab = function selectTab(tab) {
            this.selectedTab(tab);
        }

        imageUploadViewModel.prototype.clickUpload = function clickUpload(){
            $('#image-upload').click();
        }

        imageUploadViewModel.prototype.loadFile = function loadFile(fileData){
            this.src(fileData);                     
        }

        imageUploadViewModel.prototype.render = function render() {
            if (this.src()) {
                stateViewModel.selectedElement().render(this.src(), true);
            }
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
            this.toggleImageUpload();
        }

        /**
         * toggles the image upload modal
         */
        imageUploadViewModel.prototype.toggleImageUpload = function toggleImageUpload() {
            $('.canvas-container').css({opacity: 0})
            stateViewModel.toggleImageUpload()
            $('.canvas-container').css({opacity: 1})
        }

        return {
            viewModel: imageUploadViewModel,
            template: { require: 'text!/canvas/templates/upload.html' }
        }
    }
);