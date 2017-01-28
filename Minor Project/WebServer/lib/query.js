// --- Module containing queries --- //

// Main callback function: Parse the Query result to a JSON Array.
var SendQuery = function(res, callback) {
	// Create function which the Database understands
	var ParseQuery = function(err, rows, fields) {
		if(err) throw err;
		// Empty array before population
		taskArray = [];
		// Populate array
		rows.forEach(function(value) {
			var row = {};
			fields.forEach( function(field) {
				row[field.name]= value[field.name];
			})
			taskArray.push(row);	
		})
		// Send the array to the client
		res.json(taskArray);
		if (callback != null) {
			callback(taskArray);
		};
		
		};
	return ParseQuery;
};

// Function to link a Tag with a ToDoItem
var UpdateTag = function( db, tag, ItemId, callback) {
	// Check if the Tag exists, else create it.
	db.query("SELECT * FROM Tag WHERE Text = ?",tag, function( err, rows, fields) {
		if (err) throw err;
		if (rows.length == 0) {
			db.query("INSERT INTO Tag (Text) VALUES (?)", tag, function( err, result ) {
				if (err) throw err; 
				next();
			});
		} else {
		next();
		}
	});

	// Link the Tag to the ToDoItem
	var next = function() {
		db.query("SELECT Id FROM Tag WHERE Text = ?", tag, function (err, rows, fields) { 
			if (err) throw err;
			var Id = rows[0][fields[0].name];
			db.query("INSERT INTO ItemTag (ToDoId, TagId) VALUES (? , ? )", [ItemId, Id], function (err, result) {
				if (err) throw err;
				callback();
			});
		});
	};
};


// Send task to client
var sendTask = function(db, res, callback) {
db.query("SELECT ToDoItem.*, GROUP_CONCAT(DISTINCT Tag.Text ORDER BY Tag.Text SEPARATOR ' ' ) AS Tag FROM ToDoItem LEFT JOIN ItemTag ON 	ToDoItem.Id = ItemTag.ToDoId LEFT JOIN Tag ON ItemTag.TagId = Tag.Id GROUP BY ToDoItem.Id, ToDoItem.Priority", SendQuery(res, callback));
};


// Add tag to the database and link
var addTag = function(db, res, data, tag) {
	var callback = function() {res.json({})};
	// Add the ToDoItem to the Database and link the Tag
	db.query("INSERT INTO ToDoItem SET ?", data, function(err, result) {
		if (err) throw err;
		UpdateTag(db, tag, result.insertId, callback);
	});
};


// Delete task from database
var delTask = function(db, res, id) {
	var callback = function() {res.json({})};
	db.query("DELETE FROM ItemTag WHERE ToDoId = ?", id, function(err,result) {
		if (err) throw err;
		db.query("DELETE FROM ToDoItem WHERE id = ?", id,function(err,result) {
			if (err) throw err;
			callback();
		});
	});
};


// Edit Title on database
var editTitle = function(db, res, title, id) {
	var callback = function() {res.json({})};
	db.query("UPDATE ToDoItem SET Title = ? WHERE id = ?", [title, id], function(err,result) {
		if (err) throw err;
		callback();
	});
};


// Edit Date on database
var editDate = function(db, res, date, id) {
	var callback = function() {res.json({})};
	db.query("UPDATE ToDoItem SET DueDate = ? WHERE id = ?", [date, id], function(err,result) {
		if (err) throw err;
		callback();
	});
};


// Edit priority on database
var editPrio = function(db, res, prio, id) {
	var callback = function() {res.json({})};
	db.query("UPDATE ToDoItem SET Priority = ? WHERE id = ?", [prio, id], function(err,result) {
		if (err) throw err;
		callback();
	});
};


// Edit tag on database
var editTag = function(db, res, tag, id) {
	var callback = function() {res.json({})};
	db.query("DELETE ItemTag FROM ToDoItem JOIN ItemTag ON ToDoItem.Id = ItemTag.ToDoId JOIN Tag ON ItemTag.TagId = Tag.Id WHERE ToDoItem.Id = ?", id, function(err, result) {
			if (err) throw err;
			UpdateTag(db, tag, id, callback);
		}); 
};


// Edit complete on database
var doneTask = function(db, res, complete, id) {
	var callback = function() {res.json({})};
	db.query("UPDATE ToDoItem SET Completed = ?, CompletionDate = ? WHERE id = ?",[complete, new Date(), id], function (err,result) {
		if (err) throw err;
		callback();
	});
};


// --- WIDGETS --- //
// Query 1
var widget1 = function( db, res, name) {
	db.query("SELECT ToDoList.* FROM ToDoList JOIN User ON User.Id = ToDoList.Owner WHERE User.Name LIKE ?", name, SendQuery(res));
};


// Query 2
var widget2 = function (db, res, name) {
	db.query("SELECT ToDoItem.* FROM ToDoList JOIN ToDoItem ON ToDoItem.ToDoListID = ToDoList.Id WHERE ToDoList.Name LIKE ?", name, SendQuery(res));
};


// Query 3
var widget3 = function (db, res, name, limit) {
	db.query("SELECT ToDoItem.* FROM ToDoList JOIN ToDoItem ON ToDoItem.ToDoListID = ToDoList.Id WHERE ToDoList.Name LIKE ? LIMIT 0,?", [name, limit], SendQuery(res));
};


// Query 4
var widget4 = function (db, res, name, minDate, maxDate, priority, completion, limit) {
	db.query("SELECT ToDoItem.* FROM ToDoList join ToDoItem on ToDoItem.ToDoListID = ToDoList.ID WHERE ToDoList.Name LIKE ? AND ToDoItem.CreationDate > ? AND ToDoItem.CreationDate < ? AND ToDoItem.Priority > 0 AND ToDoItem.Priority < ? AND ToDoItem.Completed = ? LIMIT 0,?", [name, minDate, maxDate, priority, completion, limit], SendQuery(res));
};


// Query 5
var widget5 = function (db, res, parent) {
	db.query("SELECT td2.* FROM ToDoItem AS td1 JOIN ToDoItem AS td2 ON td1.ID = td2.ParentToDo WHERE td2.ParentToDo = ?", parent, SendQuery(res));
};


// Query 6
var widget6 = function (db, res, id) {
	db.query("SELECT t.text FROM Tag AS t JOIN ItemTag AS it ON t.id = it.TagId JOIN ToDoItem AS td ON td.id = it.ToDoId WHERE td.Id = ?", id, SendQuery(res));
};


// Query 7
var widget7 = function(db, res, id) {
	db.query("SELECT DISTINCT tdl.* FROM Tag AS t JOIN ItemTag AS it ON t.id = it.TagId JOIN ToDoItem AS td ON td.id = it.ToDoId JOIN ToDoList AS tdl ON tdl.Id = td.ToDoListID WHERE t.Id = ?", id, SendQuery(res));
};


// Query 8
var widget8 = function(db, res, tag) {
	db.query("SELECT COUNT(t.Text) , t.Text , td.Completed FROM Tag AS t JOIN ItemTag AS it ON t.Id = it.TagId JOIN ToDoItem AS td ON td.id = it.TodoId WHERE t.Text LIKE ? GROUP BY t.Text , td.Completed", tag, SendQuery(res));
};


// Query 9
var widget9 = function(db, res) {
	db.query("SELECT WEEK(CompletionDate) , COUNT(*) FROM ToDoItem WHERE CompletionDate IS NOT NULL GROUP BY WEEK(CompletionDate)", SendQuery(res));
};


// Query 10
var widget10 = function(db, res, tag) {
	db.query("SELECT t.Text , td.* , DATEDIFF(td.CompletionDate , td.CreationDate) FROM Tag AS t JOIN ItemTag AS it ON t.Id = it.TagId JOIN ToDoItem AS td ON td.id = it.TodoId WHERE td.CompletionDate IS NOT NULL AND t.Text LIKE ? ORDER BY DATEDIFF(td.CompletionDate, td.CreationDate) LIMIT 0,10",tag, SendQuery(res));
};


// Query 11
var widget11 = function(db, res) {
	db.query("SELECT it.TagId , it2.TagId , COUNT(*) FROM ItemTag AS it , ItemTag AS it2 WHERE it.ToDoId = it2.ToDoId AND it.TagId < it2.TagId GROUP BY it.TagId , it2.TagId", SendQuery(res));
};


// Query 12
var widget12 = function(db, res, ListId) {
	db.query("SELECT AVG(DATEDIFF(td.CompletionDate , td.CreationDate)) FROM ToDoItem AS td JOIN ToDoList AS tdl ON tdl.id = ToDoListID WHERE tdl.Id = ? AND td.CompletionDate IS NOT NULL", ListId, SendQuery(res));
};


// Query 13
var widget13 = function(db, res, ListId) {
	db.query("SELECT td.* FROM ToDoItem AS td JOIN ToDoList AS tdl ON tdl.id = td.ToDoListID WHERE DATEDIFF(td.CompletionDate , td.CreationDate) > ( SELECT AVG(DATEDIFF(td.CompletionDate , td.CreationDate)) FROM ToDoItem AS td JOIN ToDoList AS tdl ON tdl.id = ToDoListID WHERE tdl.Id LIKE ? AND td.CompletionDate IS NOT NULL ) AND td.CompletionDate IS NOT NULL AND tdl.Id LIKE ?", [ListId, ListId], SendQuery(res));
};

// List of functions to export
module.exports = {
	sendTask: sendTask,
	addTag: addTag,
	delTask: delTask,
	editTitle: editTitle,
	editDate: editDate,
	editPrio: editPrio,
	editTag: editTag,
	doneTask: doneTask,
	widget1: widget1,
	widget2: widget2,
	widget3: widget3,
	widget4: widget4,
	widget5: widget5,
	widget6: widget6,
	widget7: widget7,
	widget8: widget8,
	widget9: widget9,
	widget10: widget10,
	widget11: widget11,
	widget12: widget12,
	widget13: widget13
};
