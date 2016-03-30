using System.Collections.Generic;

namespace NAvocado
{
    /// <summary>
    ///     Generated from JSON using JSON2C#
    /// </summary>
    public class List
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long TimeCreated { get; set; }
        public long TimeUpdated { get; set; }
        public List<ListItem> Items { get; set; }
    }
}