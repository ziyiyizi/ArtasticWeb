using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UserDetails
    {
        public long artistId;
        public String artistName;
        public String iconURL;
        public long frenzy;
        public long worknum;
        public String joinyear;
        public String description;
        public List<ArtworkDetails> works;
        public IList followers;
        public IList following;
        public bool follow;
    }
}
