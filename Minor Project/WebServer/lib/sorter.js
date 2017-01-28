// --- Module which can sort taskArray on type --- //
module.exports = function(taskArray, type) {
switch (type) {
	case "date" :
		taskArray.sort(function(a,b) {
			var aName = a.DueDate;
			var bName = b.DueDate;
			return ((aName < bName) ? 1 : ((aName > bName) ? -1: 0));
		});
		break;
	case "importance" :
		taskArray.sort(function(a,b) {
			var aName = a.Priority;
			var bName = b.Priority;
			return ((aName < bName) ? -1 : ((aName > bName) ? 1: 0));
		});
		break;
	case "tag" :
		taskArray.sort(function(a,b) {
			// Make sure to check for null objects
			if ( typeof a.Tag != "string" || typeof b.Tag != "string") {
				return typeof a.Tag != "string" ? 1 : -1;
			} else {
				var aName = a.Tag.toLowerCase();
				var bName = b.Tag.toLowerCase();
				return ((aName < bName) ? -1 : ((aName > bName) ? 1: 0));
			}
		});
		break;
	case "done" :
		taskArray.sort(function(a,b) {
			var aDone = a.Completed;
			var bDone = b.Completed;
			return ((aDone == bDone) ? 0 : (aDone == 1) ? 1 : -1);
		});
		break;
	};
};	
