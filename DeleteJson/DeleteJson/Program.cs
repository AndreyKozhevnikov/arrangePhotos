using DeleteJson.Classes;

namespace DeleteJson {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            var wrk = new JsonWorker();
            wrk.Process();
        }
    }
}