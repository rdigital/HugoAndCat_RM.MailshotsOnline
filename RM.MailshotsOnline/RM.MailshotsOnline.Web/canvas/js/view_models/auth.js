// viewmodel to handle format data
define(['knockout', 'jquery', 'view_models/state'],
    function(ko, $, stateViewModel) {

        function authViewModel() {
            this.isAuthenticated = ko.observable(false);
            this.interval = null;

            // bound methods
            this.getAuthenticated = this.getAuthenticated.bind(this);
            this.getAuthNoCallback = this.getAuthNoCallback.bind(this);

            // subscriptions
            this.isAuthenticated.subscribe(this.handleAuth, this);
            
            this.getAuthenticated(false);
        }

        authViewModel.prototype.getAuthNoCallback = function getAuthNoCallback() {
            this.getAuthenticated(false);
        }

        authViewModel.prototype.getAuthenticated = function getAuthenticated(callback) {
            // fetch data from server using fetchURL
            var url = "/Umbraco/Api/Members/GetLoggedInStatus"; 
            //console.log('fetching data from ' + fetchURL);
            $.getJSON(url, function(data) {
                this.isAuthenticated(data.loggedIn);
                if (callback && !data.loggedIn) {
                    callback();
                }
            }.bind(this));
        };

        authViewModel.prototype.handleAuth = function handleAuth(authed) {
            if (authed) {
                this.setupPoll();
            } else {
                this.disposePoll();
            }
        }

        authViewModel.prototype.setupPoll = function setupPoll() {
            setInterval(this.getAuthNoCallback, 60000);
        }

        authViewModel.prototype.disposePoll = function disposePoll() {
            if (!this.interval) {
                return
            }
            clearInterval(this.interval);
            this.interval = null;
        }

        // for testing
        window.auth = new authViewModel();

        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return window.auth;
    }
);