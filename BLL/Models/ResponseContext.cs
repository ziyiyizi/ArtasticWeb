using Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class ResponseContext
    {
        public List<ArtworkDetails> posts;
        public ArtworkDetails post;
        public ChartData chartdata;
        public List<UserDetails> members;
        public UserDetails member;
        public List<notification> notification;
        public List<string> values;
        public int notifyNum;
        public Weekly weekly;
        public bool error;
        public string errorMsg;
        public string userName;
        public long userId;
        public string iconURL;
    }
}
