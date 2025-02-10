using System;
using OghiUnityTools.EventBus;

namespace OghiUnityTools.VR.UI.Scripts
{
    public struct DialogRequest : IEvent
    {
        public DialogRequest(string header, string subHeader,DialogType dialogType, Action onYes = null, Action onNo = null, Action onCancel = null)
        {
            Header = header;
            SubHeader = subHeader;
            DialogType = dialogType;
            OnYes = onYes;
            OnNo = onNo;
            OnCancel = onCancel;
        }
        
        public string Header { get; }
        public string SubHeader { get; }
        public DialogType DialogType { get; }
        public Action OnYes { get; }
        public Action OnNo { get; }
        public Action OnCancel { get; }

    }
}