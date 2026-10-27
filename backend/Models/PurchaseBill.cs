namespace Backend.Models;

public class PurchaseBill
{
    public int Id { get; set; }
    public DateTime Created_At { get; set; } = DateTime.UtcNow;
    public string Created_By { get; set; } = string.Empty;
    public int Total_Items { get; set; }
    public decimal Total_Qty { get; set; }
    public decimal Total_Cost { get; set; }
    public decimal Total_Selling { get; set; }
    public List<PurchaseBillItem> Items { get; set; } = new();
}
