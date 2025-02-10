using BNG;
using UnityEngine;
using System.Collections;

namespace OghiUnityTools.VR.VR_Utils
{
    public class HandsModelChanger : MonoBehaviour
    {
        public int handModelId = 6;

        public HandModelSelector hms;

        void Start() {
            if(hms == null) {
                hms = FindFirstObjectByType<HandModelSelector>();
            }

            if(hms == null) {
                Debug.Log("No Hand Model Selector Found in Scene. Will not be able to switch hand models");
            }

            StartCoroutine(ChangeHandsModel());
        }

        private IEnumerator ChangeHandsModel()
        {
            yield return new WaitForSeconds(1f);
            hms.ChangeHandsModel(handModelId, false);
            StopAllCoroutines();
        } 
    }
}