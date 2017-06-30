/*************************************************************
* This code will run only after the page will finish loading.
* It will get the session storage value of the key "active"
* and set the corresponding element ID as class "active".
* If the user is logged in (if "username" key has a value in the
* session storage), this code will modify the main bar to have a
* log out button, a "hello {username}!" statement and set the
* multiplayer button link to the session storage value of the key
* "multiplayerHTMLPath".
***************************************************************/
$(document).ready(function () {
    // set active header
    var tab = sessionStorage.getItem("active");
    if (tab != null) {
        document.getElementById(tab).className = "active";
    }
    else {
        document.getElementById("home").className = "active";
    }
    // set log-in/log-out options
    var username = sessionStorage.getItem("username");
    if (username != null) {
        document.getElementById("multi").setAttribute("href", sessionStorage.getItem("multiplayerHTMLPath"));
        document.getElementById("register").outerHTML = '<p id="register">Welcome ' + username + '!</p>';
        var loginTab = document.getElementById("login");
        loginTab.innerHTML = "Log out";
        loginTab.setAttribute("href", "/views/Home.html");
        loginTab.onclick = logout;
    }
});

/*************************************************************
* This function removes "username" and "multiplayerHTMLPath"
* values from the session storage. Then, it calls 'activate("home")'.
***************************************************************/
function logout() {
    sessionStorage.removeItem("username");
    sessionStorage.removeItem("multiplayerHTMLPath");
    activate("home");
}

/*************************************************************
* This function sets "active" key in the session storage to the
* value of elementId argument.
***************************************************************/
function activate(elementId)
{
    sessionStorage.setItem("active", elementId);
}