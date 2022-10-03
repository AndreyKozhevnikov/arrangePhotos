using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace InnerMovePhotos.Classes {
    public class MoveWorker {
        static string photoPath = @"f:\photo";
        public void Process() {
            List<PhotoMoveData> movedPhotos = new List<PhotoMoveData>();
            ProcessGoogleFolders(photoPath, movedPhotos);
        }

        void ProcessGoogleFolders(string rootFolder, List<PhotoMoveData> movedPhotos) {
            var folders = Directory.GetDirectories(rootFolder);

            foreach(var folder in folders) {
                ProcessGoogleFolders(folder, movedPhotos);
            }
            if(!rootFolder.Contains("CommonGL")) {
                return;
            }
            var folderName = Path.GetFileName(rootFolder);
            var timePart = folderName.Substring(0, 10);
            var targetFolderName = timePart + " Common";
            var targetFolderPath = Path.Combine(photoPath, targetFolderName);
            if(!Directory.Exists(targetFolderPath)) {
                Directory.CreateDirectory(targetFolderPath);
            }
            var photos = Directory.GetFiles(rootFolder);
            var k = 0;
            foreach(var photo in photos) {
                k++;
                Console.Write("\r" + k + "/" + photos.Count());
                var photoName = Path.GetFileName(photo);
                if(photoName == "index.json") {
                    File.Delete(photo);
                    continue;
                }

                var newPhotoPath = Path.Combine(targetFolderPath, photoName);
                if(File.Exists(newPhotoPath)) {
                    File.Delete(photo);
                    continue;
                }
                File.Move(photo, newPhotoPath);
            }
            Console.WriteLine();
            Console.WriteLine(rootFolder + " done");

        }

        void ProcessFolders(string rootFolder, List<PhotoMoveData> movedPhotos) {
            var folders = Directory.GetDirectories(rootFolder);

            foreach(var folder in folders) {
                ProcessFolders(folder, movedPhotos);
            }
            if(!rootFolder.Contains("Common")) {
                return;
            }
            if(rootFolder.Contains("0000-00-01 CommonGL")) {
                return;
            }
            if(rootFolder.Contains("2016-10-08 Common")) {

            }
            var photos = Directory.GetFiles(rootFolder);
            var folderName = Path.GetFileName(rootFolder);
            var k = 0;
            foreach(var photo in photos) {
                k++;
                Console.Write("\r" + k + "/" + photos.Count());
                var photoName = Path.GetFileName(photo);
                if(photoName == "index.json") {
                    continue;
                }
                FileInfo fi = new FileInfo(photo);
                var modifiedTime = fi.LastWriteTime.ToString("yyyy-MM-01");
                if(folderName.Substring(0, 10) == modifiedTime) {
                    continue;
                }
                var movedPhoto = new PhotoMoveData();
                movedPhoto.Name = photoName;
                movedPhoto.OldPath = photo;

                var newFolderName = modifiedTime + " Common";
                var newFolderPath = Path.Combine(photoPath, newFolderName);
                if(!Directory.Exists(newFolderPath)) {
                    Directory.CreateDirectory(newFolderPath);
                }
                var newPhotoPath = Path.Combine(newFolderPath, photoName);
                movedPhoto.NewPath = newPhotoPath;
                movedPhotos.Add(movedPhoto);
                if(File.Exists(newPhotoPath)) {
                    var existingFile = new FileInfo(newPhotoPath);
                    if(existingFile.LastWriteTime == fi.LastWriteTime && existingFile.Length == fi.Length) {
                        File.Delete(photo);
                    }
                    continue;
                }
                File.Move(photo, newPhotoPath);
                movedPhoto.IsMoved = true;
            }
            Console.WriteLine();
            Console.WriteLine(rootFolder + " done");
        }
        void Export(List<PhotoMoveData> movedPhotos) {
            var options = new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(movedPhotos, options);

            File.WriteAllText(@"c:\temp\movedFiles.json", jsonString);
        }
    }
}
