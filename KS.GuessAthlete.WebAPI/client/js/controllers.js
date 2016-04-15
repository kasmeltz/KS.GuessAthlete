var app = angular.module('app');
var thisYear = new Date().getFullYear();

var secondsToMinutesDisplay = function(sec_num) {
	var minutes = Math.floor(sec_num / 60);
	var seconds = sec_num - (minutes * 60);
	if (minutes < 10) { minutes = '0' + minutes; }
	if (seconds < 10) { seconds = '0' + seconds; }
	return minutes + ':' + seconds;
};

var getEditDistance = function(a, b) {
	if(a.length == 0) return b.length;
	if(b.length == 0) return a.length;
	
	a = a.toLowerCase();
	b = b.toLowerCase();
	
	var matrix = [];
	
	var i;
	for(i = 0; i <= b.length; i++) {
		matrix[i] = [i];
	}
	
	var j;
	for(j = 0;j <= a.length; j++) {
		matrix[0][j] = j;
	}
	
	for(i = 1; i <= b.length;i++) {
		for(j = 1;j <= a.length;j++) {
			if(b.charAt(i-1) == a.charAt(j-1)) {
				matrix[i][j] = matrix[i-1][j-1];
			} else {
				matrix[i][j] = Math.min(matrix[i-1][j-1] + 1,
					Math.min(matrix[i][j-1] + 1,
						matrix[i-1][j] + 1));
			}
		}
	}
	
	return matrix[b.length][a.length];
};

app.controller('gameController', ['$rootScope', '$scope', '$route', '$interval', '$pickAthleteDataService', '$seasonDataService', '$awardsDataService', '$conferencesDataService', '$divisionsDataService', '$teamIdentitiesDataService', '$teamIdentityDivisionsDataService',
	function ($rootScope, $scope, $route, $interval, $pickAthleteDataService, $seasonDataService, $awardsDataService, $conferencesDataService, $divisionsDataService, $teamIdentitiesDataService, $teamIdentityDivisionsDataService) {
    $scope.$route = $route;
	
	// misc data
	$scope.countries = [ 'Canada', 'USA', 'Sweden', 'Czechoslovakia', 'Union of Soviet Socialist Republics', 
		'Finland', 'United Kingdom', 'Germany', 'Switzerland', 'Russian Federation', 'Denmark', 'Austria',
		'France', 'Norway', 'Poland', 'Ireland', 'Czech Republic', 'Yugoslavia', 'Italy', 'Georgia', 'Latvia',
		'Venezuela', 'East Germany', 'Japan', 'Brazil', 'Netherlands', 'Republic of Korea', 'Lithuania', 
		'Nigeria', 'Lebanon', 'Indonesia', 'Jamaica', 'Belarus', 'Belgium', 'Slovenia', 'Haiti',
		'Brunei Darussalam', 'United Republic of Tanzania', 'Taiwan', 'Paraguay', 'South Africa' ];		
	$scope.Canada = ['Ontario', 'Quebec', 'Alberta', 'Saskatchewan', 'Manitoba', 'British Columbia', 
		'Nova Scotia', 'New Brunswick', 'Prince Edward Island', 'Newfoundland and Labrador', 'Northwest Territories', 'Yukon' ];
	$scope.USA = ['Minnesota', 'Massachusetts', 'Michigan', 'New York', 'Illinois', 'California', 'Pennsylvania', 'Wisconsin', 'Connecticut',
					'Ohio', 'Rhode Island', 'Missouri', 'New Jersey', 'North Dakota', 'Alaska', 'Washington', 'Colorado', 'New Hampshire',
					'Indiana', 'Florida', 'Texas', 'Maine', 'Oklahoma', 'Oregon', 'Utah', 'Virginia', 'Maryland', 'Georgia', 
					'District of Columbia', 'Alabama', 'Nebraska', 'North Carolina', 'Vermont', 'Idaho', 'Arizona', 'Iowa', 
					'Delaware', 'South Carolina', 'Montana' ];
	
	$scope.awardNumbers = [];
	for(var n = 1; n <= 10;n++) {
		$scope.awardNumbers.push(n);
	}
	$scope.jerseys = [];
	for(var n = 1; n <= 99;n++) {
		$scope.jerseys.push(n);
	}
	$scope.rounds = [ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 ];
	$scope.numbers = [];
	for(var n = 1; n <= 300;n++) {
		$scope.numbers.push(n);
	}
	$scope.seasonTypes = ['regular season', 'playoff', 'total'];
	$scope.letters = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];	
	$scope.names = ['first', 'last'];	
	$scope.years = [];
	for (var year = 1917;year <= thisYear;year++) {
		$scope.years.push(year);
	}
	$scope.positions = ['C', 'LW', 'RW', 'D', 'G']
	$scope.hundreds = [];
	for(var h = 50; h <= 3500;h += 50) {
		$scope.hundreds.push(h)
	}	
	
	// load data from web service	
	$scope.getSeasons = function() {
		$seasonDataService.load(function (data) {
			$scope.seasons = {};
			for (var idx in data) {
				var season = data[idx];
				season.StartYear = new Date(season.StartDate).getFullYear();
				season.EndYear = new Date(season.EndDate).getFullYear();
				$scope.seasons[season.Id] = season;
			}			
			
			$scope.getAwards();
        });			
	};
	
	$scope.getAwards = function() {
		$awardsDataService.load(function (data) {
			$scope.awards = [];
			$scope.awardsMap = {};
			for (var idx in data) {
				var award = data[idx];
				if (award.Name == 'Presidents Trophy') { continue; }
				if (award.Name == 'Jack Adams Award') { continue; }
				if (award.Name == 'Stanley Cup') { continue; }
				$scope.awards.push(award);
				$scope.awardsMap[award.Id] = award;
			}			
			
			$scope.getConferences();
        });	
	}
		
	$scope.getConferences = function() {
		$conferencesDataService.load(function (data) {
			$scope.conferences = [];
			$scope.conferencesMap = {};
			for (var idx in data) {
				var conference = data[idx];
				var startYear = conference.StartYear;
				var endYear = 'now'
				if (conference.EndYear) {
					endYear = conference.EndYear;
				}				
				conference.FullName = conference.Name +
					' (' + startYear + '-' + endYear + ')';	
				$scope.conferences.push(conference);
				$scope.conferencesMap[conference.Id] = conference;
			}			
			
			$scope.conferences.sort(function(a,b) {
				return a.StartYear < b.StartYear ? -1 : 1;
			});
			
			$scope.getDivisions();
        });	
	}	
	
	$scope.getDivisions = function() {
		$divisionsDataService.load(function (data) {
			$scope.divisions = [];
			$scope.divisionsMap = {};
			for (var idx in data) {
				var division = data[idx];
				var startYear = division.StartYear;
				var endYear = 'now'
				if (division.EndYear) {
					endYear = division.EndYear;
				}				
				var conference = $scope.conferencesMap[division.ConferenceId];				
				division.FullName = division.Name +
					' (' + conference.Name + '-' + startYear + '-' + endYear + ')';	
				$scope.divisions.push(division);
				$scope.divisionsMap[division.Id] = division;
			}			
			
			$scope.divisions.sort(function(a,b) {
				return a.StartYear < b.StartYear ? -1 : 1;
			});
			
			$scope.getTeamIdentities();
        });	
	}
	
	$scope.getTeamIdentities = function() {
		$teamIdentitiesDataService.load(function (data) {
			$scope.teamIdentities = [];
			$scope.teamIdentityMap = {};
			for (var idx in data) {
				var teamIdentity = data[idx];		
				var startYear = new Date(teamIdentity.StartDate).getFullYear();
				var endYear = 'now'
				if (teamIdentity.EndDate) {
					endYear = new Date(teamIdentity.EndDate).getFullYear();
				}				
				teamIdentity.StartYear = startYear;
				teamIdentity.EndYear = endYear;
				teamIdentity.FullName = teamIdentity.City + ' ' + teamIdentity.Name +
					' (' + startYear + '-' + endYear + ')';					
				$scope.teamIdentities.push(teamIdentity);
				$scope.teamIdentityMap[teamIdentity.Id] = teamIdentity;
			}			
			
			$scope.teamIdentities.sort(function(a,b) {
				if (a.City == b.City) {
					return a.StartYear < b.StartYear ? -1 : 1;
				}
				return a.FullName < b.FullName ? -1 : 1;
			});
			
			$scope.getTeamIdentityDivisions();
        });			
	};	
	
	$scope.getTeamIdentityDivisions = function() {
		$teamIdentityDivisionsDataService.load(function (data) {
			$scope.teamIdentityDivisionsMap = {};
			for (var idx in data) {
				var teamIdentityDivision = data[idx];
				if (!$scope.teamIdentityDivisionsMap[teamIdentityDivision.TeamIdentityId]) {
					$scope.teamIdentityDivisionsMap[teamIdentityDivision.TeamIdentityId] = [];
				}
				$scope.teamIdentityDivisionsMap[teamIdentityDivision.TeamIdentityId].push(teamIdentityDivision);
			}			
			
			$scope.loadQuestions();	
			$scope.newGame();
        });	
	}
	
	$scope.newGame = function() {	
		$scope.questionsAsked = [];
		$scope.incorrectGuesses = [];	
		$scope.guessedAthlete = '';
		$scope.correctGuess = false;		
		$scope.roundStarted = false;
		$scope.correctGuess = false;
		$scope.gameEnded = false;
		$scope.lives = 10;
		$scope.gameRound = 1;
		$scope.gamePoints = 0;
		$scope.athleteOptions = {
				skaterGamesPlayed: 1600,
				skaterPoints: 1300,
				skaterPPG: 0,
				goalieGamesPlayed: 900,
				goalieWins: 500,
				startYear: 1970
			};	
		$scope.roundSeconds = 20 * 60;
		$scope.timeLeft = secondsToMinutesDisplay($scope.roundSeconds);
		$scope.availablePoints = $scope.roundSeconds * $scope.gameRound;
		$rootScope.$broadcast('clearRoundHistory');
	}

	$scope.timerExpired = function() {
		$scope.timeLeft = 0;
		$scope.availablePoints = 0;
		$interval.cancel($scope.timerUpdate);
		$scope.lives--;
		$scope.roundStarted = false;
		if ($scope.lives <= 0) {
			$scope.lives = 0;
			$scope.gameOver();
		}
	}
	
	$scope.gameOver = function() {
		$scope.roundStarted = false;
		$scope.gameEnded = true;
		if ($scope.timerUpdate) {
			$interval.cancel($scope.timerUpdate);
		}
	}
	
    $scope.startRound = function () {
		if ($scope.roundStarted) {
			return;
		}
		
		if ($scope.gameEnded == true) {
			$scope.newGame();			
			return;
		}
		
		$scope.incorrectGuesses = [];
		$scope.questionsAsked = [];
		$scope.guessedAthlete = '';

		if ($scope.correctGuess == true) {
			$scope.athleteOptions.skaterGamesPlayed -= 50;			
			$scope.athleteOptions.skaterGamesPlayed = Math.max(200, $scope.athleteOptions.skaterGamesPlayed);
			$scope.athleteOptions.skaterPoints -= 25;
			$scope.athleteOptions.skaterPoints = Math.max(200, $scope.athleteOptions.skaterPoints);
			$scope.athleteOptions.goalieGamesPlayed -= 50;
			$scope.athleteOptions.goalieGamesPlayed = Math.max(200, $scope.athleteOptions.goalieGamesPlayed);
			$scope.athleteOptions.goalieWins -= 25;
			$scope.athleteOptions.goalieWins = Math.max(100, $scope.athleteOptions.goalieWins);
			$scope.athleteOptions.startYear -= 2;
			$scope.athleteOptions.startYear = Math.max(1915, $scope.athleteOptions.startYear);
			$scope.gameRound++;
		}
		
        $pickAthleteDataService.pickAthlete($scope.athleteOptions, function (data) {
			$scope.correctGuess = false;			
			$scope.athlete = data;			
			$scope.canAddQuestion = true;	
			$scope.roundStartTime = new Date();
			$scope.timeLeft = secondsToMinutesDisplay($scope.roundSeconds);
			$scope.availablePoints = $scope.roundSeconds * $scope.gameRound;
			$scope.roundStarted = true;						
			
			if ($scope.timerUpdate) {
				$interval.cancel($scope.timerUpdate);
			}
			
			$scope.timerUpdate = $interval(function() {
				var now = new Date();
				var elapsed = Math.floor((now.getTime() - $scope.roundStartTime.getTime()) / 1000);
				$scope.timeLeft = $scope.roundSeconds - elapsed;
				if ($scope.timeLeft <= 0) {
					$scope.timerExpired();
				}
				$scope.availablePoints = $scope.timeLeft * $scope.gameRound;
				$scope.timeLeft = secondsToMinutesDisplay($scope.timeLeft);
			}, 250);				
        });		
    };
	
	$scope.$on('$destroy', function() {
		if ($scope.timerUpdate) {
			$interval.cancel($scope.timerUpdate);
		}
	});	
		
	$scope.addQuestion = function(questionType) {		
		if (!$scope.canAddQuestion) {
			return;
		}
		
		var question = $scope.addQuestions[questionType.type]();
		question.type = questionType.type;
		question.answered = false;
		$scope.questionsAsked.push(question)
		$scope.canAddQuestion = false;
	};
	
	$scope.askQuestion = function(question) {
		question.answer = $scope.questionResponses[question.type](question);		
		question.answered = true;
		$scope.canAddQuestion = true;		
	};	
	
	$scope.questionTypes = [];
	$scope.addQuestions = {};
	$scope.questionResponses = {};
	
	$scope.addQuestionType = function(displayText, type, abbreviation, questionProto, responder) {
		$scope.questionTypes.push({ display:displayText, type:type, abbreviation: abbreviation });
		$scope.addQuestions[type] = questionProto;
		$scope.questionResponses[type] = responder;
	}
	
	$scope.loadQuestions = function() {
		// positions
		$scope.addQuestionType('Primary position', 'pos', 'POS',
			function() {
				return { selectedPosition:'C' };
			},
			function(question) {
				return $scope.athlete.Position.indexOf(question.selectedPosition) >= 0 ? true :  false;
			});		
		
		// first letter of name	
		$scope.addQuestionType('First letter of name', 'let', 'LET',
			function() {
				return { selectedLetter:'A', selectedName:'first' };
			},
			function(question) {
				var names = $scope.athlete.Name.split(' ');		
				if (question.selectedName == 'first') {
					var name = names[0];
					if (name.substring(0,1) == question.selectedLetter) {
						return true;
					} 
				} else if (question.selectedName == 'last') {
					var name = names[names.length-1];
					if (name.substring(0,1) == question.selectedLetter) {
						return true;
					} 
				}			
				return false;
			});		
		
		// played at least one year between years
		$scope.addQuestionType('Played at least one year between', 'oneyear', 'YEA',
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
						return true;
					}
				}	

				return false;
			});
			
		// played every year year between years
		$scope.addQuestionType('Played every year between', 'allyears', 'AYE',
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
					return true;
				}

				return false;
			});
			
		// played for team
		$scope.addQuestionType('Played for team', 'forteam', 'TEA',
			function() {
				return { selectedTeam:$scope.teamIdentities[0] };
			},
			function(question) {					
				for (var idx in $scope.athlete.Stats) {
					var stat = $scope.athlete.Stats[idx];
					if (stat.TeamIdentityId == question.selectedTeam.Id) {
						return true;
					}
				}
				return false;
			});	

		$scope.getDivisionForTeamAndSeason = function(teamIdentityId, seasonId) {
			var season = $scope.seasons[seasonId];
			var teamIdentityDivisionList = $scope.teamIdentityDivisionsMap[teamIdentityId];
			for(var idx in teamIdentityDivisionList) {
				var teamIdentityDivision = teamIdentityDivisionList[idx];
				if (season.StartYear >= teamIdentityDivision.StartYear &&
                    (!teamIdentityDivision.EndYear || season.EndYear <= teamIdentityDivision.EndYear)) {
						return $scope.divisionsMap[teamIdentityDivision.DivisionId];
				}
			}	
		};
		
		// played in conference
		$scope.addQuestionType('Played in conference', 'forconference', 'CON',
			function() {
				return { selectedConference:$scope.conferences[0] };
			},
			function(question) {	
				for (var idx in $scope.athlete.Stats) {
					var stat = $scope.athlete.Stats[idx];					
					var division = $scope.getDivisionForTeamAndSeason(stat.TeamIdentityId, stat.SeasonId);
					if (division && division.ConferenceId == question.selectedConference.Id) {
						return true;
					}
				}
				return false;
			});		
			
		// played in division
		$scope.addQuestionType('Played in division', 'fordivision', 'DIV',
			function() {
				return { selectedDivision:$scope.divisions[0] };
			},
			function(question) {	
				for (var idx in $scope.athlete.Stats) {
					var stat = $scope.athlete.Stats[idx];					
					var division = $scope.getDivisionForTeamAndSeason(stat.TeamIdentityId, stat.SeasonId);
					if (division && division.Id == question.selectedDivision.Id) {
						return true;
					}
				}
				return false;
			});					
			
		$scope.sumStat = function(seasonType, statName) {
			var total = 0;				
			for (var idx in $scope.athlete.Stats) {
				var stat = $scope.athlete.Stats[idx];	
				if (stat.IsPlayoffs == 1 && seasonType == 'regular season') {
					continue;
				}
				if (stat.IsPlayoffs == 0 && seasonType == 'playoff') {
					continue;
				}					
				var value = stat[statName];
				if (value) {
					total += value;
				}
			}
			
			return total;
		}
		
		$scope.checkStat = function(seasonType, statName, threshold) {
			var total = $scope.sumStat(seasonType, statName);	
			if (total >= threshold) {
				return true;
			}
			return false;
		}
		
		// jersey number
		$scope.addQuestionType('Jersey number', 'jersey', 'NUM',
			function() {
				return { selectedValue:$scope.jerseys[0] };
			},
			function(question) {
				for (var idx in $scope.athlete.JerseyNumbers) {
					var jerseyNumber = $scope.athlete.JerseyNumbers[idx];	
					if (jerseyNumber.Number == question.selectedValue) { 
						return true; 
					}
				}
				return false;
			});			
		
		// games played
		$scope.addQuestionType('Games Played', 'gamesplayed', 'GP',
			function() {
				return { selectedValue:$scope.hundreds[0], selectedSeasonType:$scope.seasonTypes[0] };
			},
			function(question) {
				return $scope.checkStat(question.selectedSeasonType, 'GamesPlayed', question.selectedValue);
			});		

		// points
		$scope.addQuestionType('Points', 'points', 'P',
			function() {
				return { selectedValue:$scope.hundreds[0], selectedSeasonType:$scope.seasonTypes[0] };
			},
			function(question) {
				return $scope.checkStat(question.selectedSeasonType, 'Points', question.selectedValue);
			});		
			
		// goals
		$scope.addQuestionType('Goals', 'goals', 'G',
			function() {
				return { selectedValue:$scope.hundreds[0], selectedSeasonType:$scope.seasonTypes[0] };
			},
			function(question) {
				return $scope.checkStat(question.selectedSeasonType, 'Goals', question.selectedValue);
			});		

		// assists
		$scope.addQuestionType('Assists', 'assists', 'A',
			function() {
				return { selectedValue:$scope.hundreds[0], selectedSeasonType:$scope.seasonTypes[0] };
			},
			function(question) {
				return $scope.checkStat(question.selectedSeasonType, 'Assists', question.selectedValue);
			});
			
		// wins
		$scope.addQuestionType('Wins', 'wins', 'W',
			function() {
				return { selectedValue:$scope.hundreds[0], selectedSeasonType:$scope.seasonTypes[0] };
			},
			function(question) {
				return $scope.checkStat(question.selectedSeasonType, 'Wins', question.selectedValue);
			});	

		// drafted by
		$scope.addQuestionType('Drafted by', 'draftedby', 'DRB',
			function() {
				return { selectedTeam:$scope.teamIdentities[0] };
			},
			function(question) {
				for (var idx in $scope.athlete.Drafts) {
					var draft = $scope.athlete.Drafts[idx];	
					if (draft.TeamIdentityId == question.selectedTeam.Id) { 
						return true; 
					}
				}
				return false;
			});	
			
		// draft round
		$scope.addQuestionType('Draft round', 'draftround', 'DRR',
			function() {
				return { selectedValue:$scope.numbers[0] };
			},
			function(question) {
				for (var idx in $scope.athlete.Drafts) {
					var draft = $scope.athlete.Drafts[idx];	
					if (draft.Round == question.selectedValue) { 
						return true; 
					}
				}
				return false;
			});			
			
		// draft position
		$scope.addQuestionType('Draft position', 'draftposition', 'DRP',
			function() {
				return { selectedValue:$scope.numbers[0] };
			},
			function(question) {
				for (var idx in $scope.athlete.Drafts) {
					var draft = $scope.athlete.Drafts[idx];	
					if (draft.Position <= question.selectedValue) { 
						return true; 
					}
				}
				return false;
			});	

		// any awards
		$scope.addQuestionType('Any awards', 'anyawards', 'AAW',
			function() {
				return { };
			},
			function(question) {
				for (var idx in $scope.athlete.Awards) {
					var award = $scope.athlete.Awards[idx];	
					if (award.Position == 1) { 						
						return true; 
					}
				}
				return false;
			});		

		// awards
		$scope.addQuestionType('Awards', 'awards', 'AWS',
			function() {
				return { selectedAward:$scope.awards[0], selectedValue:$scope.awardNumbers[0] };
			},
			function(question) {
				var total = 0;
				for (var idx in $scope.athlete.Awards) {
					var award = $scope.athlete.Awards[idx];	
					if (award.Position == 1 && award.AwardId == question.selectedAward.Id ) { 
						total++;
					}
				}				
				if (total >= question.selectedValue) {
					return true;
				}
				return false;
			});		
	
		// stanley cup
		$scope.addQuestionType('Stanley Cup', 'stanleycup', 'CUP',
			function() {
				return { selectedValue:$scope.awardNumbers[0] };
			},
			function(question) {
				var total = 0;
				for (var idx in $scope.athlete.Stats) {
					var stat = $scope.athlete.Stats[idx];	
					if (stat.StanleyCup == 1) {
						total++;
					}
				}				
				if (total >= question.selectedValue) {
					return true;
				}
				return false;
			});		
			

		// birth country
		$scope.addQuestionType('Birth country', 'birthcountry', 'COU',
			function() {
				return { selectedCountry:$scope.countries[0] };
			},
			function(question) {
				if (question.selectedCountry == $scope.athlete.BirthCountry) {
					return true;
				}
				if ($scope[question.selectedCountry]) {
					var country = $scope[question.selectedCountry];
					for(var idx in country) {
						var province = country[idx];
						if (province == $scope.athlete.BirthCountry) {
							return true;
						}
					}
				}
				return false;
			});			
	};
	
	$scope.isEntryCorrect = function(expected, actual) {
		expected = expected.toLowerCase();
		actual = actual.toLowerCase();
		
		var expectedParts = expected.split(' ');
		var actualParts = actual.split(' ');
		
		// check only last name if the first name used is a short form
		if (expectedParts.length == 2 && actualParts.length == 2) {
			if (actualParts[0].indexOf(expectedParts[0]) >= 0 ||
				expectedParts[0].indexOf(actualParts[0]) >= 0) {									
					var distance = getEditDistance(actualParts[1], expectedParts[1]);	
					var ratio = distance / expectedParts[1].length;
					
					if (ratio < 0.25) {		
						return true;
					} else {
						return false;
					}
			}
		}
						
		var distance = getEditDistance(expected, actual);	
		var ratio = distance / expected.length;
		
		if (ratio < 0.25) {		
			return true;
		} else {
			return false;
		}
	}
	
	$scope.makeGuess = function() {
		if ($scope.guessedAthlete) {
			if ($scope.isEntryCorrect($scope.athlete.Name, $scope.guessedAthlete)) {				
				$scope.madeCorrectGuess();
			} else {
				$scope.madeIncorrectGuess();
			}
		}
	}
	
	$scope.madeCorrectGuess = function() {
		$scope.roundStarted = false;
		$scope.roundEndTime = new Date();
		$interval.cancel($scope.timerUpdate);
		$scope.correctGuess = true;
		$scope.gamePoints += $scope.availablePoints;
		$scope.canAddQuestion = false;				
		
		var roundHistory = {};
		roundHistory.athlete = $scope.athlete;
		roundHistory.roundStartTime = $scope.roundStartTime;
		roundHistory.roundEndTime = $scope.roundEndTime;
		
		roundHistory.incorrectGuesses = [];
		for(var idx in $scope.incorrectGuesses) {
			roundHistory.incorrectGuesses.push($scope.incorrectGuesses[idx]);
		}
		
		roundHistory.questionsAsked = [];
		for(var idx in $scope.questionsAsked) {
			roundHistory.questionsAsked.push($scope.questionsAsked[idx]);
		}
		
		roundHistory.availablePoints = $scope.availablePoints;		
		roundHistory.timeLeft = $scope.timeLeft;
		roundHistory.gameRound = $scope.gameRound;
		var elapsed = Math.floor((roundHistory.roundEndTime.getTime() - roundHistory.roundStartTime.getTime()) / 1000);				
		roundHistory.timeElapsed = secondsToMinutesDisplay(elapsed);
		
		$rootScope.$broadcast('addRoundHistory', roundHistory);
	}
	
	$scope.madeIncorrectGuess = function() {
		for(var idx in $scope.incorrectGuesses) {
			var guess = $scope.incorrectGuesses[idx];
			if (guess == $scope.guessedAthlete) {
				return;
			}
		}
		
		$scope.incorrectGuesses.push($scope.guessedAthlete);
		$scope.lives--;
		if ($scope.lives <= 0) {
			$scope.lives = 0;
			$scope.gameOver();
		}
	}
	
	$scope.getSeasons();
}]);

app.controller('roundHistoryController', ['$scope', '$route', 
	function ($scope, $route) {
    $scope.$route = $route;
	
	$scope.rounds = [];
		
	$scope.$on('clearRoundHistory', function (event, data) {
		$scope.rounds = [];
	});
	
	$scope.$on('addRoundHistory', function (event, data) {
		$scope.rounds.push(data);
	});
}]);

