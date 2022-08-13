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

		public async Task<IActionResult> OnGet(Guid recipeId)
		{
			try
			{
				var httpClient = _httpClientFactory.CreateClient("Api");
				var response = await httpClient.GetFromJsonAsync<Recipe>($"{httpClient.BaseAddress.ToString()}recipes/{recipeId}");
				if (response == null)
					return NotFound();
				Recipe = response;
				return Page();
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong please try again later";
				return RedirectToPage("/Index");
			}
		}

		public async Task<IActionResult> OnPostAsync(Guid recipeId)
		{
			try
			{
				var httpClient = _httpClientFactory.CreateClient("Api");
				var response = await httpClient.DeleteAsync($"recipes/{recipeId}");
				//var response = await httpClient.DeleteAsync("recipes?recipeId=" + recipeId);
				response.EnsureSuccessStatusCode();
				ActionResult = "Successfully Deleted";
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong please try again later";
			}
			return RedirectToPage("/Index");
		}
		
	}
}


