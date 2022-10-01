using MovePhotos.Classes;

namespace MovePhotos {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            var wrk = new PhotoWorker();
            wrk.Process();
        }
    }
}