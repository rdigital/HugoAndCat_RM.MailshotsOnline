//adds the resource to umbraco.resources module:
angular.module('umbraco.resources').factory('cryptoTextResource',
    function ($q, $http) {
    	//the factory object returned
    	return {
    		//this cals the Api Controller we setup earlier
    		getDecrypted: function (text, nodeId) {
    			return $http.get("backoffice/Cryptography/CryptoApi/GetDecrypted?text=" + text + "&nodeId=" + nodeId);
    		}
    	};
    }
);