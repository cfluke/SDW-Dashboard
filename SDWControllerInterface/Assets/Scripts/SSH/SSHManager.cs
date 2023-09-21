using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace SSH
{
    public class SSHManager : MonoBehaviour
    {
        private Dictionary<string, string> _pids = new ();

        public void LaunchClient(string ipAddress, string username, string password, string appPath)
        {
            string sshCommand = $"sshpass -p '{password}' ssh {username}@{ipAddress} '{appPath} & echo $!'";
            ProcessStartInfo psi = new ProcessStartInfo("/bin/bash", $"-c \"{sshCommand}\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process { StartInfo = psi };
            process.Start();
            string pid = process.StandardOutput.ReadToEnd().Trim();
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