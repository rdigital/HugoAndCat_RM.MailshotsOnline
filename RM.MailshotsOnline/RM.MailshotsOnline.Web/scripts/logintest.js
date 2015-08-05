$(function () {

    function CheckLoggedInStatus(callback) {
        $.ajax({
            url: '/Umbraco/Api/Members/CurrentlyLoggedIn',
            type: 'POST',
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
                400: function (response) {
                    console.log(response);
                    alert(response.responseJSON.error);
                }
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

    $('#checkLoggedInStatus').on('click', function (event) {
        event.preventDefault();
        CheckLoggedInStatus(function (loggedIn) {
            if (loggedIn) {
                $('#login').attr('disabled', 'disabled');
                $('#logout').removeAttr('disabled');
            }
            else {
                $('#logout').attr('disabled', 'disabled');
                $('#login').removeAttr('disabled');
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
});