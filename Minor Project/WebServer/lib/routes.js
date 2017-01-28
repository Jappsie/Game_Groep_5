var config = require('../lib/config.js');
var path = require("path");
var request = require("request");

module.exports = function(app, passport) {

//Get route to main page
app.get(/main/, function (req, res) {
	console.log("Send mainpage");
	var data = {tasks: ["test"], css: ""};
	if (typeof req.cookies.color !== 'undefined') {
		var data = {css: 'body {background-color: ' + req.cookies.color + ';}'};
	}
	request.get("http://localhost:" + config.port + "/DBTasks",{
		json: true		
	}, function(error, response, body) {
		if (!error && response.statusCode === 200) {
			data.tasks = body;
			res.render('main', data);		
		}	
	});

});

//Get route to analytics page
app.get("/analy?t?i?c?s?",function(req, res) {
	console.log("Send Analyticspage");
	if (typeof req.cookies.color !== 'undefined') {
		var data = {css: 'body {background-color: ' + req.cookies.color + ';}'};
	}
	res.render('Analytics', data);
});

//Get route to splash page
app.get("/(:newTitle)?",function(req, res) {
	var data = req.query;

	var setCookie = function(color) {
		var date = new Date()
		date.setDate(date.getDate() + config.cookieExpireDate);
		res.cookie("color", color, {expires: date, httpOnly: true});
	};

	if (typeof data.color === 'undefined') {
		if (typeof req.cookies.color === 'undefined') {
			req.cookies.color = "lightblue";
			setCookie("lightblue");
		}
		var title = config.defaultTitle;
		if (req.params.newTitle) {
			title = req.params.newTitle;
		}
		var data = {title: title, css: 'body {background-color: ' + req.cookies.color + ';}'};
		res.render('index',data);
		
	}
	else 
	{
		setCookie(data.color);
		res.redirect("/");
	}
});

// Get route to facebook authentication page
app.get("/auth/facebook", passport.authenticate('facebook'));

// Callback location of facebook authentication
app.get("/auth/facebook/callback", passport.authenticate('facebook', {failureRedirect: 'failure'}), function( req, res) {
	res.redirect('success');
	}
);

// Facebook authentication success page
app.get("/auth/facebook/success", function (req, res) {
	console.log("Login successful");
	res.send("<a href='/main'>User login via Facebook successful</a>");
});


// Facebook authentication failure page
app.get("/auth/facebook/failure", function (req, res) {
	console.log("Login failed");
	res.send("<a href='/'>User login via Facebook failed</a>");
});

};
