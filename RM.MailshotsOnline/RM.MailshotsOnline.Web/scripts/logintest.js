$(function () {

    function CheckLoggedInStatus(callback) {
        $.ajax({
            url: '/Umbraco/Api/Members/GetLoggedInStatus',
            type: 'GET',
            success: function (data) {
                var loggedInMessage = 'You are logged in';
                if (data.loggedIn != true) {
                    loggedInMessage = 'Not logged in';
                }
                $('#loggedInStatus').val(loggedInMessage);

                callback(data.loggedIn);
            }
        });
    }

    function Login() {
        $.ajax({
            url: '/Umbraco/Api/Members/Login',
            type: 'POST',
            data: { Email: $('#email').val(), Password: $('#password').val()},
            success: function (data) {
                console.log(data);
                $('#checkLoggedInStatus').trigger('click');
            },
            statusCode: {
                400: function (response) { HandleError(response); }
            }
        });
    }

    function Logout() {
        $.ajax({
            url: '/Umbraco/Api/Members/Logout',
            type: 'POST',
            success: function (data) {
                console.log(data);
                $('#checkLoggedInStatus').trigger('click');
            }
        });
    }

    function Register() {
        var registration = {
            Email: $('#registerEmail').val(),
            Title: $('#registerTitle').val(),
            FirstName: $('#registerFirstname').val(),
            LastName: $('#registerLastname').val(),
            Password: $('#registerPassword').val(),
            'g-recaptcha-response': $('#g-recaptcha-response').val(),
            AgreeToRoyalMailContact: $('#registerAgreeRmContact').is(':checked'),
            AgreeToThirdPartyContact: $('#registerAgreeThirdPartyContact').is(':checked')
        };
        $.ajax({
            url: '/Umbraco/Api/Members/Register',
            type: 'POST',
            data: registration,
            success: function (data) {
                console.log(data);
                alert("You have successfully reigistered!");
                $('#checkLoggedInStatus').trigger('click');
            },
            statusCode: {
                400: function (response) { HandleError(response); },
                409: function (response) { HandleError(response); }
            }
        });
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

    $('#checkLoggedInStatus').on('click', function (event) {
        event.preventDefault();
        CheckLoggedInStatus(function (loggedIn) {
            if (loggedIn) {
                $('#login').attr('disabled', 'disabled');
                $('#logout').removeAttr('disabled');
                $('#register').attr('disabled', 'disabled');
            }
            else {
                $('#logout').attr('disabled', 'disabled');
                $('#login').removeAttr('disabled');
                $('#register').removeAttr('disabled');
            }
        });
    });

    $('#login').on('click', function (event) {
        event.preventDefault();
        Login();
    });

    $('#logout').on('click', function (event) {
        event.preventDefault();
        Logout();
    })

    $('#register').on('click', function (event) {
        event.preventDefault();
        Register();
    });
});