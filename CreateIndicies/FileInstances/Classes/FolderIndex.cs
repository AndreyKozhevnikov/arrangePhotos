using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateIndicies.Classes {
    public class FolderIndex {
        public FolderIndex(string _fullPath) : this() {
            FullPath = _fullPath;
        }
        public FolderIndex() {
            PhotoList = new List<PhotoData>();
        }
        public string Name { get; set; }
        public string FullPath { get; set; }
        public DateTime CreatedTime { get; set; }
        public int PhotoCount {
            get {
                return PhotoList.Count();
            }
        }
      public  List<PhotoData> PhotoList { get; set; }
    }
}
