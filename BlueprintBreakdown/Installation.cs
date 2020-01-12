using System;
using System.Security.AccessControl;
using System.IO;

using Microsoft.Win32;

namespace BlueprintBreakdown
{
    sealed class Installation
    {
        private const int SpaceEngineersAppId = 244850;

        private static readonly string SpaceEngineersWindowsUninstallNode = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App " + SpaceEngineersAppId;

        public bool SpaceEngineersInstalled { get; private set; }

        public DirectoryInfo InstallDirectory { get; private set; }

        public DirectoryInfo BlueprintsDirectory { get; private set; }

        public Installation(bool installed, DirectoryInfo installdir, DirectoryInfo blueprintdir)
        {
            this.SpaceEngineersInstalled = installed;
            this.InstallDirectory = installdir;
            this.BlueprintsDirectory = blueprintdir;
        }

        public static Installation CreateFromWinRegistry()
        {
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var seNode = hklm.OpenSubKey(SpaceEngineersWindowsUninstallNode, RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.ReadKey | RegistryRights.QueryValues);

                if (seNode != null)
                {
                    try
                    {
                        var installdir = new DirectoryInfo(seNode.GetValue("InstallLocation").ToString());

                        var blueprintspath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        var blueprintsdir = new DirectoryInfo(Path.Combine(blueprintspath, "SpaceEngineers" + Path.DirectorySeparatorChar + "Blueprints"));

                        return new Installation(installdir.Exists, installdir, blueprintsdir);
                    }
                    finally
                    {
                        seNode.Close();
                    }
                }

                return new Installation(false, null, null);
            }
        }
    }
}
