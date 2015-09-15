angular.module("umbraco")
    .controller("RM.HC.CryptoLabel", function ($scope, $routeParams, cryptoTextResource) {

    	var encrypted_contents = $scope.model.value;
    	$scope.model.value = '';

        cryptoTextResource.getDecrypted(encrypted_contents.replace('/', '@'), $routeParams.id).then(function (response) {
        	$scope.model.value = JSON.parse(response.data);
    	});

    });