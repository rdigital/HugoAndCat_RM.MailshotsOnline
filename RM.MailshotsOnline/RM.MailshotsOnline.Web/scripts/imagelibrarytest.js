$(function () {
    $imageTag = $('#imageTag');
    $publicImageGrid = $('#publicImageGrid');
    $loading = $('#loading');

    function DisplayImageTags(callback) {
        $.ajax({
            url: '/Umbraco/Api/ImageLibrary/GetTags',
            success: function (data) {
                console.log(data);
                PopulateTags(data, callback);
            },
            statusCode: {
                500: function (response) { HandleError(response); }
            }
        });
    }

    function PopulateTags(tags, callback) {
        
        if (tags.length > 0) {
            var selectItems = '<option value="">Please select</option>';
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
                console.log(data);
                DisplayPublicImages(data, callback);
           } 
        })
    }

    function DisplayPublicImages(images, callback) {

        if (images.length > 0) {
            var container = $(document.createElement('div'));
            for (i = 0; i < images.length; i++) {
                var image = images[i];
                var link = $(document.createElement('a'))
                    .attr('href', '#openImage/' + image.Src)
                    .data('fullImageUrl', image.Src)
                    .data('usage', image.MailshotUses)
                    .data('mediumImageUrl', image.MediumSrc);
                link.on('click', function (event) {
                    event.preventDefault();
                    OpenLargeImage($(this).data('mediumImageUrl'), $(this).data('usage'));
                });
                var thumbnail = $(document.createElement('img')).attr('src', image.SmallSrc);
                link.append(thumbnail);
                container.append(link);
            }
            $('#noImages').hide();
            $('#publicImageGrid').html('');
            $('#publicImageGrid').append(container);
            $('#publicImageGrid').show();
            $loading.hide();
        }
        else {
            $('#noImages').show();
            $('#publicImageGrid').hide();
            $loading.hide();
        }

        if (typeof (callback) != "undefined") {
            callback();
        }
    }

    function OpenLargeImage(url, usageCount) {
        $('#large-image').attr('src', url);
        $('#mailshotUsage').text(usageCount);
        $('.image-viewer').show()
    }

    function HandleError(response) {
        console.log(response);
        grecaptcha.reset();
        var errorMessage = response.responseJSON.error;
        if (response.responseJSON.fieldErrors) {
            for (i = 0; i < response.responseJSON.fieldErrors.length; i++) {
                errorMessage += "\n" + response.responseJSON.fieldErrors[i];
            }
        }
        alert(errorMessage);
    }

    function ShowLibrary() {
        $('#libraryItems').show();
    }

    function HideLoading() {
        $('#loading').hide();
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

    $('.image-viewer a').on('click', function (event) {
        event.preventDefault();
        $('.image-viewer').hide();
    })
});