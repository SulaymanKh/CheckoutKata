namespace CheckoutKata
{
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
}
