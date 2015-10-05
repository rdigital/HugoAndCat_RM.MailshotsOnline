define(['knockout', 'view_models/notification'],

    function(ko, notificationModel) {
        
        function notificationViewModel() {
            this.visible = notificationModel.visible;
            this.message = notificationModel.message;
            this.message_type = notificationModel.message_type;
        }

        return {
            viewModel: notificationViewModel,
            template: { require: 'text!/canvas/templates/notification.html' }
        };
});