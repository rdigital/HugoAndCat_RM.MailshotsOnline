using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PplItem = PayPal.Api.Item;

namespace HC.RM.Common.PayPal.Models
{
    public class PurchaseItem
    {
        /// <summary>
        /// Creates a new Purchase Item
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="quantity">Quantity of the item</param>
        /// <param name="currency">Three-letter currency code</param>
        /// <param name="price">Price for the item</param>
        /// <param name="tax">Tax on the item</param>
        public PurchaseItem(string name, int quantity, string currency, decimal price, decimal tax)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.Currency = currency;
            this.Price = price;
            this.Tax = tax;
        }

        /// <summary>
        /// Creates a new Purchase item from the PayPal Item object
        /// </summary>
        /// <param name="item">PayPal Item object</param>
        internal PurchaseItem(PplItem item)
        {
            this.Currency = item.currency;
            this.Description = item.description;
            this.Name = item.name;
            this.Price = decimal.Parse(item.price);
            this.Quantity = int.Parse(item.quantity);
            this.StockCode = item.sku;
            this.Tax = decimal.Parse(item.tax);
            this.Url = item.url;
        }

        /// <summary>
        /// Three-letter currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Description of the item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Price of the item per unit
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity of the items
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Internal stock code (Stock Keeping Unit)
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// Tax on the item
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// URL for the user to see the item (optional)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Creates a PayPal Item object
        /// </summary>
        /// <returns>PayPal Item object</returns>
        internal PplItem ToPaypalItem()
        {
            var result = new PplItem();
            result.currency = this.Currency;
            result.description = this.Description;
            result.name = this.Name;
            result.price = this.Price.ToString("F");
            result.quantity = this.Quantity.ToString();
            result.sku = this.StockCode;
            result.tax = this.Tax.ToString("F");
            result.url = this.Url;

            return result;
        }
    }
}
