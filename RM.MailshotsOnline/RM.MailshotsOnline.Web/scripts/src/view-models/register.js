define([
        'knockout',
        'select2'
    ],

    function(ko) {


        var registerViewModel = {

            proceedToNextStage: function() {
                console.log('register view model');
                alert('proceed to next stage');
            }
        }



        return registerViewModel;
});