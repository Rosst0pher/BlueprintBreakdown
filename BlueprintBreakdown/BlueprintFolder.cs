using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace BlueprintBreakdown
{
    sealed class BlueprintFolder
    {
        public IReadOnlyCollection<string> Local => this.local;

        public IReadOnlyCollection<string> Workshop => this.workshopnames.Keys;

        private DirectoryInfo localdir;
        private DirectoryInfo workshopdir;
        private List<string> local;

        private Dictionary<string, string> workshopnames;

        public BlueprintFolder(DirectoryInfo folder)
        {
            this.local = new List<string>();
            this.workshopnames = new Dictionary<string, string>();
            this.localdir = new DirectoryInfo(folder.FullName + "\\local\\");
            this.workshopdir = new DirectoryInfo(folder.FullName + "\\workshop\\");

            this.ReadLocalBlueprints();
            this.ReadWorkshopItems();
        }

        private void ReadLocalBlueprints()
        {
            if (this.localdir.Exists)
            {
                foreach (var bpdir in this.localdir.GetDirectories())
                {
                    this.local.Add(bpdir.Name);
                }
            }
        }

        private void ReadWorkshopItems()
        {
            if (this.workshopdir.Exists)
            {
                foreach(var sbbfile in this.workshopdir.GetFiles("*.sbb"))
                {
                    var name = Blueprint.LoadNameFromSbbFile(sbbfile);
                    this.workshopnames.Add(name, sbbfile.FullName);
                }
            }
        }

        public Blueprint Load(string name)
        {
            if (this.local.Contains(name))
            {
                var sbcfile = new FileInfo(this.localdir + "\\" + name + "\\bp.sbc");

                if (sbcfile.Exists)
                {
                    return Blueprint.LoadFromSbcFile(sbcfile);
                }
            }
            else
            {
                if(this.workshopnames.TryGetValue(name, out var sbbfile))
                {
                    return Blueprint.LoadFromSbbFile(new FileInfo(sbbfile));
                }
            }
            return null;
        }

        public static BlueprintFolder CreateFromInstallation(Installation installation)
        {
            return new BlueprintFolder(new DirectoryInfo(installation.BlueprintsDirectory.FullName));
        }
    }
}
