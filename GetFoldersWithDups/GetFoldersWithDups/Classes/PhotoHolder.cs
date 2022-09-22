using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetFoldersWithDups.Classes {
    public class PhotoHolder {
        public PhotoHolder(string _name, string _dateStamp, string _size) {

            Paths = new List<string>();
            Name= _name;
            DateStamp= _dateStamp;
            Size= _size;
        }
        public string Name { get; set; }
        public string DateStamp{ get; set; }
        public string Size{ get; set; }

        public List<string> Paths{ get; set; }
    }
}
