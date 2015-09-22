// simple custom binding to bind an individual element from a template to an observable
// such that it can be referenced directly in a view model
define(['knockout', 'jquery'],
  function(ko, $) {
    ko.bindingHandlers.element = {
        init: function(element, valueAccessor) {
          var value = valueAccessor();
          value($(element));
        }
    };
  }
);