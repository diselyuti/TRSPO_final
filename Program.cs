using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRSPO_final
{
    class Program
    {
        static void Main(string[] args)
        {
            // Запустити сервер в окремому потоці
            Task serverTask = Task.Run(() => StartServer());

            // Дати серверу час запуститися
            System.Threading.Thread.Sleep(1000);

            // Запустити кілька клієнтів для запиту на обчислення
            Task.Run(() => StartClient());
            Task.Run(() => StartClient());
            System.Threading.Thread.Sleep(1000);
            Task.Run(() => StartClient());


            // Демонструємо можливість відправки запиту на обчислення при натисканні Enter в будь-який момент
            for (int i = 0; i < 10; i++)
            {
                Console.ReadLine();
                Task.Run(() => StartClient());
            }

            Task.WaitAll(serverTask);
        }

        static void StartServer()
        {
            Console.WriteLine("Server started.");
            var server = new Server();
            server.Run();
        }

        static void StartClient()
        {
            Console.WriteLine("Client started.");
            var client = new Client();
            client.Run();
        }
    }
}
