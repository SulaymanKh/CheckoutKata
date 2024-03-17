using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckoutKata
{
    interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }

    public class Checkout : ICheckout
    {
        private readonly Dictionary<string, (int UnitPrice, (int SpecialQuantity, int SpecialPrice))> _pricingRules;
        public readonly List<string> _scannedItems = new List<string>();

        public Checkout(Dictionary<string, (int UnitPrice, (int SpecialQuantity, int SpecialPrice))> pricingRules)
        {
            _pricingRules = pricingRules;
        }

        public void Scan(string sku)
        {
            if (_pricingRules.ContainsKey(sku))
            {
                _scannedItems.Add(sku);
            }
            else
            {
                throw new Exception($"Invalid SKU {sku} Scanned!");
            }
        }

        public int GetTotalPrice()
        {
            int totalPrice = 0;

            foreach (var sku in _scannedItems.Distinct())
            {
                var unitPrice = _pricingRules[sku].UnitPrice;
                var specialQuantity = _pricingRules[sku].Item2.SpecialQuantity;
                var specialPrice = _pricingRules[sku].Item2.SpecialPrice;

                var itemCount = _scannedItems.Count(x => x == sku);
                var specialOffersUsed = itemCount / specialQuantity;
                var remainingItems = itemCount % specialQuantity;

                totalPrice += specialOffersUsed * specialPrice;

                totalPrice += remainingItems * unitPrice;
            }

            return totalPrice;
        }
    }
}