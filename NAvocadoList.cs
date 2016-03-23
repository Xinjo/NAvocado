using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAvocado
{
    /// <summary>
    /// Generated from JSON using JSON2C#
    /// </summary>
    public class NAvocadoList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long TimeCreated { get; set; }
        public long TimeUpdated { get; set; }
        public List<NAvocadoListItem> Items { get; set; }
    }
}
