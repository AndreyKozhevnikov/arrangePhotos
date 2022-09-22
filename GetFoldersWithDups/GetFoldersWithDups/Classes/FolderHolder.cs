using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetFoldersWithDups.Classes {
    public class FolderHolder {
        public FolderHolder(string _name) {
            DuplicateFolders = new Dictionary<string, DuplicateFolder>();
            Name = _name;
        }
        public string Name { get; set; }
        public int AllDupsCount { get; set; }
        public Dictionary<string, DuplicateFolder> DuplicateFolders { get; set; }
    }

    public class DuplicateFolder {

        public DuplicateFolder(string _name) {
            Name = _name;
            PhotoNames = new List<string>();
        }
        public string Name { get; set; }
        public List<string> PhotoNames { get; set; }
        public int PhotoCount {
            get {
                return PhotoNames.Count;
            }
        }
    }
}
