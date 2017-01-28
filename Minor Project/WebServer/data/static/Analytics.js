var main = function() {
	console.log("Document ready");
	//getQuery();
	$("#Refresh").click(function() {
		
		getQuery();
	});
};

var getQuery = function () {
	// Get Query1
	var query1 = function(){
		$.ajax({
			type: "POST",
			url: "/query1",
			data: JSON.stringify({name: "user1"}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List1");
			}
		});
	}();

	// Get Query2
	var query2 = function(){
		$.ajax({
			type: "POST",
			url: "/query2",
			data: JSON.stringify({name: "School"}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List2");
			}
		});
	}();

	// Get Query3
	var query3 = function(){
		$.ajax({
			type: "POST",
			url: "/query3",
			data: JSON.stringify({name: "School", limit: 3}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List3");
			}
		});
	}();

	// Get Query4
	var query4 = function(){
		$.ajax({
			type: "POST",
			url: "/query4",
			data: JSON.stringify({name: "School", minDate: new Date("2014-01-01"), maxDate: new Date("2016-01-01"), priority: 4, completion: 0, limit: 7}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List4");
			}
		});
	}();

	// Get Query5
	var query5 = function(){
		$.ajax({
			type: "POST",
			url: "/query5",
			data: JSON.stringify({parent: "6"}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List5");
			}
		});
	}();

	// Get Query6
	var query6 = function(){
		$.ajax({
			type: "POST",
			url: "/query6",
			data: JSON.stringify({id: 1}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List6");
			}
		});
	}();

	// Get Query7
	var query7 = function(){
		$.ajax({
			type: "POST",
			url: "/query7",
			data: JSON.stringify({id: 1}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List7");
			}
		});
	}();

	// Get Query8
	var query8 = function(){
		$.ajax({
			type: "POST",
			url: "/query8",
			data: JSON.stringify({tag: "School"}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List8");
			}
		});
	}();

	// Get Query9
	var query9 = function(){
		$.ajax({
			type: "POST",
			url: "/query9",
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List9");
			}
		});
	}();

	// Get Query10
	var query10 = function(){
		$.ajax({
			type: "POST",
			url: "/query10",
			data: JSON.stringify({tag: "School"}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List10");
			}
		});
	}();
	// Get Query11
	var query11 = function(){
		$.ajax({
			type: "POST",
			url: "/query11",
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List11");
			}
		});
	}();

	// Get Query12
	var query12 = function(){
		$.ajax({
			type: "POST",
			url: "/query12",
			data: JSON.stringify({ListId: 1}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List12");
			}
		});
	}();

	// Get Query13
	var query13 = function(){
		$.ajax({
			type: "POST",
			url: "/query13",
			data: JSON.stringify({ListId: 1}),
			contentType: "application/json",
			dataType: "json",
			success: function(data) {
				fillList(data, "List13");
			}
		});
	}();
};

var fillList = function(tasks, id) {
	var taskList = document.getElementById(id);
	$(taskList).empty();
	for (var key in tasks) {
		var li = document.createElement("li");
		li.innerHTML = JSON.stringify(tasks[key]);
		taskList.appendChild(li);
	}
};



$(document).ready(main);

