/**
 * base data view model to be extended for handling of immutable data sources
 * (formats, templates, themes)
 */
define(['knockout', 'komapping', 'jquery'],
    function(ko, komapping, $) {

    	/**
    	 * View model, not to be instanciated directly, but rather used as a base
    	 * for extension by other viewmodels with similar properties (e.g. themes)
    	 */
        function dataViewModel() {
            // this.objects contains the data returned from the server
            this.objects = ko.observableArray([]);
            // use selection by ID (instead of direct ref to object)
            // in case we want to use routing to describe selections
            this.selected = ko.pureComputed(function(){
                return this.getObjByID(this.objects, this.selectedID);
            }, this);

            this.getVars();

            // fetch data
            this.fetch();
        }

        dataViewModel.prototype.toJSON = function toJSON() {
            console.log(komapping.toJSON(this.objects))
        }

        /**
         * helper method to fetch an object from an observable array by ID property
         * @param  {ko.observable} obsArr observable array to be searched
         * @param  {ko.observable} obsID  observable containing ID to look for
         * @return {Object}      	 matching object from the observable array
         */
        dataViewModel.prototype.getObjByID = function getObjByID(obsArr, obsID) {
            var objID = obsID(),
                selectedObjs = ko.utils.arrayFilter(obsArr(), function(obj) {
                    return obj.id == objID;
                });
            return (selectedObjs.length) ? selectedObjs[0] : {}
        }

        dataViewModel.prototype.fetch = function fetch() {
            // fetch data from server using fetchURL
            //console.log('fetching data from ' + this.fetchURL);
            // XXX TEMP XXX
            //setTimeout(function(){
            //	this.objects(this.testData || {});
            //}.bind(this), 2000)
            this.objects(this.testData || {});
            //console.log(this.objects());
            return
            $.getJSON(this.fetchURL, function(data) {
                this.objects(data);
            }.bind(this))
        }

        dataViewModel.prototype.select = function select(obj) {
            // fetch data from server using fetchURL
            this.selectedID(obj.id);
        }

        return dataViewModel;
    }
)