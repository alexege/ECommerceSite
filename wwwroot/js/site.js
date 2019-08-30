// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.console.clear();
$(document).ready(function(){

    console.log("Ready");

    $(".p-grid-in").hover(function(){
        // console.log($(this).children("button"));
        $(this).children(":button").stop().animate({"opacity":"1"});
        $(this).css({"border":"2px solid #06aae2"});
        $(this).children(".p-img").animate({"padding": 0});
    }, function(){
        $(this).children(":button").stop().animate({"opacity":"0"});
        $(this).css({"border":"1px solid #06aae2"});
        $(this).children(".p-img").animate({"padding": 5});
    })

});