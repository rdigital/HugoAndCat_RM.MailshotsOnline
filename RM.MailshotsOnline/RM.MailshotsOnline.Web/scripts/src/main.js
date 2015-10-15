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
		'stopbinding'
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
		registerView
	) {

		// register components
		ko.components.register('lists-component', listsComponent);
		ko.components.register('create-list-component', createListComponent);
		ko.components.register('list-detail-component', listDetailComponent);
		ko.components.register('my-images-component', myImagesComponent);
		ko.components.register('pagination-component', paginationComponent);
		ko.components.register('error-component', errorComponent);
		ko.components.register('notification-component', notificationComponent);


		koValidation.init({insertMessages: false});


		// apply bindings
		ko.applyBindings();


		ko.applyBindings(registerView, $('.register')[0]);
		

		// initialise perfect scrollbar plugin

		domReady(function(){
			$('.scrollable').perfectScrollbar();
		});
	}
);