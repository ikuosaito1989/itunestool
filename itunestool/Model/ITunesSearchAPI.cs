using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITunEsTooL.Model
{
    class ITunesSearchAPI
    {
        public int resultCount { get; set; }
        public Result[] results { get; set; }
    }

    public class Result
    {
        public string artistName { get; set; }
        public string artworkUrl30 { get; set; }
        public string collectionCensoredName { get; set; }
    }

}
