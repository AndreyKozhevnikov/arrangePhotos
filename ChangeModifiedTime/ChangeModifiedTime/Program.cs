using ChangeModifiedTime.Classes;

namespace ChangeModifiedTime {
    internal class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Hello, World!");
            var wrk = new ChangeWorker();
            wrk.Process();
        }
    }
}