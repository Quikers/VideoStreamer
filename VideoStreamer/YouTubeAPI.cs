using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Resources;
using System.Threading;
using Google.Apis;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace VideoStreamer {
    class YouTubeAPI {

        private static YouTubeService ytService = Auth();

        private static Stream GenerateStreamFromString (string s) {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private static YouTubeService Auth() {
            UserCredential credentials;
            using (Stream stream = GenerateStreamFromString(Encoding.ASCII.GetString(Properties.Resources.client_secret))) {
                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("YouTubeAPI")
                ).Result;
            }

            YouTubeService service = new YouTubeService(new BaseClientService.Initializer {
                HttpClientInitializer = credentials,
                ApplicationName = "YouTubeAPI"
            });

            return service;
        }

        public static void GetVideoInfo(YouTubeVideo video) {
            VideosResource.ListRequest videoRequest = ytService.Videos.List("snippet");
            videoRequest.Id = video.id;

            Console.Write("Executing video request...");
            VideoListResponse response = videoRequest.Execute();
            Console.WriteLine(" Done!");
            if (response.Items.Count > 0) {
                Console.Write("Video found, processing...");
                video.title = response.Items[0].Snippet.Title;
                video.description = response.Items[0].Snippet.Description;
                video.publishedDate = response.Items[0].Snippet.PublishedAt.Value;
                Console.WriteLine(" Done!");
            } else {
                Console.Write("Video NOT found, processing...");
                video.title = "Video ID \"" + video.id + "\" not found...";
                Console.WriteLine(" Done!");
            }

        }

    }
}
