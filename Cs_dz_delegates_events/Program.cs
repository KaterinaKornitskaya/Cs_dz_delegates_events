using System;
using System.Security.Cryptography.X509Certificates;

namespace Cs_dz_delegates_events
{
    // Создайте набор методов для работы с массивами:
    // ■ Метод для получения всех четных чисел в массиве;
    // ■ Метод для получения всех нечетных чисел в массиве;
    // ■ Метод для получения всех простых чисел в массиве;
    // ■ Метод для получения всех чисел Фибоначчи в массиве.
    // Используйте механизмы делегатов

    public delegate void MyDel(int[] arr);  // объявление делегата
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3, 4, 5, 8, 9, 11, 13, 14, 17, 19, 21, 23, 24 };

            Console.WriteLine("My Array:");

            for (int i = 0; i < arr.Length; i++)
            {
               Console.Write(arr[i] + " ");
            }
          
            MyDel del = new MyDel(IsEven);  // инициализыция делегата методом
            // подписка других методов на делегат:
            del += IsOdd;  
            del += IsPrime;
            del += IsFib;

            del(arr);
        }


        public static void IsEven(int[] arr)  // Метод для получения всех четных чисел в массиве
        {
            Console.WriteLine("\nEven:");
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 == 0)
                    Console.Write(arr[i] + " ");
            }
        }
        public static void IsOdd(int[] arr)  // Метод для получения всех нечетных чисел в массиве
        {
            Console.WriteLine("\nOdd:");
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 != 0)
                    Console.Write(arr[i] + " ");
            }
        }

        public static void IsPrime(int[] arr)  // Метод для получения всех простых чисел в массиве
        {
            Console.WriteLine("\nSimple:");
            for (int i = 2; i < arr.Length; i++)
                if (_IsPrime(arr[i]) )
                {
                    Console.Write(arr[i] + " ");
                }
        }

        public static void IsFib(int[] arr)  // Метод для получения всех чисел Фибоначчи в массиве
        {
            Console.WriteLine("\nFib:");
            for (int i = 2; i < arr.Length; i++)
                if (_IsFib(arr[i]))
                {
                    Console.Write(arr[i] + " ");
                }
        }

        public static bool _IsPrime(int n)  // метод для определения, что число простое
        {          
            int div;
            for (div = 2; div <= Math.Sqrt( n); div++)
            {
                if (n % div == 0)
                    return false;               
            }
            return true;
        }

        public static bool _IsFib(int n)  // метод для определения числа Фибоначчи
        {
            int a = 0;
            int b = 1;
            while (b < n)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }
            return b == n;
        }
    }    
}