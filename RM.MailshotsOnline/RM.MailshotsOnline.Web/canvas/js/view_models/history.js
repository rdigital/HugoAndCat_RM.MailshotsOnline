define(['require', 'knockout', 'view_models/user'],
    function(require, ko, userViewModel) {
        function historyViewModel() {
            this.history = ko.observableArray();
            this.historyIdx = ko.observable(0);

            this.pushToHistory = this.pushToHistory.bind(this)
            this.cancelChanges = this.cancelChanges.bind(this)
            this.undo = this.undo.bind(this)
            this.redo = this.redo.bind(this)
        }

        historyViewModel.prototype.pushToHistory = function pushToHistory() {
            this.history(this.history.slice(0, this.historyIdx() +1))
            this.history.push(require('view_models/user').toHistoryJSON());
            this.historyIdx(this.history().length-1);
        }

        historyViewModel.prototype.cancelChanges = function cancelChanges() {
            userViewModel.fromJSON(this.historyIdx());
        }

        historyViewModel.prototype.undo = function undo() {
            var idx = this.historyIdx();
            if (idx > 0) {
                this.historyIdx(idx-1);
                require('view_models/user').fromJSON(this.history()[this.historyIdx()]);
            }
        }

        historyViewModel.prototype.redo = function redo() {
            var idx = this.historyIdx();
            if (idx < this.history().length - 1) {
                this.historyIdx(idx+1);
                require('view_models/user').fromJSON(this.history()[this.historyIdx()]);
            }
        }

        // testing
        window.historyModel = new historyViewModel()
        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return window.historyModel;
    }
)