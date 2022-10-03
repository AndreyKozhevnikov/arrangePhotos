using InnerMovePhotos.Classes;

namespace InnerMovePhotos {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            var wrk = new MoveWorker();
            wrk.Process();
        }
    }
}