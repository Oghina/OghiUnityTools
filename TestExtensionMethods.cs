using OghiUnityTools.EventBus;
using OghiUnityTools.VR.UI.Scripts;
using UnityEngine;

namespace OghiUnityTools
{
    public class TestExtensionMethods : MonoBehaviour
    {
        [SerializeField] private Sprite item1;
        [SerializeField] private Sprite item2;
        [SerializeField] private Sprite item3;
        
        private void Start()
        {
            EventBus<AddProductToCatalogRequest>.Raise(new AddProductToCatalogRequest("item1", item1));
            EventBus<AddProductToCatalogRequest>.Raise(new AddProductToCatalogRequest("item2", item2));
            EventBus<AddProductToCatalogRequest>.Raise(new AddProductToCatalogRequest("item3", item3));
        }

        
        // Show dialog example
        //
    /*    private void ShowDialog()
        {
            EventBus<DialogRequest>.Raise(new DialogRequest(
                "Info", "Asta e info.", DialogType.Info,
                () => Debug.Log("Info button clicked")
                ));
        } */
    }
}