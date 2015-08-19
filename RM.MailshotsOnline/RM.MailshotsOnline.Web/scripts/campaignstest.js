$(function () {
    $loading = $('#loading');
    $myCampaigns = $('#myCampaigns');
    $campaignList = $('#campaignList');
    $addCampaignForm = $('#addCampaignForm');

    function GetMyCampaigns(callback) {
        $myCampaigns.show();
        $loading.show();
        $campaignList.hide();

        $.ajax({
            url: '/Umbraco/Api/Campaign/GetAll',
            type: 'GET',
            success: function (data) {
                DisplayCampaigns(data, callback);
            },
            statusCode: {
                400: function (response) {
                    HandleError(response);
                    if (typeof (callback) != "undefined") {
                        callback();
                    }
                },
                401: function (response) {
                    HandleError(response);
                    if (typeof (callback) != "undefined") {
                        callback();
                    }
                },
                500: function (response) {
                    HandleError(response);
                    if (typeof (callback) != "undefined") {
                        callback();
                    }
                }
            }
        });
    }

    function DisplayCampaigns(data, callback) {
        //console.log(data);
        if (data.length > 0) {

        }
        else {
            $campaignList.html('<li>No campaigns to display.</li>').show();
            $loading.hide();
        }

        if (typeof (callback) != "undefined") {
            callback();
        }
    }

    function DisplayCampaignForm() {
        $addCampaignForm.show();
    }

    CheckLoggedInStatus(function (loggedIn) {
        $loading.hide();
        if (loggedIn) {
            GetMyCampaigns(function () { DisplayCampaignForm(); });
        }
        else {
            $('#loginForm').show();
        }
    });

    $('#loginButton').on('click', function (event) {
        event.preventDefault();
        var email = $('#email').val();
        var password = $('#password').val();
        $('#loginButton').text('Logging in ...').attr('disabled', 'disabled');
        Login(email, password, function (loggedIn) {
            if (loggedIn) {
                $('#loginForm').hide();
                GetMyCampaigns(function () { DisplayCampaignForm(); });
            }
            else {
                $('#loginButton').text('Login').removeAttr('disabled');
            }
        });
    });
});