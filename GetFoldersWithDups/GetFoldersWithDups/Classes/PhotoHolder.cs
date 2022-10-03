using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetFoldersWithDups.Classes {
    public class PhotoHolder {
        public PhotoHolder(string _name) {

            Paths = new List<string>();
            Name= _name;
        }
        public string Name { get; set; }
        public string CreatedTime{ get; set; }
        public string ModifiedTime{ get; set; }
        public string Size{ get; set; }

        public List<string> Paths{ get; set; }

        public string Key { get; set; }
    }
}
