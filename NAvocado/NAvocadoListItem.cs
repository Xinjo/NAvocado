using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAvocado
{
    public class NAvocadoListItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool Complete { get; set; }
        public bool Important { get; set; }
        public string UserId { get; set; }
        public long UpdateTime { get; set; }
        public NAvocadoImageUrls ImageUrls { get; set; }
        public NAvocadoImageInfo ImageInfo { get; set; }
    }
}
