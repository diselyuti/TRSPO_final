using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TRSPO_final
{
    public class Client
    {
        private const int Port = 12345;
        private const string ServerAddress = "127.0.0.1";
        private const int MinMatrixSize = 10;

        public void Run()
        {
            // Генерація та заповнення рандомними значеннями матриць NxM та MxL, де N,M,L >= 1000
            var rnd = new Random();
            var N = MinMatrixSize + rnd.Next(2);
            var M = MinMatrixSize + rnd.Next(2);
            var L = MinMatrixSize + rnd.Next(2);
            var matrix1 = new MatrixRequestContainer(GenerateMatrix(N, M), true);
            var matrix2 = new MatrixRequestContainer(GenerateMatrix(M, L), true);

            // Встановлення з'єднання з сервером через TCP/IP
            using (var client = new TcpClient(ServerAddress, Port))
            using (var stream = client.GetStream())
            {
                var formatter = new BinaryFormatter();
                // Передача матриць
                formatter.Serialize(stream, matrix1);
                formatter.Serialize(stream, matrix2);

                // Отримання результату обчислень
                var resultMatrix = (MatrixRequestContainer)formatter.Deserialize(stream);

                if (!resultMatrix.ok)
                {
                    Console.WriteLine("Client message: " + resultMatrix.error);
                    return;
                }
                
                // Виведення в консоль результатів обчислень
                DisplayMatrix(resultMatrix.matrix);
            }
        }

        private int[,] GenerateMatrix(int rows, int cols)
        {
            var matrix = new int[rows, cols];
            var rand = new Random();
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = rand.Next(10);
            return matrix;
        }

        private void DisplayMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write($"{matrix[i, j]} ");
                Console.WriteLine();
            }
        }
    }
}
