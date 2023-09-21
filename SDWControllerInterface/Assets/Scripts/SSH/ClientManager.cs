using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Diagnostics;

namespace SSH
{
    public class ClientManager : MonoBehaviour
    {
        // Folder name for scripts
        private const string ScriptsFolderName = "KeckDisplays";

        public void StartClient(string ipAddress, string username, string password, string clientId, string listenerPath)
        {
            string path = GenerateStartScript(ipAddress, username, password, clientId, listenerPath);
            ExecuteBashScript(path);
        }

        public void StopAllClients()
        {
            string scriptsFolderPath = Path.Combine(Application.persistentDataPath, ScriptsFolderName);

            foreach (var ipAddress in Directory.GetFiles(scriptsFolderPath, "pid_*.txt"))
                GenerateStopScript(ipAddress);
        }

        private string GenerateStartScript(string ipAddress, string username, string password, string clientId, string listenerPath)
        {
            string scriptsFolderPath = Path.Combine(Application.persistentDataPath, ScriptsFolderName);
            Directory.CreateDirectory(scriptsFolderPath);

            // generate the bash script content for starting the client
            string scriptContent = $"sshpass -p {password} ssh {username}@{ipAddress} {listenerPath} &";
            string scriptPath = Path.Combine(scriptsFolderPath, $"start_{clientId}.sh");
            File.WriteAllText(scriptPath, scriptContent);
            
            return scriptPath;
        }

        private string GenerateStopScript(string clientId)
        {
            string scriptsFolderPath = Path.Combine(Application.persistentDataPath, ScriptsFolderName);

            // generate the bash script content for stopping the client
            string scriptContent = $"pid=$(cat {Path.Combine(scriptsFolderPath, $"pid_{clientId}.txt")}); kill -9 $pid";
            string scriptPath = Path.Combine(scriptsFolderPath, $"stop_{clientId}.sh");
            File.WriteAllText(scriptPath, scriptContent);
            
            return scriptPath;
        }

        private void ExecuteBashScript(string scriptPath)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = scriptPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process process = new Process
            {
                StartInfo = psi
            };

            process.Start();
            process.WaitForExit();

            // Optionally, delete the script file after execution
            File.Delete(scriptPath);
        }
    }
}