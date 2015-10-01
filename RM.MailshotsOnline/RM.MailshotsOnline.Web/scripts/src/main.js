require([
		'knockout',
		'domReady',
		'perfectScrollbar',
		'components/lists',
		'components/createList',
		'components/pagination'
	],
	function(ko, domReady, prefectScrollbar, listsComponent, createListComponent, paginationComponent) {

		// register components
		ko.components.register('lists-component', listsComponent);
		ko.components.register('create-list-component', createListComponent);
		ko.components.register('pagination-component', paginationComponent);

		// apply bindings
		ko.applyBindings();

		// initialise perfect scrollbar plugin

		domReady(function(){
			$('.scrollable').perfectScrollbar();
		});
	}
);