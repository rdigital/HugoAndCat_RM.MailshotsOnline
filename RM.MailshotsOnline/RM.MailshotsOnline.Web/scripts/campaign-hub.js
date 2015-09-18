function PreviewDesign(mailshotId, successCallback, failureCallback) {
    $.ajax({
        url: '/Umbraco/Api/ProofPdf/CreateProof/' + mailshotId,
        type: 'POST',
        success: function (createProofData) {
            AwaitPreview(mailshotId, successCallback, failureCallback);
        },
        statusCode: {
            400: function (response) { failureCallback(response); },
            401: function (response) { failureCallback(response); },
            403: function (response) { failureCallback(response); },
            404: function (response) { failureCallback(response); },
            409: function (response) { failureCallback(response); },
            500: function (response) { failureCallback(response); }
        }
    });
}

function AwaitPreview(mailshotId, successCallback, failureCallback) {
    $.ajax({
        url: '/Umbraco/Api/ProofPdf/Get/' + mailshotId,
        type: 'GET',
        success: function (result) {
            if (typeof (successCallback) != 'undefined') {
                successCallback(result);
            }
        },
        statusCode: {
            412: function () {
                window.setTimeout(function () { AwaitPreview(mailshotId, successCallback, failureCallback); }, 1000);
            },
            400: function (response) { failureCallback(response); },
            500: function (response) { failureCallback(response); },
            404: function (response) { failureCallback(response); }
        }
    })
}

$(function () {
    var $campaignId = $('#campaignId');
    var $campaignTitle = $('#campaignTitle');
    var $editTitleButton = $('#editTitle');
    var $editTitleValue = $('#editTitleValue');
    var $dataApproval = $('#dataApproval');
    var $designApproval = $('#designApproval');
    var $unapproveData = $('#unapproveData');
    var $unapproveDesign = $('#unapproveDesign');
    var $postalOptions = $('#postalOptions');
    var $previewDesign = $('#previewDesign');

    $previewDesign.on('click', function (event) {
        event.preventDefault();
        var proofPdfUrl = $('#previewPdfUrl').val();
        if (proofPdfUrl.length > 0) {
            window.open(proofPdfUrl);
        }
        else {
            $previewDesign.attr('disabled', 'disabled').text('Creating');
            var mailshotId = $('#mailshotId').val();
            PreviewDesign(
                mailshotId,
                function (data) {
                    console.log(data);
                    $('#previewPdfUrl').val(data.url);
                    window.open(data.url);
                    $previewDesign.removeAttr('disabled').text('Preview');
                },
                function (response) {
                    console.log(response);
                    alert("Unable to generate preview PDF.");
                    $previewDesign.removeAttr('disabled').text('Preview');
                });
        }
    });

    $postalOptions.on('change', function (event) {
        $postalOptions.attr('disabled', 'disabled');
        SetPostageOption(
            $campaignId.val(),
            $postalOptions.val(),
            function (data) {
                location.reload();
            },
            function (response) {
                alert(response);
            }
        );
    })

    $unapproveData.on('click', function (event) {
        event.preventDefault();
        SetCampaignDataApproval(
            $campaignId.val(),
            false,
            function (data) {
                location.reload();
            },
            function (response) {
                alert(response);
            }
        );
    })

    $dataApproval.on('click', function (event) {
        event.preventDefault();
        $dataApproval.attr('disabled', 'disabled');
        SetCampaignDataApproval(
            $campaignId.val(),
            true,
            function (data) {
                location.reload();
            },
            function (response) {
                alert(response);
                $dataApproval.removeAttr('disabled');
            }
        );
    });

    $designApproval.on('click', function (event) {
        event.preventDefault();
        $designApproval.attr('disabled', 'disabled');
        SetCampaignDesignApproval(
            $campaignId.val(),
            true,
            function (data) {
                location.reload();
            },
            function (response) {
                alert(response);
                $designApproval.removeAttr('disabled');
            }
        );
    });

    $unapproveDesign.on('click', function (event) {
        event.preventDefault();
        SetCampaignDesignApproval(
            $campaignId.val(),
            false,
            function (data) {
                location.reload();
            },
            function (response) {
                alert(response);
            }
        );
    });

    $editTitleButton.on('click', function (event) {
        event.preventDefault();
        $editTitleValue.show();
        $editTitleValue.focus();
        $campaignTitle.hide();
        $editTitleButton.hide();
    });

    $editTitleValue.on('focusout', function (event) {
        $editTitleValue.attr('disabled', 'disabled');
        RenameCampaign(
            $campaignId.val(),
            $editTitleValue.val(),
            function (data) {
                $editTitleValue.removeAttr('disabled');
                $campaignTitle.text($editTitleValue.val());
                $editTitleValue.hide();
                $campaignTitle.show();
                $editTitleButton.show();
            },
            function (response) {
                alert("Unable to update title");
                $editTitleValue.removeAttr('disabled');
            }
        );
    });
});