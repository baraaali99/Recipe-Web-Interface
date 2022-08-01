using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipe_Web_Interface.Model;
namespace Recipe_Web_Interface.Pages.Categories
{
    public class ListCategoriesModel : PageModel
    {
        public string? ActionResult { get; set; }
        private readonly IHttpClientFactory? _httpClientFactory;
        public ListCategoriesModel(IHttpClientFactory? httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<string> CategoryList { get; set; } = new List<string>();
        public async Task<IActionResult> OnGet()
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("Api");
                string baseAddress = httpClient.BaseAddress.ToString();
                var response = await httpClient.GetFromJsonAsync<List<string>>($"{baseAddress}category", default);
                if (response != null)
                {
                    CategoryList = response;
                }
                return Page();
            }
            catch (Exception)
            {
                ActionResult = "something went wrong";
                return RedirectToPage("/Index");
            }
        }
    }
}
