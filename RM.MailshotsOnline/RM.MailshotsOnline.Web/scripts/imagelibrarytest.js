$(function () {
    $imageTag = $('#imageTag');
    $publicImageGrid = $('#publicImageGrid');
    $loading = $('#loading');
    $privateLoading = $('#privateLoading');
    $loginForm = $('#loginForm');
    $privateImageGrid = $('#privateImageGrid');
    $noPrivateImages = $('#noPrivateImages');
    $deleteLink = $('#deleteLink');
    $closeLink = $('#closeLink');
    $refreshButton = $('#refreshButton');
    $refreshPrivateImages = $('#refreshPrivateImages');

    function DisplayImageTags(callback) {
        $.ajax({
            url: '/Umbraco/Api/ImageLibrary/GetTags',
            success: function (data) {
                PopulateTags(data, callback);
            },
            statusCode: {
                500: function (response) { HandleError(response); }
            }
        });
    }

    function PopulateTags(tags, callback) {
        
        if (tags.length > 0) {
            var selectItems = '<option value="">No filter</option>';
            for (i = 0; i < tags.length; i++) {
                var tag = tags[i];
                selectItems += '<option value="' + tag + '">' + tag + '</option>';
            }
            
            $imageTag.html(selectItems);
        }
        else {
            $imageTag.attr('disabled', 'disabled');
        }
        if (typeof (callback) != "undefined") {
            callback();
        }
    }

    function FilterImages() {
        var selectedTag = $imageTag.val();
        //console.log(selectedTag);
        GetPublicImages(selectedTag);
    }

    function GetPublicImages(tag, callback) {
        $publicImageGrid.hide();
        $loading.show();
        var getImagesUrl = '/Umbraco/Api/ImageLibrary/GetImages'
        if (typeof (tag) != "undefined") {
            if (tag != '') {
                getImagesUrl += '?tag=' + encodeURIComponent(tag);
            }
        }

        $.ajax({
            url: getImagesUrl,
            type: 'GET',
            success: function (data) {
                DisplayImages(data, $publicImageGrid, $loading, $('#noImages'), callback);
            },
            statusCode: {
                500: function (response) { HandleError(response); }
            }
        });
    }

    function DisplayImages(images, target, loadingTarget, noImagesTarget, callback) {

        if (images.length > 0) {
            var container = $(document.createElement('div'));
            for (i = 0; i < images.length; i++) {
                var image = images[i];
                var link = $(document.createElement('a'))
                    .attr('href', '#openImage/' + image.Src)
                    .data('fullImageUrl', image.Src)
                    .data('usage', image.MailshotUses)
                    .data('mediumImageUrl', image.MediumSrc);
                if (typeof (image.ImageId) != "undefined") {
                    link.data('imageId', image.ImageId);
                }
                link.on('click', function (event) {
                    event.preventDefault();
                    if (typeof ($(this).data('imageId')) != "undefined") {
                        OpenLargeImage($(this).data('mediumImageUrl'), $(this).data('usage'), $(this).data('imageId'));
                    }
                    else {
                        OpenLargeImage($(this).data('mediumImageUrl'), $(this).data('usage'));
                    }
                });
                var thumbnail = $(document.createElement('img')).attr('src', image.SmallSrc);
                link.append(thumbnail);
                container.append(link);
            }
            noImagesTarget.hide();
            target.html('');
            target.append(container);
            target.show();
            loadingTarget.hide();
        }
        else {
            noImagesTarget.show();
            target.hide();
            loadingTarget.hide();
        }

        if (typeof (callback) != "undefined") {
            callback();
        }
    }

    function OpenLargeImage(url, usageCount, imageId) {
        if (typeof (imageId) != "undefined") {
            $('#deletePara').show();
            
            $deleteLink.attr('href', '#/delete/' + imageId).data('imageId', imageId);
        }

        $('#large-image').attr('src', url);
        $('#mailshotUsage').text(usageCount);
        $('.image-viewer').show();
    }

    function ShowLibrary() {
        $('#libraryItems').show();
    }

    function HideLoading() {
        $('#loading').hide();
    }

    function GetPrivateImages(callback) {
        $privateLoading.show();
        $.ajax({
            url: '/Umbraco/Api/ImageLibrary/GetMyImages',
            type: 'GET',
            statusCode: {
                401: function (response) { HandleError(response); },
                500: function (response) { HandleError(response); }
            },
            success: function (images) {
                DisplayImages(images, $privateImageGrid, $privateLoading, $noPrivateImages, function () {
                    ShowNewImageForm();
                    $refreshPrivateImages.show();
                    if (typeof (callback) != "undefined") {
                        callback();
                    }
                });
            }
        });
    }

    function ShowNewImageForm() {
        $('#newImageForm').show();
        $('#imageName').removeAttr('disabled').val('');
        $('#imageBaseString').removeAttr('disabled').val('');
        $('#saveImageButton').text('Save image').removeAttr('disabled');
        var iframeTarget = $('#uploadIframe').attr('src');
        $('#uploadIframe').attr('src', iframeTarget);
    }

    function SaveNewImage() {
        var imageName = $('#imageName').val();
        var imageBaseString = $('#imageBaseString').val();
        var url = '/Umbraco/Api/ImageLibrary/UploadImage';
        var dataValid = true;

        if (imageName.length == 0) {
            alert("You must enter a name for the image");
            $('#imageName').focus();
            dataValid = false;
        }

        if (dataValid && imageBaseString.length == 0) {
            alert("You must enter a valid Base 64 string");
            $('#imageBaseString').focus();
            dataValid = false;
        }

        if (dataValid) {
            $('#imageName').attr('disabled', 'disabled');
            $('#imageBaseString').attr('disabled', 'disabled');
            $('#saveImageButton').text('Saving ...').attr('disabled', 'disabled');
            $.ajax({
                url: url,
                type: 'POST',
                data: { imageString: imageBaseString, name: imageName },
                statusCode: {
                    400: function (response) { HandleError(response); }, 
                    401: function (response) { HandleError(response); },
                    500: function (response) { HandleError(response); }
                },
                success: function (data) {
                    ShowNewImageForm();
                    GetPrivateImages();
                }
            });
        }
    }

    // Start up stuff and bindings
    DisplayImageTags(function () {
        GetPublicImages('', function () {
            $imageTag.on('change', function () {
                FilterImages();
            });

            ShowLibrary();
            HideLoading();
        });
    });

    CheckLoggedInStatus(function (loggedIn) {
        if (loggedIn) {
            GetPrivateImages(function () {
                $('#loginForm').hide();
            });
        }
        else {
            $('#loginForm').show();
        }
    });

    $closeLink.on('click', function (event) {
        event.preventDefault();
        $('.image-viewer').hide();
        $('#deletePara').hide();
    });

    $deleteLink.on('click', function (event) {
        event.preventDefault();
        if (confirm('Are you sure you want to delete this image?')) {
            $.ajax({
                url: '/Umbraco/Api/ImageLibrary/DeleteImage/' + $(this).data('imageId'),
                type: 'DELETE',
                success: function (data) {
                    $('.image-viewer').hide();
                    $('#deletePara').hide();
                    GetPrivateImages();
                },
                statusCode: {
                    500: function (response) { HandleError(response); },
                    401: function (response) { HandleError(response); },
                    404: function (response) { HandleError(response); }
                }
            })
        }
    })

    $refreshButton.on('click', function (event) {
        event.preventDefault();
        GetPrivateImages();
    });

    $('#loginButton').on('click', function (event) {
        event.preventDefault();
        var email = $('#email').val();
        var password = $('#password').val();
        Login(email, password, function (loggedIn) {
            if (loggedIn) {
                GetPrivateImages(function () {
                    $('#loginForm').hide();
                });
            }
        });
    });

    $('#saveImageButton').on('click', function (event) {
        event.preventDefault();
        SaveNewImage();
    })
});