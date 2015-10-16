// viewmodel to handle template data
define(['jquery', 'knockout', 'koValidation'],
    function($, ko, koValidation) {

        function loginViewModel() {
            this.email = ko.observable();
            this.password = ko.observable();

            //validation stuff
            this.email.extend({
                required: {
                    message: 'Please provide an email address'
                },
                email: true
            });

            this.password.extend({
                required: {
                    message: 'Please provide a password'
                }
            });

            this.errors = koValidation.group(this);
        }

        loginViewModel.prototype.validateAndSubmit = function validateAndSubmit() {
            if (this.errors().length === 0) {
                return true;
            }
            else {
                this.errors.showAllMessages();
            }
        };

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return new loginViewModel();
    }
);