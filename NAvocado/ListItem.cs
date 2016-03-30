namespace NAvocado
{
    public class ListItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool Complete { get; set; }
        public bool Important { get; set; }
        public string UserId { get; set; }
        public long UpdateTime { get; set; }
        public ImageUrls ImageUrls { get; set; }
        public ImageInfo ImageInfo { get; set; }
    }
}