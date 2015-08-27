function RenameCampaign(campaignId, newName, successCallback, errorCallback) {
    $.ajax({
        url: '/Umbraco/Api/Campaign/Rename/' + campaignId,
        data: { name: newName },
        type: 'POST',
        success: function (data) {
            if (typeof (successCallback) != "undefined") {
                successCallback(data);
            }
        },
        statusCode: {
            400: function (response) {
                if (typeof(errorCallback) != "undefined") {
                    errorCallback(response);
                }
            },
            401: function (response) {
                if (typeof (errorCallback) != "undefined") {
                    errorCallback(response);
                }
            },
            403: function (response) {
                if (typeof (errorCallback) != "undefined") {
                    errorCallback(response);
                }
            },
            404: function (response) {
                if (typeof (errorCallback) != "undefined") {
                    errorCallback(response);
                }
            },
            500: function (response) {
                if (typeof (errorCallback) != "undefined") {
                    errorCallback(response);
                }
            }
        }
    })
}