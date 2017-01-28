var main = function() {
	console.log("Document ready");

	// New task
	$("#newTask").click(function(){
		var taskName = prompt("What is the name of the task?");
		var dueDate = prompt("What is the due date? YYYY-MM-DD");
		dueDate = new Date(dueDate);
		var importance = prompt("What is the importance? [0-5]");
		var tag = prompt("What is the tag?");
		var Task = {Title: taskName, DueDate: dueDate, Priority: importance, Tag: tag, Completed: 0};
		$.ajax({
			type: "POST",
			url: "/newtask",
			data: JSON.stringify(Task),
			contentType: "application/json",
			dataType: "json",
			success: function() {
				refreshList();
			}
		});
	});

	// Delete task
	$("#deleteTask").click(function(){
		var delIndex = prompt("Which item do you want to delete? Please enter the number of the task.");
		$.ajax({
			type: "POST",
			url: "/deltask",
			data: JSON.stringify({index: delIndex}),
			contentType: "application/json",
			dataType: "json",
			success: function() {
				refreshList();
			}
		});
	});

	// Edit task
	$("#editTask").click(function(){
		var editIndex = prompt("Which item do you want to edit? Please enter the number of the task.");
		var field = prompt("Do you want to change: Title , DueDate, Priority, Tag ?");
		if (field === "DueDate") {
			var data = prompt("What do you want to change it to? [YYYY-MM-DD]");
			var value = new Date(data);
		} else {
		var value = prompt("What do you want to change it to?");
		}
		$.ajax({
			type: "POST",
			url: "/edittask",
			data: JSON.stringify({"editIndex":editIndex,"field":field,"value":value}),
			contentType: "application/json",
			dataType: "json",
			success: function() {
				refreshList();
			}
		})
		
	});

	// Change done
	$("#doneTask").click(function(){
		var index = prompt("Which item do you want to finish? Please enter the number of the task.");
		$.ajax({
			type: "POST",
			url: "/donetask",
			data: JSON.stringify({"index":index}),
			contentType: "application/json",
			dataType: "json",
			success: function() {
				refreshList();
			}
		});
	});

	// Sort Due date
	$("#SortDueDate").click(function(){
		$.ajax({
			type: "POST",
			url: "/sorttask",
			data: JSON.stringify({"type":"date"}),
			contentType: "application/json",
			dataType: "json",
			success: function() {
				refreshList(true);
			}
		});
	});

	// Sort importance
	$("#SortImportance").click(function(){
		$.ajax({
			type: "POST",
			url: "/sorttask",
			data: JSON.stringify({"type":"importance"}),
			contentType: "application/json",
			dataType: "json",
			success: function() {
				refreshList(true);
			}
		});
	});

	// Sort tag
	$("#SortTag").click(function(){
		$.ajax({
			type: "POST",
			url: "/sorttask",
			data: JSON.stringify({"type":"tag"}),
			contentType: "application/json",
			dataType: "json",
			success: function() {
				refreshList(true);
			}
		});
	});

	// Sort done
	$("#SortDone").click(function() {
		$.ajax({
			type: "POST",
			url: "/sorttask",
			data: JSON.stringify({"type":"done"}),
			contentType: "application/json",
			dataType: "json",
			success: function() {
				refreshList(true);
			}
		});
	});
};

// Function to populate the list from the server data
var refreshList = function( update ) {
	var addTaskToList = function(tasks) {
		var taskList = document.getElementById("TaskList");
		$(taskList).empty();
		for (var key in tasks) {
			var li = document.createElement("li");
			tasks[key].DueDate = new Date(tasks[key].DueDate);
			if (tasks[key].Completed == 1) {
				li.className = "Done";
				
			} else {
				li.className = "Busy";
				if (tasks[key].DueDate < new Date()) {
					$(li).addClass("OverDue");
				}
			}

			$(li).addClass("imp" + tasks[key].Priority);							

			li.innerHTML = tasks[key].Title + ": " + tasks[key].DueDate.toDateString();
			taskList.appendChild(li);
		}
	};
	// update is by default false
	if (update) {
		console.log("Loading tasks from server");
		$.getJSON("../tasks", addTaskToList);
	} else {
		console.log("Loading tasks from database");
		$.getJSON("../DBtasks", addTaskToList);	
	}
};

$(document).ready(main);
