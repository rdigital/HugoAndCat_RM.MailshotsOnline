define([],

    function() {

        // ViewModel
        function rentListComponentViewModel() {

            console.log('rentList.js');
        }

        return {
            viewModel: rentListComponentViewModel,
            template: { require: 'text!/scripts/src/templates/rent-list.html' }
        };
});