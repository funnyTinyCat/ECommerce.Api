using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Api.DTOs;


public class CreateProductDto
{

    public string Name {get; set;} = string.Empty;

    public string Description {get; set;} = string.Empty;

    [Column(TypeName="decimal(18,2)")]
    public decimal Price {get; set;} 

    public int Stock {get; set;}
}