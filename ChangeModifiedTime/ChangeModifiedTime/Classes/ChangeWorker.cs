using CreateIndicies.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ChangeModifiedTime.Classes {
    public class ChangeWorker {
        public void Process() {

              ProcessFolders(@"f:\photo");
         //   ProcessFolders(@"f:\Photo\2022-09-01 Common\");
        }
        void ProcessFolders(string rootFolder) {
            var folders = Directory.GetDirectories(rootFolder);

            foreach(var folder in folders) {
                ProcessFolders(folder);
            }
            var indexFileName = Path.Combine(rootFolder, "index.json");
            if(!File.Exists(indexFileName)) {
                throw new Exception("no index: " + rootFolder);
            }
            var indexText = File.ReadAllText(indexFileName);
            var index = JsonSerializer.Deserialize<FolderIndex>(indexText);
            foreach(var photo in index.PhotoList) {
                if(photo.CreatedTime != null) {
                    DateTime dt;
                    var res = DateTime.TryParse(photo.CreatedTime, out dt);
                    if(res) {
                        File.SetLastWriteTime(photo.FullName, dt);
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine(rootFolder + " done");


        }
    }
}
