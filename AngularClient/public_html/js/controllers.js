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
			var question = $scope.addQuestions[$scope.selectedQuestionType.type]();
			question.type = $scope.selectedQuestionType.type;
			$scope.questionsAsked.push(question)
			$scope.canAddQuestion = false;
		}		
		
		$scope.selectedQuestionType = null;
	};
	
	$scope.askQuestion = function(question) {
		question.answered = $scope.questionResponses[question.type](question);		
		$scope.canAddQuestion = true;		
	};	
	
	// $question add functions
	$scope.addQuestions = [];
	// first letter of name
	$scope.addQuestions.push(
		function() {
			return { selectedLetter:'A', selectedName:'first' };
		}
	);
	// played between years
	$scope.addQuestions.push(
		function() {
			return { selectedStart:1917, selectedEnd:thisYear };
		}
	);
	
	// question response functions
	$scope.questionResponses = [];
	// first letter of name
	$scope.questionResponses.push(
		function(question) {
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
	);
	// played between years
	$scope.questionResponses.push(
		function(question) {
			alert(question.selectedStart);
			alert(question.selectedEnd);
		}
	);	

	$scope.canAddQuestion = true;	
	$scope.questionsAsked = [];

	$scope.letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];	
	$scope.names = ['first', 'last'];	
	$scope.years = [];
	for (var year = 1917;year <= thisYear;year++) {
		$scope.years.push(year);
	}
	
	$scope.questionTypes = [];
	$scope.questionTypes.push({ display:'First letter of name', type:FIRST_LETTER_QUESTION });
	$scope.questionTypes.push({ display:'Play during years', type:PLAY_DURING_YEARS_QUESTION });
	
	$scope.getRandomAthlete();
}]);
