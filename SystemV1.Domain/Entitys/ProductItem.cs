﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Entitys
{
    public class ProductItem
    {
        public Product Product { get; set; }
        public string Modelo { get; set; }
        public  decimal Value { get; set; }
        public bool IsSold { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageZip { get; set; }
    }
}
