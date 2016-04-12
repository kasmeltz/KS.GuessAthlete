var app = angular.module('app');

app.controller('homeController', ['$scope', '$route', '$pickAthleteDataService', function ($scope, $route, $pickAthleteDataService) {
    $scope.$route = $route;

    $scope.getRandomAthlete = function () {
		var options = {
			skaterGamesPlayed: 1500,
			skaterPoints: 2500,
			skaterPPG: 1.5,
			goalieGamesPlayed: 10000,
			goalieWins: 10000,
			startYear: 1960
		};
		
        $pickAthleteDataService.pickAthlete(options, function (data) {
            alert(data);
			alert(data.Name);
        });
    };

    $scope.getRandomAthlete();
}]);
