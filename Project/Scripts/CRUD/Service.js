//Service to get data from service..
myapp.service('crudservice', function ($http) {
    
    this.getAllCatogories = function () {
        return $http.get("/api/HomeAPI");
    }

    this.getAllPosition = function () {
        return $http.get("api/Position")
    }

    //save
    this.savePosition = function (Position) {
        var request = $http({
            method: 'post',
            url: 'api/Position/',
            data: Position
        });
        return request;
    }

    //get single record by Id
    this.getPositionId = function (PositionId) {
        //debugger;
        return $http.get("api/Position/" + PositionId);
    }



    //update records
    this.updatePosition = function (PositionId, Position) {
        //debugger;
        var updaterequest = $http({
            method: 'put',
            url: "api/Position/" + PositionId,
            data: Position
        });
        return updaterequest;
    }


    //delete record
    this.deletePosition = function (PositionId) {
        debugger;
        var deleterecord = $http({
            method: 'delete',
            url: "api/Position/" + PositionId
        });
        return deleterecord;
    }

});