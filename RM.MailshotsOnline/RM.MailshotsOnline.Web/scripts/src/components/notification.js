define(['knockout', 'view-models/notification'],

    function(ko, notificationModel) {
        
        function notificationViewModel() {
            this.visible = notificationModel.visible;
            this.message = notificationModel.message;
            this.messageType = notificationModel.messageType;
        }

        return {
            viewModel: notificationViewModel,
            template: { require: 'text!/scripts/src/templates/notification.html' }
        };
});