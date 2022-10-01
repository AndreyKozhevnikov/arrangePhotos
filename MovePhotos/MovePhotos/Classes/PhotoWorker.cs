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


namespace MovePhotos.Classes {
    public class PhotoWorker {
        public void Process() {
            string sourceFolder;
            string destinationFolder;
            bool copyFiles;
            Console.WriteLine("start");
            GetConfig(out sourceFolder, out destinationFolder, out copyFiles);
            Dictionary<string, FolderData> foldersToMove = new Dictionary<string, FolderData>();
            List<PhotoData> problemPhotos = new List<PhotoData>();
            Console.WriteLine("process");
            ProcessFolders(sourceFolder, destinationFolder, foldersToMove, problemPhotos);
            Console.WriteLine("export");
            ExportFolders(foldersToMove);
            ExportProblemFiles(problemPhotos);
            var listFolders = foldersToMove.Values.ToList();
            if(copyFiles) {
                Console.WriteLine("copy");
                CopyFiles(listFolders);
            }
            Console.WriteLine("All done");
            Console.ReadKey();
        }

        void CopyFiles(List<FolderData> foldersToMove) {
            foreach(var folder in foldersToMove) {
                Console.WriteLine(folder.Name + " start");
                if(!folder.Exists) {
                    Directory.CreateDirectory(folder.TargetPath);
                }
                int k = 0;
                foreach(var file in folder.Photos) {
                    if(file.IsExists) {
                        continue;
                    }
                    var sourceName = Path.Combine(file.SourcePath, file.Name);
                    var targetName = file.DestinationPath;
                    if(file.ShouldCopy) {
                        File.Copy(sourceName, targetName);
                    }
                    else {
                        File.Move(sourceName, targetName);
                    }
                    k++;
                    Console.Write("\r" + k + "/" + folder.Photos.Count());
                }
                Console.WriteLine();
                Console.WriteLine(folder.Name + " done");
            }
        }



        void ProcessFolders(string rootFolder, string destinationFolder, Dictionary<string, FolderData> foldersToMove, List<PhotoData> problemPhotos) {
            var folders = Directory.GetDirectories(rootFolder);

            foreach(var folder in folders) {
                ProcessFolders(folder, destinationFolder, foldersToMove, problemPhotos);
            }

            var photos = Directory.GetFiles(rootFolder);
            var k = 0;
            foreach(var photo in photos) {
                k++;
                Console.Write("\r" + k + "/" + photos.Count());
                var photoName = Path.GetFileName(photo);
                string fileModifiedDate = "0000-00-00";
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
               
                var folderNameForPhoto = GetCommonFolderName(fileModifiedDate);
                var photoData = new PhotoData(photoName, fileModifiedDate, rootFolder);
                var initialFolderName = Path.GetFileName(rootFolder);
                if(initialFolderName.StartsWith("Kate") || initialFolderName.StartsWith("Best")) {
                    folderNameForPhoto = initialFolderName;
                    photoData.ShouldCopy = true;
                }
                var folderData = GetFolder(foldersToMove, folderNameForPhoto, destinationFolder);

                photoData.DestinationPath = Path.Combine(destinationFolder, folderNameForPhoto, photoName);
                photoData.IsExists = File.Exists(photoData.DestinationPath);
                if(photoData.IsExists) {
                    problemPhotos.Add(photoData);
                }
                folderData.Photos.Add(photoData);

               
            }
            Console.WriteLine();
            Console.WriteLine(rootFolder + " done");
        }
        void ExportProblemFiles(List<PhotoData> problemPhotos) {
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(problemPhotos, options);

            File.WriteAllText(@"c:\temp\problemPhotos.json", jsonString);
        }
        void ExportFolders(Dictionary<string, FolderData> foldersToMove) {
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(foldersToMove, options);

            File.WriteAllText(@"c:\temp\foldersToMove.json", jsonString);
        }
        FolderData GetFolder(Dictionary<string, FolderData> folders, string folder, string destinationFolder) {
            var res = folders.ContainsKey(folder) ? folders[folder] : null;
            if(res == null) {
                res = new FolderData(folder);
                var finalPath = Path.Combine(destinationFolder, folder);
                res.Exists = Directory.Exists(finalPath);
                res.TargetPath = finalPath;
                folders[folder] = res;
            }
            return res;
        }
        void GetConfig(out string sourceFolder, out string destinationFolder, out bool copyFiles) {
            string[] pathes = File.ReadAllLines("rootPath.txt");
            sourceFolder = pathes[0];
            destinationFolder = pathes[1];
            copyFiles = bool.Parse(pathes[2]);
        }
        public string GetPhotoModifiedTime(string path) {
            DateTime modDate = File.GetLastWriteTime(path);
            return modDate.ToString("yyyy-MM-dd");
        }
        public string GetCommonFolderName(string photoDate) {
            var photoDateCut = photoDate.Substring(0, 7);
            var res = string.Format("{0}-01 CommonGL", photoDateCut);
            return res;
        }
        private static Regex r = new Regex(":");
        public string GetDateTakenFromImage(string path) {
            using(FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using(Image myImage = Image.FromStream(fs, false, false)) {
                PropertyItem propItem = null;
                try {
                    propItem = myImage.GetPropertyItem(36867);
                }
                catch { }
                if(propItem == null) {
                    return "0000-00-00";
                }
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                var dt = DateTime.Parse(dateTaken);
                return dt.ToString("yyyy-MM-dd");
            }
        }
        public string GetDateTakenFromMOV(string path) {

            var fl = GenerateFileInfo.GetFileTag(path);
            var dateTaken = fl[0].TagValue;
            dateTaken = Regex.Replace(dateTaken, @"\p{C}+", string.Empty);
            DateTime dt;
            var res = DateTime.TryParse(dateTaken, out dt);
            if(res) {
                return dt.ToString("yyyy-MM-dd");
            }
            else {
                return "0000-00-00";
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

    }
}
