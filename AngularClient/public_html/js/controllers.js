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
			$scope.athlete = data;
        });
    };
	
	$scope.askQuestion = function(question) {
		var yesNo = 'no';
		
		var names = $scope.athlete.Name.split(' ');		
		if (question.selectedName == 'first') {
			var name = names[0];
			if (name.substring(0,1) == question.selectedLetter) {
				yesNo = 'yes';
			} 
		} else if (question.selectedName == 'last') {
			var name = names[names.length-1];
			if (name.substring(0,1) == question.selectedLetter) {
				yesNo = 'yes';
			} 
		}
		
		question.answered = yesNo;		
		$scope.questionsAsked.push({selectedLetter:'A',selectedName:'first'});
	}
	
	$scope.letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];	
	$scope.names = ['first', 'last'];	
	$scope.questionsAsked = [{selectedLetter:'A',selectedName:'first'}];
    $scope.getRandomAthlete();
}]);
