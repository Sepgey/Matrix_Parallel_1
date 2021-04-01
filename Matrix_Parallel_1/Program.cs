using System;
using System.Diagnostics;
using System.Threading;

namespace Matrix_Parallel_1
{
    class Program
    {
        const int Size = 10;
        private static Random random = new Random(DateTime.Now.Millisecond);
        private static int[,] Multiply(int[,] firstMatrix, int[,] secondMatrix)
        {
            int[,] matrix = new int[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < Size; k++)
                    {
                        matrix[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                    }
                }
            }
            return matrix;
        }

        private static int[,] MultiplyParallel(int[,] matrixFirst, int[,] matrixSecond)
        {

            int[,] matrix = new int[Size, Size];
            int totalSize = Size * Size;
            int systemConcurrency = Environment.ProcessorCount;
            int cellsPerThread = totalSize / systemConcurrency;
            Semaphore semaphore = new Semaphore(systemConcurrency, systemConcurrency);
            for (int thread = 0; thread < systemConcurrency; thread++)
            {
                int localThread = thread;
                new Thread(() =>
                {
                    semaphore.WaitOne();
                    int start = localThread * cellsPerThread;
                    for (int cell = start; cell < start + cellsPerThread; cell++)
                    {
                        int i = cell / Size;
                        int j = cell % Size;

                        for (int k = 0; k < Size; k++)
                        {
                            matrix[i, j] += matrixFirst[i, k] * matrixSecond[k, j];
                        }
                    }
                    semaphore.Release();
                }).Start();
            }

            Thread.Sleep(7);

            for (int i = 0; i < systemConcurrency; i++)
            {
                semaphore.WaitOne();
            }

            return matrix;
        }
        static void Main(string[] args)
        {
            var matrix1 = GenerateMatrix();
            var matrix2 = GenerateMatrix();

            Console.WriteLine("Matrix 1: ");
            PrintMatrix(matrix1);

            Console.WriteLine("Matrix 2: ");
            PrintMatrix(matrix2);

            var stopwatch = Stopwatch.StartNew();

            var result = MultiplyParallel(matrix1, matrix2);

            stopwatch.Stop();

            long elapsedTime = stopwatch.ElapsedMilliseconds;

            Console.WriteLine(elapsedTime);

            Console.WriteLine("Matrix Result: ");
            PrintMatrix(result);
        }

        private static int[,] GenerateMatrix()
        {
            int[,] matrix = new int[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    matrix[i, j] = random.Next(0, 10);
                }
            }

            return matrix;
        }

        private static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Console.Write("{0} ", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
