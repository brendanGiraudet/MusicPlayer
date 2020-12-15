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

        public List<string> SearchedVideos { get; set; } = new List<string>();

        public async Task SearchAsync(string wordToSearch)
        {
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
                            SearchedVideos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
