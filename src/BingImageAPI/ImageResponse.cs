using System.Collections.Generic;

namespace BingImageAPI
{
    /// <summary>
    /// Image object, contains information about individual image
    /// </summary>
    public class Image
    {
        // Date information, not needed
        public string startdate { get; set; }
        public string fullstartdate { get; set; }
        public string enddate { get; set; }

        /// <summary>
        /// Full url to access image
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Partial url which can be appended with parameters
        /// </summary>
        public string urlbase { get; set; }

        /// <summary>
        /// Name and credit to author
        /// </summary>
        public string copyright { get; set; }

        /// <summary>
        /// Link to bing search with more data
        /// </summary>
        public string copyrightlink { get; set; }

        // Image information, not needed
        public bool wp { get; set; }
        public string hsh { get; set; }
        public int drk { get; set; }
        public int top { get; set; }
        public int bot { get; set; }
        public List<object> hs { get; set; }
    }

    /// <summary>
    /// Tooltips provided with the image
    /// </summary>
    public class Tooltips
    {
        public string loading { get; set; }
        public string previous { get; set; }
        public string next { get; set; }
        public string walle { get; set; }
        public string walls { get; set; }
    }

    /// <summary>
    /// Immediate object returned, main image is the first in list, created 
    /// with json2csharp.com 
    /// </summary>
    public class ImageResponse
    {
        /// <summary>
        /// List of images, first element is today's image
        /// </summary>
        public List<Image> images { get; set; }

        /// <summary>
        /// Tooltip data
        /// </summary>
        public Tooltips tooltips { get; set; }
    }
}