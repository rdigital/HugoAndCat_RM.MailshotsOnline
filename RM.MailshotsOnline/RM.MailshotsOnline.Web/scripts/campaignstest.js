$(function () {
    var $loading = $('#loading');
    var $myCampaigns = $('#myCampaigns');
    var $campaignList = $('#campaignList');
    var $addCampaignForm = $('#addCampaignForm');
    var $saveCampaignButton = $('#saveCampaign');
    var $mailshotList = $('#mailshot');
    var $postageList = $('#postalOption');
    var $formTitle = $('#addCampaignForm h2');
    var $readonlyFields = $('#readonlyFields');
    var $noCampaignsMessage = $('#noCampaignsMessage');
    var $newCampaignButton = $('#newCampaignButton');

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
            $noCampaignsMessage.hide();
            //$campaignList.html('<tr><th>Name</th><th>Updated</th><th>Status</th><th>Mailshot</th><th>Own data</th><th>Rented data</th><th>Postage</th><th></th><th></th><th></th><th></th></tr>').show();
            $campaignList.html('<tr><th>Name</th><th>Updated</th><th>Status</th><th>Mailshot</th><th>Own data</th><th>Rented data</th><th>Postage</th><th></th><th></th><th></th></tr>').show();
            $loading.hide();
            for (i = 0; i < data.length; i++) {
                var campaign = data[i];
                var updatedDate = new Date(campaign.UpdatedDate);

                var row = $(document.createElement('tr'));
                var name = $(document.createElement('td')).text(campaign.Name);
                var updated = $(document.createElement('td')).text(updatedDate.toDateString());
                var statusText = GetStatusText(campaign.Status);
                var status = $(document.createElement('td')).text(statusText);
                if (statusText == "Draft") {
                    status.attr('class', 'draft');
                }
                if (statusText == "Ready") {
                    status.attr('class', 'ready');
                }

                var mailshot = $(document.createElement('td'))
                if (campaign.MailshotTitle != null) {
                    mailshot.text(campaign.MailshotTitle);
                }
                else {
                    mailshot.attr('class', 'empty').text('None selected');
                }
                
                //var dataSelected = $(document.createElement('td'))
                //if (campaign.HasDataSearches || campaign.HasDistributionLists) {
                //    dataSelected.text('Yes');
                //}
                //else {
                //    dataSelected.attr('class', 'empty').text('No');
                //}

                var ownData = $(document.createElement('td')).text(campaign.OwnDataRecipientCount);
                if (campaign.OwnDataRecipientCount == 0) {
                    ownData.attr('class', 'empty');
                }

                var rentedData = $(document.createElement('td')).text(campaign.RentedDataRecipientCount);
                if (campaign.RentedDataRecipientCount == 0) {
                    rentedData.attr('class', 'empty');
                }

                var postalOption = $(document.createElement('td'));
                if (campaign.PostalOption != null) {
                    postalOption.text(campaign.PostalOption.Name);
                }
                else {
                    postalOption.text('None selected').attr('class','empty');
                }

                //var editLink = $(document.createElement('a'))
                //    .text('Open')
                //    .attr('class', 'openLink')
                //    .attr('href', '#/Edit/' + campaign.CampaignId)
                //    .data('campaignId', campaign.CampaignId)
                //    .on('click', function (event) {
                //        event.preventDefault();
                //        LoadCampaign($(this).data('campaignId'));
                //    });
                //var editCell = $(document.createElement('td')).append(editLink);

                var copyLink = $(document.createElement('a'))
                    .text('Copy')
                    .attr('class', 'openLink')
                    .attr('href', '#/copy/' + campaign.CampaignId)
                    .data('campaignId', campaign.CampaignId)
                    .on('click', function (event) {
                        event.preventDefault();
                        CopyCampaign($(this).data('campaignId'));
                    });
                var copyCell = $(document.createElement('td')).append(copyLink);

                var deleteLink = $(document.createElement('a'))
                    .text('Delete')
                    .attr('class', 'deleteLink')
                    .attr('href', '#/delete/' + campaign.CampaignId)
                    .data('campaignId', campaign.CampaignId)
                    .on('click', function (event) {
                        event.preventDefault();
                        DeleteCampaign($(this).data('campaignId'))
                    });
                var deleteCell = $(document.createElement('td')).append(deleteLink);

                var hubLink = $(document.createElement('a'))
                    .text('Hub')
                    .attr('href', '/campaigns/campaign-hub/?campaignId=' + campaign.CampaignId);
                var hubCell = $(document.createElement('td')).append(hubLink);

                //row.append(name, updated, status, mailshot, ownData, rentedData, postalOption, editCell, copyCell, deleteCell, hubCell);
                row.append(name, updated, status, mailshot, ownData, rentedData, postalOption, copyCell, deleteCell, hubCell);

                $campaignList.append(row);
            }
        }
        else {
            $campaignList.hide();
            $noCampaignsMessage.show();
            $loading.hide();
        }

        if (typeof (callback) != "undefined") {
            callback();
        }
    }

    function GetStatusText(statusId) {
        var status = 'Draft';
        switch (statusId) {
            case 2:
                status = "Pending Moderation";
                break;
            case 3:
                status = "Ready for fulfilment";
                break;
            case 4:
                status = "Sent for fulfilment";
                break;
            case 5:
                status = "Fulfilled";
                break;
            case 6:
                status = "Ready";
                break;
            case -1:
                status = "Exception";
                break;
        }
        return status;
    }

    function CopyCampaign(campaignId) {
        $loading.show();
        $.ajax({
            url: '/Umbraco/Api/Campaign/GetCopy/' + campaignId,
            type: 'GET',
            success: function (data) {
                GetMyCampaigns(function () {
                    LoadCampaign(data.CampaignId);
                });
            },
            statusCode: {
                400: function (response) {
                    HandleError(response);
                },
                401: function (response) {
                    HandleError(response);
                },
                403: function (response) {
                    HandleError(response);
                },
                500: function (response) {
                    HandleError(response);
                }
            }
        })
    }

    function LoadCampaign(campaignId) {
        DisplayCampaignForm();
        $formTitle.text('Loading ...');
        $.ajax({
            url: '/Umbraco/Api/Campaign/Get/' + campaignId,
            type: 'GET',
            success: function (data) {
                $formTitle.text('Edit campaign');
                $('#campaignName').val(data.Name);
                notes = $('#notes').val(data.Notes);
                $mailshotList.val(data.MailshotId);
                $postageList.val(data.PostalOptionId);
                $('#campaignId').val(data.CampaignId);
                $readonlyFields.show();
                $('#hasOwnData').val(data.HasDistributionLists);
                $('#hasRentedData').val(data.HasDataSearches);
                var status = GetStatusText(data.Status);
                $('#status').val(status);

                LoadPricingInformation(data.CampaignId);
            },
            statusCode: {
                400: function (response) {
                    HandleError(response);
                },
                401: function (response) {
                    HandleError(response);
                },
                403: function (response) {
                    HandleError(response);
                },
                500: function (response) {
                    HandleError(response);
                }
            }
        })
    }

    function DeleteCampaign(campaignId) {
        if (confirm('Are you sure you want to delete this campaign?')) {
            $loading.show();
            $.ajax({
                url: '/Umbraco/Api/Campaign/Delete/' + campaignId,
                type: 'DELETE',
                success: function () {
                    DisplayCampaignForm();
                    GetMyCampaigns();
                },
                statusCode: {
                    400: function (response) {
                        HandleError(response);
                    },
                    401: function (response) {
                        HandleError(response);
                    },
                    403: function (response) {
                        HandleError(response);
                    },
                    404: function (response) {
                        HandleError(response);
                    },
                    500: function (response) {
                        HandleError(response);
                    }
                }
            });
        }
    }

    function DisplayCampaignForm() {
        //$addCampaignForm.show();
        $formTitle.text('Add a campaign');
        $saveCampaignButton.removeAttr('disabled').text('Save');
        $('#campaignName').val('');
        notes = $('#notes').val('');
        $mailshotList.val('');
        $postageList.val('');
        $('#campaignId').val('');
        $readonlyFields.hide();

        $('#pricingInformation').hide();
        $('#serviceFee').text('');
        $('#printingCosts').text('');
        $('#dataCosts').text('');
        $('#postageCosts').text('');
        $('#vat').text('');
        $('#totalCost').text('');
    }

    function LoadPricingInformation(campaignId) {
        $.ajax({
            url: '/Umbraco/Api/Campaign/GetPriceBreakdown/' + campaignId,
            type: 'GET',
            success: function (data) {
                if (data.Complete) {
                    $('#order').removeAttr('disabled');
                }
                else {
                    $('#order').attr('disabled', 'disabled');
                }

                console.log(data);
                $('#pricingInformation').show();
                if (data.ServiceFee != null) {
                    $('#serviceFee').text(data.ServiceFee);
                }
                if (data.PrintingCost != null) {
                    $('#printingCosts').text(data.PrintingCost);
                }
                if (data.DataRentalCost != null) {
                    $('#dataCosts').text(data.DataRentalCost);
                }
                if (data.PostageCost != null) {
                    $('#postageCosts').text(data.PostageCost);
                }
                if (data.TotalTax != null) {
                    $('#vat').text(data.TotalTax);
                }
                if (data.Total != null) {
                    $('#totalCost').text(data.Total);
                }
            },
            statusCode: {
                400: function (response) {
                    HandleError(response);
                },
                401: function (response) {
                    HandleError(response);
                },
                403: function (response) {
                    HandleError(response);
                },
                500: function (response) {
                    HandleError(response);
                }
            }
        })
    }


    function PopulateMailshotList(callback) {
        $.ajax({
            url: '/Umbraco/Api/Mailshots/GetAll/',
            success: function (data) {
                $mailshotList.html('');
                if (data.length == 0) {
                    $mailshotList.html('<option value="">You currently have no saved mailshots</option>')
                }
                else {
                    $mailshotList.html('<option value="">Please select a mailshot</option>')
                    for (var i = 0; i < data.length; i++) {
                        var mailshot = data[i];
                        var optionItem = $(document.createElement('option'));
                        optionItem.attr('value', mailshot.MailshotId).text(mailshot.Name);
                        $mailshotList.append(optionItem);
                    }
                }
                if (typeof (callback != "undefined")) {
                    callback();
                }
            },
            statusCode: {
                401: function () {
                    $mailshotList.html('<option value="">No mailshots because user is not logged in.</option>');
                }
            }
        });
    }


    function PopulatePostageList(callback) {
        $.ajax({
            url: '/Umbraco/Api/Postage/Get/',
            success: function (data) {
                $postageList.html('');
                if (data.length == 0) {
                    $postageList.html('<option value="">No postage options</option>');
                }
                else {
                    $postageList.html('<option value="">Please select a postal option</option>')
                    for (var i = 0; i < data.length; i++) {
                        var postalOption = data[i];
                        var optionItem = $(document.createElement('option'));
                        optionItem.attr('value', postalOption.PostalOptionId).text(postalOption.Name);
                        $postageList.append(optionItem);
                    }
                }
                if (typeof (callback != "undefined")) {
                    callback();
                }
            }
        })
    }

    function CreateNewCampaign() {
        $('#campaignName').val('New Campaign');
        CreateCampaign(function (data) {
            window.location.assign('/campaigns/campaign-hub/?campaignId=' + data.CampaignId);
        })
    }

    function ExecuteCampaignCall(url, successCallback) {
        var name = $('#campaignName').val();
        var notes = $('#notes').val();
        var mailshotId = $mailshotList.val();
        var campaignId = $('#campaignId').val();
        var postalOptionId = $postageList.val();

        var dataValid = true;

        if (name.length == 0) {
            alert('You must provide a name for your campaign.');
            $('#campaignName').focus();
            dataValid = false;
        }

        if (dataValid) {
            $saveCampaignButton.attr('disabled', 'disabled').text('Saving ...');

            var postData = {
                Name: name,
                Notes: notes,
                MailshotId: mailshotId,
                PostalOptionId: postalOptionId
            };

            $.ajax({
                url: url,
                type: 'POST',
                data: postData,
                success: function (data) {
                    if (typeof (successCallback != "undefined")) {
                        successCallback(data);
                    }
                },
                statusCode: {
                    400: function (response) {
                        HandleError(response);
                        $saveCampaignButton.removeAttr('disabled').text('Save');
                    },
                    500: function (response) {
                        HandleError(response);
                        $saveCampaignButton.removeAttr('disabled').text('Save');
                    }
                }
            })
        }
    }


    function CreateCampaign(successCallback) {
        return ExecuteCampaignCall('/Umbraco/Api/Campaign/Save', successCallback);
    }

    function UpdateCampaign(successCallback) {

        var campaignId = $('#campaignId').val();
        return ExecuteCampaignCall('/Umbraco/Api/Campaign/Update/' + campaignId, successCallback);
    }


    // Startup stuff below

    CheckLoggedInStatus(function (loggedIn) {
        $loading.hide();
        if (loggedIn) {
            GetMyCampaigns();
            PopulateMailshotList(function () {
                PopulatePostageList(function () {
                    DisplayCampaignForm();
                });
            });
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
                GetMyCampaigns();
                PopulateMailshotList(function () {
                    PopulatePostageList(function () {
                        DisplayCampaignForm();
                    });
                });
            }
            else {
                $('#loginButton').text('Login').removeAttr('disabled');
            }
        });
    });

    $('#saveCampaign').on('click', function (event) {
        event.preventDefault();
        var campaignId = $('#campaignId').val();
        if (campaignId.length > 0) {
            UpdateCampaign(function () {
                GetMyCampaigns();
                DisplayCampaignForm();
            });
        }
        else {
            CreateCampaign(function () {
                GetMyCampaigns();
                DisplayCampaignForm();
            });
        }
    });

    $('#cancelButton').on('click', function (event) {
        event.preventDefault();
        DisplayCampaignForm();
    });

    $newCampaignButton.on('click', function (event) {
        $newCampaignButton.attr('disabled', 'disabled');
        event.preventDefault();
        CreateNewCampaign();
    });
});