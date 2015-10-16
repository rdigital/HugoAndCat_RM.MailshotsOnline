require([
		'knockout',
		'domReady',
		'perfectScrollbar',
		'koValidation',
		'components/lists',
		'components/createList',
		'components/listDetail',
		'components/campaignList',
		'components/myImages',
		'components/pagination',
		'components/error',
		'components/notification'
	],
	function(
		ko, 
		domReady, 
		prefectScrollbar, 
		koValidation, 
		listsComponent, 
		createListComponent,
		listDetailComponent,
		campaignListComponent,
		myImagesComponent,
		paginationComponent, 
		errorComponent, 
		notificationComponent
	) {

		// register components
		ko.components.register('lists-component', listsComponent);
		ko.components.register('create-list-component', createListComponent);
		ko.components.register('list-detail-component', listDetailComponent);
		ko.components.register('campaign-list-component', campaignListComponent);
		ko.components.register('my-images-component', myImagesComponent);
		ko.components.register('pagination-component', paginationComponent);
		ko.components.register('error-component', errorComponent);
		ko.components.register('notification-component', notificationComponent);

		koValidation.init({insertMessages: false});

		// apply bindings
		ko.applyBindings();

		// initialise perfect scrollbar plugin

		domReady(function(){
			$('.scrollable').perfectScrollbar();
		});
	}
);