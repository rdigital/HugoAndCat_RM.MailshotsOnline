// viewmodel to handle template data
define(['knockout'],
    function(ko) {

        function loginViewModel() {
            this.email = ko.observable();
            this.password = ko.observable();

            //validation stuff
            this.email.extend({
                required: {
                    message: 'Please provide an email address'
                }
            });

            this.password.extend({
                required: {
                    message: 'Please provide a password'
                }
            });
        }

        loginViewModel.prototype.validateAndSubmit = function validateAndSubmit() {
            console.log('validateAndSubmit');
        };

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return new loginViewModel();
    }
);