using System;
using BNG;
using OghiUnityTools.VR.VR_Utils;
using UnityEngine;

namespace OghiUnityTools.VR.SnapzoneTighteningWithToolMechanism
{
    /// <summary>
    /// Manages a VR-based tool tightening mechanism that uses a combination of a tool snapzone, 
    /// a lever, and a grabbable hinge to simulate tightening and loosening operations.
    /// </summary>
    /// <remarks>
    /// This class coordinates the following mechanics:
    /// - Tool installation/uninstallation through a snapzone
    /// - Lever-based tightening/loosening operations
    /// - State management for tool and tightening status
    /// - Event notifications for state changes
    /// 
    /// Usage:
    /// 1. Assign required references in the Unity Inspector (toolSnapzoneToCheck, lever, hingeGrabbable)
    /// 2. The system will automatically handle tool installation/removal and tightening/loosening operations
    /// 3. Subscribe to OnTighteningStatusModified and OnWrenchStatusModified events to react to state changes
    /// </remarks>
    public class TightenWithGrabbableTool : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SnapZone toolSnapzoneToCheck;
        [SerializeField] private Lever lever;
        [SerializeField] private Grabbable hingeGrabbable;
   
        public event Action<TightenType> OnTighteningStatusModified; 
        public event Action<ToolInstallation> OnWrenchStatusModified;

        private TightenType tighteningType = TightenType.Loose;
        private ToolInstallation toolInstallation = ToolInstallation.Uninstalled;
        
        public SnapZone ToolSnapzoneToCheck => toolSnapzoneToCheck;
        
        public TightenType TighteningType => tighteningType;
        
        public ToolInstallation ToolInstallation => toolInstallation;
        
        private void Awake()
        {
            if (!toolSnapzoneToCheck) 
                Debug.LogError($"{nameof(toolSnapzoneToCheck)} not assigned", this);
            if (!lever) 
                Debug.LogError($"{nameof(lever)} not assigned", this);
            if (!hingeGrabbable) 
                Debug.LogError($"{nameof(hingeGrabbable)} not assigned", this);
            
            toolSnapzoneToCheck.OnSnapEvent.AddListener(InstallTool);
            toolSnapzoneToCheck.OnDetachEvent.AddListener(UninstallTool);
            
            lever.onLeverDown.AddListener(Loosen);
            lever.onLeverUp.AddListener(Tighten);
            
            hingeGrabbable.CanBeSnappedToSnapZone = false;
            hingeGrabbable.LockGrabbable();
        }
        
        private void OnDestroy()
        {
            toolSnapzoneToCheck.OnSnapEvent.RemoveListener(InstallTool);
            toolSnapzoneToCheck.OnDetachEvent.RemoveListener(UninstallTool);
            
            lever.onLeverDown.RemoveListener(Loosen);
            lever.onLeverUp.RemoveListener(Tighten);
        }

        private void InstallTool(Grabbable grabbable)
        {
            toolInstallation = ToolInstallation.Installed;

            LockSnapzoneAndActivateHinge();
            
            OnWrenchStatusModified?.Invoke(toolInstallation);
        }

        private void UninstallTool(Grabbable grabbable)
        {
            toolInstallation = ToolInstallation.Uninstalled;
            
            OnWrenchStatusModified?.Invoke(toolInstallation);
        }

        private void Tighten()
        {
            if (toolInstallation != ToolInstallation.Installed)
            {
                Debug.LogWarning("Cannot tighten without an installed wrench", this);
                return;
            }
            
            tighteningType = TightenType.Tight;

            UnlockSnapzoneAndDeactivateHinge();
            
            OnTighteningStatusModified?.Invoke(tighteningType); 
        }

        private void Loosen()
        {
            tighteningType = TightenType.Loose;

            UnlockSnapzoneAndDeactivateHinge();
            
            OnTighteningStatusModified?.Invoke(tighteningType); 
        }

        private void LockSnapzoneAndActivateHinge()
        {
            // Lock the snapzone 
            toolSnapzoneToCheck.LockSnapzone();
            
            // Unlock the hinge so the user can grab and rotate it
            hingeGrabbable.UnlockGrabbable();
        }

        private void UnlockSnapzoneAndDeactivateHinge()
        {
            // Lock the hinge
            hingeGrabbable.LockGrabbable();

            // Unlock the snapzone 
            toolSnapzoneToCheck.UnlockSnapzone();
        }
    }
}