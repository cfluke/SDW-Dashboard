using TMPro;
using UnityEngine;

namespace DialogManagement.Confirm
{
    public class ConfirmDialogArgs
    {
        public string Message { get; set; }
    }

    public class ConfirmDialog : Dialog<bool, ConfirmDialogArgs>
    {
        [SerializeField] private TMP_Text message;
        
        public override void Init(ConfirmDialogArgs parameters)
        {
            message.text = parameters.Message;
        }

        public override void Confirm()
        {
            OnConfirm.Invoke(true);
        }

        public override void Cancel()
        {
            OnConfirm.Invoke(false);
        }
    }
}