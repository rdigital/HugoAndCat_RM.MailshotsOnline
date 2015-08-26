// content editable custom binding
define(['knockout', 'jquery'],
    function(ko, $) {

        ko.bindingHandlers.htmlLazy = {
            init: function (element, valueAccessor) {
                var value = ko.unwrap(valueAccessor());
                
                element.innerHTML = value;
            },
            update: function (element, valueAccessor) {
                var value = ko.unwrap(valueAccessor());
                
                if (!element.isContentEditable) {
                    element.innerHTML = value;
                }
            }
        };
        ko.bindingHandlers.contentEditable = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                var value = ko.unwrap(valueAccessor()),
                    htmlLazy = allBindingsAccessor().htmlLazy;
                
                $(element).on("input", function () {
                    if (this.isContentEditable && ko.isWriteableObservable(htmlLazy)) {
                        htmlLazy(this.innerHTML);
                    }
                });
            },
            update: function (element, valueAccessor) {
                var value = ko.unwrap(valueAccessor());
                
                element.contentEditable = value;
                
                if (!element.isContentEditable) {
                    $(element).trigger("input");
                }
            }
        };
      
    }
)