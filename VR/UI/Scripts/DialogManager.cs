using System.Collections.Generic;
using OghiUnityTools.EventBus;
using UnityEngine;

namespace OghiUnityTools.VR.UI.Scripts
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private Dialog dialog;
        
        private readonly Queue<DialogRequest> dialogQueue = new();
        
        private bool isDialogShowing;

        private EventBinding<DialogRequest> dialogRequestBinding;
        
        private void Awake()
        {
            dialog.gameObject.SetActive(false);

            dialogRequestBinding = new EventBinding<DialogRequest>(ShowDialog);
            
            EventBus<DialogRequest>.Register(dialogRequestBinding);
        }

        private void OnDestroy()
        {
            EventBus<DialogRequest>.Deregister(dialogRequestBinding);
        }

        private void ShowDialog(DialogRequest request)
        {
            dialogQueue.Enqueue(request);
            if (!isDialogShowing)
            {
                ShowNextDialog();
            }
        }

        private void ShowNextDialog()
        {
            if (dialogQueue.Count > 0)
            {
                isDialogShowing = true;
                var request = dialogQueue.Dequeue();
                dialog.Show(request, OnDialogFinished);
            }
            else
            {
                isDialogShowing = false;
            }
        }

        private void OnDialogFinished()
        {
            isDialogShowing = false;
            ShowNextDialog();
        }
    }
}