
function tabClicked(element, htmlFile) {
    // deactivate the previous active tab
    document.getElementsByClassName("active")[0].className = "";
    // activate the clicked tab
    element.className = "active";
    // load the given html file
    $("#page-placeholder").load(htmlFile);
}