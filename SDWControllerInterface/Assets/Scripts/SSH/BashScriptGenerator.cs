namespace SSH
{
    public class BashScriptGenerator
    {
        public static string GenerateSSHScript(string ipAddress, string privateKeyPath, string command)
        {
            // Create the Bash script
            string script = $"#!/bin/bash\n";
            script += $"ssh -i {privateKeyPath} user@{ipAddress} \"{command}\"\n";
            return script;
        }
    }
}