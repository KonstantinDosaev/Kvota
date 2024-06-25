﻿using Kvota.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Kvota.Models.Products
{
    public class ProductsInStorage
    {
        [ForeignKey(nameof(Product))] public Guid? ProductId { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }
        [ForeignKey(nameof(Storage))] public Guid? StorageId { get; set; }
        public virtual Storage? Storage { get; set; }
        public int Quantity { get; set; }

    }
}
