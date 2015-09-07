// custom binding to drop in a spectrum.js colour picker and bind it's value to an observable
define(['knockout', 'jquery', 'spectrum'],
    function(ko, $, spectrum) {
        ko.bindingHandlers.spectrum = {
            init : function(element, valueAccessor){    
                var value = valueAccessor();
                $(element).spectrum({
                    beforeShow: function(){
                        $(this).spectrum("set", value.colour());
                        if (value.callback) {
                            value.callback();
                        }
                        $(element).css('pointer-events', 'none')
                    },

                    hide: function(){
                        if (value.hideCallback) {
                            value.hideCallback();
                        }
                        $(element).css('pointer-events', 'all')
                    },

                    move: function(color){
                        value.colour(color.toHexString().toUpperCase());
                    },

                    appendTo: '.colour-dropdown .dropdown-options'
                });

                ko.utils.domNodeDisposal.addDisposeCallback(element, function() {
                    $(element).spectrum("destroy");
                });
            }
        };
    }
)