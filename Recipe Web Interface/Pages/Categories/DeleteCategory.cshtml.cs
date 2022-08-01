using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Web_Interface.Pages.Categories
{
    public class DeleteCategoryModel : PageModel
    {
		[TempData]
		public string? ActionResult { get; set; }
		[FromRoute(Name = "category")]
		public string Category { get; set; } = String.Empty;
		private readonly IHttpClientFactory _httpClientFactory;

		public DeleteCategoryModel(IHttpClientFactory httpClientFactory) =>
				_httpClientFactory = httpClientFactory;

		public void OnGet()
		{
		}
		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				var httpClient = _httpClientFactory.CreateClient("Api");
				var response = await httpClient.DeleteAsync("category?category=" + Category);
				response.EnsureSuccessStatusCode();
				ActionResult = "Deleted successfully";
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, Try again later";
			}
			return RedirectToPage("./ListCategories");
		}

	}

}

