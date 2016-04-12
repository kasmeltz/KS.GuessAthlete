﻿var app = angular.module('app');
var thisYear = new Date().getFullYear();

app.controller('homeController', ['$scope', '$route', '$pickAthleteDataService', '$seasonDataService', function ($scope, $route, $pickAthleteDataService, $seasonDataService) {
    $scope.$route = $route;

	$scope.getSeasons = function() {
		$seasonDataService.load(function (data) {
			$scope.seasons = {};
			for (var idx in data) {
				var season = data[idx];
				$scope.seasons[season.Id] = season;
			}			
        });
	};
	
    $scope.getRandomAthlete = function () {
		var options = {
			skaterGamesPlayed: 1600,
			skaterPoints: 1300,
			skaterPPG: 0,
			goalieGamesPlayed: 1000,
			goalieWins: 600,
			startYear: 1965
		};
		
		var options = {
			skaterGamesPlayed: 100000,
			skaterPoints: 1300,
			skaterPPG: 0,
			goalieGamesPlayed: 1000,
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
	
	$scope.addQuestions = [];
	$scope.questionResponses = [];
	$scope.questionTypes = [];
	
	$scope.addQuestionType = function(displayText, questionProto, responder) {
		var currentType = $scope.questionTypes.length;
		$scope.questionTypes.push({ display:displayText, type:currentType });
		$scope.addQuestions.push(questionProto);
		$scope.questionResponses.push(responder);
	}
	
	// positions
	$scope.addQuestionType('Primary position', 
		function() {
			return { selectedPosition:'C' };
		},
		function(question) {
			return $scope.athlete.Position.indexOf(question.selectedPosition) >= 0 ? 'yes' : 'no';
		});		
	
	// first letter of name	
	$scope.addQuestionType('First letter of name', 
		function() {
			return { selectedLetter:'A', selectedName:'first' };
		},
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
		});		
	
	// played at least one year between years
	$scope.addQuestionType('Played at least one year between', 
		function() {
			return { selectedStart:1917, selectedEnd:thisYear };
		},
		function(question) {
			for (var idx in $scope.athlete.Stats) {
				var stat = $scope.athlete.Stats[idx];
				var season = $scope.seasons[stat.SeasonId];
				var startYear = new Date(season.StartDate).getFullYear();
				var endYear = new Date(season.EndDate).getFullYear();
				
				if (startYear >= question.selectedStart && endYear <= question.selectedEnd) {
					return 'yes';
				}
			}	

			return 'no';
		});
		
	// played every year year between years
	$scope.addQuestionType('Played every year between', 
		function() {
			return { selectedStart:1917, selectedEnd:thisYear };
		},
		function(question) {
			var years = {};
			var yearTotal = 0;
						
			for (var idx in $scope.athlete.Stats) {
				var stat = $scope.athlete.Stats[idx];
				var season = $scope.seasons[stat.SeasonId];
				var startYear = new Date(season.StartDate).getFullYear();
				var endYear = new Date(season.EndDate).getFullYear();
				
				if (startYear >= question.selectedStart && endYear <= question.selectedEnd) {
					if (!years[startYear]) {
						years[startYear] = true;
						yearTotal++; 
					}
					
					if (!years[endYear]) {
						years[endYear] = true;
						yearTotal++; 
					}
				}
			}	
				
			if (yearTotal >= (question.selectedEnd - question.selectedStart) + 1) {
				return 'yes';
			}

			return 'no';
		});		

	// misc data
	$scope.letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];	
	$scope.names = ['first', 'last'];	
	$scope.years = [];
	for (var year = 1917;year <= thisYear;year++) {
		$scope.years.push(year);
	}
	$scope.positions = ['C', 'LW', 'RW', 'D', 'G']

	$scope.canAddQuestion = true;	
	$scope.questionsAsked = [];
	$scope.getSeasons();
	$scope.getRandomAthlete();
}]);
