/****************************************************
First load the images for player and opponent.
Save cell width,height,the goal destination and the context
of the canvas so that each function could use it.
******************************************************/
var playerImage = new Image();
playerImage.src = "/views/Images/mario.png";
var opponentImage = new Image();
opponentImage.src = "/views/Images/luigi.png";
var goalImage = new Image();
goalImage.src = "/views/Images/goal.png";
var playerCurrent, opponentCurrent, isValid;
var cellWidth, cellHeight, goal, context;


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

/*****************************************************************************
* Function Name: drawMaze.
* Input: The maze array,start and end positions, the x and y offsets that the maze
* will be drawn on the canvas,the height of the maze and a boolean that tells us if
* the maze is ours or the opponents.
* Output: void.
* Operation: Fill the rectangles of the canvas according to the maze array in order
* to the draw the given array on the canvas in the given offsets, later we will check
* the boolean to know what image we need to draw of the canvas, that of the player or
* that of the opponent.
*******************************************************************************/
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

    //Draw player. If it's ours draw the player's otherwise the opponents.
    if (isMine == 1) {
        context.drawImage(playerImage, xInd + start.Col * cellWidth,
            yInd + start.Row * cellHeight, cellWidth, cellHeight);
    } else {
        context.drawImage(opponentImage, xInd + start.Col * cellWidth,
            yInd + start.Row * cellHeight, cellWidth, cellHeight);
    }

    //Draw the image of the goal of the maze.
    context.drawImage(goalImage, xInd + end.Col * cellWidth,
        yInd + end.Row * cellHeight, cellWidth, cellHeight);
}


/*************************************************************
* Function Name: checkVal.
* Input: The value that we will check and the name we will use in the
* alert.
* Output: 0 if there is an error and 1 otherwise.
* Operation: Check if a value was entered otherwise notify the user
* by using the alert function.
*************************************************************/
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
var start_func, join_func;

//Runs when document is ready.
$(document).ready(function () {

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
        var canvas = document.getElementById("mazeCanvas");
        context = mazeCanvas.getContext("2d");
        //Clear the canvas.
        context.clearRect(0,0,925,500);
        arr = strToArr(maze.Maze, maze.Rows, maze.Cols);
        //Draw the first maze.
        drawMaze(arr, maze.Start, maze.End, 0, 0, 450, 500, 1);
        //Save positions of my and opponents positions.
        playerCurrent = maze.Start;
        opponentCurrent = maze.Start;
        goal = maze.End;
       
        //Set the canvas inorder to draw 2 mazes.
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
        //Draw the second maze.
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
    
    //Set the following function to operate when hub start() function is done.
    $.connection.hub.start().done(function () {
        start_func = function () {
            // disable join
            $("#joinButton").off();
            var myCanvas = document.getElementById("mazeCanvas");
            var ctx = mazeCanvas.getContext("2d");
            ctx.clearRect(0, 0, 925, 500);
            ctx.font = "40px Georgia";
            ctx.fillText("Loading...", 350, 250);
            var name = $("#mazeName").val();
            //Check the input in the name field.
            if (!checkVal(name, "name")) {
                return;
            }
            var rows = $("#rows").val();
            //Check the value in row field.
            if (!checkVal(rows, "rows")) {
                return;
            }
            rows = checkNumber(rows, "rows");
            var cols = $("#cols").val();
            //Check the value in cols field.
            if (!checkVal(cols, "cols")) {
                return;
            }
            cols = checkNumber(cols, "cols");
            //Start game from hub.
            hub.server.startGame(name, rows, cols, sessionStorage.getItem("username"));
        };
        $("#startButton").click(start_func);
    }).fail(function (error) {
        console.log('Invocation of start failed. Error:' + error)
        });

    //Set games button.When it is clicked send list request to the server.
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
            //Again set click for the following click.
            $("#mylist li").click(function () {
                $('#games').val($(this).text());
            });
        });
    });

    //Set click for join button.
    join_func = function () {
        // disable start
        $("#startButton").off();
        //Connect through the hub to join the game.
        hub.server.joinGame($("#games").val(), sessionStorage.getItem("username"));
    };
    $("#joinButton").click(join_func);

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

/*******************************************************************
* Function Name: movePlayer.
* Input: The key,index of x and y values, the image of the player,the
* current position and a boolean that tells us if it's the player or
* the opponent.
* Output: The direction.
* Operation: Switch case based on the key that removes the previous image
* of the player updates the position and draws a new one.
********************************************************************/
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
    //Check which key we got.
    switch (myKey) {
        case 37:
            //Move left.
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
            //Move up.
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
            //Move right
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
            //Move down.
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
    //Check if it's ours.
    if (isMine == 1) {
        //Check if we won.
        if (key.keyCode >= 37 && key.keyCode <= 40 && currentPos.Row == goal.Row &&
            currentPos.Col == goal.Col) {
            setTimeout(function () {
                alert("You won!!!");
                gameEnded();
            }, 40);
            $("body").off();
            $("#joinButton").click(join_func);
            $("#startButton").click(start_func);
        }
    } else {
        //Check if the opponent won.
        if (key >= 37 && key <= 40 && currentPos.Row == goal.Row &&
            currentPos.Col == goal.Col) {
            setTimeout(function () {
                alert("You lost!!!");
            }, 40);
            $("body").off();
            $("#joinButton").click(join_func);
            $("#startButton").click(start_func);
        }
    }

    if (isMine == 1) {
        //If it's my player prevent it from moving twice.
        key.stopImmediatePropagation();
    }
    return direction;
}