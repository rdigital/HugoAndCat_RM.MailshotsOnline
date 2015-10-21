define(['knockout', 'jquery'],
  function(ko, $) {
    ko.bindingHandlers.initializeValue = {
        init: function(element, valueAccessor) {
            valueAccessor()(element.getAttribute('value'));
        },
        update: function(element, valueAccessor) {
            var value = valueAccessor();
            element.setAttribute('value', ko.utils.unwrapObservable(value))
        }
    };
  }
);