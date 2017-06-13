
function register() {
    var username = document.getElementById("username").value;
    // TODO check if username exists
    var password = document.getElementById("password").value;
    var passwordValidation = document.getElementById("passwordValidation").value;
    var email = document.getElementById("email").value;
    if (password !== passwordValidation) {
        alert("Password and the reapted password differ!")
        return;
    }

    // register at the server and get the new bar
    $.post("/views/MainBar.html", {
        "username": username,
        "password": password,
        "email": email
    })

    // go to main page
    location.replace("/views/Home.html")

    // TODO send the server the password and username
    //alert("Welcome to a world of mazes and wonder!");
    //// load the home page
    //tabClicked(document.getElementById("home"), '/views/Home.html');
    //loggedIn(username);

    // set the main bar
    
    //var mainBarFile = document.open("/views/MainBar.html");
    //mainBarFile.wr
    //document.getElementById("bar").getElementById("login").innerHTML = "log out";
    //document.getElementById("login").href = "/views/Home.html";
    //document.getElementById("login").onsubmit = "logOut()";
    //<a style="float:right;" href="/views/RegisterSheet.html" id="register">Register</a>
    //<a style="float:right;" href="/views/LoginSheet.html" id="login">Login</a>
}