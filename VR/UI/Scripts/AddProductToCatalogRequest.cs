using OghiUnityTools.EventBus;
using UnityEngine;

namespace OghiUnityTools.VR.UI.Scripts
{
    public struct AddProductToCatalogRequest : IEvent
    {
        public readonly string ItemName;
        public readonly Sprite ItemPicture;
        
        public AddProductToCatalogRequest(string itemName, Sprite itemPicture = null)
        {
            ItemName = itemName;
            ItemPicture = itemPicture;
        }
    }
}