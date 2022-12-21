using WebBlog.Models;

namespace WebBlog.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task AddTagsToBlogPostAsync(string tagNames, int blogPostId);

        public Task AddTagsToBlogPostAsync(IEnumerable<int> tagIds, int blogPostId);

        public Task<List<BlogPost>> GetAllBlogPostsAsync();

        public Task<List<BlogPost>> GetPopularBlogPostsAsync();

        public Task<List<BlogPost>> GetRecentBlogPostsAsync(int count);

        public Task<List<Category>> GetCategoriesAsync();

        public Task<List<Tag>> GetTagsAsync();

        public Task<List<Tag>> GetBlogPostTags(int blogPostId);

        public Task RemoveAllBlogPostTagsAsync(int blogPostId);

        public IEnumerable<BlogPost> SearchBlogPosts(string searchString);

        public Task<bool> ValidateSlugAsync(string title, int blogPostId);
    }
}
