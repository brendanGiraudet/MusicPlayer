using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPlayerApplication.ViewModels
{
    public class YoutubePlayerViewModel : IYoutubePlayerViewModel
    {
        readonly YouTubeService _youTubeService;
        public YoutubePlayerViewModel(YouTubeService youTubeService)
        {
            _youTubeService = youTubeService;
        }

        public async Task<List<string>> SearchAsync(string wordToSearch)
        {
            var videos = new List<string>();
            try
            {
                var searchListRequest = _youTubeService.Search.List("snippet");
                searchListRequest.Q = wordToSearch;
                searchListRequest.MaxResults = 5;

                var searchListResponse = await searchListRequest.ExecuteAsync();
                foreach (var searchResult in searchListResponse.Items)
                {
                    switch (searchResult.Id.Kind)
                    {
                        case "youtube#video":
                            videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                            break;
                    }
                }

                return videos;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<string>();
            }
        }
    }
}
