using Backend.Data;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Services;

public class PurchaseBillService(AppDbContext db)
{
    public async Task<PurchaseBill> CreateAsync(PurchaseBillRequest request, string user)
    {
        var items = request.Items.Select(i => new PurchaseBillItem
        {
            Item = i.Item,
            Batch = i.Batch,
            Standard_Cost = i.StandardCost,
            Standard_Price = i.StandardPrice,
            Margin = i.StandardPrice - i.StandardCost,
            Qty = i.Qty,
            Free_Qty = i.FreeQty,
            Discount = i.Discount,
            Total_Cost = i.StandardCost * i.Qty * (1 - i.Discount / 100),
            Total_Selling = i.StandardPrice * i.Qty
        }).ToList();

        var bill = new PurchaseBill
        {
            Created_By = user,
            Items = items,
            Total_Items = items.Count,
            Total_Qty = items.Sum(i => i.Qty),
            Total_Cost = items.Sum(i => i.Total_Cost),
            Total_Selling = items.Sum(i => i.Total_Selling)
        };

        db.PurchaseBills.Add(bill);
        await db.SaveChangesAsync();
        return bill;
    }
}
