using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovePhotos.Classes {
    public class PhotoWorker {
        public void Process() {
            string sourceFolder;
            string destinationFolder;
            bool copyFiles;
            GetConfig(out sourceFolder,out destinationFolder, out copyFiles);


        }

        void ProcessFolders(string rootFolder) {
            var folders = Directory.GetDirectories(rootFolder);
            foreach (var folder in folders) {
                ProcessFolders(folder);
            }
            var photos = Directory.GetFiles(folder);
            foreach(var photo in )
        }

        void GetConfig(out string sourceFolder, out string destinationFolder, out bool copyFiles) {
            string[] pathes = File.ReadAllLines("rootPath.txt");
            sourceFolder = pathes[0];
            destinationFolder = pathes[1];
            copyFiles=bool.Parse(pathes[2]);
        }
    }
}
