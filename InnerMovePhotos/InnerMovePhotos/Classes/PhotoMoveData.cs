using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnerMovePhotos.Classes {
    public class PhotoMoveData {
        public string Name { get; set; }
        public string OldPath { get; set; }
        public string NewPath { get; set; }

        public bool IsMoved { get; set; }
      //  public bool Deleted { get; set; }
    }
}
