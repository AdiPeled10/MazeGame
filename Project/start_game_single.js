var playerImage = new Image();
playerImage.src = "/views/Images/mario.png";
var goalImage = new Image();
goalImage.src = "/views/Images/goal.png";

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

var current;
var cellWidth;
var cellHeight;
var context;
$(document).ready(function () {
    //Set all the stuff needed.
    var goal;
    var myCanvas = document.getElementById("mazeCanvas");
    context = mazeCanvas.getContext("2d");
    var isValid;
    var mazeName;
    $("#mylist li").click(function () {
        $('#datebox').val($(this).text());
    });
    $("#StartButton").click(function () {
        context.clearRect(0, 0, 500, 500);
        context.font = "40px Georgia";
        context.fillText("Loading...", 150, 250);
        mazeName = $("#mazeName").val();
        var mazeRows = $("#rows").val();
        var mazeCols = $("#cols").val();
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

        var apiUrl = "../api/Maze" + "/" + mazeName + "/" + mazeRows + "/" + mazeCols;
        //Send request.
        var maze_board;
        $.getJSON(apiUrl).done(function (maze) {
            //initialize vars for 
            cellWidth = mazeCanvas.width / maze.Cols;
            cellHeight = mazeCanvas.height / maze.Rows;
            //After we get the maze draw it on canvas.
            current = maze.Start;
            goal = maze.End;
            arr = strToArr(maze.Maze, maze.Rows, maze.Cols);
            maze_board = $("#mazeCanvas").mazeBoard(maze);
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

/*
function animateMazeSolution(solution) {
    /*Animate the solution by using SetTimeout function
    * we will set a timer that everytime it's done the player will
    * move one spot according to the solution./
    moveAccordingToSolution(solution, 0);
    setTimeout(function (index) {
        moveAccordingToSolution()
    })

}*/
/*
function move(solution, index) {
    moveAccordingToSolution(solution, index);
    setTimeout(function () {
        if (index < solution.length) {
            move(solution, index + 1);
        } else {
            alert("Solution Animation Done.");
            $("body").off();
            return;
        }
    }, 200);
}

function moveAccordingToSolution(solution, currIndex) {
    context.clearRect(current.Col * cellWidth, current.Row * cellHeight, cellWidth, cellHeight);
    switch (solution[currIndex]) {
        case '0':
            //Move right.
            current.Col--;
            break;
        case '1':
            //Go right.
            current.Col++;
            break;

        case '2':
            //Go down.
            current.Row--;
            break;

        case '3':
            //Go up.
            current.Row++;
            break;
    }
    //Now draw the player.
    context.drawImage(playerImage, current.Col * cellWidth,
                            current.Row * cellHeight, cellWidth, cellHeight);


}*/

/*
function drawMaze(maze,start,end) {
    var myCanvas = document.getElementById("mazeCanvas");
    var context = mazeCanvas.getContext("2d");
    context.clearRect(0, 0, myCanvas.width, myCanvas.height);
    var rows = maze.length;
    var cols = maze[0].length;
    var cellWidth = mazeCanvas.width / cols;
    var cellHeight = mazeCanvas.height / rows;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < cols; j++) {
            if (maze[i][j] == 1) {
                context.fillRect(cellWidth * j, cellHeight * i,
               cellWidth, cellHeight);
            }
        }
    }

    //Draw player.
    context.drawImage(playerImage, start.Col * cellWidth, start.Row * cellHeight,cellWidth, cellHeight);

    context.drawImage(goalImage, end.Col * cellWidth, end.Row * cellHeight, cellWidth, cellHeight);
    
}
*/

