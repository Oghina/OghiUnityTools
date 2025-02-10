using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OghiUnityTools.VR.UI.Scripts
{
    public class CatalogItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text itemNameOrDescription;
        [SerializeField] private Image image;

        public void Initialize(string itemName, Sprite itemSprite)
        {
            itemNameOrDescription.text = itemName;
            image.sprite = itemSprite;
        }
    }
}