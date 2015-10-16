define(['knockout'],
  function(ko) {

    /*
     * Ensures a field has the same value as another field (E.g. "Confirm Password" same as "Password"
     * Parameter: otherField: the field to compare to
     * Example
     *
     * viewModel = {
     *   var vm = this;
     *   vm.password = ko.observable();
     *   vm.confirmPassword = ko.observable();
     * }   
     * viewModel.confirmPassword.extend( {areSame: { params: viewModel.password, message: "Confirm password must match password" }});
    */
    ko.validation.rules['areSame'] = {
        getValue: function (o) {
            return (typeof o === 'function' ? o() : o);
        },
        validator: function (val, otherField) {
            // console.log(val, otherField);
            return val === this.getValue(otherField);
        },
        message: 'The fields must have the same value'
    };

    ko.validation.registerExtenders();
  }
);