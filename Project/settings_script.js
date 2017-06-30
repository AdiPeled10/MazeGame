/*************************************************************
* This function will save the input values to the session storage.
* If an input field is empty, its current value will be removed
* form the session storage.
* if an input field has a bad value(g.e. "asdsa" where a number is
* expected) the save will fail and nothing will change.
***************************************************************/
function saveSettings() {
    // get the values
    var rows = document.getElementById("rows").value;
    var defaultRows = localStorage.getItem("defaultRows");
    var cols = document.getElementById("cols").value;
    var algo = document.getElementById("algo").value;
    // set defaults
    if (rows !== "")
    {
        rows = Number(rows);
        if (!isNaN(rows))
        {
            localStorage.setItem("defaultRows", rows);
        }
        else
        {
            document.getElementById("msg").innerHTML = "Invalid rows value!";
            return;
        }
    }
    else
    {
        localStorage.removeItem("defaultRows");
    }

    if (cols !== "") {
        cols = Number(cols);
        if (!isNaN(cols)) {
            localStorage.setItem("defaultCols", cols);
        }
        else
        {
            // undo the rows saving
            localStorage.setItem("defaultRows", defaultRows);
            
            document.getElementById("msg").innerHTML = "Invalid cols value!";
            return;
        }
    }
    else {
        localStorage.removeItem("defaultCols");
    }

    if (algo !== "") {
        localStorage.setItem("defaultAlgo", algo);
    }
    else {
        localStorage.removeItem("defaultAlgo");
    }

    // tell the user the setting are saved
    document.getElementById("msg").innerHTML = "Setting Saved!"
    setTimeout(function () {
        document.getElementById("msg").innerHTML = "";
    }, 2000);
}