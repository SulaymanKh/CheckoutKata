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
}

interface ICheckout
{
    void Scan(string item);
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
}