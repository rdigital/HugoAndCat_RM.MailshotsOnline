function CheckLoggedInStatus(callback) {
    $.ajax({
        url: '/Umbraco/Api/Members/GetLoggedInStatus',
        type: 'GET',
        success: function (data) {
            if (typeof (callback) != "undefined") {
                callback(data.loggedIn);
            }
        }
    });
}

function Login(email, password, callback) {
    $.ajax({
        url: '/Umbraco/Api/Members/Login',
        type: 'POST',
        data: { Email: email, Password: password },
        success: function (data) {
            if (typeof (callback) != "undefined") {
                callback(data);
            }
        },
        statusCode: {
            400: function (response) {
                HandleError(response);
                if (typeof (callback) != "undefined") {
                    callback(false);
                }
            }
        }
    });
}

function HandleError(response) {
    console.log(response);
    if (typeof (grecaptcha) != "undefined") {
        grecaptcha.reset();
    }
    var errorMessage = response.responseJSON.error;
    if (response.responseJSON.fieldErrors) {
        for (i = 0; i < response.responseJSON.fieldErrors.length; i++) {
            errorMessage += "\n" + response.responseJSON.fieldErrors[i];
        }
    }
    alert(errorMessage);
}