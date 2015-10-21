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
		'components/notification',
		'view-models/login',
		'kostopbinding',
		'koselect2',
        'koaresame'
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
		notificationComponent,
		loginViewModel
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


		koValidation.init({
			insertMessages: true,
			decorateInputElement: true,
			decorateElementOnModified: true,
			errorMessageClass: 'validation-msg',
			errorElementClass: 'validation-error'
		});


		// apply bindings
		ko.applyBindings();

		if ($('.register').length > 0) {
			require(['view-models/register'],function(registerView) {
				console.log('before binding');
				ko.applyBindings(registerView, $('.register')[0]);
			});
		}

		if ($('#login').length > 0) {
			ko.applyBindings(loginViewModel, $('#login')[0]);
		}
		

		// initialise perfect scrollbar plugin

		domReady(function(){
			$('.scrollable').perfectScrollbar();
		});
	}
);