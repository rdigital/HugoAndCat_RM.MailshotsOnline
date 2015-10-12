$.validator.setDefaults({
    errorElement: "span",
    
    highlight: function (element, errorClass, validClass) {
        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
    },
    errorPlacement: function (error, element) {
        if (element.parent('.input-group').length || element.prop('type') === 'checkbox' || element.prop('type') === 'radio') {
            error.insertAfter(element.parent());
        } else {
            error.insertAfter(element);
        }
    }
});

$.validator.addMethod('requiredif',
    function (value, element, parameters) {
        var id = '#' + parameters['dependentproperty'];

        // get the target value (as a string, 
        // as that's what actual value will be)
        var targetstrings = parameters['targetstrings'].split(",");

        // get the actual value of the target control
        // note - this probably needs to cater for more 
        // control types, e.g. radios
        var control = $(id);
        var controltype = control.attr('type');
        var actualvalue =
            controltype === 'checkbox' ?
            control.attr('checked').toString() :
            control.val();

        // if the condition is true, reuse the existing 
        // required field validator functionality
        if (targetstrings.indexOf(actualvalue) > -1) {
            return $.validator.methods.required.call(this, value, element, parameters);
        }

        return true;
    }
);


if ($("form") !== undefined) {
    $("form").validate({
        errorClass: "field-validation-error text-danger"
    });
}