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

public class IngredientRow
{
    public int IngredientID { get; set; }
    public decimal Quantity { get; set; }
    public string? UnitOverride { get; set; }
}