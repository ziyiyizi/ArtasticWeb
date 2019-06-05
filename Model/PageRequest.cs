using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class PageRequest
    {
        public PageRequest(int size, int no)
        {
            PageNumber = no;
            PageSize = size;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
