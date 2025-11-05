using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreInventoryApp
{
    enum Category
    {
        Food = 1,
        Electronics,
        Clothes
    }

    class Product
    {
        private static int nextCode = 1; // Уникальный код будет увеличиваться автоматически

        public int Code { get; private set; }
        public string Name { get; private set; }
        public double Price { get; private set; }
        public int Quantity { get; private set; }
        public bool InStock => Quantity > 0;
        public Category ProductCategory { get; private set; }