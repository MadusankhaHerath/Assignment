using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos;

public class PurchaseBillItemDto
{
    [Required]
    public string Item { get; set; } = string.Empty;

    [Required]
    public string Batch { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal StandardCost { get; set; }

    [Range(0, double.MaxValue)]
    public decimal StandardPrice { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Qty { get; set; }

    [Range(0, double.MaxValue)]
    public decimal FreeQty { get; set; }

    [Range(0, 100)]
    public decimal Discount { get; set; }
}

public class PurchaseBillRequest
{
    [MinLength(1, ErrorMessage = "At least one item is required.")]
    public List<PurchaseBillItemDto> Items { get; set; } = new();
}
