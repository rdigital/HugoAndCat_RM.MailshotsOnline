$(function () {
    $('#new').on('click', function (event) {
        event.preventDefault();
        $('#mailshotId').val('');
        $('#name').val('');
        $('#content').val('');
    });

    $('#save').on('click', function (event) {
        event.preventDefault();
        var mailshotId = $('#mailshotId').val();
        if (mailshotId.length) {
            // UPDATE MAILSHOT
            $('#loadingIndicator').text('Updating ...').show();
            $('#save').attr('disabled', 'disabled');
            var mailshot = {
                name: $('#name').val(),
                contentText: $('#content').val(),
                themeId: 1,
                templateId: 1,
                layoutId: 1,
                draft: true
            };
            $.ajax({
                url: '/Umbraco/Api/Mailshots/Update/' + mailshotId,
                data: mailshot,
                type: 'POST',
                success: function (response) {
                    //console.log(response);
                    RefreshMailshots(function () {
                        $('#mailshotId').val(response.id);
                        $('#save').removeAttr('disabled');
                        $('#loadingIndicator').hide();
                    });
                }
            })
        }
        else {
            // SAVE NEW MAILSHOT
            $('#loadingIndicator').text('Saving ...').show();
            $('#save').attr('disabled', 'disabled');
            var mailshot = {
                name: $('#name').val(),
                contentText: $('#content').val(),
                themeId: 1,
                templateId: 1,
                layoutId: 1,
                draft: true
            };
            $.ajax({
                url: '/Umbraco/Api/Mailshots/Save',
                data: mailshot,
                type: 'POST',
                success: function (response) {
                    //console.log(response);
                    RefreshMailshots(function () {
                        $('#mailshotId').val(response.id);
                        $('#save').removeAttr('disabled');
                        $('#loadingIndicator').hide();
                    });
                }
            })
        }
    });

    RefreshMailshots(function () { });

});

function LoadMailshot(link)
{
    $('#loadingIndicator').text('Loading ...').show();
    var mailshotId = link.data('id');
    $.ajax({
        url: '/Umbraco/Api/Mailshots/Get/' + mailshotId,
        success: function (data) {
            $('#mailshotId').val(data.MailshotId);
            $('#name').val(data.Name);
            $('#content').val(data.ContentText);
            $('#loadingIndicator').hide();
        }
    })
}

function RefreshMailshots(callback) {
    // Fill list of existing mailshots
    var $mailshotList = $('#existingMailshots');
    $.ajax({
        url: '/Umbraco/Api/Mailshots/GetAll/',
        success: function (data) {
            $mailshotList.html('');
            for (var i = 0; i < data.length; i++) {
                var mailshot = data[i];
                var listItem = $(document.createElement('li'));
                var title = $(document.createElement('span')).text(mailshot.Name);
                var editLink = $(document.createElement('a')).attr('href', '#').text('Edit').data('id', mailshot.MailshotId).on('click', function (event) {
                    event.preventDefault();
                    LoadMailshot($(this));
                });
                listItem.append(title);
                listItem.append(editLink);
                $mailshotList.append(listItem);
                callback();
            }
        }
    });
}