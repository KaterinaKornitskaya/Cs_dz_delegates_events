using System.Net.NetworkInformation;

namespace task3
// Задание 3
// Создайте класс «Кредитная карточка». Класс должен содержать:
// ■ Номер карты;
// ■ ФИО владельца;
// ■ Срок действия карты;
// ■ PIN;
// ■ Кредитный лимит;
// ■ Сумма денег.
// Создайте необходимый набор методов класса. Реализуйте события для следующих ситуаций:
// ■ Пополнение счёта;
// ■ Расход денег со счёта;
// ■ Старт использования кредитных денег;
// ■ Достижение заданной суммы денег;
// ■ Смена PIN.
{
    internal class Program
    {
        public static void IventMoney(string message)  // метод для события
        {
            Console.WriteLine(message);
        }

        static void Main(string[] args)
        {
            CreditCard user = new CreditCard();  // создание и инициализация объекта класса Кредитная карты
            user.MoneySpend += IventMoney;       // подписка на события
            user.TopUp += IventMoney;
            user.StartCredit += IventMoney;
            user.PinChange += IventMoney;

            bool exit = false;  // переменная для цикла
            while (!exit)
            {
                Console.WriteLine("\n***************************");
                Console.WriteLine("1. Пополнение счёта");
                Console.WriteLine("2. Снятие денег со счёта");
                Console.WriteLine("3. Показать информацию о счёте");
                Console.WriteLine("4. Изменить пин-код");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите действие: ");
                int num = int.Parse(Console.ReadLine());
                switch (num)
                {
                    case 1:
                        user.CardReplenishment();
                        break;
                    case 2:
                        user.CardSpending();
                        break;
                    case 3:
                        user.ShowAccountInfo();
                        break;
                    case 4:
                        user.ToChangePin();
                        break;
                    case 5:
                        exit = true;
                        break;
                }
            }
        }

        class CreditCard  // класс Кредитная карта
        {
            long cardNumber;         // номер карты
            string cardholderName;   // имя владельца карты
            DateTime expiryDate;     // срок действия карты
            int pinCode;             // пин код
            int creditLimit;         // кредитный лимит
            int moneyAmount;         // баланс личных средств     
            const int CREDIT = 500;  // константное значение кредитного лимита

            public event Action<string> TopUp;        // событие Пополнение карты
            public event Action<string> MoneySpend;   // событие Трата денег
            public event Action<string> StartCredit;  // событие Использование кредитного лимита
            public event Action<string> PinChange;    // событие Изменение пин-кода

            public CreditCard()  // конструктор по умолчанию
            {
                cardNumber = 1111222233334444;
                cardholderName = "Kornytska Kateryna Sergiivna";
                expiryDate = new DateTime(2024, 11, 01);
                pinCode = 9999;               
                creditLimit = CREDIT;
                moneyAmount = 300;
            }

            public void CardReplenishment()  // метод Пополнение карты
            {               
                Console.WriteLine("Введите пин-код:");
                if (expiryDate <= DateTime.Now)                      // если срок действия карты меньше даты сегодня
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Срок действия карты истек");  // вывод сообщения "Срок истек"
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
                else if (int.Parse(Console.ReadLine()) == pinCode)  // если пин-код введен верно
                {
                    ShowBalanceInfo();                              // вызов метода Показать Баланс
                    Console.WriteLine("Пополнение карты. Введите сумму пополнения:");
                    int num = int.Parse(Console.ReadLine());        // считываем сумму пополнения
                    if (creditLimit < CREDIT)                       // если текущий кредитный лимит меньше константного значения
                    {
                        if (num <= (CREDIT - creditLimit))          // если сумма пополнения меньше, чем нужно на покрытие кредита
                        {
                            creditLimit += num;                     // сумма пополнения идет в счет погашения кредита
                        }
                        else if (num > (CREDIT - creditLimit))      // если сумма пополнения больше, чем нужно на покрытие кредита
                        {
                            moneyAmount += (num - (CREDIT - creditLimit));  // сумма после погашения кредита идет в личные средства
                            creditLimit += (CREDIT - creditLimit);          // кредитный лимит погашается до константного значения
                        }
                    }
                    else if (creditLimit >= CREDIT)  // если кредитный лимит не использован
                    {
                        moneyAmount += num;          // сумма пополнения идет в личные средства
                    }
                    Console.WriteLine($"Вы пополнили счет на {num} грн.");
                    ShowBalanceInfo();               // вызов метода Показать Баланс
                   
                    TopUp?.Invoke($"СОБЫТИЕ ПОПОЛНЕНИЕ СЧЕТА");  // вызов события
                }
                else  // если пин-код введен НЕверно
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный пин-код!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            public void CardSpending()  // метод Трата с карты
            {
                
                Console.WriteLine("Введите пин-код:");
                if (expiryDate <= DateTime.Now)                      // если срок действия карты меньше даты сегодня
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Срок действия карты истек");  // вывод сообщения "Срок истек"
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (int.Parse(Console.ReadLine()) == pinCode)        // если пин-код введен верно
                {
                    ShowBalanceInfo();                               // вызов метода Показать Баланс
                    Console.WriteLine($"Снятие с карты. Введите сумму снятия:");
                    int num = int.Parse(Console.ReadLine());
                    if (num <= moneyAmount)                          // если личных средств хватает на покрытие траты
                    {
                        moneyAmount -= num;                          // берем средства с личных
                        Console.WriteLine($"Вы сняли с карты {num} грн.");
                       
                        MoneySpend?.Invoke($"СОБЫТИЕ СНЯТИЕ ДЕНЕГ");  // вызов события
                    }
                    else if (num > moneyAmount 
                        && num <= (creditLimit+moneyAmount))          // если на покрытие траты нужны также кредитные
                    {
                        int x = num - moneyAmount;                    // считаем разницу трата минус личные средства
                        creditLimit -= x;                             // эту разницу забираем из кредитных средств
                        moneyAmount = 0;                              // личные средства равны 0
                        Console.WriteLine($"Вы сняли с карты {num} грн.");
                        MoneySpend?.Invoke($"СОБЫТИЕ СНЯТИЕ ДЕНЕГ");                      // вызов события
                        StartCredit?.Invoke($"СОБЫТИЕ ИСПОЛЬЗОВАНИЕ КРЕДИТНЫХ СРЕДСТВ");  // вызов события
                    }
                    else  // если на трату нехватает средств (личных и кредитных)
                    {
                        Console.WriteLine($"Ошибка, недостаточно средств");
                    }
                    ShowBalanceInfo();  // вызов метода Показать Баланс
                }
                else  // если пин-код введен НЕверно
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный пин-код!");
                    Console.ForegroundColor = ConsoleColor.White;
                }                                 
            }

            public void ToChangePin()  // метод Изменить пин-код
            {
                Console.WriteLine("Изменение пин-кода. Введите новый пин-код:");
                pinCode = int.Parse(Console.ReadLine());          // считываем новый пин-код

                PinChange?.Invoke("СОБЫТИЕ ИЗМЕНЕНИЕ ПИН-КОДА");  // вызов события
            }

            public void ShowBalanceInfo()  // метод Показать Баланс
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Текущий баланс своих денег {moneyAmount} грн." +
                    $"\nТекущий баланс кредитных денег {creditLimit} грн.");
                Console.ForegroundColor = ConsoleColor.White;
            }

            public void ShowAccountInfo()  // метод Показать информацию о счете
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine
                    ($"\nCard number: {cardNumber}" +
                    $"\nCardholder name: {cardholderName}" +
                    $"\nExpiry Date: {expiryDate}" +
                    $"\nPIN code: {pinCode}" +
                    $"\nCredit limit: {creditLimit}" +
                    $"\nCurrent balance: {moneyAmount}\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}