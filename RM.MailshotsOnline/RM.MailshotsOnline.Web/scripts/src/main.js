require([
		'knockout',
		'components/lists'
	],
	function(ko, listsComponent) {

		// register components
		ko.components.register('lists-component', listsComponent);

		// apply bindings
		ko.applyBindings();
	}
);