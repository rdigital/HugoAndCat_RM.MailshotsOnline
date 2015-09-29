require([
		'knockout',
		'components/lists',
		'components/pagination'
	],
	function(ko, listsComponent, paginationComponent) {

		// register components
		ko.components.register('lists-component', listsComponent);
		ko.components.register('pagination-component', paginationComponent);

		// apply bindings
		ko.applyBindings();
	}
);