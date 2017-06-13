
/* setting up */
// opening the home page and setting it as active
document.getElementById("home").className = "active"
$("#page-placeholder").load("/views/Home.html");
// at the begining the user isn't logged in so we'll change the multiplayer tab behavior
const initMultiplayerTabOnClick = document.getElementById("multi").onclick;
document.getElementById("multi").onclick =
    (event) => tabClicked(document.getElementById("register"), '/views/RegisterSheet.html');
// save the original register and login deatures that will change after logging in
const initRegisterTabHTML = document.getElementById("register").outerHTML;
const initLoginTabOnClicked = document.getElementById("login").onclick;

function tabClicked(element, htmlFile) {
    // deactivate the previous active tab
    document.getElementsByClassName("active")[0].className = "";
    // activate the clicked tab
    element.className = "active";
    // load the given html file
    $("#page-placeholder").load(htmlFile);
}

function loggedIn(username) {
    // TODO add confermition of log in
    document.getElementById("multi").onclick = initMultiplayerTabOnClick;
    document.getElementById("register").outerHTML = '<p id="register">Welcome ' + username + '!</p>';
    var loginTab = document.getElementById("login");
    loginTab.innerHTML = "Log out";
    loginTab.onclick = (event) => loggedOut(username);
}

function loggedOut(username) {
    // load the home page
    tabClicked(document.getElementById("home"), '/views/Home.html');
    // TODO add confermition of log out
    document.getElementById("multi").onclick = 
        (event) => tabClicked(document.getElementById("register"), '/views/RegisterSheet.html');
    document.getElementById("register").outerHTML = initRegisterTabHTML;
    var loginTab = document.getElementById("login");
    loginTab.innerHTML = "log in";
    loginTab.onclick = initLoginTabOnClicked;
}

function fuckkkk() {
    // load the given html file
    document.load("/views/MainBar.html");
}