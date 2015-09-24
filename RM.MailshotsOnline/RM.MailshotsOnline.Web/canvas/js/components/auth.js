define(['knockout', 'view_models/state', 'view_models/auth'],

    function(ko, stateViewModel, authViewModel) {

        // ViewModel
        function authComponentViewModel(params) {
            this.isAuthenticated = authViewModel.isAuthenticated;
            this.viewing = ko.observable('register');

            // bound methods
            //this.togglePreview = stateViewModel.togglePreview.bind(this);
        }

        authComponentViewModel.prototype.showRegister = function showRegister() {
            this.viewing('register');
        }

        authComponentViewModel.prototype.showLogin = function showLogin() {
            this.viewing('login');
        }

        return {
            viewModel: authComponentViewModel,
            template: { require: 'text!/canvas/templates/auth.html' }
        };
    }
);