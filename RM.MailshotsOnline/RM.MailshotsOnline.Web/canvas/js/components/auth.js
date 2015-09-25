define(['knockout', 'koelement', 'view_models/state', 'view_models/auth'],

    function(ko, koelement, stateViewModel, authViewModel) {

        // ViewModel
        function authComponentViewModel(params) {
            this.isAuthenticated = authViewModel.isAuthenticated;
            this.toggleAuth = stateViewModel.toggleAuth;
            this.viewing = ko.observable('register');
            this.title = ko.observable('Mr');
            this.captchaEl = ko.observable();
            this.titleOptions = ko.observableArray([
                {name: 'Mr', value: 'Mr'},
                {name: 'Mrs', value: 'Mrs'},
                {name: 'Miss', value: 'Miss'},
            ])

            // bound methods
            this.prepCaptcha = this.prepCaptcha.bind(this);
        }

        authComponentViewModel.prototype.showRegister = function showRegister() {
            this.viewing('register');
        }

        authComponentViewModel.prototype.showRegisterTwo = function showRegisterTwo() {
            this.viewing('register2');
        }

        authComponentViewModel.prototype.showLogin = function showLogin() {
            this.viewing('login');
        }

        authComponentViewModel.prototype.showForgot = function showForgot() {
            this.viewing('forgot');
        }

        authComponentViewModel.prototype.prepCaptcha = function prepCaptcha() {
            if (typeof (grecaptcha) != "undefined") {
                grecaptcha.render(this.captchaEl()[0] , {sitekey: '6LecKwoTAAAAABNgC26LCUFC9bhTODQv8McHP25G'});
            }
        }

        return {
            viewModel: authComponentViewModel,
            template: { require: 'text!/canvas/templates/auth.html' }
        };
    }
);