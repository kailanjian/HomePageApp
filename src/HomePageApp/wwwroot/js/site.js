/* site wide javascript */

const TEMP_UNIT_COOKIE_NAME = "temperature_unit";


// Hours which describe the begginning of afternoon and evening
const EVENING_LOWER_BOUND = 18;
const AFTERNOON_LOWER_BOUND = 12;

/* 
 * initialize things and set handlers for events
 */
$(document).ready(function () {
   
    // set initial value for the temperature units
    var tempUnit = Cookies.get(TEMP_UNIT_COOKIE_NAME);
    if (tempUnit == null)
    {
        Cookies.set(TEMP_UNIT_COOKIE_NAME, "F");
        $("#temperature").text(TEMP_F_STRING);
    }
    else
    {
        $("#temperature").text(TEMP_F_STRING);
        if (tempUnit == "C")
        {
            $("#celsius-radio").prop("checked", true);
            $("#temperature").text(TEMP_C_STRING);
        }
    }

    // Handle hovering on the settings widget icon
    $("#settings-widget-icon").hover(

        // handlerIn
        function (eventObject) {
            $(eventObject.target).animate({ "font-size": "2em" }, 500);
        },

        // handlerOut
        function (eventObject) {
            $(eventObject.target).animate({ "font-size": "1em" }, 500);
        }
    );

    // handle hovering on the search widget input (note that focus behaviour overrides this in the css)
    $("#search-widget-input").hover(

        //handlerIn
        function (eventObject) {
            $(eventObject.target).animate({"opacity":"1"}, 500);
        },

        //handlerOut
        function (eventObject) {
            $(eventObject.target).animate({ "opacity": "0" }, 500);
        });

    // update beforehand then add a handler for when the button is 
    // triggered
    updateLabeledHiddenRadios();

    $(".labeled-hidden-radio").click(function (eventObject) {
        updateLabeledHiddenRadios();

        // check if we updated the temperature radio button group
        if ($(eventObject.target).attr("name") == "temperature")
        {
            updateTempUnitCookie();
        }
    });



});

function updateTempUnitCookie()
{
    if ($("#farenheit-radio").prop("checked"))
    {
        Cookies.set(TEMP_UNIT_COOKIE_NAME, "F");
        $("#temperature").text(TEMP_F_STRING);
    }
    else
    {
        Cookies.set(TEMP_UNIT_COOKIE_NAME, "C");
        $("#temperature").text(TEMP_C_STRING);
    }
}

/* 
 * function to go through every .labeled-hidden-radio and set it to 
 * a style if the matching radio-button is checked and a different 
 * style if the matching radio-button is not checked.
 */
function updateLabeledHiddenRadios()
{
    $(".labeled-hidden-radio").each(function (index, element) {
        // Radio button is checked
        if ($(element).prop("checked")) {
            // find label with for attribute matching this radio's id
            var matchingLabel = $(element).siblings("label[for=" + $(element).attr("id") + "]");

            // set label to white
            matchingLabel.css("color", "white");
        }
            // Radio button is not checked
        else {
            var matchingLabel = $(element).siblings("label[for=" + $(element).attr("id") + "]");

            matchingLabel.css("color", "gray");
        }
    });
}

var settingsWidgetEnabled = false;

function settingWidgetClick ()
{
    if (!settingsWidgetEnabled)
    {
    /*    document.getElementById("clock-widget").setAttribute("hidden", "hidden");
        document.getElementById("settings-widget").hidden = false;
        document.getElementById("settings-widget-icon").id = "settings-widget-icon-active"; */

        $("#clock-widget").prop("hidden", true);
        $("#settings-widget").prop("hidden", false);
        //$("#settings-widget-icon").addClass("icon-active")
        $("#settings-widget-icon").animate({ "font-size": "2em" }, 500);
        settingsWidgetEnabled = true;
    }
    else
    {
        document.getElementById("clock-widget").hidden = false;
        document.getElementById("settings-widget").hidden = true;
        //document.getElementById("settings-widget-icon-active").id = "settings-widget-icon";
        $("#settings-widget-icon").animate({ "font-size": "1em" }, 500);
        settingsWidgetEnabled = false;
    }
}


function updateClock ( )
{
    var currentTime = new Date();

    var currentHours = currentTime.getHours();
    var currentMinutes = currentTime.getMinutes();

    // Pad the minutes and seconds with leading zeros, if required
    currentMinutes = (currentMinutes < 10 ? "0" : "") + currentMinutes;

    // Convert the hours component to 12-hour format if needed
    currentHours = ( currentHours > 12 ) ? currentHours - 12 : currentHours;

    // Convert an hours component of "0" to "12"
    currentHours = ( currentHours == 0 ) ? 12 : currentHours;

    // Compose the string for display
    var currentTimeString = currentHours + ":" + currentMinutes;

    // Update the time display
    document.getElementById("clock").firstChild.nodeValue 
        = currentTimeString;
}

function updateGreeting (name)
{
    var currentTime = new Date();
    var greeting = "Good Morning, ";
    var currentHours = currentTime.getHours();
    
    // TODO: remove server side logic for this
    if ( currentHours > (AFTERNOON_LOWER_BOUND) ) {
        greeting = "Good Afternoon, ";
    }
    if ( currentHours > (EVENING_LOWER_BOUND)) {
        greeting = "Good Evening, ";
    }
    
    greeting += name;

    document.getElementById("welcome").firstChild.nodeValue 
        = greeting;
}

