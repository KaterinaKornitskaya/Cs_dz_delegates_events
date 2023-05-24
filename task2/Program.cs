namespace task2
{
    // Задание 2
    // Создайте набор методов:
    // ■ Метод для отображения текущего времени;
    // ■ Метод для отображения текущей даты;
    // ■ Метод для отображения текущего дня недели;
    // ■ Метод для подсчёта площади треугольника;
    // ■ Метод для подсчёта площади прямоугольника.
    // Для реализации проекта используйте делегаты Action, Predicate, Func.
    
    internal class Program
    {
        // объявление делегата
        static Func<double, double, double>? func;

        // объявление делегата
        static Action<string>? ShowTime 
            = (Time)=>Console.WriteLine($"Time: {DateTime.Now.ToString(Time)}");

        static Action<string> act;
        static void Main(string[] args)
        {
            func = TriangleS;
            Console.WriteLine($"Trian: {func(3,4)}");

            func = RectangleS;
            Console.WriteLine($"Rect: {func(3, 4)}");
           
            // отображения текущего времени
            ShowTime("h:mm:ss");
            // отображения текущей даты
            ShowTime("dd.MM.yy");
            // отображения текущего дня недели
            ShowTime("dddd");
        }
      
        static double TriangleS(double a, double h)  // метод Площадь Треугольника
        {
            return (a/2)*h;
        }
        static double RectangleS(double a, double b)  // метод Площадь Прямоугольника
        {
            return a*b;
        }
    }
}