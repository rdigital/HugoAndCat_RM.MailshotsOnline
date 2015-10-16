define([
        'knockout',
        'jquery',
        'koValidation',
        'select2'
    ],

    function(ko, $, koValidation) {

        function registerViewModel() {

            // create the stages (each part of the form)
            this.stage1 = {};
            this.stage2 = {};


            this.init = function(argument) {
                // hide the sections
                // $('.register-stage').hide();
                this.stage1Init();
                this.stage1Errors = koValidation.group(this.stage1, {deep: true});
                this.stage1Errors.showAllMessages(false);
            };


            this.stage1Init = function() {
                var _this = this;

                $('.register-stage').eq(0).show();
                
                this.stage1.Email = ko.observable().extend({ 
                    required: {
                        message: 'Please enter your email address'
                    },
                    email: true
                });

                this.stage1.ConfirmEmail = ko.observable().extend({
                    required: {
                        message: 'Please retype your email address'
                    },
                    areSame: { 
                        params: _this.stage1.Email, 
                        message: "Confirm password must match password" 
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
                    },
                    areSame: { 
                        params: _this.stage1.Password, 
                        message: "Confirm password must match password" 
                    }
                });
                this.stage1.royalMailMailAgreement = ko.observable();
                this.stage1.thirdPartyMailAgreement = ko.observable();
                this.stage1.AgreeToTermsAndConditions = ko.observable().extend({
                    // required: {
                    //     message: 'Please accept T&C agreement'
                    // }
                });
            };

            this.stage2Init = function() {
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

                this.stage2Errors = koValidation.group(this.stage2, {deep: true});
                this.stage2Errors.showAllMessages(false);
            };

            this.proceedToStage2 = function() {
                if (this.stage1Errors().length === 0) {
                    $('.register-stage').eq(0).hide();
                    $('.register-stage').eq(1).show();

                    this.stage2Init();
                }
                else {
                    console.log(this.stage1Errors());
                    this.stage1Errors.showAllMessages();
                }
            };

            this.submit = function() {
                if (this.stage1Errors().length === 0) {
                    // alert('Thank you.');
                }
                else {
                    // alert('Please check your submission.');
                    this.stage1Errors.showAllMessages();
                }
                if (this.stage2Errors().length === 0) {
                    // alert('Thank you.');
                }
                else {
                    // alert('Please check your submission.');
                    this.stage2Errors.showAllMessages();
                }
            };

            this.init();

        }


        return new registerViewModel();
});