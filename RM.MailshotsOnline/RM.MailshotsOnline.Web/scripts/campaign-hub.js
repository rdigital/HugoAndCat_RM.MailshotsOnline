$(function () {
    var $campaignId = $('#campaignId');
    var $campaignTitle = $('#campaignTitle');
    var $editTitleButton = $('#editTitle');
    var $editTitleValue = $('#editTitleValue');

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