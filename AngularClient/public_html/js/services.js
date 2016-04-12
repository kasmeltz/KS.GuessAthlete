var app = angular.module('app');

app.service('$con', function ($http) {
    var base_url = 'http://localhost:51986/';
    return {
        getBaseUrl: function () { return base_url; },
		get: function(url, callback, cache) {
			$http.get(base_url + url,
                { cache: cache })
                .then(function (response) {
                    callback(response.data);
                });
		}
    };
});

app.service('$seasonDataService', function ($con) {
    return {
        load: function (callback) {	
			var url = 'api/seasons';
			$con.get(url, callback, true);
        }
    };
});

app.service('$awardsDataService', function ($con, $http) {
    return {
        load: function (callback) {	
			var url = 'api/awards';
			$con.get(url, callback, true);
        }
    };
});

app.service('$conferencesDataService', function ($con, $http) {
    return {
        load: function (callback) {	
			var url = 'api/conferences';
			$con.get(url, callback, true);
        }
    };
});

app.service('$divisionsDataService', function ($con, $http) {
    return {
        load: function (callback) {	
			var url = 'api/divisions';
			$con.get(url, callback, true);
        }
    };
});

app.service('$teamIdentitiesDataService', function ($con, $http) {
    return {
        load: function (callback) {	
			var url = 'api/teamidentities';
			$con.get(url, callback, true);
        }
    };
});

app.service('$teamIdentityDivisionsDataService', function ($con, $http) {
    return {
        load: function (callback) {	
			var url = 'api/teamidentitydivisions';
			$con.get(url, callback, true);
        }
    };
});

app.service('$pickAthleteDataService', function ($con, $http) {
    return {
        pickAthlete: function (options, callback) {	
			var url = 'api/pickAthlete';
            url += '?skaterGamesPlayed=' + options.skaterGamesPlayed;
			url += '&skaterPoints=' + options.skaterPoints;
			url += '&skaterPPG=' + options.skaterPPG;
			url += '&goalieGamesPlayed=' + options.goalieGamesPlayed;
			url += '&goalieWins=' + options.goalieWins;
			url += '&startYear=' + options.startYear;
			$con.get(url, callback, false);
        }
    };
});