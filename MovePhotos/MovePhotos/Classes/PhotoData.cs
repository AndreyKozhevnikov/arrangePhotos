using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovePhotos.Classes {
    public class PhotoData {
        public PhotoData(string _name, string _modifieDate, string _sourcePath) {
            Name = _name;
            ModifiedDate = _modifieDate;
            SourcePath = _sourcePath;
        }
        public PhotoData() {

        }
        public string Name { get; set; }
        public string ModifiedDate { get; set; }
        public string SourcePath { get; set; }
        public bool IsExists { get; set; }
        public string DestinationPath { get; set; }
        public bool ShouldCopy { get; set; }

    }
}
