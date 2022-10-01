using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace SuspiciousFolders.Classes {
    public class FolderWorker {

        public void Process() {
            Console.WriteLine("start");
            var rootFolder = GetRootPath();
            var susFolders = GetSuspiciousFolders(rootFolder);
            ExportData(susFolders);
            Console.WriteLine("end");
        }
        string GetRootPath() {
            string path = File.ReadAllText("rootPath.txt");
            return path;
        }
        public void ExportData(List<FolderData> folders) {
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(folders, options);

           File.WriteAllText(@"c:\temp\susFolders.json", jsonString);
        }

        public List<FolderData> GetSuspiciousFolders(string rootPath) {
            var mainFolders = Directory.GetDirectories(rootPath);
            Dictionary<string, FolderData> suspiciousFolders = new Dictionary<string, FolderData>();
            foreach(var folder in mainFolders) {
                var innerDirs = Directory.GetDirectories(folder);
                FolderData susFolder;
                if(innerDirs.Count() > 0) {
                    susFolder = GetSusFolder(suspiciousFolders, folder);
                    susFolder.Directories = innerDirs.ToList();
                }
                var photos = Directory.GetFiles(folder);
                var folderDate = GetFolderDate(folder);
                if(folderDate == null) {
                    susFolder = GetSusFolder(suspiciousFolders, folder);
                    continue;
                }
                foreach(var photo in photos) {
                    var photoModifiedDate = GetPhotoModifiedTime(photo);
                    var isPhotoFit = IsPhotoFitFolder(folderDate, photoModifiedDate);
                    if(!isPhotoFit) {
                        susFolder = GetSusFolder(suspiciousFolders, folder);
                        susFolder.Files[photo] = photoModifiedDate;
                    }
                }
            }
            return suspiciousFolders.Values.ToList();
        }

        public bool IsPhotoFitFolder(string folderDate, string photoDate) {
            if(folderDate == photoDate) {
                return true;
            }
            if(folderDate.Substring(8, 2) == "01") {
                if(folderDate.Substring(0, 7) == photoDate.Substring(0, 7)) {
                    return true;
                }
            }
            return false;
        }

        FolderData GetSusFolder(Dictionary<string, FolderData> suspiciousFolders, string folder) {
            var res = suspiciousFolders.ContainsKey(folder) ? suspiciousFolders[folder] : null;
            if(res == null) {
                res = new FolderData(folder);
                suspiciousFolders[folder] = res;
            }
            return res;
        }

        public string GetPhotoModifiedTime(string path) {
            DateTime modDate = File.GetLastWriteTime(path);
            return modDate.ToString("yyyy-MM-dd");
        }

        public string? GetFolderDate(string path) {
            var folderName = Path.GetFileName(path);
            if(folderName.Length < 10)
                return null;
            return folderName.Substring(0, 10);
        }

    }
}
