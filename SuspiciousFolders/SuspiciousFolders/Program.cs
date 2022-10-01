using SuspiciousFolders.Classes;

namespace SuspiciousFolders {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            var wrk = new FolderWorker();
            wrk.Process();
        }
    }
}