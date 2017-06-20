
function register() {
    // get the values
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    var passwordValidation = document.getElementById("passwordValidation").value;
    var email = document.getElementById("email").value;
    // clear the values
    document.getElementById("password").value = "";
    document.getElementById("passwordValidation").value = "";
    // validate the validation password
    if (password !== passwordValidation) {
        document.getElementById("errorMsg").textContent = "Password and the repeated password differ!";
        return;
    }

    // register at the server and get the new bar
    url = "../api/Users/Register/" + username + "/" + password + "/" + email;
    $.getJSON(url).done(function (answer) {
        answer = answer.ans;
        if (answer !== "0") {
            onSuccessfulLogin(username, answer)
        }
        else {
            document.getElementById("errorMsg").textContent = "Username already exists";
        }
    });
}

function login() {
    // get the values
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    // clear the password field
    document.getElementById("password").value = ""

    // login at the server and get the new bar
    url = "../api/Users/Login/" + username + "/" + password;
    $.get(url).done(function (answer) {
        onSuccessfulLogin(username, answer)
    }).fail(function () {
        document.getElementById("errorMsg").textContent = "Wrong username or password"
    });
}

function onSuccessfulLogin(username, multiplayerHTMLPath)
{
    // save the data for the main bar
    sessionStorage.setItem("username", username);
    sessionStorage.setItem("multiplayerHTMLPath", multiplayerHTMLPath);
    // go to main page
    sessionStorage.setItem("active", "home");  // set the home page as active
    window.location.href = "/views/Home.html";
}

//function logout(username, multiplayerHTMLPath)
//{
//    sessionStorage.removeItem("username");
//    sessionStorage.removeItem("multiplayerHTMLPath");
//}