using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Widgets
{
    
    public class IPAddressWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text ipAddress;
        private const string IpifyUrl = "https://api.ipify.org?format=json";

        private void Start()
        {
            StartCoroutine(FetchPublicIPAddress());
        }

        private IEnumerator FetchPublicIPAddress()
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(IpifyUrl))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string responseText = webRequest.downloadHandler.text;
                    PublicIPAddressData data = JsonUtility.FromJson<PublicIPAddressData>(responseText);

                    if (data != null)
                    {
                        string publicIP = data.ip;
                        ipAddress.text = publicIP;
                        Debug.Log("Public IP Address: " + publicIP);
                    }
                    else
                    {
                        ipAddress.text = "<color=\"red\">Error<color>";
                        Debug.LogError("Failed to parse JSON response.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch public IP address: " + webRequest.error);
                }
            }
        }
    }

    [Serializable]
    public class PublicIPAddressData
    {
        public string ip;
    }
}
