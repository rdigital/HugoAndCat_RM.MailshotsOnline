define(['knockout', 'view_models/state'],

    function(ko, stateViewModel) {

        // ViewModel
        function backgroundToolsViewModel(params) {
            this.backgroundSelected = stateViewModel.backgroundSelected;
            this.attachment = {
                'top': 160,
                'left': '50%'
            };
            this.colours = this.getColoursComputed();
            this.colour = this.getStyleComputed('background-color');
        }

        /**
         * get computed which evaluates the available colours for the selected element
         * @return {ko.pureComputed} 
         */
        backgroundToolsViewModel.prototype.getColoursComputed = function getColoursComputed() {
            return ko.pureComputed(function() {
                if (this.backgroundSelected()) {
                    return this.backgroundSelected().getColours();
                }
                return [];
            }, this);
        };

        /**
         * computed generator for styles. Provide a style property name and the returned computed
         * will provide a read / write computed which evaluates to that property's current value
         * @return {ko.pureComputed} 
         */
        backgroundToolsViewModel.prototype.getStyleComputed = function getStyleComputed(property) {
            return ko.pureComputed( {
                read: function() {
                    var face = this.backgroundSelected();
                    if (face) {
                        return face.getStyle(property);
                    }
                    return;
                },
                write: function(val) {
                    var face = this.backgroundSelected();
                    if (face) {
                        face.setStyle(property, val);
                    }
                }
            }, this);
        };

        return {
            viewModel: backgroundToolsViewModel,
            template: { require: 'text!/canvas/templates/backgroundtools.html' }
        };
});