define(['require', 'knockout', 'view_models/user', 'view_models/state'],
    function(require, ko, userViewModel, stateViewModel) {
        function historyViewModel() {
            this.history = ko.observableArray();
            this.historyIdx = ko.observable(0);

            this.pushToHistory = this.pushToHistory.bind(this);
            this.cancelChanges = this.cancelChanges.bind(this);
            this.undo = this.undo.bind(this);
            this.redo = this.redo.bind(this);

            stateViewModel.selectedElement.subscribe(this.pushToHistory, this);

            this.redoAvailable = this.getRedoAvailable();
            this.undoAvailable = this.getUndoAvailable();
        }

        historyViewModel.prototype.pushToHistory = function pushToHistory() {
            var history_idx = this.historyIdx(),
                current_history = this.history()[history_idx],
                new_history = require('view_models/user').toHistoryJSON();

            if (current_history == new_history) {
                return
            }
            this.history(this.history.slice(0, history_idx+1));
            this.history.push(new_history);
            this.historyIdx(this.history().length-1);
        }

        historyViewModel.prototype.cancelChanges = function cancelChanges() {
            userViewModel.fromJSON(this.historyIdx());
        }

        historyViewModel.prototype.reset = function reset() {
            this.historyIdx(0);
            require('view_models/user').fromJSON(this.history()[0]);
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

        historyViewModel.prototype.getRedoAvailable = function getRedoAvailable() {
            return ko.computed(function () {
                return (this.historyIdx() < this.history().length - 1);
            }, this)
        }

        historyViewModel.prototype.getUndoAvailable = function getUndoAvailable() {
            return ko.computed(function () {
                return (this.historyIdx() > 0);
            }, this)
        }

        // testing
        window.historyModel = new historyViewModel()
        // return instance of viewmodel, so all places where AMD is used
        // to load the viewmodel will get the same instance
        return window.historyModel;
    }
)