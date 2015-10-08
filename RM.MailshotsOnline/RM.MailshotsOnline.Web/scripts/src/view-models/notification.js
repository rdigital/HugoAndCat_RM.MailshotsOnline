// viewmodel to handle template data
define(['knockout'],
    function(ko) {

        function notificationViewModel() {
            this.visible = ko.observable(false);
            this.message = ko.observable();
            this.messageType = ko.observable('message');
        }

        notificationViewModel.prototype.show = function show(message, type) {
            this.message(message);
            this.messageType(type);
            this.visible(true);
            
            if (type === "error") {
                setTimeout(function () {
                    this.visible(false);
                }.bind(this), 5000);
            }
        };

        notificationViewModel.prototype.hideWithMessage = function hideWithMessage(message, type) {
            this.visible(true);
            this.message(message);
            this.messageType(type);
            setTimeout(function () {
                this.visible(false);
            }.bind(this), 2000);
        };

        notificationViewModel.prototype.hide = function hide() {
            this.visible(false);
        };

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return new notificationViewModel();
    }
);