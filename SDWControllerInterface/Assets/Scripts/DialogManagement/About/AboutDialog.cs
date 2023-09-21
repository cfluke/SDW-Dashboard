using TMPro;
using UnityEngine;

namespace DialogManagement.About
{
    public class AboutDialogArgs {
    
    }
    
    public class AboutDialog : Dialog<object, AboutDialogArgs>
    {
        [SerializeField] private TMP_Text projectVersion;
        [SerializeField] private TMP_Text team;
        
        public override void Init(AboutDialogArgs parameters)
        {
            projectVersion.text = Application.productName + " " + Application.version;
            team.text = Application.companyName;
        }

        public override void Confirm()
        {
            OnConfirm.Invoke(null);
        }

        public override void Cancel()
        {
            OnConfirm.Invoke(null);
        }
    }
}