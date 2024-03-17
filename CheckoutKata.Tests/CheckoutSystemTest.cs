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

