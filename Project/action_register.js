
function register() {
    var username = document.getElementById("username").value;
    // TODO check if username exists
    var password = document.getElementById("password").value;
    var passwordValidation = document.getElementById("passwordValidation").value;
    var email = document.getElementById("email").value;
    if (password !== passwordValidation) {
        document.getElementById("errorMsg").textContent = "Password and the repeated password differ!";
        return;
    }

    // register at the server and get the new bar
    url = "../api/Users" + "/" + username + "/" + password + "/" + email;
    $.getJSON(url).done(function (answer) {
        answer = answer.ans;
        if (answer === "1") {
            // go to main page
            location.replace("/views/Home.html");
        }
        else {
            document.getElementById("errorMsg").textContent = "Username already exists";
        }
    });
    //$.ajax({
    //    type: "POST",
    //    traditional: true,
    //    dataType: "json",
    //    url: "../api/Users",
    //    data: {
    //        username: username,
    //        password: password,
    //        email: email
    //    },
    //    cache: false,
    //    complete: function (data) {
    //        var answer = data.ans;
    //        if (answer === "1") {
    //            // go to main page
    //            location.replace("/views/Home.html");
    //        }
    //        else {
    //            document.getElementById("errorMsg").textContent = "Username already exists";
    //        }
    //    }
    //});
    //$.post("../api/Users",
    //    {
    //        username: username,
    //        password: password,
    //        email: email
    //    }).done(function (answer) {
    //        answer = answer.ans;
    //        if (answer === "1") {
    //            // go to main page
    //            location.replace("/views/Home.html");
    //        }
    //        else {
    //            document.getElementById("errorMsg").textContent = "Username already exists";
    //        }
    //    }).fail(function (error) {
    //        console.log('Invocation of start failed. Error:' + error);
    //    });

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