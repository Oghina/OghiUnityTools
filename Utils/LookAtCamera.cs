using UnityEngine;

namespace OghiUnityTools.Utils
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;

            if (!mainCamera)
            {
                Debug.LogWarning($"No MAIN Camera found, can not rotate toward Camera");
            }
        }

        private void LateUpdate()
        {
            if (!mainCamera) return;
            
            transform.LookAt(mainCamera.transform.position);
        }
    }
}