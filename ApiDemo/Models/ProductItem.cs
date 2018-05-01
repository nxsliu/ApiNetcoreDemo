using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDemo.Models
{
    public class ProductItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Balance { get; set; }
    }
}
