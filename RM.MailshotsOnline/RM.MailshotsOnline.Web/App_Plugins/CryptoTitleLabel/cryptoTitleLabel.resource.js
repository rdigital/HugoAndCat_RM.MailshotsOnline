//adds the resource to umbraco.resources module:
angular.module('umbraco.resources').factory('cryptoTitleTextResource',
    function ($q, $http) {
    	//the factory object returned
    	return {
    		//this cals the Api Controller we setup earlier
    		getDecryptedTitle: function (id, nodeId) {
    			return $http.get("backoffice/Cryptography/CryptoApi/GetDecryptedTitle?id=" + id + "&nodeId=" + nodeId);
    		}
    	};
    }
);