var credentials = require('./credentials.js');

// --- Use Facebook as a authentication service --- //
var FacebookStrategy = require('passport-facebook').Strategy;


module.exports = function(passport) {

passport.serializeUser(function(user, done) {
	done(null, user);
});

passport.deserializeUser(function(user, done) {
	done(null, user);
});

// Important function of this module
passport.use(new FacebookStrategy(credentials.facebook, function(token, tokenSecret, profile, done) {
	console.log("Facebook user with id "+profile.id+" appeared");
	done(null, {message: 'Facebook user signed in!' });
	}
));

};

