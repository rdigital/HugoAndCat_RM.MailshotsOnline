angular.module("umbraco")
    .controller(
        "RM.HC.ImageUsageCountController",
        // inject the umbraco asset service
        function ($scope, assetsService) {
            // Get the ID of the media item
            var mediaId = 0;
            var hash = window.location.hash;
            if (hash.startsWith('#/media/media/edit/')) {
                var idText = hash.replace('#/media/media/edit/', '');
                var array = idText.split('?');
                if (array.length > 0) {
                    mediaId = array[0];
                }
            }

            if (mediaId > 0) {
                $.ajax({
                    url: '/Umbraco/Api/ImageLibrary/GetImageUsageCount/' + mediaId,
                    type: 'GET',
                    success: function (count) {
                        $scope.usageCount = count;
                    }
                });
            }
            else {
                $scope.usageCount = 0;
            }
        });