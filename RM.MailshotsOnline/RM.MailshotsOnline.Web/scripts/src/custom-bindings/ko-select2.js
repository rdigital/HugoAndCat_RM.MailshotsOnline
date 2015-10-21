define(['knockout', 'jquery'],
  function(ko, $) {
    ko.bindingHandlers.select2 = {
        init: function(element, valueAccessor) {
            $(element).select2(valueAccessor()); 
            ko.utils.domNodeDisposal.addDisposeCallback(element, function() { 
                $(element).select2('destroy'); 
            });
        }
    }; 
  }
);