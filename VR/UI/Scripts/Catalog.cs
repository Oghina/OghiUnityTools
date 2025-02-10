using System.Collections.Generic;
using UnityEngine;

namespace OghiUnityTools.VR.UI.Scripts
{
    public class Catalog
    {
        private readonly Dictionary<string, Sprite> products = new();

        public Dictionary<string, Sprite> Products => products;
        
        public void AddProduct(string productName, Sprite product)
        {
            if (products.TryGetValue(productName, out var productSprite))
            {
                Debug.LogWarning("Trying to add a product which has already been added to your catalog.");
                return;
            }
            
            products.Add(productName, product);
        }

        public void RemoveProduct(string productName)
        {
            // Check if the product is there
            if (!products.Remove(productName, out var product))
            {
                Debug.LogWarning("Trying to remove a non-existing product from catalog");
            }
        }
    }
}