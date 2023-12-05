using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TRSPO_final
{
    public class Server
    {
        private const int Port = 12345;

        public void Run()
        {
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            while (true)
            {
                // Постійно слухаємо TCP/IP порт
                var client = listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                using (var stream = client.GetStream())
                {
                    var formatter = new BinaryFormatter();

                    // Приймаємо матриці
                    var matrix1 = (MatrixRequestContainer)formatter.Deserialize(stream);
                    var matrix2 = (MatrixRequestContainer)formatter.Deserialize(stream);

                    if (!matrix1.ok || !matrix2.ok) {
                        const string errorMessage = "Server error while getting matrixes";
                        Console.WriteLine("Server message: " + errorMessage);
                        formatter.Serialize(stream, new MatrixRequestContainer(new int[0,0], false, errorMessage));
                        return;
                    }

                    if (matrix1.matrix.GetLength(0) != matrix2.matrix.GetLength(1))
                    {
                        const string errorMessage = "Server error Matrixes can't be multiplied";
                        Console.WriteLine("Server message: " + errorMessage);
                        formatter.Serialize(stream, new MatrixRequestContainer(new int[0, 0], false, errorMessage));
                        return;
                    }

                    // Обчислення
                    var resultMatrix = new MatrixRequestContainer(MultiplyMatrices(matrix1.matrix, matrix2.matrix), true);

                    // Відправка результатів
                    formatter.Serialize(stream, resultMatrix);
                }
            }
            finally
            {
                client.Close();
            }
        }

        private int[,] MultiplyMatrices(int[,] matrix1, int[,] matrix2)
        {
            var result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];

            // Використовуємо паралельні обчислення для перемноження матриць
            Parallel.For(0, matrix1.GetLength(0), i =>
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix1.GetLength(1); k++)
                        result[i, j] += matrix1[i, k] * matrix2[k, j];
                }
            });
            return result;
        }
    }
}
