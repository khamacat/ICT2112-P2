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
        detail.UpdateName(name);
        detail.UpdateDescription(description);
        detail.UpdateTotalQuantity(totalQuantity);
        detail.UpdateWeight(weight);
        detail.UpdateImage(image);
        detail.UpdatePrice(price);
        detail.UpdateDepositRate(depositRate);
        return detail;
    }

    public int GetDetailsId() => _detailsid;
    public int GetProductId() => _productid;
    public string GetName() => _name;
    public string? GetDescription() => _description;
    public int GetTotalQuantity() => _totalquantity;
    public decimal? GetWeight() => _weight;
    public string? GetImage() => _image;
    public decimal GetPrice() => _price;
    public decimal? GetDepositRate() => _depositrate;

    public void AssignDetailsId(int detailsId) => SetDetailsId(detailsId);
    public void AssignProductId(int productId) => SetProductId(productId);
    public void UpdateName(string name) => SetName(name);
    public void UpdateDescription(string? description) => SetDescription(description);
    public void UpdateTotalQuantity(int totalQuantity) => SetTotalQuantity(totalQuantity);
    public void UpdateWeight(decimal? weight) => SetWeight(weight);
    public void UpdateImage(string? image) => SetImage(image);
    public void UpdatePrice(decimal price) => SetPrice(price);
    public void UpdateDepositRate(decimal? depositRate) => SetDepositRate(depositRate);

    private void SetDetailsId(int detailsId) => _detailsid = detailsId;
    private void SetProductId(int productId) => _productid = productId;
    private void SetName(string name) => _name = name;
    private void SetDescription(string? description) => _description = description;
    private void SetTotalQuantity(int totalQuantity) => _totalquantity = totalQuantity;
    private void SetWeight(decimal? weight) => _weight = weight;
    private void SetImage(string? image) => _image = image;
    private void SetPrice(decimal price) => _price = price;
    private void SetDepositRate(decimal? depositRate) => _depositrate = depositRate;
}