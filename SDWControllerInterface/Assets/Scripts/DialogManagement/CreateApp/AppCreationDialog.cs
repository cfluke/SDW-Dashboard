using System.Collections;
using System.Linq;
using AppLayout;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace DialogManagement.CreateApp
{
    public class AppCreationArgs 
    { 
        [CanBeNull] public App App { get; set; }
    }

    public class AppCreationDialog : Dialog<App, AppCreationArgs>
    {
        [SerializeField] private TMP_InputField path;
        [SerializeField] private TMP_InputField args;
        [SerializeField] private TMP_InputField title;
        [SerializeField] private AppIconSelector icon;

        [SerializeField] private Color compulsoryFieldColour;

        public override void Init(AppCreationArgs parameters)
        {
            if (parameters.App == null)
                return; // no existing app details, nothing to init
            
            path.text = parameters.App.Path;
            args.text = parameters.App.Args;
            title.text = parameters.App.Name;
            icon.Icon = parameters.App.IconPath;
        }

        public override void Confirm()
        {
            if (path.text.Length == 0)
            {
                Debug.Log("Path needed");
                StartCoroutine(FlashCompulsoryFields());
                return;
            }
            
            // get all the values from the input fields
            string appPath = path.text.Trim();
            string arguments = args.text.Trim();
            string appName = title.text.Trim();
            string iconPath = icon.Icon;
            
            if(appPath.Split('/').Last() == "firefox")
            {
                arguments = string.Format("-new-instance --profile usr/firefoxProfiles/{1} {2}", UnityEngine.Random.Range(10000000, 99999999) ,arguments);

                //Need to add: system("rm -r usr/firefoxProfiles/*");  to the listener app

            }

            // create the app
            App app = new App(appPath, appName, arguments, iconPath);
            OnConfirm.Invoke(app);
        }

        public override void Cancel()
        {
            OnConfirm.Invoke(null);
        }

        private IEnumerator FlashCompulsoryFields()
        {
            path.targetGraphic.color = compulsoryFieldColour;
            float elapsedTime = 0f;
            float fadeDuration = 1.0f;
            while (elapsedTime < fadeDuration)
            {
                path.targetGraphic.color = Color.Lerp(compulsoryFieldColour, path.colors.normalColor, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}