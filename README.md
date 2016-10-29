Release branch contains the code which should be deployed. Master branch has minor differences that simply make it easier to debug.

# HomePageApp

To run this project, you will need a Wunderground API Key (https://www.wunderground.com/weather/api/).
Then you need to create a resource file for the WeatherAPI library. This can be done through Visual Studio. 
The file should be named Resource.resx and should be in the directory /src/WeatherAPI/.
Once the file is created, you can add a resource of the name "APIKey" with your API key as the value.

Inspired by the Momentum extension for Chrome.
