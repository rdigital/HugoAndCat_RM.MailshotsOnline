define(['knockout', 'jquery'],
  function(ko, $) {
    ko.bindingHandlers.attrIf = {
        update: function (element, valueAccessor, allBindingsAccessor) {
            var h = ko.utils.unwrapObservable(valueAccessor());
            var show = ko.utils.unwrapObservable(h._if);
            if (show) {
                ko.bindingHandlers.attr.update(element, valueAccessor, allBindingsAccessor);
            } else {
                for (var k in h) {
                    if (h.hasOwnProperty(k) && k.indexOf("_") !== 0) {
                        $(element).removeAttr(k);
                    }
                }
            }
        }
    };
  }
);