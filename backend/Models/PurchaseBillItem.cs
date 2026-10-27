namespace Backend.Models;

public class PurchaseBillItem
{
    public int Id { get; set; }
    public int Purchase_Bill_Id { get; set; }
    public string Item { get; set; } = string.Empty;
    public string Batch { get; set; } = string.Empty;
    public decimal Standard_Cost { get; set; }
    public decimal Standard_Price { get; set; }
    public decimal Margin { get; set; }
    public decimal Qty { get; set; }
    public decimal Free_Qty { get; set; }
    public decimal Discount { get; set; }
    public decimal Total_Cost { get; set; }
    public decimal Total_Selling { get; set; }
}
