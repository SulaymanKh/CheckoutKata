namespace CheckoutKata.Tests;

public class CheckoutSystemTest
{
    private Dictionary<string, (int, int)> pricingRules;

    public CheckoutSystemTest()
    {
        pricingRules = new Dictionary<string, (int, int)>
        {
            {"A", (1, 50) },
            {"B", (1, 30) },
            {"C", (1, 20) },
            {"D", (1, 15) }
        };
    }

    [Fact]
    public void Scan_ExistingItem_AddsItemToList()
    {
        //Arrange
        var checkout = new Checkout(pricingRules);
        string sku = "A";

        //Act
        checkout.Scan(sku);

        //Assert
        Assert.Contains(sku, checkout._scannedItems);
    }

    [Fact]
    public void Scan_NonExistingItem_ThrowsError()
    {
        //Arrange
        var checkout = new Checkout(pricingRules);
        string? error = null;
        string sku = "Z";

        //Act
        try
        {
            checkout.Scan(sku);
        }
        catch (Exception ex)
        {
            error = ex.Message;
        }

        //Assert
        Assert.NotNull(error);
        Assert.Equal($"Invalid Item {sku} Scanned!", error);
    }

    [Fact]
    public void Scan_ExistingItem_AddsToTotalPriceUsingSpecialOffer()
    {
        //Arrange
        pricingRules = new Dictionary<string, (int, int)>
        {
            {"A", (3, 150) },
            {"B", (2, 45) },
            {"C", (1, 20) },
            {"D", (1, 15) }
        };
        var checkout = new Checkout(pricingRules);

        //Act
        checkout.Scan("B");
        checkout.Scan("B");
        int totalPrice = checkout.GetTotalPrice();

        //Assert
        Assert.Equal(45, totalPrice);
    }

    [Fact]
    public void Scan_ExistingItem_AddsToTotalPriceWithMultipleSpecialOffers()
    {
        //Arrange
        pricingRules = new Dictionary<string, (int, int)>
        {
            {"A", (3, 150) },
            {"B", (2, 45) },
            {"C", (1, 20) },
            {"D", (1, 15) }
        };
        var checkout = new Checkout(pricingRules);

        //Act
        checkout.Scan("B");
        checkout.Scan("B");
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("A");
        int totalPrice = checkout.GetTotalPrice();

        //Assert
        Assert.Equal(195, totalPrice);
    }

    [Fact]
    public void Scan_ExistingItem_AddsToTotalPriceWithMultipleSpecialOfferInDifferentOrder()
    {
        //Arrange
        pricingRules = new Dictionary<string, (int, int)>
        {
            {"A", (3, 150) },
            {"B", (2, 45) },
            {"C", (1, 20) },
            {"D", (1, 15) }
        };
        var checkout = new Checkout(pricingRules);

        //Act
        checkout.Scan("A");
        checkout.Scan("B");
        checkout.Scan("A");
        checkout.Scan("A");
        checkout.Scan("B");
        int totalPrice = checkout.GetTotalPrice();

        //Assert
        Assert.Equal(195, totalPrice);
    }
}

interface ICheckout
{
    void Scan(string item);
    int GetTotalPrice();
}

public class Checkout : ICheckout
{
    private readonly Dictionary<string, (int, int)> _pricingRules;

    public readonly List<string> _scannedItems = new List<string>();

    public Checkout(Dictionary<string, (int, int)> pricingRules)
    {
        _pricingRules = pricingRules;
        _scannedItems = new List<string>();
    }

    public void Scan(string sku)
    {
        if (_pricingRules.ContainsKey(sku))
        {
            _scannedItems.Add(sku);
        }
        else
        {
            throw new Exception($"Invalid Item {sku} Scanned!");
        }
    }

    public int GetTotalPrice()
    {
        int totalPrice = 0;

        foreach (var item in _pricingRules.Keys)
        {
            var quantity = _pricingRules[item].Item1;
            var price = _pricingRules[item].Item2;

            var itemCount = _scannedItems.Count(x => x == item);
            var specialOffersUsed = itemCount / quantity;
            var remainingItems = itemCount % quantity;

            totalPrice += specialOffersUsed * price + remainingItems * price;
        }

        return totalPrice;
    }
}