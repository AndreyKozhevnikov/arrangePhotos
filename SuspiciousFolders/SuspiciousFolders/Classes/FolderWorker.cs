using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousFolders.Classes {
    public class FolderWorker {

        public void Process() {
            var rootFolder = @"c:\tempPhotos";
            var mainFolders = Directory.GetDirectories(rootFolder);
            Dictionary<string, FolderData> suspiciousFolders = new Dictionary<string, FolderData>();
            foreach(var folder in mainFolders) {
                var innerDirs=Directory.GetDirectories(folder);
                if(innerDirs.Count() > 0) {
                    var susFolder = GetSusFolder(suspiciousFolders, folder);
                    susFolder.Directories = innerDirs.ToList();
                }
                var photos=Directory.GetFiles(folder);
                foreach(var photo in photos) {

                }
            }
        }
        FolderData GetSusFolder(Dictionary<string, FolderData> suspiciousFolders, string folder) {
            var res=suspiciousFolders.ContainsKey(folder) ? suspiciousFolders[folder] : null;
            if(res == null) {
                res = new FolderData(folder);
                suspiciousFolders[folder] = res;
            }
            return res;
        }

    }
}
