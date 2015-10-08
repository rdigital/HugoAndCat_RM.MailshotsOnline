require([
		'jquery',
		'knockout',
		'komapping',
		'pointerevents',
		'components/editor',
		'components/theme',
		'components/template',
		'view_models/state',
		'view_models/history',
		'view_models/format',
		'view_models/template',
		'view_models/theme',
		'view_models/user',
		'view_models/auth',
		'domReady!'
	],
	function($, ko, mapping, pointerevents, editorComponent, themeComponent, templateComponent, stateViewModel, historyViewModel, authViewModel) {
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
			stateViewModel.backgroundSelected(null);
			historyViewModel.pushToHistory();
		};

		// apply bindings
		ko.applyBindings();

		$(document).ready(function(){
	        pointerevents.initialize({});
	        // ie10 hack
	        if (Function('/*@cc_on return document.documentMode===10@*/')()){
		        document.documentElement.className+=' ieOld';
		    }
		    document.body.addEventListener('mscontrolselect', function (evt) {
			    evt.preventDefault();
			});
	    });
	}
);