var app = angular.module('app');

app.service('$con', function ($http) {
    var base_url = 'http://localhost:51986/';
    return {
        getBaseUrl: function () { return base_url; },
    };
});

app.service('$seasonDataService', function ($con, $http) {
    return {
        load: function (callback) {	
			var url = 'api/seasons';
			
            $http.get($con.getBaseUrl() + url,
                { cache: true })
                .then(function (response) {
                    callback(response.data);
                });
        }
    };
});

app.service('$teamIdentitiesDataService', function ($con, $http) {
    return {
        load: function (callback) {	
			var url = 'api/teamidentities';
			
            $http.get($con.getBaseUrl() + url,
                { cache: true })
                .then(function (response) {
                    callback(response.data);
                });
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
			
            $http.get($con.getBaseUrl() + url,
                { cache: true })
                .then(function (response) {
                    callback(response.data);
                });
        }
    };
});