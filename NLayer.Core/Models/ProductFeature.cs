﻿namespace NLayer.Core.Models
{
    //baseEntityden miras almasına gerek yok product ile arasında bire bir ilişki olacağı için o alanlara zaten sahip olacak.
    public class ProductFeature
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        //navigation property. bire bir ilişki
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}