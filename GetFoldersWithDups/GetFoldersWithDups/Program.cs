using GetFoldersWithDups.Classes;

namespace GetFoldersWithDups {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            var wrk = new Worker();
            wrk.Process();
        }
    }
}