require([
		'knockout',
		'domReady',
		'perfectScrollbar',
		'koValidation',
		'components/lists',
		'components/createList',
		'components/listDetail',
		'components/myImages',
		'components/pagination',
		'components/error',
		'components/notification',
		'view-models/register',
		'view-models/login',
		'kostopbinding',
        'koaresame'
	],
	function(
		ko, 
		domReady, 
		prefectScrollbar, 
		koValidation, 
		listsComponent, 
		createListComponent,
		listDetailComponent,
		myImagesComponent,
		paginationComponent, 
		errorComponent, 
		notificationComponent,
		registerView,
		loginViewModel
	) {


		// register components
		ko.components.register('lists-component', listsComponent);
		ko.components.register('create-list-component', createListComponent);
		ko.components.register('list-detail-component', listDetailComponent);
		ko.components.register('my-images-component', myImagesComponent);
		ko.components.register('pagination-component', paginationComponent);
		ko.components.register('error-component', errorComponent);
		ko.components.register('notification-component', notificationComponent);


		koValidation.init({
			insertMessages: true,
			errorMessageClass: 'validation-msg'
		});


		// apply bindings
		ko.applyBindings();

		if ($('.register').length > 0) {
			ko.applyBindings(registerView, $('.register')[0]);
		}

		if ($('#login').length > 0) {
			ko.applyBindings(loginViewModel, $('#login')[0]);
		}
		

		// initialise perfect scrollbar plugin

		domReady(function(){
			$('.scrollable').perfectScrollbar();
		});
	}
);