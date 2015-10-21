define(['knockout', 'jquery', 'kofile', 'view_models/myimages', 'view_models/user', 'view_models/notification', 'view_models/history', 'view_models/state', 'view_models/auth'],

    function(ko, $, kofile, myImagesViewModel, userViewModel, notificationViewModel, historyViewModel, stateViewModel, authViewModel) {

        function imageUploadViewModel(params) {
            this.src = ko.observable();
            this.fileData = ko.observable({
                dataURL: ko.observable()
            });
            this.deleteImage = ko.observable();
            this.selectedTab = ko.observable(stateViewModel.imageTab() || 'upload');
            this.selectedElement = stateViewModel.selectedElement();
            this.isAuthenticated = authViewModel.isAuthenticated;

            // XXX note to whoever has time, move these into their own data model so
            // they can be persisted between opening up the image panel
            this.libraryImage = ko.observable();
            this.libraryImages = ko.observableArray();
            this.tag = ko.observable();
            this.libraryLoading = ko.observable(true);

            // my image library
            this.myImage = myImagesViewModel.selected;
            this.myImages = myImagesViewModel.objects;
            this.myImagesLoading = myImagesViewModel.loading;
            this.selectMyImage = myImagesViewModel.select;
            this.removeMyImage = myImagesViewModel.remove;

            // computeds
            this.tags = this.getTagsComputed();
            this.libraryImagesFiltered = this.getLibraryComputed();

            // subscriptions
            this.fileData.subscribe(this.loadFile, this);
            this.src.subscribe(this.render, this);
            this.libraryImage.subscribe(this.renderLibraryImage, this);
            this.subscription = this.myImage.subscribe(this.renderMyImage, this);

            // bound functions
            this.selectLibraryImage = this.selectLibraryImage.bind(this);
            this.selectTab = this.selectTab.bind(this);
            this.dispose = this.dispose.bind(this);
            this.setDeleteImage = this.setDeleteImage.bind(this);
            this.clearDeleteImage = this.clearDeleteImage.bind(this);

            this.elements = userViewModel.objects.elements;

            this.fetchLibrary();
        }

        imageUploadViewModel.prototype.dispose = function dispose() {
            this.subscription.dispose();
            var element = this.selectedElement;
            if (this.src() || this.libraryImage() || this.myImage()) {
                // we are updating the image, set element src to null
                element.setUrlSrc(null);
            }
            if (this.src()) {

                // we are uploading a new image
                notificationViewModel.show('Uploading Image...')

                var name = this.getRandomInt().toString(),
                    data = {
                        imageString: this.src(),
                        name: this.getRandomInt().toString() 
                    },
                    imageSrc = '',
                    post = $.post('/Umbraco/Api/ImageLibrary/UploadImage', data, function(image) {
                        image.deleting = ko.observable(false);
                        myImagesViewModel.add(image);
                        element.setUrlSrc(image.Src);
                        imageSrc = image.Src;
                        notificationViewModel.hideWithMessage("Upload complete!");
                    }).fail(function(error) {
                        notificationViewModel.show("Error uploading image", "error");
                        element.setUrlSrc(null);
                    }).always(function() {
                        stateViewModel.uploadingImages.remove(name);
                        historyViewModel.replaceUrlSrc(name, imageSrc);
                    });

                // push the unique identifier for this upload onto the upload trackin array
                stateViewModel.uploadingImages.push(name);
                
                // temporarily set the URL source to be the ranomly generated
                // number. Upon completion, we can swap out this integer in the history
                // so that undeoing / redoing changes made while the image was uploading
                // does not lose the URL
                element.setUrlSrc(name);
            }
            this.selectMyImage(null);
        };

        imageUploadViewModel.prototype.getRandomInt = function getRandomInt() {
            var min = 0,
                max = 99999999;
            return Math.floor(Math.random() * (max - min)) + min;
        };

        imageUploadViewModel.prototype.getTagsComputed = function getTagsComputed() {
            return ko.pureComputed( function() {
                var tags = [];
                ko.utils.arrayForEach(this.libraryImages(), function(image) {
                    ko.utils.arrayForEach(image.Tags, function(tag) {
                        var match = ko.utils.arrayFirst(tags, function(existing) {
                            return existing.name == tag;
                        });
                        if (!match) {
                            tags.push({
                                name: tag,
                                value: tag
                            });
                        }
                    });
                });
                return tags.sort(function (a, b) {
                    return a.name.localeCompare(b.name, 'en', {'sensitivity': 'base'});
                });
            }, this);
        };

        imageUploadViewModel.prototype.getLibraryComputed = function getLibraryComputed() {
            return ko.pureComputed( function() {
                var tag = this.tag();
                if (tag) {
                    return ko.utils.arrayFilter(this.libraryImages(), function(image) {
                        return image.Tags.indexOf(tag) >= 0;
                    });
                }
                return this.libraryImages();
            }, this);
        };

        // XXX move this into it's own data model
        imageUploadViewModel.prototype.fetchLibrary = function fetchLibrary() {
            $.get('/Umbraco/Api/ImageLibrary/GetImages', function(data) {
                this.libraryLoading(false);
                this.libraryImages(data);
            }.bind(this)).fail(function() {
                console.log('There was an error fetching the image library');
            });
        };

        imageUploadViewModel.prototype.selectLibraryImage = function selectLibraryImage(image) {
            if (image) {
                this.libraryImage(image);
            }
        };

        imageUploadViewModel.prototype.renderLibraryImage = function renderLibraryImage() {
            if (this.libraryImage()) {
                this.selectedElement.render(this.libraryImage().Src, true);
                this.selectMyImage(null);
                this.src(null);
            }
        };

        imageUploadViewModel.prototype.renderMyImage = function renderMyImage() {
            if (this.myImage()) {
                this.selectedElement.render(this.myImage().Src, true);
                this.libraryImage(null);
                this.src(null);
            }
        };

        imageUploadViewModel.prototype.selectTab = function selectTab(tab) {
            this.selectedTab(tab);
        };

        imageUploadViewModel.prototype.clickUpload = function clickUpload(){
            $('#image-upload').click();
        };

        imageUploadViewModel.prototype.loadFile = function loadFile(fileData){
            this.src(fileData);         
        };

        imageUploadViewModel.prototype.render = function render() {
            if (this.src()) {
                this.selectedElement.render(this.src(), true);
                this.libraryImage(null);
                this.selectMyImage(null);
            }
        };

        /**
         * Event handler for drag over during file drag and drop
         */
        imageUploadViewModel.prototype.dragOver = function dragOver(obj, e) {
            e.preventDefault();
        };

        /**
         * drop even handler. Prevents default then loads the image dropped by the user
         */
        imageUploadViewModel.prototype.drop = function drop(obj, e) {
            e.preventDefault();
            obj.loadImage(e.originalEvent.dataTransfer.files[0], e.target);
        };

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
            if (typeof(FileReader) == 'undefined') {
                return
            }
            var reader = new FileReader();
            reader.onload = function(e){
                this.src(e.target.result);
                //this.render(e.target.result, target);
            }.bind(this);
            reader.readAsDataURL(src);
        };

        /**
         * Call to accept the dropped image and close the modal
         */
        imageUploadViewModel.prototype.confirm = function confirm() {
            this.toggleImageUpload();
        };

        /**
         * toggles the image upload modal
         */
        imageUploadViewModel.prototype.toggleImageUpload = function toggleImageUpload() {
            $('.canvas-container').css({opacity: 0});
            stateViewModel.toggleImageUpload();
            $('.canvas-container').css({opacity: 1});
        };

        imageUploadViewModel.prototype.setDeleteImage = function setDeleteImage(image) {
            // this covers images in use on this mailshot before the mailshot is saved
            var error = false;
            ko.utils.arrayForEach(this.elements(), function(el) { 
                if (el.writeSrc) { 
                    if (image.OriginalUrl == el.writeSrc()) {
                        error = true;
                    }
                }
            });
            // this covers images in use on other mailshots
            if (image.MailshotUses || error) {
                notificationViewModel.show('This image is currently in use on a campaign. It cannot be deleted.', 'error');
                return;
            }
            this.deleteImage(image);
        }

        imageUploadViewModel.prototype.clearDeleteImage = function clearDeleteImage() {
            this.deleteImage(null);
        }

        return {
            viewModel: imageUploadViewModel,
            template: { require: 'text!/canvas/templates/upload.html' }
        };
    }
);