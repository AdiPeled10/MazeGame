/*************************************************************
* This code will run only after the page will finish loading.
* It will ask the server for the top 10 players and then show
* them in a table (will add them to the HTML).
***************************************************************/
$(document).ready(function () {
    $.get("../api/Users/GetTop10").done(function (lst) {
        //Update list of games.
        var ranks = document.getElementById("rankTable");
        var td1, td2, td3, td4;
        for (var i = 0; i < lst.length; i++) {
            var tr = document.createElement("tr");
            var obj = lst[i];
            // generate columns
            td1 = document.createElement("td");
            td2 = document.createElement("td");
            td3 = document.createElement("td");
            td4 = document.createElement("td");
            // add content
            td1.innerText = i + 1;
            td2.innerText = obj.username;
            td3.innerText = obj.wins;
            td4.innerText = obj.loses;
            //tr.innerHTML = "< td >" + i + "</td >" + "< td >" + obj.username + "</td >" +
            //    "< td >" + obj.wins + "</td >" + "< td >" + obj.loses + "</td >";
            // add the columns to the row
            tr.appendChild(td1);
            tr.appendChild(td2);
            tr.appendChild(td3);
            tr.appendChild(td4);
            // add row to the table
            ranks.appendChild(tr);
        }
    }).fail(function (error) {
        console.log('Invocation of GetTop10 failed. Error:' + error)
    });
});