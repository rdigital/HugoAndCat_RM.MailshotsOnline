function GetMyOrders(successCallback, errorCallback) {
    $.ajax({
        url: '/Umbraco/Api/Orders/GetAll/',
        type: 'GET',
        success: function (data) {
            if (typeof (successCallback) != "undefined") {
                successCallback(data);
            }
        },
        statusCode: {
            500: { function (response) { if (typeof (errorCallback) != "undefined") { errorCallback(response); } } },
            401: { function (response) { if (typeof (errorCallback) != "undefined") { errorCallback(response); } } }
        }
    })
}

function CancelOrder(campaignId, successCallback, errorCallback) {
    if (confirm("Are you sure you want to cancel this order?")) {
        $.ajax({
            url: '/Umbraco/Api/Orders/Cancel/' + campaignId,
            type: 'POST',
            success: function (data) {
                if (typeof (successCallback) != "undefined") {
                    successCallback(data);
                }
            },
            statusCode: {
                500: { function (response) { if (typeof (errorCallback) != "undefined") { errorCallback(response); } } },
                401: { function (response) { if (typeof (errorCallback) != "undefined") { errorCallback(response); } } }
            }
        })
    }
}