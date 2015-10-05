define(['jquery', 'knockout', 'koMapping', 'select2', 'view-models/state'],

    function($, ko, koMapping, select2, stateViewModel) {

        // ViewModel
        function matchDataComponentViewModel() {
            this.currentList = stateViewModel.currentList;
            this.mappings = ko.observableArray([]);
            this.plainObject;
        }

        matchDataComponentViewModel.prototype.createMappings = function createMappings() {
            var self = this;
            this.mappings([]);
            $('select').each(function(){
                self.mappings.push($(this).val());
            });
            this.currentList().Mappings(this.mappings());

            this.plainObject = koMapping.toJS(this.currentList());
        }

        matchDataComponentViewModel.prototype.initSelect = function initSelect(array, data) {
            $('select').select2();
        }

        return {
            viewModel: matchDataComponentViewModel,
            template: { require: 'text!/scripts/src/templates/match-data.html' }
        };
});