define(['knockout', 'jquery'],
  function(ko, $) {
    ko.bindingHandlers.initializeValue = {
        init: function(element, valueAccessor) {
            if (valueAccessor().serverValidationErrors === true) {
                valueAccessor().value(element.getAttribute('value'));
            }
        },
        update: function(element, valueAccessor) {
            var value = valueAccessor().value;
            element.setAttribute('value', ko.utils.unwrapObservable(value))
        }
    };
  }
);