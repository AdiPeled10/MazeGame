/****************************************************
This part is code taken from the single player for some reason
taking it from another js file doesn't work so it's here for now
TODO- move it to a file that the single player and multi player will use.
******************************************************/
var playerImage = new Image();
playerImage.src = "/views/Images/mario.png";
var opponentImage = new Image();
opponentImage.src = "/views/Images/luigi.png";
var goalImage = new Image();
goalImage.src = "/views/Images/goal.png";
var playerCurrent, opponentCurrent, isValid;
var cellWidth, cellHeight, goal, context;


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

function drawMaze(maze, start, end, xInd, yInd, mazeWidth, mazeHeight,isMine) {
    var myCanvas = document.getElementById("mazeCanvas");
    var context = mazeCanvas.getContext("2d");
    context.clearRect(xInd, yInd, mazeWidth, mazeHeight);
    var rows = maze.length;
    var cols = maze[0].length;
    var cellWidth = mazeWidth / cols;
    var cellHeight = mazeHeight / rows;
    for (var i = 0; i < rows; i++) {
        for (var j = 0; j < cols; j++) {
            if (maze[i][j] == 1) {
                context.fillRect(xInd + cellWidth * j, yInd + cellHeight * i,
               cellWidth, cellHeight);
            }
        }
    }

    //Draw player.
    if (isMine == 1) {
        context.drawImage(playerImage, xInd + start.Col * cellWidth, yInd + start.Row * cellHeight, cellWidth, cellHeight);
    } else {
        context.drawImage(opponentImage, xInd + start.Col * cellWidth, yInd + start.Row * cellHeight, cellWidth, cellHeight);
    }

    context.drawImage(goalImage, xInd + end.Col * cellWidth, yInd + end.Row * cellHeight, cellWidth, cellHeight);
}


/****************
End of part of identical code.
********************/
function checkVal(myVal, name) {
    if (myVal.length == 0) {
        alert("Please enter " + name + " in order to continue");
        return 0;
    }
    return 1;
}

function checkNumber(myVal, fieldName) {
    try {
        var num = parseInt(myVal)
        return num;
    } catch (err) {
        alert("The " + fieldName + " field needs to be a number.");
        return 0;
    }
}

var gameEnded;

//Runs when document is ready.
$(document).ready(function () {
    //// for dynamic canvas size
    //var tmp = document.getElementById("mazeCanvas");
    //tmp.width = window.innerWidth - tmp.getBoundingClientRect().left - 35;
    //tmp.height = window.innerHeight - tmp.getBoundingClientRect().top;

    //Activates when start button is clicked.
    //Declare reference to hub.
    //Set list of games text.
    $("#mylist li").click(function () {
        $('#games').val($(this).text());
    });
    var hub = $.connection.gameHub;

    //Create a function that the player will operate when he want to play
    hub.client.play = function (move) {
        var key;
        switch (move) {
            case 1:
                key = 37;
                break;
            case 2:
                key = 38;
                break;
            case 3:
                key = 39;
                break;
            case 4:
                key = 40;
                break;
        }
        //Move opponents player.
        movePlayer(key,475,0,opponentImage,opponentCurrent,0);
    };

    //Create function that accepts the maze and draws it.
    hub.client.getmaze = function (maze) {
        //Draw the maze.
        arr = strToArr(maze.Maze, maze.Rows, maze.Cols);
        drawMaze(arr, maze.Start, maze.End, 0, 0, 450, 500, 1);
        //Save positions of my and opponents positions.
        playerCurrent = maze.Start;
        opponentCurrent = maze.Start;
        goal = maze.End;
        //Draw line on canvas.
        var canvas = document.getElementById("mazeCanvas");
        context = mazeCanvas.getContext("2d");
        // clear the canvas
        context.clearRect();
        context.beginPath();
        context.lineWidth = 3;
        context.moveTo(450, 0);
        context.lineTo(450, 500);
        context.stroke();
        
        context.beginPath();
        context.lineWidth = 3;
        context.moveTo(475, 0);
        context.lineTo(475, 500);
        context.stroke();
        drawMaze(arr, maze.Start, maze.End, 475, 0, 450, 500, 0);
        //Set is valid function.
        rows = maze.Rows;
        cols = maze.Cols;
        cellWidth = 450 / maze.Cols;
        cellHeight = 500 / maze.Rows;
        isValid = function (i, j) {
            return 0 <= j && j < cols && 0 <= i && i < rows && arr[i][j] != 1;
        };
    };

    // set a function to end a game
    gameEnded = hub.server.gameEnded;

    $.connection.hub.start().done(function () {
        $("#startButton").click(function () {
            var myCanvas = document.getElementById("mazeCanvas");
            var ctx = mazeCanvas.getContext("2d");
            ctx.font = "40px Georgia";
            ctx.fillText("Loading...", 350, 250);
            var name = $("#mazeName").val();
            if (!checkVal(name, "name")) {
                return;
            }
            var rows = $("#rows").val();
            if (!checkVal(rows, "rows")) {
                return;
            }
            rows = checkNumber(rows, "rows");
            var cols = $("#cols").val();
            if (!checkVal(cols, "cols")) {
                return;
            }
            cols = checkNumber(cols, "cols");
            //Start game from hub.
            hub.server.startGame(name, rows, cols, sessionStorage.getItem("username"));
        });
    }).fail(function (error) {
        console.log('Invocation of start failed. Error:' + error)
        });

    //Set games button.
    $("#gamesButton").click(function () {
        //Send list request to server.
        var apiUrl = "../api/Maze/gamelist"
        $.getJSON(apiUrl).done(function (lst) {
            //Update list of games.
            var options = document.getElementById("mylist");
            options.innerHTML = "";
            for (var i = 0; i < lst.length; i++) {
                var li = document.createElement("li");
                li.appendChild(document.createTextNode(lst[i]));
                options.appendChild(li);
            }
            //Again set click
            $("#mylist li").click(function () {
                $('#games').val($(this).text());
            });
        });
    });

    //Set click for join button.
    $("#joinButton").click(function () {
        //Connect through the hub to join the game.
        hub.server.joinGame($("#games").val(), sessionStorage.getItem("username"));
    });

    //Set reaction to movement by setting reaction to
    //keyboard on the body.
    /****************************************************
    This is code is identical to the one in the single player
    the only difference is that after each move we will send
    to the server the movement of the player.
    ****************************************************/
    $("body").keydown(function (key) {
        var direction = movePlayer(key,0,0,playerImage,playerCurrent,1);
        if (direction != 0) {
            //Send through hub.
            hub.server.play(direction);
        }
    });
});


function movePlayer(key,xInd,yInd,image,currentPos,isMine) {
    //left = 37
    //up = 38
    //right = 39
    //down = 40
    var direction = 0;
    var myKey;
    if (isMine == 1) {
        myKey = key.keyCode;
    } else {
        myKey = key;
    }
    switch (myKey) {
        case 37:
            if (isValid(currentPos.Row, currentPos.Col - 1)) {
                direction = 1;
                context.clearRect(xInd + currentPos.Col * cellWidth,
                    yInd + currentPos.Row * cellHeight, cellWidth, cellHeight);
                //Draw player.
                currentPos.Col--;
                context.drawImage(image, xInd + currentPos.Col * cellWidth,
                   yInd + currentPos.Row * cellHeight, cellWidth, cellHeight)
            }
            break;
        case 38:
            if (isValid(currentPos.Row - 1, currentPos.Col)) {
                direction = 2;
                context.clearRect(xInd + currentPos.Col * cellWidth,
                      yInd + currentPos.Row * cellHeight, cellWidth, cellHeight);
                //Draw player.
                currentPos.Row--;
                context.drawImage(image, xInd + currentPos.Col * cellWidth,
                      yInd + currentPos.Row * cellHeight, cellWidth, cellHeight)
            }
            break;
        case 39:
            if (isValid(currentPos.Row, currentPos.Col + 1)) {
                direction = 3;
                context.clearRect(xInd + currentPos.Col * cellWidth,
                      yInd + currentPos.Row * cellHeight, cellWidth, cellHeight);
                //Draw player.
                currentPos.Col++;
                context.drawImage(image, xInd + currentPos.Col * cellWidth,
                      yInd + currentPos.Row * cellHeight, cellWidth, cellHeight)
            }
            break;
        case 40:
            if (isValid(currentPos.Row + 1, currentPos.Col)) {
                direction = 4;
                context.clearRect(xInd + currentPos.Col * cellWidth,
                      yInd + currentPos.Row * cellHeight, cellWidth, cellHeight);
                //Draw player.
                currentPos.Row++;
                context.drawImage(image, xInd + currentPos.Col * cellWidth,
                       yInd + currentPos.Row * cellHeight, cellWidth, cellHeight)
            }
            break;
    }
    if (key.keyCode >= 37 && key.keyCode <= 40 && currentPos.Row == goal.Row &&
        currentPos.Col == goal.Col) {
        setTimeout(function () {
            alert("You won!!!");
            gameEnded();
        }, 40);
        $("body").off();
    }
    if (isMine == 1) {
        //If it's my player prevent it from moving twice.
        key.stopImmediatePropagation();
    }
    return direction;
}