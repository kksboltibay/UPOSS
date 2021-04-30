﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UPOSS.Models
{
    public class RootProductObject
    {
        public string Status { get; set; }

        public string Msg { get; set; }

        public List<Product> Data { get; set; }

        public int Total { get; set; }
    }

    public class Product : ObservableObject
    {
        public int Id { get; set; }

        public string Product_no { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Design_code { get; set; }
        
        public string Colour_code { get; set; }

        public string Price { get; set; }

        public string Barcode { get; set; }

        public string Total_stock { get; set; }

        public string Is_active { get; set; }

        public List<ProductQuantity> Stock { get; set; }
    }

    public class ProductQuantity : ObservableObject
    {
        public string Branch_name { get; set; }

        public string Quantity { get; set; }
    }
}
