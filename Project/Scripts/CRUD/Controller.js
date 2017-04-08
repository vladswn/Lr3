//Angular controller 
myapp.controller('crudcontroller', function ($scope, crudservice) {
    
    $scope.clear = function () {
        $scope.Title = " ";
    }

    //Loads all Employee records when page loads
    loadCatogories();
    function loadCatogories() {
        var CategoriesRecords = crudservice.getAllCatogories();
        CategoriesRecords.then(function (d) {     //success
            $scope.Categories = d.data;
        },
        function(){
            console("Oops..","Error occured while loading","error"); //fail
        });
    }


    loadPositions();
    function loadPositions() {
        var PositionsRecords = crudservice.getAllPosition();
        PositionsRecords.then(function (d) {     //success
            $scope.Positions = d.data;
        },
        function () {
            console("Oops..", "Error occured while loading", "error"); //fail
        });
    }

    $scope.savePosition = function () {
        var Position = {
            PositionId: $scope.PositionId,
            Title: $scope.Title
        };
        var saverecords = crudservice.savePosition(Position);
        saverecords.then(function (d) {
            $scope.PositionId = d.data.PositionId;
            loadPositions();
            $scope.Title = '';
            console("Reord inserted successfully");
        },
        function () {
            console("Oops..", "Error occured while saving", 'error');
        });
    }


    $scope.getPositionId = function (Position) {
        var singlerecord = crudservice.getPositionId(Position.PositionId);
        singlerecord.then(function (d) {
            // debugger;
            var record = d.data;
            $scope.PositionId = record.PositionId;
            $scope.Title = record.Title;
        },
       function () {
           console("Oops...", "Error occured while getting record", "error");
       });
    }

    //update data
    $scope.updatePosition = function () {
        //debugger;
        var Position = {
            PositionId: $scope.PositionId,
            Title: $scope.Title
        };
        var updaterecords = crudservice.updatePosition($scope.PositionId, Position);
        updaterecords.then(function (d) {
            loadPositions();
            cler();
            alert('Запись успешно отредактирована!');
        },
        function () {
            console("Opps...", "Error occured while updating", "error");
        });
    }

    //delete record
    $scope.deletePosition = function (PositionId) {
        var deleterecord = crudservice.deletePosition($scope.PositionId);
        deleterecord.then(function (d) {
            var Position = {
                PositionId: '',
                Title: '',
            };
            loadPositions();
        });
    }



});