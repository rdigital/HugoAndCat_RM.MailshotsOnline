define([
        'knockout',
        'select2'
    ],

    function() {

        function registerComponentViewModel() {

        }

        registerComponentViewModel.prototype.proceedToNextStage = function() {
        };


        registerComponentViewModel.prototype.initSelect = function() {
            $('select').select2();
        };



        registerComponentViewModel.prototype.submitRegistration = function() {

            var formData = $('form.register__form').serialize();
            
            $.ajax({
                url: '/register/',
                data: formData,
                method: "POST",
                success: function(result) {
                    console.log(result);
                },
                error: function(error) {
                    if (error) {
                        console.log(error);
                    } else {
                        console.log("Oops!");
                        console.log("Looks like something went wrong, please try again");
                    }
                }
            });
        };

        return {
            viewModel: registerComponentViewModel,
            template: { require: 'text!/scripts/src/templates/register.html' }
        };
});