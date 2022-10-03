using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteJson.Classes {
    public class JsonWorker {
        public void Process() {
            string sourceFolder;

            Console.WriteLine("start");
            GetConfig(out sourceFolder);
           // DeleteJson(sourceFolder);
            DeleteJson(@"F:\Photo\");
        }
        void DeleteJson(string rootFolder) {
            var folders = Directory.GetDirectories(rootFolder);

            foreach(var folder in folders) {
                DeleteJson(folder);
            }
            var files = Directory.GetFiles(rootFolder);
            if(folders.Count() == 0 && files.Count() == 0) {
                Directory.Delete(rootFolder);
                Console.WriteLine("delete "+ rootFolder);
                return;
            }
            var k = 0;
            foreach(var file in files) {
                k++;
                Console.Write("\r" + k + "/" + files.Count());

                var fileExtension = Path.GetExtension(file).ToLower();
                if(fileExtension == ".json") {
                    File.Delete(file);
                }
            }
            Console.WriteLine();
            Console.WriteLine(rootFolder + " done");
        }

        void GetConfig(out string sourceFolder) {
            string[] pathes = File.ReadAllLines("rootPath.txt");
            sourceFolder = pathes[0];

        }
    }
}
