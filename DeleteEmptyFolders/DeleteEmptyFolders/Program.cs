namespace DeleteEmptyFolders {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            var wrk = new DeleteWorker();
            wrk.Process();
        }
    }

    public class DeleteWorker {
        static string photoPath = @"f:\photo";
        public void Process() {
            ProcessFolders(photoPath);
        }

        void ProcessFolders(string rootFolder) {
            var folders = Directory.GetDirectories(rootFolder);

            foreach(var folder in folders) {
                ProcessFolders(folder);
            }
           
            var photos = Directory.GetFiles(rootFolder);
            if(photos.Count()==0&& folders.Count() == 0) {
                Directory.Delete(rootFolder);
                Console.WriteLine(rootFolder);
            }
        }
    }
}
        