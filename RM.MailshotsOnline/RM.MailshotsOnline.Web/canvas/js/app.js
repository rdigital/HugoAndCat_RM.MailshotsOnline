require([
		'knockout',
		'komapping',
		'components/editor',
		'components/theme',
		'components/template',
		'view_models/state',
		'view_models/format',
		'view_models/template',
		'view_models/theme',
		'view_models/user',
		'view_models/history',
		'domReady!'
	],
	function(ko, mapping, editorComponent, themeComponent, templateComponent, stateViewModel, historyViewModel) {
		// register components
		ko.components.register('editor-component', editorComponent);
		ko.components.register('theme-component', themeComponent);
		ko.components.register('template-component', templateComponent);

		/**
		 * nullify selected element on app view model, which has the effect
		 * of deselecting all elements in the editor
		 */
		this.unfocus = function unfocus() {
			stateViewModel.selectElement(null);
			historyViewModel.pushToHistory();
		}

		// apply bindings
		ko.applyBindings();
	}
);