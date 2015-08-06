$(function () {
    $('#new').on('click', function (event) {
        event.preventDefault();
        ClearEditorForm();
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
                        LoadMailshot(response.id);
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
                        LoadMailshot(response.id);
                    });
                }
            })
        }
    });

    RefreshMailshots(function () { });

});

function LoadMailshot(mailshotId)
{
    $('#loadingIndicator').text('Loading ...').show();
    $.ajax({
        url: '/Umbraco/Api/Mailshots/Get/' + mailshotId,
        success: function (data) {

            $('#mailshotId').val(data.MailshotId);
            $('#name').val(data.Name);
            $('#content').val(data.ContentText);
            DisplayProofStatus(data.ProofPdfStatus);
            if (data.ProofPdfStatus == 3) {
                $('#pdfLink').attr('href', data.ProofPdfUrl).show();
            }
            else {
                $('#pdfLink').attr('href', '#').hide();
            }
            $('#createPdf').removeAttr('disabled');
            $('#createPdf').on('click', function (event) {
                event.preventDefault();
                $('#pdfButtonContainer').hide();
                $('#pdfLoading').show();
                $.ajax({
                    url: '/Umbraco/Api/ProofPdf/CreateProof/' + data.MailshotId,
                    type: 'POST',
                    success: function (createProofData) {
                        $('#pdfButtonContainer').show();
                        $('#pdfLoading').hide();
                        DisplayProofStatus(2);
                        LoadMailshot(data.MailshotId);
                    }
                });
            });
            $('#loadingIndicator').hide();
            $('#save').removeAttr('disabled');
        }
    })
}

function DisplayProofStatus(proofStatusCode)
{
    var proofStatus = "Not requested";
    switch (proofStatusCode) {
        case 2:
            proofStatus = "Pending";
            break;
        case 3:
            proofStatus = "Complete";
            break;
        case 4:
            proofStatus = "Failed";
            break;
    }
    $('#proofStatus').val(proofStatus);
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
                var editLink = $(document.createElement('a')).attr('href', '#/open/' + mailshot.MailshotId).attr('class', 'openLink').text('Open').data('id', mailshot.MailshotId).on('click', function (event) {
                    event.preventDefault();
                    LoadMailshot($(this).data('id'));
                });
                var deleteLink = $(document.createElement('a')).attr('href', '#/delete/' + mailshot.MailshotId).attr('class', 'deleteLink').text('Delete').data('id', mailshot.MailshotId).on('click', function (event) {
                    event.preventDefault();
                    DeleteMailshot($(this).data('id'));
                });
                listItem.append(title);
                listItem.append(editLink);
                listItem.append(deleteLink);
                $mailshotList.append(listItem);
                callback();
            }
        }
    });
}

function DeleteMailshot(mailshotId) {
    if (confirm("Do you really want to delete this mailshot?")) {
        $.ajax({
            url: '/Umbraco/Api/Mailshots/Delete/' + mailshotId,
            type: 'DELETE',
            success: function() {
                RefreshMailshots();
                ClearEditorForm();
            }
        });
    }
}

function ClearEditorForm() {
    $('#mailshotId').val('');
    $('#name').val('');
    $('#content').val('');
    $('#proofStatus').val('');
    $('#pdfLink').hide();
    $('#createPdf').attr('disabled','disabled');
    $('#pdfLoading').hide();
    $('#save').removeAttr('disabled');
}