using CreateIndicies.Classes;

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;


namespace ExportAllPhotos.Classes {
    public class ExportWorker {
        public void Process() {
            List<PhotoData> photoList = new List<PhotoData>();
            ProcessFolders(@"f:\photo", photoList);
            Export(photoList);
        }

        void ProcessFolders(string rootFolder, List<PhotoData> photoList) {
            var folders = Directory.GetDirectories(rootFolder);

            foreach(var folder in folders) {
                ProcessFolders(folder, photoList);
            }
            var indexFileName = Path.Combine(rootFolder, "index.json");
            if(!File.Exists(indexFileName)) {
                throw new Exception("no index: " + rootFolder);
            }
            var indexText = File.ReadAllText(indexFileName);
            var index = JsonSerializer.Deserialize<FolderIndex>(indexText);
            photoList.AddRange(index.PhotoList);
            Console.WriteLine();
            Console.WriteLine(rootFolder + " done");

        }

        public void Export(List<PhotoData> photoList) {

            Console.WriteLine("export start");
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(photoList, options);
            var exportFileName = @"c:\temp\allPhotos.json";

            File.WriteAllText(exportFileName, jsonString);

            Console.WriteLine("export done");
        }

    }
}
