angular.module("umbraco")
    .controller(
        "RM.HC.PrivateImageViewerController",
        // inject the umbraco asset service
        function ($scope, assetsService) {
            $scope.image = "/umbraco/backoffice/UmbracoApi/Images/GetResized?width=200&imagePath=" + encodeURI($scope.model.value);
            //$scope.clearImage = function () {
            //    alert("Clicked!");
            //    $scope.model.value = "";
            //};
        });