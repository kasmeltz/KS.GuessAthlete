var app = angular.module('app');

app.directive('positionQuestion', function() {
	return {
		restrict: 'E',
		template: 'Were they primarily considered to be a <select ng-if="question.answered == false" ng-model="$parent.question.selectedPosition" ng-options="position for position in positions" ></select><span ng-if="question.answered == true">{{$parent.question.selectedPosition}}</span>?'
	}	
});

app.directive('letterQuestion', function() {
	return {
		restrict: 'E',
		template: 'Does their <select ng-if="question.answered == false" ng-model="$parent.question.selectedName" ng-options="name for name in names" ></select><span ng-if="question.answered == true">{{$parent.question.selectedName}}</span> name start with <select ng-if="question.answered == false" ng-model="$parent.question.selectedLetter" ng-options="letter for letter in letters" ></select><span ng-if="question.answered == true">{{$parent.question.selectedLetter}}</span>?'
	}	
});

app.directive('oneYearQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they play at least one year between <select ng-if="question.answered == false" ng-model="$parent.question.selectedStart" ng-options="year for year in years" ></select><span ng-if="question.answered == true">{{$parent.question.selectedStart}}</span> and <select ng-if="question.answered == false" ng-model="$parent.question.selectedEnd" ng-options="year for year in years" ></select><span ng-if="question.answered == true">{{$parent.question.selectedEnd}}</span>?'
	}	
});

app.directive('allYearsQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they play every year between <select ng-if="question.answered == false" ng-model="$parent.question.selectedStart" ng-options="year for year in years" ></select><span ng-if="question.answered == true">{{$parent.question.selectedStart}}</span> and <select ng-if="question.answered == false" ng-model="$parent.question.selectedEnd" ng-options="year for year in years" ></select><span ng-if="question.answered == true">{{$parent.question.selectedEnd}}</span>?'
	}	
});

app.directive('forTeamQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they play for <select ng-if="question.answered == false" ng-model="$parent.question.selectedTeam" ng-options="teamIdentity.FullName for teamIdentity in teamIdentities" ></select><span ng-if="question.answered == true">{{$parent.question.selectedTeam.FullName}}</span>?'		
	}	
});

app.directive('forConferenceQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they play in the <select ng-if="question.answered == false" ng-model="$parent.question.selectedConference" ng-options="conference.FullName for conference in conferences" ></select><span ng-if="question.answered == true">{{$parent.question.selectedConference.FullName}}</span> conference?'
	}	
});

app.directive('forDivisionQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they play in the <select ng-if="question.answered == false" ng-model="$parent.question.selectedDivision" ng-options="division.FullName for division in divisions" ></select><span ng-if="question.answered == true">{{$parent.question.selectedDivision.FullName}}</span> division?'
	}	
});

app.directive('jerseyQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they wear jersey number <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="jersey for jersey in jerseys" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span>?'
	}	
});

app.directive('gamesPlayedQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they play at least <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="hundred for hundred in hundreds" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span> <select ng-if="question.answered == false" ng-model="$parent.question.selectedSeasonType" ng-options="seasonType for seasonType in seasonTypes" ></select><span ng-if="question.answered == true">{{$parent.question.selectedSeasonType}}</span> games?'
	}	
});

app.directive('pointsQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they score at least <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="hundred for hundred in hundreds" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span> <select ng-if="question.answered == false" ng-model="$parent.question.selectedSeasonType" ng-options="seasonType for seasonType in seasonTypes" ></select><span ng-if="question.answered == true">{{$parent.question.selectedSeasonType}}</span> points?'			
	}	
});

app.directive('goalsQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they score at least <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="hundred for hundred in hundreds" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span> <select ng-if="question.answered == false" ng-model="$parent.question.selectedSeasonType" ng-options="seasonType for seasonType in seasonTypes" ></select><span ng-if="question.answered == true">{{$parent.question.selectedSeasonType}}</span> goals?'
	}	
});

app.directive('assistsQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they score at least <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="hundred for hundred in hundreds" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span> <select ng-if="question.answered == false" ng-model="$parent.question.selectedSeasonType" ng-options="seasonType for seasonType in seasonTypes" ></select><span ng-if="question.answered == true">{{$parent.question.selectedSeasonType}}</span> assists?'
	}	
});

app.directive('winsQuestion', function() {
	return {
		restrict: 'E',
		template: 'Do they have at least <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="hundred for hundred in hundreds" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span> <select ng-if="question.answered == false" ng-model="$parent.question.selectedSeasonType" ng-options="seasonType for seasonType in seasonTypes" ></select><span ng-if="question.answered == true">{{$parent.question.selectedSeasonType}}</span> wins?'
	}	
});

app.directive('draftedByQuestion', function() {
	return {
		restrict: 'E',
		template: 'Were they drafted by the <select ng-if="question.answered == false" ng-model="$parent.question.selectedTeam" ng-options="teamIdentity.FullName for teamIdentity in teamIdentities" ></select><span ng-if="question.answered == true">{{$parent.question.selectedTeam.FullName}}</span>?'		
	}	
});

app.directive('draftRoundQuestion', function() {
	return {
		restrict: 'E',
		template: 'Were they drafted in the <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="round for round in rounds" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span>	round?'		
	}	
});

app.directive('draftPositionQuestion', function() {
	return {
		restrict: 'E',
		template: 'Were they drafted at or higher than <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="number for number in numbers" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span> overall?'
	}	
});

app.directive('anyAwardsQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they win any major awards?'
	}	
});

app.directive('awardsQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they win the <select ng-if="question.answered == false" ng-model="$parent.question.selectedAward" ng-options="award.Name for award in awards" ></select><span ng-if="question.answered == true">{{$parent.question.selectedAward.Name}}</span> at least <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="awardNumber for awardNumber in awardNumbers" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span> times?'
	}	
});

app.directive('stanleyCupQuestion', function() {
	return {
		restrict: 'E',
		template: 'Did they win the Stanley Cup at least <select ng-if="question.answered == false" ng-model="$parent.question.selectedValue" ng-options="awardNumber for awardNumber in awardNumbers" ></select><span ng-if="question.answered == true">{{$parent.question.selectedValue}}</span> times?'
	}	
});

app.directive('birthCountryQuestion', function() {
	return {
		restrict: 'E',
		template: 'Were they born in <select ng-if="question.answered == false" ng-model="$parent.question.selectedCountry" ng-options="country for country in countries" ></select><span ng-if="question.answered == true">{{$parent.question.selectedCountry}}</span>?'
	}	
});	





				

				


				


				

				

				







				

