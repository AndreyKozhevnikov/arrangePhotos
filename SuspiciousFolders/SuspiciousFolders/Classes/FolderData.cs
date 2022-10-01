using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousFolders.Classes {
    public class FolderData {
        public FolderData(string _path) {
            Path = _path;
            Files = new Dictionary<string, string>();
            Directories= new List<string>(); 
        }

        public string Path { get; set; }
        public Dictionary<string,string> Files{get; set; }   
        public List<string> Directories{ get; set; }

        public int FilesCount {
            get {
                return Files.Count;
            }
        }
        public int DirectoriesCount {
            get {
                return Directories.Count;
            }
        }
    }
}
