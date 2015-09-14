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