using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovePhotos.Classes {
    public class FolderData {
        public FolderData(string _name) {
            Name = _name;
            Photos = new List<PhotoData>();
        }
        public string Name { get; set; }
        public bool Exists { get; set; }
        public List<PhotoData> Photos { get; set; }
        public string TargetPath { get; set; }
        public int FileCount {
            get {
                return Photos.Count;
            }
        }
    }
}
