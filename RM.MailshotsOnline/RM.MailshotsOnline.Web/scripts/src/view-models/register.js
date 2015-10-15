define([
        'knockout',
        'koValidation',
        'select2'
    ],

    function(ko, koValidation) {

        function registerViewModel() {
            this.stage1 = {};
            this.stage2 = {};
            this.stage3 = {};


            this.stage1.Email = ko.observable().extend({ 
                required: {
                    email: true
                }
            });
            this.stage1.ConfirmEmail = ko.observable().extend({ 
                required: {
                    message: 'Please re-enter a valid email address'
                }
            });
            this.stage1.FirstName = ko.observable().extend({ 
                required: {
                    message: 'Please enter your first name'
                }
            });
            this.stage1.LastName = ko.observable().extend({ 
                required: {
                    message: 'Please enter your last name'
                }
            });
            this.stage1.Title = ko.observable().extend({ 
                required: {
                    message: 'Please select a Title'
                }
            });
            this.stage1.Password = ko.observable().extend({ 
                required: {
                    message: 'Please enter an alphanumeric password of eight characters or more'
                }
            });
            this.stage1.ConfirmPassword = ko.observable().extend({ 
                required: {
                    message: 'Please re-type your password.'
                }
            });
            this.stage1.royalMailMailAgreement = ko.observable();
            this.stage1.thirdPartyMailAgreement = ko.observable();

            this.stage1.AgreeToTermsAndConditions = ko.observable().extend({ 
                required: {
                    message: 'Please accept T&amp;C agreement'
                }
            });




            this.stage2.Postcode = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });


            this.stage2.Organisation = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });


            this.stage2.Address1 = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });

            this.stage2.Address2 = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });

            this.stage2.Town = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });

            this.stage2.City = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });

            this.stage2.Country = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });

            this.stage2.PhoneWork = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });

            this.stage2.PhoneMobile = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });

            this.stage2.Email = ko.observable().extend({ 
                required: {
                    message: 'Please'
                }
            });

            this.stage1Errors = koValidation.group(this.stage1, {deep: true});
            this.stage2Errors = koValidation.group(this.stage2, {deep: true});
            this.stage3Errors = koValidation.group(this.stage2, {deep: true});

            this.proceedToNextStage = function() {
                console.log('proceedToNextStage');
                if (this.stage1Errors().length === 0) {
                    alert('Thank you.');
                }
                else {
                    alert('Please check your submission.');
                    this.stage1Errors.showAllMessages();
                }
            };

            this.stage1Errors.showAllMessages(false);
            this.stage2Errors.showAllMessages(false);
            this.stage3Errors.showAllMessages(false);

            this.submit = function() {
                if (this.stage2Errors().length === 0) {
                    alert('Thank you.');
                }
                else {
                    alert('Please check your submission.');
                    this.stage2Errors.showAllMessages();
                }
                if (this.stage3Errors().length === 0) {
                    alert('Thank you.');
                }
                else {
                    alert('Please check your submission.');
                    this.stage3Errors.showAllMessages();
                }
            }

        }



        return new registerViewModel();
});