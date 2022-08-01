using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Recipe_Web_Interface.Models;
namespace Recipe_Web_Interface.Pages
{


    public class CreateRecipeModel : PageModel
    {

		[TempData]
		public string? ActionResult { get; set; }
        [BindProperty,Required]
		public string Title { get; set; }
		[BindProperty,Required]
		public Recipe recipe { get; set; } = new();
		public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();
		[BindProperty]
		public IEnumerable<string> SelectedCategories { get; set; } = Enumerable.Empty<string>();
		[BindProperty]
		public string? Ingredients { get; set; } = string.Empty;
		[BindProperty]
		public string? Instructions { get; set; } = string.Empty;

		private readonly IHttpClientFactory _httpClientFactory;
		public CreateRecipeModel(IHttpClientFactory httpClientFactory) =>
				_httpClientFactory = httpClientFactory;
		public async Task<IActionResult> OnGetAsync()
		{
			try
			{
				var httpClient = _httpClientFactory.CreateClient("Api");
				string baseAddress = httpClient.BaseAddress.ToString();
				var response = await httpClient.GetFromJsonAsync<IEnumerable<string>>($"{baseAddress}category" , default);
				if (response != null)
					Categories = response;
				return Page();
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, please try again";
				return RedirectToPage("/Index");
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var httpClient = _httpClientFactory.CreateClient("Api");
			try
			{
				string baseAddress = httpClient.BaseAddress.ToString();
				var response = await httpClient.GetFromJsonAsync<IEnumerable<string>>($"{baseAddress}category" , default);
				if (response != null)
					SelectedCategories = response;
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, please try again";
				return RedirectToPage("/Index");
			}

			if (!ModelState.IsValid)
				return Page();

			if(Title != null)
				recipe.Title = Title;
			recipe.Id = Guid.Empty;

			if (Ingredients != null)
				recipe.Ingredients = Ingredients.Split(Environment.NewLine).ToList();

			if (Instructions != null)
				recipe.Instructions = Instructions.Split(Environment.NewLine).ToList();

			if (SelectedCategories != null)
				recipe.Categories = (List<string>)SelectedCategories;

		


			try
			{
				string baseAddress = httpClient.BaseAddress.ToString();
				var response = await httpClient.PostAsJsonAsync($"{baseAddress}recipes",
					  new Recipe {
						  Id = recipe.Id, 
						  Title = recipe.Title,
						  Ingredients = recipe.Ingredients, 
						  Instructions = recipe.Instructions, 
						  Categories = recipe.Categories
					  }
					 , new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				response.EnsureSuccessStatusCode();
				ActionResult = "Created successfully";
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, please try again";
			}
			return RedirectToPage("./ListRecipes");
		}
	}
}
