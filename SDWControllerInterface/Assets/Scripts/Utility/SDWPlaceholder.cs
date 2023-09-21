using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Utility
{
    public class SDWPlaceholder : MonoBehaviour
    {
        public TMP_Text title;
        private string originalTitle;

        private void Start()
        {
            originalTitle = title.text;
            StartCoroutine(LoadingAnimation());
        }
        
        private IEnumerator LoadingAnimation()
        {
            while (true)
            {
                title.text = originalTitle;
                yield return new WaitForSeconds(1f);

                title.text = originalTitle + ".";
                yield return new WaitForSeconds(1f);

                title.text = originalTitle + "..";
                yield return new WaitForSeconds(1f);

                title.text = originalTitle + "...";
                yield return new WaitForSeconds(1f);
            }
        }
    }
}