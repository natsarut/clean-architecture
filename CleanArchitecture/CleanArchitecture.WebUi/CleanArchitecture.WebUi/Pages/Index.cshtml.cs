using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CleanArchitecture.WebUi.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string Title { get; set; } = "Page-focused web UI with Razor Pages";

        public IndexModel(ILogger<IndexModel> logger) => _logger = logger;

        public void OnGet()
        {

        }
    }
}
