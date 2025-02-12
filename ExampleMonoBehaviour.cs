using OghiUnityTools.EventBus;
using OghiUnityTools.VR.UI.Scripts;
using UnityEngine;

namespace OghiUnityTools
{
    public class ExampleMonoBehaviour : MonoBehaviour
    {
        private void Start()
        {
           ShowDialogs();
        }
        
        private void ShowDialogs()
        {
            EventBus<DialogRequest>.Raise(new DialogRequest(
                "Info", "This is a info dialog with one button.", DialogType.Info,
                () => Debug.Log("Info button clicked")
                ));
            
            EventBus<DialogRequest>.Raise(new DialogRequest(
                "Yes no dialog", "This is a Yes or No dialog with two buttons.", DialogType.YesNo,
                () => Debug.Log("Yes button clicked"),
                () => Debug.Log("No button clicked")
            ));
            
            EventBus<DialogRequest>.Raise(new DialogRequest(
                "Yes no cancel dialog", "This is a Yes, No or Cancel dialog with three buttons.", DialogType.YesNoCancel,
                () => Debug.Log("Yes button clicked"),
                () => Debug.Log("No button clicked"),
                () => Debug.Log("Cancel button clicked")
            ));
            
            EventBus<DialogRequest>.Raise(new DialogRequest(
                "Info", "This is a info dialog with one button.", DialogType.Info,
                () => Debug.Log("Info button clicked")
            ));
        } 
    }
}