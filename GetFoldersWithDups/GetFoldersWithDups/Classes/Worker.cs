using CreateIndicies.Classes;
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
            List<PhotoData> lines = GetDataFromCSV();
            var dupPhotos = BuildDuplicatesPhotos(lines);

            var dupFolders = BuildDuplicatesFolders(dupPhotos);
           // dupFolders = dupFolders.OrderByDescending(x => x.DuplicateFolders.Count()).ToList();
            dupFolders = dupFolders.OrderBy(x => x.Name).ToList();
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(dupFolders, options);

            File.WriteAllText(@"c:\temp\Mydups.json", jsonString);
            Console.WriteLine("done");
            Console.ReadKey();
        }

        private List<FolderHolder> BuildDuplicatesFolders(List<PhotoHolder> dupPhotos) {
            var foldersDict = new Dictionary<string, FolderHolder>();

            foreach(var photo in dupPhotos) {
                var firstFolderName = Path.GetDirectoryName(photo.Paths[0]);
                FolderHolder firstFolder;
                if(foldersDict.ContainsKey(firstFolderName)) {
                    firstFolder = foldersDict[firstFolderName];
                    firstFolder.AllDupsCount++;

                }
                else {
                    firstFolder = new FolderHolder(firstFolderName);
                    foldersDict[firstFolderName] = firstFolder;
                }
                for(int i = 1; i < photo.Paths.Count; i++) {
                    var dupFolderName = Path.GetDirectoryName(photo.Paths[i]);
                    if(firstFolder.DuplicateFolders.ContainsKey(dupFolderName)) {
                        var dupFolder = firstFolder.DuplicateFolders[dupFolderName];
                        dupFolder.PhotoNames.Add(photo.Name);
                    }
                    else {
                        firstFolder.DuplicateFolders[dupFolderName] = new DuplicateFolder(dupFolderName);
                        firstFolder.DuplicateFolders[dupFolderName].PhotoNames.Add(photo.Name);
                    }
                }
            }
            var folderList = foldersDict.Values.ToList();


            return folderList;

        }

        List<PhotoHolder> BuildDuplicatesPhotos(List<PhotoData> photos) {
            Dictionary<string, PhotoHolder> photoDict = new Dictionary<string, PhotoHolder>();
            List<PhotoHolder> duplicates = new List<PhotoHolder>();
            int k = 0;
            foreach(var photo in photos) {
                // var data = line.Split(";");
                var fullPath = photo.FullName;
                if(fullPath.Contains("Kate") || fullPath.Contains("Best") ||fullPath.Contains("Repair")) {
                    continue;
                }
                var name = photo.Name;
                if(name == "P1010125.JPG") {
                    var t = 3;
                    var k1 = t + 1;
                }
                var createdTime = photo.CreatedTime;
                var modifiedTime = photo.ModifiedTime;
                var size = photo.Size;
                List<string> keys = new List<string>();
                keys.Add(name+modifiedTime);
                if(createdTime != null && modifiedTime != createdTime) {
                    keys.Add(name+ createdTime);
                }
                foreach(var key in keys) {
                    if(photoDict.ContainsKey(key)) {
                        var existingPhoto = photoDict[key];
                        if(existingPhoto.CreatedTime == photo.CreatedTime || existingPhoto.ModifiedTime == photo.ModifiedTime) {
                            existingPhoto.Paths.Add(fullPath);
                            var isAlreadyDups = duplicates.Where(x => x.Key == key).Count() > 0;
                            if(!isAlreadyDups) {
                                duplicates.Add(existingPhoto);
                            }
                        }
                        else {
                            // throw new Exception(String.Format("photo with the same name {0} {1}", fullPath, name));
                        }
                    }
                    else {
                        photoDict[key] = new PhotoHolder(name);
                        photoDict[key].ModifiedTime = modifiedTime;
                        photoDict[key].CreatedTime = createdTime;
                        photoDict[key].Size = size;
                        photoDict[key].Paths.Add(fullPath);
                        photoDict[key].Key = key;
                    }
                }
                k++;
                if(k % 5000 == 0) {
                    Console.WriteLine(k);
                }
            }

            return duplicates;
        }

        List<PhotoData> GetDataFromCSV() {
            var path = @"c:\temp\allPhotos.json";
            var indexText = File.ReadAllText(path);
            var indexList = JsonSerializer.Deserialize<List<PhotoData>>(indexText);
            return indexList;
        }
    }

}
