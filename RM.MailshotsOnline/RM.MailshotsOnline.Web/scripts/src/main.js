require([
		'knockout',
		'domReady',
		'perfectScrollbar',
		'components/lists',
		'components/createList',
		'components/pagination',
		'components/error',
		'view-models/state'
	],
	function(ko, domReady, prefectScrollbar, listsComponent, createListComponent, paginationComponent, errorComponent, stateViewModel) {

		// register components
		ko.components.register('lists-component', listsComponent);
		ko.components.register('create-list-component', createListComponent);
		ko.components.register('pagination-component', paginationComponent);
		ko.components.register('error-component', errorComponent);

		// apply bindings
		ko.applyBindings();

		// initialise perfect scrollbar plugin

		domReady(function(){
			$('.scrollable').perfectScrollbar();
		});
	}
);