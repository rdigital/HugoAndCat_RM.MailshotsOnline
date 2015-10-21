define([
        'knockout',
        'jquery',
        'koValidation',
        'koMapping',
        'koaresame',
        'koselect2',
        'koinitializevalue',
        'select2'
    ],

    function(ko, $, koValidation, koMapping) {

        function registerViewModel() {

            // create the stages (each part of the form)
            this.stage1 = {};
            this.stage2 = {};
            this.stage3 = {};
            this.stages = [this.stage1, this.stage2, this.stage3];
            this.currentStage = ko.observable(this.stages[0]);
            this.addressNow = {};

            this.setCurrentStage = function(i) {
                return this.currentStage() === this.stages[i] ? 'current-stage' : '';
            };
    
            this.init = function() {
                var _this = this;
                // hide the sections
                this.stage1Init();
                this.stage3Init();

                // check for server-side validation
                if ($('.validation-messages').length > 0) {
                    var parentIndex = $('.validation-messages').first().parents('.register-stage').index();
                    this.currentStage(this.stages[parentIndex]);
                }

                // listen for updates to dom from Address now
                addressNow.listen('populate', function() {
                    _this.addressNow = arguments;
                });

            };


            this.stage1Init = function() {
                var _this = this;
                window.stage1 = this.stage1;
                this.stage1.Title = ko.observable("").extend({ 
                    required: {
                        message: 'Please select a Title'
                    }
                });
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
                this.stage1.AgreeToRoyalMailContact = ko.observable();
                this.stage1.AgreeToThirdPartyContact = ko.observable();

                this.stage1Errors = koValidation.group(this.stage1, {deep: true});
                this.stage1Errors.showAllMessages(false);
            };

            this.stage3Init = function() {
                this.stage3 = koMapping.fromJS({
                                OrganisationName: null,
                                JobTitle: null,
                                FlatNumber: null,
                                BuildingNumber: null,
                                BuildingName: null,
                                Address1: null,
                                Address2: null,
                                City: null,
                                Country: null,
                                WorkPhoneNumber: null,
                                MobilePhoneNumber: null
                            });


                this.stage3.OrganisationName.extend({ 
                    required: {
                        message: 'Please add your company or organisation name'
                    }
                });
                this.stage3.JobTitle.extend({ 
                    required: {
                        message: 'Please enter your job title'
                    }
                });
                this.stage3.Address1.extend({ 
                    required: {
                        message: 'Please add the first line of your address'
                    }
                });
                this.stage3.City.extend({ 
                    required: {
                        message: 'Please enter your City'
                    }
                });
                this.stage3.Country.extend({ 
                    required: {
                        message: 'Please enter your country'
                    }
                });
                this.stage3.WorkPhoneNumber.extend({ 
                    required: {
                        message: 'Please enter your work telephone number'
                    }
                });

                this.stage3Errors = koValidation.group(this.stage3, {deep: true});
                this.stage3Errors.showAllMessages(false);
            };

            this.proceedToStage1 = function() {
                this.currentStage(this.stages[0]);
            };

            this.proceedToStage2 = function() {
                if (this.stage1Errors().length === 0) {
                    this.currentStage(this.stages[1]);
                }
                else {
                    this.stage1Errors.showAllMessages();
                }
            };

            this.proceedToStage3 = function() {
                var addressData = this.addressNow[1] || false;

                if (addressData) {
                    // Update address observables with data from AddressNow
                    this.stage3.OrganisationName(addressData.Company);
                    this.stage3.Address1(addressData.FormattedLine1);
                    this.stage3.City(addressData.City);
                    this.stage3.Country(addressData.Country);
                }

                this.stage3Errors.showAllMessages(false);
                this.currentStage(this.stages[2]);
            };

            this.submit = function(data, event) {
                event.preventDefault();
                if (this.stage3Errors().length === 0) {
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