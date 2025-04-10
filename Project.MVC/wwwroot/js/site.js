var navbar = document.getElementById("left-navbar");
var hamburgerIcon = document.getElementById("hamburger-icon");
var closeIcon = document.getElementById("close-icon");

hamburgerIcon.addEventListener("click", function () {
    navbar.classList.add("visible");
    hamburgerIcon.style.display = "none";
});

closeIcon.addEventListener("click", function () {
    navbar.classList.remove("visible");
    hamburgerIcon.style.display = "block";
});

$(document).ready(function () {
    $('select').select2();
});