using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing.Printing;
using WebBlog.Data;
using WebBlog.Models;
using WebBlog.Services.Interfaces;
using X.PagedList;

namespace WebBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IBlogPostService _blogPostService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IBlogPostService blogPostService)
        {
            _logger = logger;
            _context = context;
            _blogPostService = blogPostService;
        }

        public async Task<IActionResult> Index(int? pageNum)
        {// Was (" int pageSize = 5; ")
            int pageSize = 3;
            int page = pageNum ?? 1;

            // Made this together with Jacob?!
            /* List<BlogPost> model = await _context.BlogPosts
												  .Include(b => b.Comments)
												  .Include(b => b.Category)
												  .OrderByDescending(b => b.DateCreated)
												  .ToListAsync(); */

            // Made this together with Antonio?! 11/8/2022
            IPagedList<BlogPost> model = (await _blogPostService.GetAllBlogPostsAsync())
                                            .Where(b => b.IsDeleted == false && b.IsPublished == true)
                                            .ToPagedList(page, pageSize);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}