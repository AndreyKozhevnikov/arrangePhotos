using CreateIndicies.Classes;

namespace CreateIndicies {
    internal class Program {
        static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Hello, World!");
            var crt =new IndexCreator();
            crt.Process();
        }
    }
}