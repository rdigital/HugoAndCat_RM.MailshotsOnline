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
            this.stage3 = {};

            window.viewModel = this;

            this.init = function() {
                // hide the sections
                // $('.register-stage').hide();
                this.stage1Init();
                this.stage2Init();
                this.stage3Init();
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
                this.stage1.AgreeToTermsAndConditions = ko.observable().extend({
                    required: {
                        message: 'You must agree to the Terms and Conditions'
                    }
                });
                this.stage1.royalMailMailAgreement = ko.observable();
                this.stage1.thirdPartyMailAgreement = ko.observable();

                this.stage1Errors = koValidation.group(this.stage1, {deep: true});
                this.stage1Errors.showAllMessages(false);
            };

            this.stage2Init = function() {
                this.stage2.Postcode = ko.observable().extend({
                    required: {
                        message: 'Please enter a valid postcode'
                    }
                });
                this.stage2Errors = koValidation.group(this.stage2, {deep: true});
                this.stage2Errors.showAllMessages(false);
            };

            this.stage3Init = function() {
                this.stage3.OrganisationName = ko.observable().extend({ 
                    required: {
                        message: 'Please'
                    }
                });
                this.stage3.JobTitle = ko.observable().extend({ 
                    required: {
                        message: 'Please'
                    }
                });
                this.stage3.Address1 = ko.observable().extend({ 
                    required: {
                        message: 'Please'
                    }
                });
                this.stage3.Address2 = ko.observable().extend({ 
                    required: {
                        message: 'Please'
                    }
                });
                this.stage3.City = ko.observable().extend({ 
                    required: {
                        message: 'Please'
                    }
                });
                this.stage3.Country = ko.observable().extend({ 
                    required: {
                        message: 'Please'
                    }
                });
                this.stage3.WorkPhoneNumber = ko.observable().extend({ 
                    required: {
                        message: 'Please'
                    }
                });
                this.stage3.MobilePhoneNumber = ko.observable().extend({ 
                    required: {
                        message: 'Please'
                    }
                });

                this.stage3Errors = koValidation.group(this.stage3, {deep: true});
                this.stage3Errors.showAllMessages(false);

                window.stage3 = this.stage3;
            };

            this.proceedToStage2 = function() {
                console.log(this.stage1Errors());
                if (this.stage1Errors().length === 0) {
                    console.log('proceed to stage 2');
                    $('.register-stage').eq(0).hide();
                    $('.register-stage').eq(1).show();
                }
                else {
                    this.stage1Errors.showAllMessages();
                }
            };

            this.proceedToStage3 = function() {
                if (this.stage2Errors().length === 0) {
                    console.log('proceed to stage 3');
                    $('.register-stage').eq(1).hide();
                    $('.register-stage').eq(2).show();
                }
                else {
                    console.log('Stage 2');
                    this.stage2Errors.showAllMessages();
                }
            };

            this.submit = function(data, event) {
                event.preventDefault();
                if (this.stage3Errors().length === 0) {
                    alert('Thank you.');
                    $('form.register__form').submit();
                }
                else {
                    this.stage3Errors.showAllMessages();
                }
            };

            this.init();

        }


        return new registerViewModel();
});