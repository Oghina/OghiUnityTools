using System.Collections.Generic;
using OghiUnityTools.EventBus;
using OghiUnityTools.ExtensionMethods;
using UnityEngine;

namespace OghiUnityTools.VR.UI.Scripts
{
    public class CatalogManager : MonoBehaviour
    {
        [SerializeField] private CatalogItem itemPrefab;
        [SerializeField] private Transform contentParent;

        [SerializeField] private Sprite defaultCatalogIcon;
        
        private EventBinding<AddProductToCatalogRequest> addProductToCatalogRequestBinding;

        private readonly Catalog catalog = new();
        
        private void Awake()
        {
            addProductToCatalogRequestBinding = new EventBinding<AddProductToCatalogRequest>(OnAddProductToCatalogRequest);
            EventBus<AddProductToCatalogRequest>.Register(addProductToCatalogRequestBinding);
        }

        private void OnDestroy() => EventBus<AddProductToCatalogRequest>.Deregister(addProductToCatalogRequestBinding);

        private void OnAddProductToCatalogRequest(AddProductToCatalogRequest obj)
        {
            var image = obj.ItemPicture ? obj.ItemPicture : defaultCatalogIcon;
            catalog.AddProduct(obj.ItemName, image);
            UpdateView(catalog.Products);
        }

        private void UpdateView(Dictionary<string, Sprite> catalogDictionary)
        {
            // First delete all the items in the list
            contentParent.DestroyChildren();
            
            catalogDictionary.ForEach(item =>
            {
                var catalogItem = Instantiate(itemPrefab, contentParent);
                catalogItem.Initialize(item.Key, item.Value);
            });
        }
    }
}