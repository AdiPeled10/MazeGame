
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
        loginTab.setAttribute("href", "/views/Home.html");//document.getElementById("home").getAttribute("href");
        loginTab.onclick = logout;
    }
});

function logout() {
    sessionStorage.removeItem("username");
    sessionStorage.removeItem("multiplayerHTMLPath");
    activate("home");
    //window.location.href = "/views/Home.html";
}

function activate(elementId)
{
    sessionStorage.setItem("active", elementId);
}