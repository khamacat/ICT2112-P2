namespace ProRental.Domain.Entities;

public partial class Productdetail
{
    public static Productdetail Create(
        string name,
        string? description,
        int totalQuantity,
        decimal? weight,
        string? image,
        decimal price,
        decimal? depositRate)
    {
        var detail = new Productdetail();
        detail.Name = name;
        detail.Description = description;
        detail.Totalquantity = totalQuantity;
        detail.Weight = weight;
        detail.Image = image;
        detail.Price = price;
        detail.Depositrate = depositRate;
        return detail;
    }

    public int GetDetailsId() => Detailsid;
    public int GetProductId() => Productid;
    public int GetTotalQuantity() => Totalquantity;
    public string GetName() => Name;
    public string? GetDescription() => Description;
    public decimal? GetWeight() => Weight;
    public string? GetImage() => Image;
    public decimal GetPrice() => Price;
    public decimal? GetDepositRate() => Depositrate;

    public void SetDetailsId(int detailsId) => Detailsid = detailsId;
    public void SetProductId(int productId) => Productid = productId;
    public void SetTotalQuantity(int totalQuantity) => Totalquantity = totalQuantity;
    public void SetName(string name) => Name = name;
    public void SetDescription(string? description) => Description = description;
    public void SetWeight(decimal? weight) => Weight = weight;
    public void SetImage(string? image) => Image = image;
    public void SetPrice(decimal price) => Price = price;
    public void SetDepositRate(decimal? depositRate) => Depositrate = depositRate;
}