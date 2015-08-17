$(function () {

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

    $('#checkLoggedInStatus').on('click', function (event) {
        event.preventDefault();
        CheckLoggedInStatus(function (loggedIn) {
            var loggedInMessage = 'You are logged in';
            if (loggedIn != true) {
                loggedInMessage = 'Not logged in';
            }
            $('#loggedInStatus').val(loggedInMessage);
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
        var email = $('#email').val();
        var password = $('#password').val();
        Login(email, password, function (data) {
            console.log(data);
            $('#checkLoggedInStatus').trigger('click');
        });
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