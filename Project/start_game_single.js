//Load player and goal images.
var playerImage = new Image();
playerImage.src = "/views/Images/mario.png";
var goalImage = new Image();
goalImage.src = "/views/Images/goal.png";

/*************************************************************
* The strToArr function receives a string and a number for rows and
* cols, this will turn the string format of the array that we receive
* from the server to a multidimensional array that we will use later.
***************************************************************/
function strToArr(myString, rows, cols) {
    var maze = new Array(rows);
    for (var i = 0; i < rows; i++) {
        var colsArr = new Array(cols);
        for (var j = 0; j < cols; j++) {
            colsArr[j] = parseInt(myString[i * rows + j]);
        }
        maze[i] = colsArr;
    }
    return maze;
}   

//Save current position,cellWidth,cellHeight and the context.
var current;
var cellWidth;
var cellHeight;
var context;

//Run when the document is ready.
$(document).ready(function () {
    //Set all the stuff needed.
    var goal;
    var myCanvas = document.getElementById("mazeCanvas");
    context = mazeCanvas.getContext("2d");
    var isValid;
    var mazeName;

    //Set click event for dropdown list to update the text.
    $("#mylist li").click(function () {
        $('#datebox').val($(this).text());
    });

    //Set click event for start button to send GET request to server.
    $("#StartButton").click(function () {
        //Draw on the canvas that the game is loading.
        context.clearRect(0, 0, 500, 500);
        context.font = "40px Georgia";
        context.fillText("Loading...", 150, 250);

        //Extract the values that the user entered.
        mazeName = $("#mazeName").val();
        var mazeRows = $("#rows").val();
        var mazeCols = $("#cols").val();

        //Check validity of all values, take from local storage if needed.
        if (mazeName === "") {
            document.getElementById("errMsg").innerHTML = "Name wasn't entered!";
            return;
        }
        if (mazeRows === "") {
            mazeRows = localStorage.getItem("defaultRows");
            if (mazeRows === null) {
                document.getElementById("errMsg").innerHTML = "Rows weren't entered!";
                return;
            }
        }
        if (mazeCols === "") {
            mazeCols = localStorage.getItem("defaultCols");
            if (mazeCols === null) {
                document.getElementById("errMsg").innerHTML = "Cols weren't entered!";
                return;
            }
        }

        document.getElementById("errMsg").innerHTML = "";
        document.title = mazeName;

        //Build url that we will send the GET request to.
        var apiUrl = "../api/Maze" + "/" + mazeName + "/" + mazeRows + "/" + mazeCols;
        //Send request.
        var maze_board;
        //Set GET request and expect JSON object in response.
        $.getJSON(apiUrl).done(function (maze) {
            //initialize vars for 
            cellWidth = mazeCanvas.width / maze.Cols;
            cellHeight = mazeCanvas.height / maze.Rows;
            //After we get the maze draw it on canvas.
            current = maze.Start;
            goal = maze.End;
            arr = strToArr(maze.Maze, maze.Rows, maze.Cols);
            //Use the plugin for the maze board.
            maze_board = $("#mazeCanvas").mazeBoard(maze);
            //Draw the maze by using a function from the plugin.
            maze_board.drawMaze(arr);
            var cols = maze.Cols;
            var rows = maze.Rows;
            isValid = function (i, j) {
                return 0 <= j && j < cols && 0 <= i && i < rows && arr[i][j] != 1;
            };
        }
        );
        
        //Set function for click of solve game.
        $("#solveButton").click(function (event) {
            //Check value of algorithm.
            var algorithm = $('#datebox').val();
            if (algorithm === "") {
                algorithm = localStorage.getItem("defaultAlgo");
                if (algorithm === null) {
                    document.getElementById("errMsg").innerHTML = "Algorithm wasn't selected!";
                    return;
                }
            }
            
            var url = "../api/Maze/" + mazeName + "/" + algorithm;
            //Send ajax get request to the server in order to get solution.
            $.getJSON(url).done(function (solution,event) {
                //We get an object that was already parsed from json.
                var solutionStr = solution.Solution;
                maze_board.solve(solutionStr, 0);
                
            });
            event.stopImmediatePropagation();
        });
    });

});

