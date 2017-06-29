/*********************************************************
* This is our mazeboard plugin, the plugin will receive the maze
* object that we will receive from the server and it will contain all 
* the functions that operate on the server in order to create an api for
* us when we are using the maze in the single player game.
*********************************************************/
(function ($) {

    $.fn.mazeBoard = function (
        mazeObj
       ) {

        //Load the images.
        var playerImage = new Image();
        playerImage.src = "/views/Images/mario.png";
        var goalImage = new Image();
        goalImage.src = "/views/Images/goal.png";
        var isValid = function (i, j) {
            return 0 <= j && j < mazeObj.Cols && 0 <= i &&
                i < mazeObj.Rows && arr[i][j] != 1;
        };
        var goal = mazeObj.End;

        /*************************************************
        * The draw maze function will extract the start,end,rows
        * and cols from the maze object,and draw the maze according
        * to the maze string.
        *************************************************/
        $.fn.drawMaze = function(maze) {
            var start = mazeObj.Start;
            var end = mazeObj.End;
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

            //Draw player and goal images.
            context.drawImage(playerImage, start.Col * cellWidth, start.Row * cellHeight, cellWidth, cellHeight);

            context.drawImage(goalImage, end.Col * cellWidth, end.Row * cellHeight, cellWidth, cellHeight);

        }

        /*********************************************
        * Move the player based on the key that was entered
        * by using a switch case.
        **********************************************/
        var movePlayer = function (key) {
            //left = 37
            //up = 38
            //right = 39
            //down = 40
            switch (key.keyCode) {
                case 37:
                    if (isValid(current.Row, current.Col - 1)) {
                        context.clearRect(current.Col * cellWidth, current.Row * cellHeight, cellWidth, cellHeight);
                        //Draw player.
                        current.Col--;
                        context.drawImage(playerImage, current.Col * cellWidth,
                            current.Row * cellHeight, cellWidth, cellHeight)
                    }
                    break;
                case 38:
                    if (isValid(current.Row - 1, current.Col)) {
                        context.clearRect(current.Col * cellWidth, current.Row * cellHeight, cellWidth, cellHeight);
                        //Draw player.
                        current.Row--;
                        context.drawImage(playerImage, current.Col * cellWidth,
                            current.Row * cellHeight, cellWidth, cellHeight)
                    }
                    break;
                case 39:
                    if (isValid(current.Row, current.Col + 1)) {
                        context.clearRect(current.Col * cellWidth, current.Row * cellHeight, cellWidth, cellHeight);
                        //Draw player.
                        current.Col++;
                        context.drawImage(playerImage, current.Col * cellWidth,
                            current.Row * cellHeight, cellWidth, cellHeight)
                    }
                    break;
                case 40:
                    if (isValid(current.Row + 1, current.Col)) {
                        context.clearRect(current.Col * cellWidth, current.Row * cellHeight, cellWidth, cellHeight);
                        //Draw player.
                        current.Row++;
                        context.drawImage(playerImage, current.Col * cellWidth,
                            current.Row * cellHeight, cellWidth, cellHeight)
                    }
                    break;
            }
            if (key.keyCode >= 37 && key.keyCode <= 40 && current.Row == goal.Row &&
                current.Col == goal.Col) {
                setTimeout(function () { alert("You won!!!"); }, 40);
                $("body").off();
            }
            key.stopImmediatePropagation();
        };

        /**********************************************************
        * Function Name:moveAccordingToSolution.
        * Input: solution string and current index in the string.
        * Output: void.
        * Operation: move the player according to the solution.
        **********************************************************/
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
        }

        /*********************************************
        * Function that will create the full solution animation
        * by using SetTimeout and the solution animation function
        * that we defined before.
        *********************************************/
        var solve_func = function (solution, index) {
            moveAccordingToSolution(solution, index);
            setTimeout(function () {
                if (index < solution.length) {
                    solve_func(solution, index + 1);
                } else {
                    alert("Solution Animation Done.");
                    $("body").off();
                    return;
                }
            }, 200);
        };

        $.fn.solve = solve_func;

        $("body").keydown(movePlayer);
        return $.fn;
    };
    
}(jQuery));