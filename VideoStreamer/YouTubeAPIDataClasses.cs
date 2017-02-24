using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoStreamer {
    public class YouTubeVideo {
        public string id, title, description;
        public DateTime publishedDate;

        public YouTubeVideo(string id) {
            this.id = id;

            YouTubeAPI.GetVideoInfo(this);
        }
    }
}
