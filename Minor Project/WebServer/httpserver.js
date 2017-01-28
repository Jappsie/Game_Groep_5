var passport = require('passport');
var express = require("express");
var url = require("url");
var http = require('http');
var mysql = require('mysql');
var bodyParser = require('body-parser');
var credentials = require('./lib/credentials.js');
var cookies = require('cookie-parser');
var auth = require('./lib/authentication.js')(passport);
var config = require('./lib/config.js');
var query = require('./lib/query.js');
var sorter = require('./lib/sorter.js');
var app;

// Create a server
app = express();
app.set('view engine', 'ejs');
app.use(express.static(config.staticPath));
app.use(bodyParser.json());
app.use(passport.initialize());
app.use(passport.session());
app.use(cookies(credentials.cookie.cookieSecret));

var routes = require('./lib/routes.js')(app, passport);
http.createServer(app).listen(config.port);


// Create a Database
var db = mysql.createConnection( config.database);


// Connect to the Database
db.connect(function(err) {
	if (err) {
  		console.log('<<<Error connecting to Db>>>');
		return;
  	}
  	console.log('>>>>Connection established');
});


// Local Array used for Database independend operations
var taskArray = new Array();


// Send tasks from the Database
app.get("/DBtasks", function (req, res) {
	console.log("Send DB tasks");
	query.sendTask(db, res, function(data){taskArray = data});
});


// Send tasks from local array
app.get("/tasks", function (req, res) {
	console.log("Send tasks");
	res.json(taskArray);
});


// Create a new ToDoItem on the Database
app.post("/newtask", function (req, res) {
	console.log("POST / new Task");
	// Get the data from the client
	var data = req.body;
	// Fix the date object and add a CreationDate
	data.DueDate = new Date(data.DueDate);
	data["CreationDate"] = new Date();
	// Save the tag value for later, and delete it from the object
	var tag = data.Tag;
	delete data["Tag"];
	query.addTag(db, res, data, tag); 	 
});


// Delete a ToDoItem and its Tags on the Database
app.post("/deltask", function (req, res) {
	console.log("POST / del Task");
	// Get data from the client
	var data = req.body;
	console.log("Delete: " + taskArray[data.index-1].Id);
	query.delTask(db, res, taskArray[data.index-1].Id);	
});


// Edit a ToDoItem on the Database
app.post("/edittask", function (req, res) {
	console.log("POST / edit Task");
	// Get the data from the client
	var data = req.body;
	
	// Select the type of edit
	switch (data.field) {
	case "Title":
		query.editTitle(db, res, data.value, taskArray[data.editIndex-1].Id);
		break;
	case "DueDate":
		query.editDate(db, res, new Date(data.value), taskArray[data.editIndex-1].Id);

		break;
	case "Priority":
		query.editPrio(db, res, data.value, taskArray[data.editIndex-1].Id);
		break;
	case "Tag":
		query.editTag(db, res, data.value, taskArray[data.editIndex-1].Id); 
		break;
	};
});


//Mark a ToDoItem as done and add a CompletionDate
app.post("/donetask", function (req, res) {
	console.log("POST / done Task");
	// Get the data from the client
	var data = req.body;
	query.doneTask(db, res, 1 - taskArray[data.index-1].Completed, taskArray[data.index-1].Id);
});


// Sort the local array
app.post("/sorttask", function (req, res) {
	// Get the data from the client
	var data = req.body;
	console.log("POST / sort " + data.type);
	
	// Select the sorting type
	sorter(taskArray, data.type);
	res.json({});
});


// First tutorial page
app.get("/greetme", function (req, res) {
	var query = url.parse(req.url, true).query;
	var name = ( query["name"] != undefined ) ? query["name"] : "Anonymous";

	res.send("Hello " + name);
});


// Second tutorial page
app.get("/goodbye", function (req, res) {
	res.send("Goodbye you!");
});


// --- WIDGETS --- //
// Every query has the same layout: Get the data, do the query, return the values
app.post("/query1", function (req, res) {
	var data = req.body;
	query.widget1(db, res, data.name);
});


app.post("/query2", function (req, res) {
	var data = req.body;
	query.widget2(db, res, data.name);
});


app.post("/query3", function (req, res) {
	var data = req.body;
	query.widget3(db, res, data.name, data.limit);
});


app.post("/query4", function (req, res) {
	var data = req.body;
	query.widget4(db, res, data.name, data.minDate, data.maxDate, data.priority, data.completion, data.limit);
});


app.post("/query5", function (req, res) {
	var data = req.body;
	query.widget5(db, res, data.parent);
});


app.post("/query6", function (req, res) {
	var data = req.body;
	query.widget6(db, res, data.id);
});


app.post("/query7", function (req, res) {
	var data = req.body;
	query.widget7(db, res, data.id);
});

app.post("/query8", function (req, res) {
	var data = req.body;
	query.widget8(db, res, data.tag);
});


app.post("/query9", function (req, res) {
	var data = req.body;
	query.widget9(db, res);
});


app.post("/query10", function (req, res) {
	var data = req.body;
	query.widget10(db, res, data.tag);
});


app.post("/query11", function (req, res) {
	var data = req.body;
	query.widget11(db, res);
});


app.post("/query12", function (req, res) {
	var data = req.body;
	query.widget12(db, res, data.ListId);
});


app.post("/query13", function (req, res) {
	var data = req.body;
	query.widget13(db, res, data.ListId);

});


