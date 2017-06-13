


function drawMaze(maze) {
    var myCanvas = document.getElementById("mazeCanvas");
    var context = mazeCanvas.getContext("2d");
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
}

/****************************
* Function that will generate maze for us.
* TODO - now it's only client side,later we will get the
* maze from the server.
***************************/
function generateMaze() {
    //Get maze name,rows and cols.
    var name, mazeCols, mazeRows;
    //Check if user entered a name.
    if ($('#mazeName').val().length == 0) {
        alert("Please enter a name in order to continue.");
        return;
    } else {
        mazeName = document.getElementById("mazeName").value;
    }
    //Check if user entered number of rows.
    if ($('#rows').val().length == 0) {
        alert("Enter a number of rows for your maze.");
        return;
    } else {
        try {
            mazeRows = parseInt(document.getElementById("rows").value);
        } catch (e) {
            alert("Enter rows in the correct format.");
            return;
        }
    }

    if ($('#cols').val().length == 0) {
        alert("Enter a number of columns for your maze.");
        return;
    } else {
        try {
            mazeCols = parseInt(document.getElementById("cols").value);
        } catch (e) {
            alert("Enter columns in the correct format.");
            return;
        }
    }

    //After we finished checking everything lets create some random maze.
    maze = [[0,1,0,0,1,1,0,0,1,0],
        [1,0,1,0,1,0,1,0,0,0],
        [0,0,0,1,1,1,0,1,0,1],
        [0,1,0,0,0,0,1,0,1,0],
        [0,1,0,0,1,0,0,0,1,0],
        [0,1,0,0,0,1,0,0,1,0],
        [0,1,0,0,1,0,0,0,1,0],
        [0,1,0,0,0,1,0,0,1,0],
        [0,1,0,0,1,0,0,0,1,0],
        [0,1,0,0,0,1,0,0,1,0]
    ];
    drawMaze(maze);

    return;
}