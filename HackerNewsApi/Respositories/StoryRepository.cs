using HackerNewsApi.Models;

namespace HackerNewsApi.Respositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly HttpClient _httpClient;

        public StoryRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Story>> GetTopStoriesAsync()
        {
            var storyIds = await _httpClient.GetFromJsonAsync<int[]>("https://hacker-news.firebaseio.com/v0/topstories.json");
            var stories = new List<Story>();

            foreach (var id in storyIds.Take(10))
            {
                var story = await GetStoryByIdAsync(id);
                if (story != null)
                    stories.Add(story);
            }
            return stories; 
        }

        public async Task<Story?> GetStoryByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Story>($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
        }
    }
}
