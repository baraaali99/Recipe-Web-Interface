using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Recipe_Web_Interface.Models;
namespace Recipe_Web_Interface.Pages.Recipes
{
    public class DeleteModel : PageModel
    {

		[TempData]
		public string? ActionResult { get; set; }
		public Guid RecipeId { get; set; } = Guid.Empty;
		public Recipe Recipe { get; set; } = new();
		private readonly IHttpClientFactory _httpClientFactory;

		public DeleteModel(IHttpClientFactory httpClientFactory) =>
				_httpClientFactory = httpClientFactory;

		public void OnGet()
		{
			/*try
			{
				var httpClient = _httpClientFactory.CreateClient("Api");
				string baseAddress = httpClient.BaseAddress.ToString();
				var response = await httpClient.GetFromJsonAsync<Recipe>($"{baseAddress}recipes");
				if (response == null)
					return NotFound();
				Recipe = response;
				return Page();
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, please try again";
				return RedirectToPage("./ListRecipes");
			}*/
		}

		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				var httpClient = _httpClientFactory.CreateClient("Api");
				string baseAddress = httpClient.BaseAddress.ToString();
				var response = await httpClient.DeleteAsync("recipes?id=" + RecipeId);
				response.EnsureSuccessStatusCode();
				ActionResult = "Successfully Deleted";
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, please try again";
			}
			return RedirectToPage("./ListRecipes");
		}
	}
}
