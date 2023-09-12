namespace DialogManagement
{
    public class ConfirmDialogArgs
    {
        public string Message { get; set; }
    }

    public class ConfirmDialog : Dialog<bool, ConfirmDialogArgs>
    {
        public override void Init(ConfirmDialogArgs parameters)
        {
            
        }

        public void Confirm()
        {
            OnConfirm.Invoke(true);
        }

        public void Cancel()
        {
            OnConfirm.Invoke(false);
        }
    }
}