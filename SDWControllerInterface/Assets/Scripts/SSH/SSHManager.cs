using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;
using Logger = Logs.Logger;

namespace SSH
{
    public class SSHManager : MonoBehaviour
    {
        #region singleton
        
        private static SSHManager _instance;

        // Create a public property to access the instance
        public static SSHManager Instance
        {
            get
            {
                // If the instance is null, create it
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SSHManager");
                    _instance = singletonObject.AddComponent<SSHManager>();
                    DontDestroyOnLoad(singletonObject);
                }
                return _instance;
            }
        }
        
        #endregion
        
        private Dictionary<string, string> _pids = new ();

        public void LaunchClient(string id, string ipAddress, string username, string password, string appPath)
        {
            string command = $"ssh {username}@{ipAddress} 'export DISPLAY=:0; /{appPath}/Listener 136.186.110.11 8000 {id};'";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"-c \"{command}\"",
            };

            Process process = new Process { StartInfo = psi };
            process.OutputDataReceived += (sender, e) =>
            {
                string output = e.Data?.Trim();
                if (!string.IsNullOrEmpty(output))
                    Logger.Instance.Log("Listener: " + output);
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                string output = e.Data?.Trim();
                if (!string.IsNullOrEmpty(output))
                    Logger.Instance.LogError("Listener: " + output);
            };
            
            // Start the process asynchronously
            Task.Run(() =>
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            });
        }
        
        // TODO: implement and call this function
        public void StopClient(string ipAddress, string username, string password)
        {
            string pid = _pids[ipAddress];
            string sshCommand = $"sshpass -p '{password}' ssh {username}@{ipAddress} 'kill {pid}'";
            Process.Start("/bin/bash", $"-c \"{sshCommand}\"");
            _pids.Remove(ipAddress);
        }
    }
}