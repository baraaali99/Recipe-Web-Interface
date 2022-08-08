using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipe_Web_Interface.Models;

namespace Recipe_Web_Interface.Pages.Recipes
{
    public class ListRecipesModel : PageModel
    {
		[TempData]
		public string? ActionResult { get; set; }
		private readonly IHttpClientFactory _httpClientFactory;
		public ListRecipesModel(IHttpClientFactory httpClientFactory) =>
			_httpClientFactory = httpClientFactory;

		public List<Recipe> RecipeList { get; set; } = new();
		public async Task<IActionResult> OnGet()
        {
			try
			{
				HttpClient httpClient = _httpClientFactory.CreateClient("Api");
				string baseAddress = httpClient.BaseAddress.ToString();
				var response = await httpClient.GetFromJsonAsync<List<Recipe>>($"{baseAddress}recipes", default);
				if (response != null)
					RecipeList = response;
				return Page();
			}
            catch (Exception)
            {
				ActionResult = "something went wrong";
            }
			return RedirectToPage("/Index");

		}
	}
}
