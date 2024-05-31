using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebWakeOnLan.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string adresa;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string Adresa { get => adresa; set => adresa = value; }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string key)
        {
            return RedirectToPage("Jiná stránka");
        }
    }
}
