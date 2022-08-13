﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Recipe_Web_Interface.Models
{
    public class Recipe
    {
		public Guid Id { get; set; } = new Guid();
		[Required]
		public string Title { get; set; }
		public List<string> Ingredients { get; set; } = new();
		public List<string> Instructions { get; set; } = new();
		public List<string> Categories { get; set; } = new();
	
	}
}
