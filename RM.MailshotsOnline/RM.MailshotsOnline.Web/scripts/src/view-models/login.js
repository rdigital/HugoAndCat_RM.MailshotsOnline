// viewmodel to handle template data
define(['knockout'],
    function(ko) {

        function loginViewModel() {
            this.email = ko.observable();
            this.password = ko.observable();
        }

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return new loginViewModel();
    }
);