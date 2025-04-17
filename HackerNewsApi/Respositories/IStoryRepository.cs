using HackerNewsApi.Models;

namespace HackerNewsApi.Respositories
{
    public interface IStoryRepository
    {
        Task<IEnumerable<Story>> GetTopStoriesAsync();
        Task<Story?> GetStoryByIdAsync(int id);
        Task<IEnumerable<Story>> GetNewestStoriesAsync(int page, int pageSize, string searchTerm);
    }
}
