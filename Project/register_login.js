/*************************************************************
* This function takes the user inputs from the elements "username",
* "password", "passwordValidation" and "email" (all are IDs).
* If the value of password and passwordValidation differ, the
* function display an error message and return. Otherwise, it will
* ask the server to register the username. If registerssion was
* sucessful, "onSuccessfulLogin(username, the server answer)" is
* called. Else, an error message is showed.
***************************************************************/
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

/*************************************************************
* This function takes the user inputs from the elements "username"
* and "password" (all are IDs of elements).
* It will ask the server to login to the username with the given
* password. If a username with that name and password exists, it
* will call "onSuccessfulLogin(username, the server answer)".
* Else, an error message is showed.
***************************************************************/
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

/*************************************************************
* This function saves the username argument to the session storage
* under the key "username". It also saves multiplayerHTMLPath argument
* under the key "multiplayerHTMLPath".
* Then, it set the "active" key of the session storage to "home"
* and replaces the current page with "/views/Home.html".
***************************************************************/
function onSuccessfulLogin(username, multiplayerHTMLPath)
{
    // save the data for the main bar
    sessionStorage.setItem("username", username);
    sessionStorage.setItem("multiplayerHTMLPath", multiplayerHTMLPath);
    // go to main page
    sessionStorage.setItem("active", "home");  // set the home page as active
    window.location.href = "/views/Home.html";
}