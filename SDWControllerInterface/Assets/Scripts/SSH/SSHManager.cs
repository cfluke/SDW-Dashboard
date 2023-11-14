using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
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
        
        private void Start()
        {
            // just a test to see if this works
            string command = "ifconfig";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"-c \"{command}\""
            };

            Process process = new Process { StartInfo = psi };
            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            Logger.Instance.Log(output);
        }

        public void LaunchClient(string ipAddress, string username, string password, string appPath)
        {
            string command = 
                $"ssh {ipAddress} -t 'cd {appPath}; export DISPLAY:=0; ./TestV2 136.186.110.11 8000 keckDisplay2;'";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"-c \"{command}\""
            };

            Process process = new Process { StartInfo = psi };
            process.Start(); // TODO: execution is getting stuck during/after this
            string pid = process.StandardOutput.ReadToEnd().Trim(); 
            Logger.Instance.Log("Got PID back from SSH: " + pid);
            _pids.Add(ipAddress, pid);
            process.WaitForExit();
        }
        
        public void StopClient(string ipAddress, string username, string password)
        {
            string pid = _pids[ipAddress];
            string sshCommand = $"sshpass -p '{password}' ssh {username}@{ipAddress} 'kill {pid}'";
            Process.Start("/bin/bash", $"-c \"{sshCommand}\"");
            _pids.Remove(ipAddress);
        }
    }
}