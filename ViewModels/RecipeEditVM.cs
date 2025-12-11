using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NalasTable.Data;
using NalasTable.Models;

namespace Final_Project_Nala_s_Table.Pages_Recipes;

public class RecipeEditVM
{
    public Recipe Recipe { get; set; } = new Recipe();
    public List<IngredientRow> Rows { get; set; } = new();
    public SelectList IngredientOptions { get; set; } = default!;
    public SelectList CategoryOptions { get; set; } = default!;
}