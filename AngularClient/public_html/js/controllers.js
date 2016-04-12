var app = angular.module('app');
var thisYear = new Date().getFullYear();
var FIRST_LETTER_QUESTION = 0;
var PLAY_DURING_YEARS_QUESTION = 1;

app.controller('homeController', ['$scope', '$route', '$pickAthleteDataService', function ($scope, $route, $pickAthleteDataService) {
    $scope.$route = $route;

    $scope.getRandomAthlete = function () {
		var options = {
			skaterGamesPlayed: 1500,
			skaterPoints: 1400,
			skaterPPG: 0,
			goalieGamesPlayed: 1200,
			goalieWins: 600,
			startYear: 1965
		};		
		
        $pickAthleteDataService.pickAthlete(options, function (data) {
			$scope.athlete = data;
        });
    };
	
	$scope.addQuestion = function() {		
		if ($scope.selectedQuestionType) {
			if ($scope.selectedQuestionType.value == FIRST_LETTER_QUESTION) {
				$scope.questionsAsked.push({selectedLetter:'A',selectedName:'first',type:FIRST_LETTER_QUESTION});				
			}
			
			if ($scope.selectedQuestionType.value == PLAY_DURING_YEARS_QUESTION) {
				$scope.questionsAsked.push({selectedStart:1917,selectedEnd:thisYear,type:PLAY_DURING_YEARS_QUESTION});				
			}
			
			$scope.canAddQuestion = false;
		}		
		
		$scope.selectedQuestionType = null;
	};
	
	$scope.firstLetterQuestion = function(question) {
		var names = $scope.athlete.Name.split(' ');		
		if (question.selectedName == 'first') {
			var name = names[0];
			if (name.substring(0,1) == question.selectedLetter) {
				return 'yes';
			} 
		} else if (question.selectedName == 'last') {
			var name = names[names.length-1];
			if (name.substring(0,1) == question.selectedLetter) {
				return 'yes';
			} 
		}
		
		return 'no';
	}
	
	$scope.askQuestion = function(question) {
		var yesNo = 'no';		
		
		switch(question.type) {
			case FIRST_LETTER_QUESTION:
				yesNo = $scope.firstLetterQuestion(question);
				break;
		}
		
		question.answered = yesNo;		
		$scope.canAddQuestion = true;		
	};
	
	$scope.canAddQuestion = true;	
	$scope.questionsAsked = [];
	$scope.letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];	
	$scope.names = ['first', 'last'];	
	$scope.years = [];
	for (var year = 1917;year <= thisYear;year++) {
		$scope.years.push(year);
	}
    $scope.getRandomAthlete();
	
	$scope.questionTypes = [];
	$scope.questionTypes.push({ display:'First letter of name', value:FIRST_LETTER_QUESTION });
	$scope.questionTypes.push({ display:'Play during years', value:PLAY_DURING_YEARS_QUESTION });
}]);
