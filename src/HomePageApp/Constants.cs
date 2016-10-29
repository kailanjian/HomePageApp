using System;

/// <summary>
/// Class to hold constants to be used throughout the program. It is especially
/// useful for allowing constants to be used in the .cshtml files
/// </summary>
public class Constants
{

    public const string BING_URL_BASE = "http://www.bing.com";
    
    // Hours which describe the begginning of afternoon and evening
    public const int EVENING_LOWER_BOUND = 18;
    public const int AFTERNOON_LOWER_BOUND = 12;
}
