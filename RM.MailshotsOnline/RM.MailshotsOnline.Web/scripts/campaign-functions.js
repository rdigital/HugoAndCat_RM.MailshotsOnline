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

function SetCampaignApprovals(url, approved, successCallback, errorCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        data: { Approved: approved },
        success: function (data) {
            if (typeof (successCallback) != "undefined") {
                successCallback(data);
            }
        },
        statusCode: {
            400: function (response) {
                if (typeof (errorCallback) != "undefined") {
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

function SetCampaignDesignApproval(campaignId, approved, successCallback, errorCallback) {
    SetCampaignApprovals('/Umbraco/Api/Campaign/SetMailshotApproval/' + campaignId, approved, successCallback, errorCallback);
}

function SetCampaignDataApproval(campaignId, approved, successCallback, errorCallback) {
    SetCampaignApprovals('/Umbraco/Api/Campaign/SetDataApproval/' + campaignId, approved, successCallback, errorCallback);
}

function SetPostageOption(campaignId, postageOptionId, successCallback, errorCallback) {
    $.ajax({
        url: '/Umbraco/Api/Campaign/SetPostalOption/' + campaignId,
        type: 'POST',
        data: { PostalOptionId: postageOptionId },
        success: function (data) {
            if (typeof (successCallback) != 'undefined') {
                successCallback(data);
            }
        },
        statusCode: {
            400: function (response) {
                if (typeof (errorCallback) != "undefined") {
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