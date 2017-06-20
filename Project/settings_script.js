
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