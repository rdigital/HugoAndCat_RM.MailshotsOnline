$(function () {
    var $orderContainer = $('#orderContainer');
    var $loading = $('#loading');
    var $noOrderMessage = $('#noOrderMessage');
    var $activeCount = $('#activeCount');

    function RefreshMyOrders() {
        $loading.show();
        GetMyOrders(
            function (data) {
                $activeCount.text(data.length);
                $orderContainer.html('');
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var order = data[i];
                        var orderDate = new Date(order.OrderPlaced);

                        var orderDiv = $(document.createElement('div')).addClass('order');

                        var previewDiv = $(document.createElement('div')).addClass('preview');

                        var actionsDiv = $(document.createElement('div')).addClass('actions');
                        if (order.ShowCancelLink) {
                            var cancelLink = $(document.createElement('a'))
                                .text('Cancel')
                                .attr('href', '#/cancel/' + order.CampaignId)
                                .data('campaignId', order.CampaignId)
                                .on('click', function (event) {
                                    event.preventDefault();
                                    CancelOrder(order.CampaignId, function (data) {
                                        RefreshMyOrders();
                                    }, function (response) {
                                        console.log(response);
                                        alert(response);
                                    });
                                });
                            actionsDiv.append(cancelLink);
                        }
                        if (order.InvoiceUrl != null) {
                            var invoiceLink = $(document.createElement('a'))
                                .text('Invoice')
                                .attr('href', order.InvoiceUrl);
                            actionsDiv.append(invoiceLink);
                        }

                        var titleHeading = $(document.createElement('h2'));
                        var titleLink = $(document.createElement('a'))
                            .attr('href', order.OrderDetailsUrl);
                        titleHeading.append(titleLink);

                        var statusDiv = $(document.createElement('div'))
                            .addClass('status')
                            .addClass(order.Status);
                        var statusText = $(document.createElement('h3'))
                            .text(order.StatusText);
                        var statusDescription = $(document.createElement('p'))
                            .html(order.StatusDescription);
                        statusDiv.append(statusText, statusDescription);

                        var infoDiv = $(document.createElement('div'))
                            .addClass('info');
                        var orderPlaced = $(document.createElement('span'))
                            .text('Order placed ' + orderDate.toDateString())
                        var orderNumber = $(document.createElement('span'))
                            .text('Order number ' + order.OrderNumber);
                        var total = $(document.createElement('span'))
                            .text('Total £' + order.Total);
                        infoDiv.append(orderPlaced, orderNumber, total);

                        orderDiv.append(previewDiv, actionsDiv, titleHeading, statusDiv, infoDiv);
                        $orderContainer.append(orderDiv);
                    }
                }
                else {
                    $noOrderMessage.show();
                }
                $loading.hide();
            },
            function (response) {
                console.log(response);
                alert(response);
                $loading.hide();
                $noOrderMessage.show();
            }
        );
    }

    // On load:
    RefreshMyOrders();
});