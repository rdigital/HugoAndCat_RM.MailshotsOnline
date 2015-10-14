angular.module("umbraco")
    .controller(
        "RM.HC.PrivateImageViewerController",
        // inject the umbraco asset service
        function ($scope, assetsService) {
            $scope.image = $scope.model.value + '?size=small';
            //$scope.clearImage = function () {
            //    alert("Clicked!");
            //    $scope.model.value = "";
            //};
        });