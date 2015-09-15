angular.module("umbraco")
    .controller("RM.HC.CryptoTitleLabel", function ($scope, $routeParams, cryptoTitleTextResource) {

    	var encrypted_contents = $scope.model.value;
    	$scope.model.value = '';

    	cryptoTitleTextResource.getDecryptedTitle(encrypted_contents.replace('/', '@'), $routeParams.id).then(function (response) {
        	$scope.model.value = JSON.parse(response.data);
    	});

    });