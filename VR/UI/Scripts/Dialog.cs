using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OghiUnityTools.VR.UI.Scripts
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private Button cancelButton;
   
        [SerializeField] private TMP_Text headerText;    // Add reference for header text
        [SerializeField] private TMP_Text messageText;   // Add reference for message text

        private Action onYesClicked;
        private Action onNoClicked;
        private Action onCancelClicked;

        private Action onDialogFinished;

        private void Awake()
        {
            // Add listeners to buttons
            if (yesButton != null)
                yesButton.onClick.AddListener(HandleYesClick);
        
            if (noButton != null)
                noButton.onClick.AddListener(HandleNoClick);
        
            if (cancelButton != null)
                cancelButton.onClick.AddListener(HandleCancelClick);
        }

        private void OnDestroy()
        {
            yesButton.onClick.RemoveListener(HandleYesClick);
            noButton.onClick.RemoveListener(HandleNoClick);
            cancelButton.onClick.RemoveListener(HandleCancelClick);
        }

        private void HandleYesClick()
        {
            onYesClicked?.Invoke();
            HideScreen();
        }

        private void HandleNoClick()
        {
            onNoClicked?.Invoke();
            HideScreen();
        }

        private void HandleCancelClick()
        {
            onCancelClicked?.Invoke();
            HideScreen();
        }

        private void HideScreen()
        {
            // Clear all callbacks
            onYesClicked = null;
            onNoClicked = null;
            onCancelClicked = null;

            gameObject.SetActive(false);

            onDialogFinished?.Invoke();
        }

        public void Show(DialogRequest request, Action onDialogFinishedAction)
        {
            onDialogFinished = onDialogFinishedAction;
            
            switch (request.DialogType)
            {
                case DialogType.Info:
                    SetupButtons(true, false, false);
                    onYesClicked = request.OnYes;
                    break;
                case DialogType.YesNo:
                    SetupButtons(true, true, false);
                    onYesClicked = request.OnYes;
                    onNoClicked = request.OnNo;
                    break;
                case DialogType.YesNoCancel:
                    SetupButtons(true, true, true);
                    onYesClicked = request.OnYes;
                    onNoClicked = request.OnNo;
                    onCancelClicked = request.OnCancel;
                    break;
            }

            ShowScreen(request.Header, request.SubHeader);
        }

        private void SetupButtons(bool showYes, bool showNo, bool showCancel)
        {
            if (yesButton != null)
                yesButton.gameObject.SetActive(showYes);
        
            if (noButton != null)
                noButton.gameObject.SetActive(showNo);
        
            if (cancelButton != null)
                cancelButton.gameObject.SetActive(showCancel);
        }

        private void ShowScreen(string header, string message)
        {
            if (headerText != null)
                headerText.text = header;
        
            if (messageText != null)
                messageText.text = message;

            gameObject.SetActive(true);
        }
    }
}