using System.Collections.Generic;

namespace NAvocado
{
    /// <summary>
    ///     Not implemented, relic of ye olde days
    /// </summary>
    public class UploadToken
    {
    }


    public class Data
    {
        public string Text { get; set; }
        public string Id { get; set; }
        public UploadToken UploadToken { get; set; }
        public ImageInfo Info { get; set; }
        public ImageUrls ImageUrls { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Message { get; set; }
        public string StatusType { get; set; }
        public List<string> Urls { get; set; }
    }

    public class Activity
    {
        public string Action { get; set; }
        public bool Bookmarked { get; set; }
        public Data Data { get; set; }
        public string Id { get; set; }
        public object TimeCreated { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
    }
}