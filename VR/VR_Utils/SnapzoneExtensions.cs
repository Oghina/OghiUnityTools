using BNG;
using UnityEngine;

namespace OghiUnityTools.VR.VR_Utils
{
    public static class SnapzoneExtensions
    {
        // Lock the snapzone 
        public static void LockSnapzone(this SnapZone snapZone)
        {
            snapZone.CanRemoveItem = false;
            snapZone.enabled = false; 
            
            snapZone.GetComponent<SphereCollider>().enabled = false; 
        }
  
        // Unlock the snapzone 
        public static void UnlockSnapzone(this SnapZone snapZone)
        {
            snapZone.CanRemoveItem = true;
            snapZone.enabled = true; 
            
            snapZone.GetComponent<SphereCollider>().enabled = true;
        }
    }
}