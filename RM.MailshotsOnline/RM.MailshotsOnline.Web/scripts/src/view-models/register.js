define([
        'knockout',
        'select2'
    ],

    function(ko) {

        function registerViewModel() {
            this.stage1 = {};
            this.stage2 = {};
            this.stage3 = {};


            this.stage1.Email = ko.observable();
            this.stage1.ConfirmEmail = ko.observable();
            this.stage1.FirstName = ko.observable();
            this.stage1.LastName = ko.observable();
            this.stage1.Title = ko.observable();

            this.proceedToNextStage = function() {
                console.log('proceedToNextStage');
            };

        }



        return new registerViewModel();
});