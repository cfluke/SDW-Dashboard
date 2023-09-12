using System.Collections;
using SerializableData;
using TMPro;
using UnityEngine;

namespace DialogManagement
{
    public abstract class AppCreationArgs { }
    
    public class ExistingAppCreationArgs : AppCreationArgs
    {
        public AppSerializable App { get; set; }
    }
    
    public class NewAppCreationArgs : AppCreationArgs
    {
        public Vector2Int Position { get; set; }
        public Vector2Int Dimensions { get; set; }
    }

    public class AppCreationDialog : Dialog<AppSerializable, AppCreationArgs>
    {
        [SerializeField] private TMP_InputField path;
        [SerializeField] private TMP_InputField args;
        [SerializeField] private TMP_InputField name;
        [SerializeField] private AppIconSelector icon;

        [SerializeField] private Color compulsoryFieldColour;
        
        private Vector2Int _position;
        private Vector2Int _dimensions;

        public override void Init(AppCreationArgs parameters)
        {
            switch (parameters)
            {
                case NewAppCreationArgs newApp:
                    _position = newApp.Position;
                    _dimensions = newApp.Dimensions;
                    break;
                case ExistingAppCreationArgs existingApp:
                    path.text = existingApp.App.path;
                    args.text = existingApp.App.args;
                    name.text = existingApp.App.name;
                    icon.Icon = existingApp.App.icon;
                    _position = new Vector2Int(existingApp.App.x, existingApp.App.y);
                    _dimensions = new Vector2Int(existingApp.App.w, existingApp.App.h);
                    break;
            }
        }

        public void Confirm()
        {
            if (path.text.Length == 0)
            {
                Debug.Log("Path needed");
                StartCoroutine(FlashCompulsoryFields());
                return;
            }
            
            // get all the values from the input fields
            string appPath = path.text;
            string appName = name.text;
            string arguments = args.text;
            string iconPath = icon.Icon;
            
            // get position/dimensions
            int x = _position.x;
            int y = _position.y;
            int w = _dimensions.x;
            int h = _dimensions.y;
            
            // create the app
            AppSerializable app = new AppSerializable(appPath, x, y, w, h, appName, arguments, iconPath);
            OnConfirm.Invoke(app);
        }

        public void Cancel()
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