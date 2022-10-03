using FileTagExtract;
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


namespace CreateIndicies.Classes {
    public class IndexCreator {
        public void Process() {
            string sourceFolder;
            GetConfig(out sourceFolder);
            ProcessFolders(sourceFolder);
         //   ProcessFolders("f:\\Photo\\2022-09-01 Common\\");
        }
        public void ProcessFolders(string rootFolder) {
            var folders = Directory.GetDirectories(rootFolder);

            foreach(var folder in folders) {
                ProcessFolders(folder);
            }
            var folderIndex = new FolderIndex(rootFolder);
            folderIndex.Name = Path.GetFileName(rootFolder);
            folderIndex.CreatedTime = DateTime.Now;
            Console.WriteLine(rootFolder + " start");

            var photos = Directory.GetFiles(rootFolder);
         
            var k = 0;
            foreach(var photo in photos) {
                k++;
                Console.Write("\r" + k + "/" + photos.Count());
                var phData = new PhotoData();
                phData.FullName = photo;
                phData.Name = Path.GetFileName(photo);
                FileInfo fi = new FileInfo(photo);
                phData.Size = fi.Length.ToString();


                string fileModifiedDate = null;
                var fileExtension = Path.GetExtension(photo).ToLower();
                switch(fileExtension) {
                    case ".jpg":
                    case ".png":
                        fileModifiedDate = GetDateTakenFromImage(photo);
                        break;
                    case ".mp4":
                    case ".mov":
                        fileModifiedDate = GetDateTakenFromMOV(photo);
                        break;
                    case ".zip":
                    case ".json":
                        continue;
                }

                phData.ModifiedTime = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm");
                // phData.ModifiedTime = fi.LastWriteTime;

                phData.CreatedTime = fileModifiedDate;

                folderIndex.PhotoList.Add(phData);
            }
            ExportFolderIndex(folderIndex);

            Console.WriteLine();
            Console.WriteLine(rootFolder + " done");
        }

        private void ExportFolderIndex(FolderIndex folderIndex) {
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(folderIndex, options);
            var exportFileName = Path.Combine(folderIndex.FullPath, "index.json");

            File.WriteAllText(exportFileName, jsonString);
        }

        private static Regex r = new Regex(":");
        public string GetDateTakenFromImage(string path) {
            try {
                using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using(Image myImage = Image.FromStream(fs, false, false)) {
                    PropertyItem propItem = null;
                    try {
                        propItem = myImage.GetPropertyItem(36867);
                    }
                    catch { }
                    if(propItem == null) {
                        return null;
                    }
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    DateTime dt;
                    var res = DateTime.TryParse(dateTaken, out dt);
                    if(res) {
                        return dt.ToString("yyyy-MM-dd HH:mm");
                    }
                    else {
                        return null;
                    }
                }
            }
            catch {
                return null;
            }
        }
        public string GetDateTakenFromMOV(string path) {
            try {
                var fl = GenerateFileInfo.GetFileTag(path);
                var dateTaken = fl[0].TagValue;
                dateTaken = Regex.Replace(dateTaken, @"\p{C}+", string.Empty);
                DateTime dt;
                var res = DateTime.TryParse(dateTaken, out dt);
                if(res) {
                    return dt.ToString("yyyy-MM-dd HH:mm");
                }
                else {
                    return null;
                }
            }
            catch {
                return null;
            }

            //string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
            //DateTime dt;
            //var res = DateTime.TryParse(dateTaken,out dt);
            //if(res) {
            //    return dt.ToString("yyyy-MM-dd");
            //}
            //else {
            //    return "0000-00-00";
            //}
        }


        void GetConfig(out string sourceFolder) {
            string[] pathes = File.ReadAllLines("rootPath.txt");
            sourceFolder = pathes[0];

        }
    }
}
