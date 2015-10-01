define(['knockout', 'koelement', 'view_models/state', 'view_models/auth'],

    function(ko, koelement, stateViewModel, authViewModel) {

        // ViewModel
        function authComponentViewModel(params) {
            this.isAuthenticated = authViewModel.isAuthenticated;
            this.toggleAuth = stateViewModel.toggleAuth;
            this.mailshotID = stateViewModel.mailshotID;
            this.viewing = ko.observable('login');
            this.title = ko.observable('Mr');
            this.first_name = ko.observable();
            this.last_name = ko.observable();
            this.agree_rm = ko.observable(false);
            this.agree_tp = ko.observable(false);
            this.email = ko.observable();
            this.password = ko.observable();
            this.captchaEl = ko.observable();
            this.captchaID = 0;
            this.processing = ko.observable(false);
            this.loginErrors = ko.observableArray();
            this.regErrors = ko.observableArray();
            this.passwordSent = ko.observableArray();
            this.titleOptions = ko.observableArray([
                {name: 'Mr', value: 'Mr'},
                {name: 'Mrs', value: 'Mrs'},
                {name: 'Miss', value: 'Miss'},
            ]);

            // computeds
            this.loginAvailable = this.getLoginAvailableComputed();
            this.regNextAvailable = this.getRegNextAvailableComputed();
            this.forgotAvailable = this.getForgotAvailableComputed();

            // bound methods
            this.prepCaptcha = this.prepCaptcha.bind(this);
        }

        authComponentViewModel.prototype.getLoginAvailableComputed = function getLoginAvailableComputed() {
            return ko.pureComputed(function () {
                if (this.email() && this.password()) {
                    return true;
                }
                return false;
            }, this);
        }

        authComponentViewModel.prototype.getRegNextAvailableComputed = function getRegNextAvailableComputed() {
            return ko.pureComputed(function () {
                if (this.first_name() && this.last_name() && this.password() && this.validateEmail(this.email())) {
                    return true;
                }
                return false;
            }, this);
        }

        authComponentViewModel.prototype.getForgotAvailableComputed = function getForgotAvailableComputed() {
            return ko.pureComputed(function () {
                if (this.validateEmail(this.email())) {
                    return true;
                }
                return false;
            }, this);
        }

        authComponentViewModel.prototype.validateEmail = function validateEmail(email) {
            var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
            return re.test(email);
        }

        authComponentViewModel.prototype.login = function login() {
            if (this.processing() || !this.loginAvailable()) {
                return
            }
            this.processing(true);
            $.post(
                '/Umbraco/Api/Members/Login',
                { Email: this.email(), Password: this.password() },
                function (data) {
                    this.isAuthenticated(true);
                    this.toggleAuth();
                }.bind(this)
            )
            .fail(function(data) {
                if (data.responseJSON.error) {
                    this.loginErrors([data.responseJSON.error]);
                }
            }.bind(this))
            .always(function() {
                this.processing(false);
            }.bind(this));
        }

        authComponentViewModel.prototype.register = function register() {
            if (this.processing()) {
                return
            }

            var registration = {
                Email: this.email(),
                Title: this.title(),
                FirstName: this.first_name(),
                LastName: this.last_name(),
                Password: this.password(),
                'g-recaptcha-response': $('.g-recaptcha-response').val(),
                AgreeToRoyalMailContact: this.agree_rm(),
                AgreeToThirdPartyContact: this.agree_tp()
            };

            this.processing(true);
            $.post(
                '/Umbraco/Api/Members/Register',
                registration,
                function (data) {
                    this.isAuthenticated(true);
                    this.toggleAuth();
                }.bind(this)
            )
            .fail(function(data) {
                grecaptcha.reset(this.captchaID);
                if (data.responseJSON.fieldErrors) {
                    this.regErrors(data.responseJSON.fieldErrors);
                } else if (data.responseJSON.error) {
                    this.regErrors([data.responseJSON.error]);
                }
            }.bind(this))
            .always(function() {
                this.processing(false);
            }.bind(this));

        }

        authComponentViewModel.prototype.forgotPassword = function forgotPassword() {
            if (this.processing() || !this.forgotAvailable()) {
                return
            }
            this.processing(true)
            $.post(
                '/Umbraco/Api/Members/SendPasswordResetLink',
                {Email: this.email()},
                function() {
                    console.log('forgot password call successful');
                }
            )
            .always(function() {
                this.processing(false);
                this.passwordSent(true);
            }.bind(this));
            
        }

        authComponentViewModel.prototype.showRegister = function showRegister() {
            this.regErrors([]);
            this.viewing('register');
        }

        authComponentViewModel.prototype.showRegisterTwo = function showRegisterTwo() {
            if (this.regNextAvailable()) {
                this.viewing('register2');
            }
        }

        authComponentViewModel.prototype.showLogin = function showLogin() {
            this.viewing('login');
        }

        authComponentViewModel.prototype.showForgot = function showForgot() {
            this.passwordSent(false);
            this.viewing('forgot');
        }

        authComponentViewModel.prototype.prepCaptcha = function prepCaptcha() {
            if (typeof (grecaptcha) != "undefined") {
                this.captchaID = grecaptcha.render(this.captchaEl()[0] , {sitekey: '6LecKwoTAAAAABNgC26LCUFC9bhTODQv8McHP25G'});
            }
        }

        return {
            viewModel: authComponentViewModel,
            template: { require: 'text!/canvas/templates/auth.html' }
        };
    }
);