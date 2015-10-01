require([
		'knockout',
		'domReady',
		'perfectScrollbar',
		'components/lists',
		'components/pagination'
	],
	function(ko, domReady, prefectScrollbar, listsComponent, paginationComponent) {

		// register components
		ko.components.register('lists-component', listsComponent);
		ko.components.register('pagination-component', paginationComponent);

		// apply bindings
		ko.applyBindings();

		// initialise perfect scrollbar plugin

		domReady(function(){
			$('.scrollable').perfectScrollbar();
		});
	}
);