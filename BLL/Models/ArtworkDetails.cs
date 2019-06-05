using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class ArtworkDetails
    {
        public long artistId;
        public String artistName;
        public long artworkId;
        public String artworkName;
        public String date;
        public long frenzy;
        public IEnumerable<String> tags;
        public String description;
        public String fileURL;
        public String iconURL;
        public List<Dictionary<String, String>> likerslist;
        public List<Dictionary<String, String>> comments;
        public bool isLike;
    }
}
