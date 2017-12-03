var color;
$(document).ready(function(){
  
  color = $(".selected").css("background-color");;
  var colors = ['red', 'orange', 'yellow', 'green', 'blue', 'purple', 'black', 'white'];
  var cList = $('ul.colorList');
  
  
  //When clicking on control list items
  $(".control").on("click", "li", function(){
    //Deselect sibling elements
    $(this).siblings().removeClass("selected");
    //Select clicked element
    $(this).addClass("selected");
    //cache current color
    color = $(this).css("background-color");
  });
  
  //!!!Change New Color display
  //When "New Color" is pressed
  $("#revealColorSelect").click(function(){
    //Show color select or hide the color select
    changeColor();
    $("#colorSelect").toggle();
  });
  
  //update the new color span
  function changeColor() {
    var r = $("#red").val();
    var g = $("#green").val();
    var b = $("#blue").val();
    
    $("#newColor").css("background-color", "rgb(" + r + "," + g + ", " + b + ")");
  }
  
  //When color sliders change
  $("input[type=range]").change(changeColor)
  
  //When "Add Color" is pressed
  $("#addNewColor").click(function(){
    //Append the color to the controls ul
    var $newColor = $("<li></li>");
    $newColor.css("background-color", $("#newColor").css("background-color"));
    $(".control ul").append($newColor);
    //Select the new color
    $newColor.click();
  });
});