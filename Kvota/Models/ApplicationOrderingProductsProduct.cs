﻿using Kvota.Models.Products;

namespace Kvota.Models
{
    public class ApplicationOrderingProductsProduct
    {
        public int Quantity { get; set; }

        public Guid ApplicationOrderingId { get; set; }
        public virtual ApplicationOrderingProducts? ApplicationOrdering { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
