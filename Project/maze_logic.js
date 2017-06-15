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

function drawMaze(maze,start,end,xInd,yInd,mazeWidth,mazeHeight) {
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
                context.fillRect(cellWidth * j, cellHeight * i,
               cellWidth, cellHeight);
            }
        }
    }