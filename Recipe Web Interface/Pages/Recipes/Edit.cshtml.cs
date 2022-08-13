using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Recipe_Web_Interface.Models;
namespace Recipe_Web_Interface.Pages.Recipes
{
    public class EditModel : PageModel
    {
		[TempData]
		public string? ActionResult { get; set; }
		[BindProperty]
		[Required]
		public Recipe Recipe { get; set; } = new();
		public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
		[BindProperty]
		public IEnumerable<string> SelectedCategories { get; set; } = Enumerable.Empty<string>();
		[BindProperty]
		public string Ingredients { get; set; } = string.Empty;
		[BindProperty]
		public string Instructions { get; set; } = string.Empty;
		private readonly IHttpClientFactory _httpClientFactory;

		public EditModel(IHttpClientFactory httpClientFactory) =>
				_httpClientFactory = httpClientFactory;

		public async Task<IActionResult> OnGetAsync(Guid recipeId)
		{
			var httpClient = _httpClientFactory.CreateClient("Api");
			string baseAddress = httpClient.BaseAddress.ToString();
			var categoriesResponse = await httpClient.GetFromJsonAsync<IEnumerable<string>>($"{baseAddress}category");
			if (categoriesResponse != null)
				Categories = categoriesResponse;
			var recipeResponse = await httpClient.GetFromJsonAsync<Recipe>($"{baseAddress}recipes/{recipeId}");
			if (recipeResponse != null)
				Recipe = recipeResponse; if (Recipe == null)
				return NotFound();

			Ingredients = String.Join(Environment.NewLine, Recipe.Ingredients);
			Instructions = String.Join(Environment.NewLine, Recipe.Instructions);
			return Page();
		}
		public async Task<IActionResult> OnPostAsync(Guid recipeId)
		{
			var httpClient = _httpClientFactory.CreateClient("Api");
			try
			{
				string baseAddress = httpClient.BaseAddress.ToString();
				var response = await httpClient.GetFromJsonAsync<IEnumerable<string>>($"{baseAddress}category", default);
				if (response != null)
					Categories = response;
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, please try again";
				return RedirectToPage("/Index");
			}

			Recipe.Id = recipeId;
			if (SelectedCategories != null)
				Recipe.Categories = (List<string>)SelectedCategories;
			if (Ingredients != null)
				Recipe.Ingredients = Ingredients.Split(Environment.NewLine).ToList();
			if (Instructions != null)
				Recipe.Instructions = Instructions.Split(Environment.NewLine).ToList();

			try
			{
				var response = await httpClient.PutAsJsonAsync($"recipes/{recipeId}", new Recipe {
					Categories = Recipe.Categories , 
					Id = Recipe.Id , 
					Ingredients = Recipe.Ingredients , 
					Instructions  = Recipe.Instructions , 
					Title = Recipe.Title}
				);
				response.EnsureSuccessStatusCode();
				ActionResult = "Successfully Edited";
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong please try again later";
			}
			return RedirectToPage("/Index");
		}
	
	}
}