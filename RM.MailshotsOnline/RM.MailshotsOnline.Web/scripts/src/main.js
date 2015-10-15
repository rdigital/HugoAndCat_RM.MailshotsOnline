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
		'components/register'
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
		registerComponent
	) {

		// register components
		ko.components.register('lists-component', listsComponent);
		ko.components.register('create-list-component', createListComponent);
		ko.components.register('list-detail-component', listDetailComponent);
		ko.components.register('my-images-component', myImagesComponent);
		ko.components.register('pagination-component', paginationComponent);
		ko.components.register('error-component', errorComponent);
		ko.components.register('notification-component', notificationComponent);
		ko.components.register('register-component', registerComponent);

		koValidation.init({insertMessages: false});

		// apply bindings
		ko.applyBindings();

		// initialise perfect scrollbar plugin

		domReady(function(){
			$('.scrollable').perfectScrollbar();
		});
	}
);