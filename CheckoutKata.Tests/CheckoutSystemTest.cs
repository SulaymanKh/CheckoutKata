using System;
using System.Collections.Generic;
using Xunit;
using CheckoutKata;

namespace CheckoutKata.Tests
{
    public class CheckoutSystemTest
    {
        private Dictionary<string, (int UnitPrice, (int SpecialQuantity, int SpecialPrice))> pricingRules;

        public CheckoutSystemTest()
        {
            pricingRules = new Dictionary<string, (int UnitPrice, (int SpecialQuantity, int SpecialPrice))>
            {
                {"A", (50, (1, 50)) },
                {"B", (30, (2, 45)) },
                {"C", (20, (1, 20)) },
                {"D", (15, (1, 15)) }
            };
        }

        [Fact]
        public void Scan_ValidItem_AddsItemToList()
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
            Assert.Equal($"Invalid SKU {sku} Scanned!", error);
        }

        [Fact]
        public void Scan_ValidItem_AddsToTotalPriceWithSpecialOffer()
        {
            //Arrange
            pricingRules = new Dictionary<string, (int UnitPrice, (int SpecialQuantity, int SpecialPrice))>
            {
                {"A", (50, (1, 50)) },
                {"B", (30, (2, 45)) },
                {"C", (20, (1, 20)) },
                {"D", (15, (1, 15)) }
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
        public void Scan_ValidItem_AddsToTotalPriceWithMultipleSpecialOffers()
        {
            //Arrange
            pricingRules = new Dictionary<string, (int UnitPrice, (int SpecialQuantity, int SpecialPrice))>
            {
                {"A", (50, (3, 130)) },
                {"B", (30, (2, 45)) },
                {"C", (20, (1, 20)) },
                {"D", (15, (1, 15)) }
            };
            var checkout = new Checkout(pricingRules);

            //Act
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("A");
            checkout.Scan("A");
            int totalPrice = checkout.GetTotalPrice();

            //Assert
            Assert.Equal(175, totalPrice);
        }

        [Fact]
        public void Scan_ValidItem_AddsToTotalPriceWithMultipleSpecialOffersInDifferentOrder()
        {
            //Arrange
            pricingRules = new Dictionary<string, (int UnitPrice, (int SpecialQuantity, int SpecialPrice))>
            {
                {"A", (50, (3, 130)) },
                {"B", (30, (2, 45)) },
                {"C", (20, (1, 20)) },
                {"D", (15, (1, 15)) }
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
            Assert.Equal(175, totalPrice);
        }

        [Fact]
        public void Scan_ValidItem_AddsToTotalPriceWithNoOffer()
        {
            //Arrange
            var checkout = new Checkout(pricingRules);

            //Act
            checkout.Scan("A");
            checkout.Scan("B");
            checkout.Scan("C");
            checkout.Scan("D");
            int totalPrice = checkout.GetTotalPrice();

            //Assert
            Assert.Equal(115, totalPrice);
        }

        [Fact]
        public void Scan_ValidItem_AddsToTotalPriceWithMultipleSameProductsAndUseOffer()
        {
            //Arrange
            pricingRules = new Dictionary<string, (int UnitPrice, (int SpecialQuantity, int SpecialPrice))>
            {
                {"A", (50, (1, 50)) },
                {"B", (30, (2, 45)) },
                {"C", (20, (1, 20)) },
                {"D", (15, (1, 15)) }
            };
            var checkout = new Checkout(pricingRules);

            //Act
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("B");
            checkout.Scan("B");
            int totalPrice = checkout.GetTotalPrice();

            //Assert
            Assert.Equal(90, totalPrice);
        }

        [Fact]
        public void Scan_NoItem_AddsToTotalPrice()
        {
            //Arrange
            var checkout = new Checkout(pricingRules);

            //Act
            int totalPrice = checkout.GetTotalPrice();

            //Assert
            Assert.Equal(0, totalPrice);
        }
    }
}