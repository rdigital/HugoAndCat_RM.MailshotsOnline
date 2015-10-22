define(['knockout', 'jquery'],
  function(ko, $) {
    ko.bindingHandlers.initializeChecked = {
        init: function(element, valueAccessor) {
            if (valueAccessor().serverValidationErrors === true) {
                var checked = $(element).attr('checked');
                valueAccessor().checked(checked);
            }
        }
    };
  }
);