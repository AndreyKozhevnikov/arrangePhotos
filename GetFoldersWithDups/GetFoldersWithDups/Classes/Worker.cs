using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace GetFoldersWithDups.Classes {
    public class Worker {
        public void Process() {
            List<String> lines = GetDataFromCSV();
            var dupPhotos = BuildDuplicatesPhotos(lines);

            var dupFolders = BuildDuplicatesFolders(dupPhotos);
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(dupFolders, options);

            File.WriteAllTextAsync(@"c:\Dropbox\C#\RenamePhotos\Mydups.json", jsonString);
        }

        private List<FolderHolder> BuildDuplicatesFolders(List<PhotoHolder> dupPhotos) {
            var foldersDict = new Dictionary<string, FolderHolder>();

            foreach(var photo in dupPhotos) {
                var firstFolderName = photo.Paths[0];
                FolderHolder firstFolder;
                if(foldersDict.ContainsKey(firstFolderName)) {
                    firstFolder = foldersDict[firstFolderName];
                    firstFolder.AllDupsCount++;
                    
                } else {
                    firstFolder = new FolderHolder(firstFolderName);
                    foldersDict[firstFolderName] = firstFolder;
                }
                for(int i = 1; i < photo.Paths.Count; i++) {
                    var dupFolderName = photo.Paths[i];
                    if(firstFolder.DuplicateFolders.ContainsKey(dupFolderName)) {
                        var dupFolder = firstFolder.DuplicateFolders[dupFolderName];
                        dupFolder.PhotoNames.Add(photo.Name);
                    } else {
                        firstFolder.DuplicateFolders[dupFolderName] = new DuplicateFolder(dupFolderName);
                        firstFolder.DuplicateFolders[dupFolderName].PhotoNames.Add(photo.Name);
                    }
                }
            }


            return foldersDict.Values.ToList();

        }

        List<PhotoHolder> BuildDuplicatesPhotos(List<string> lines) {
            Dictionary<string, PhotoHolder> photoDict = new Dictionary<string, PhotoHolder>();
            List<PhotoHolder> duplicates = new List<PhotoHolder>();
            int k = 0;
            foreach(var line in lines) {
                var data = line.Split(";");
                var fullPath = Path.GetDirectoryName(data[0]);
                var name = data[1];
                var dateStamp = data[2];
                var size = data[3];
                var key = name + dateStamp + size;
                if(photoDict.ContainsKey(key)) {
                    var existingPhoto = photoDict[key];
                    if(existingPhoto.DateStamp == dateStamp && existingPhoto.Size == size) {
                        existingPhoto.Paths.Add(fullPath);
                        var isAlreadyDups = duplicates.Where(x => x.Name == existingPhoto.Name).Count() > 0;
                        if(!isAlreadyDups) {
                            duplicates.Add(existingPhoto);
                        }
                    } else {
                        throw new Exception(String.Format("photo with the same name {0} {1}", fullPath, name));
                    }
                } else {
                    photoDict[key] = new PhotoHolder(name, dateStamp, size);
                    photoDict[key].Paths.Add(fullPath);
                }
                k++;
                if(k % 10000 == 0) {
                    Console.WriteLine(k);
                }
            }

            return duplicates;
        }

        List<string> GetDataFromCSV() {
            var path = @"c:\Dropbox\C#\RenamePhotos\MyCSVExport.csv";
            string[] lines = System.IO.File.ReadAllLines(path);
            return lines.ToList();
        }
    }

}
