#pragma warning disable CS8602

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // Вариант 22 
        int p = 337;
        int q = 229;
        
        // Функция Эйлера                      
        int fi = (p - 1) * (q - 1); 

        while (true)
        {
            Console.WriteLine("0 - Зашифровать");
            Console.WriteLine("1 - Расшифровать");
            Console.WriteLine("2 - Вывести ключи");
            Console.WriteLine("3 - Факторизация числа и определение закрытого ключа");
            Console.WriteLine("4 - Выход");
            int menu = SafeReadInteger();

            switch (menu)
            {
                case 0:
                    Console.WriteLine("Шифруем. Введите сообщение:");
                    //Выполняется преобразование сообщения в числовое представление
                    string M = StringConvert(Console.ReadLine().ToUpper(), true);

                    Console.WriteLine("Введите открытый ключ:");
                    int keyEncrypt = SafeReadInteger();
                    string? messEncrypt = null; 
                    string block = M[0].ToString();
                    string x;
                    for (int i = 1; i < M.Length; i++)
                    {
                        if (int.Parse(block + M[i]) < (p * q))
                        {
                            block += M[i];
                        }
                        else
                        {
                            x = Exp(int.Parse(block), keyEncrypt, p * q).ToString();
                            messEncrypt = messEncrypt + x.Length.ToString() + x;
                            block = M[i].ToString();
                        }
                    }

                    x = Exp(int.Parse(block), keyEncrypt, p * q).ToString();
                    messEncrypt = messEncrypt + x.Length.ToString() + x;
                    Console.WriteLine("Сообщение зашифровано");
                    Console.WriteLine(messEncrypt.ToString());
                    break;

                case 1:
                    Console.WriteLine("Расшифруем. Введите строку:");
                    string C = Console.ReadLine().ToString();

                    Console.WriteLine("Введите секретный ключ:");
                    int keyDecrypt = SafeReadInteger();
                    string? messDecrypt = null;
                    while (C != "")
                    {
                        messDecrypt += Exp(int.Parse(C.Substring(1, Convert.ToInt32(char.GetNumericValue(C[0])))), keyDecrypt, p * q).ToString();
                        C = C.Remove(0, Convert.ToInt32(char.GetNumericValue(C[0])) + 1);
                    }

                    Console.WriteLine("Сообщение расшифровано");
                    #pragma warning disable CS8604 
                    Console.WriteLine(StringConvert(messDecrypt, false));

                    break;

                case 2:
                    Console.WriteLine("Введите количество ключей:");
                    int keyNumber = SafeReadInteger();
                    int d = 2;
                    for (int i = 0; i < keyNumber; i++)
                    {
                        while (NOD(d, fi) != 1)
                        {
                            d++;
                        }

                        int e = Euclid(d, fi);

                        Console.WriteLine("Открытый ключ ({0}, {1}) Секретный ключ ({2}, {1})", e, p * q, d);
                        d++;
                    }

                    break;

                case 3:
                    Console.WriteLine("Введите число:");
                    double N = SafeReadInteger();
                    Console.WriteLine("Введите открытый ключ:");
                    int publKey = SafeReadInteger();
                    int pF, qF;
                    _ = Factorization(N, out pF, out qF);
                    Console.WriteLine("p = {0}, q = {1}", pF, qF);
                    Console.WriteLine("Секретный ключ: {0}", Euclid(publKey, (pF - 1) * (qF - 1)));
                    break;

                case 4:
                    return;

                default:
                    Console.WriteLine("Ошибка");
                    break;
            }
        }

        // Метод для преобразования букв сообщения в цифры и наоборот 
        static string StringConvert(string a, bool choice)
        {
            //Таблица замен для русского алфавита
            string[] alphabet = ["А10","Б11","В12","Г13","Д14","Е15","Ж16","З17","И18","Й19","К20","Л21",
                                 "М22","Н23","О24","П25","Р26","С27","Т28","У29","Ф30","Х31","Ц32","Ч33",
                                 "Ш34","Щ35","Ъ36","Ы37","Ь38","Э39","Ю40","Я41"," 99"];

            string? b = null;
            if (choice)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (a[i] == alphabet[j][0])
                        {
                            b = b + alphabet[j][1] + alphabet[j][2];
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < a.Length / 2; i++)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        string s1 = new(new char[] { a[i * 2], a[(i * 2) + 1] });
                        string s2 = new(new char[] { alphabet[j][1], alphabet[j][2] });
                        if (s1 == s2)
                        {
                            b += alphabet[j][0];
                        }
                    }
                }
            }

            #pragma warning disable CS8603
            return b;

        }

        // Метод для нахождения обратного элемента по модулю 
        static int Euclid(int a, int m)
        {
            int r;
            int k = 1;

            while (true)
            {
                k += m;
                if (k % a == 0)
                {
                    r = k / a;
                    return r;
                }
            }
        }

        static int NOD(int a, int b)
        {
            while (b != 0)
            {
                if (a > b)
                {
                    a -= b;
                }
                else
                {
                    b -= a;
                }
            }

            return a;
        }
        // Возведение в степень и деление с остатком
        static int Exp(int a, int e, int m)
        {
            long result = a;
            for (int i = 0; i < e - 1; i++)
            {
                result *= a;

                result %= m;

            }

            return (int)result;
        }

        static int SafeReadInteger()
        {
            while (true)
            {
                #pragma warning disable CS8600
                string sValue = Console.ReadLine();

                if (int.TryParse(sValue, out int iValue))
                {
                    return iValue;
                }

                Console.WriteLine("Ошибка");
            }
        }
        // Факторизация методом Ферма
        static int Factorization(double n, out int p, out int q)
        {
            int i = 0;
            double a = Math.Floor(Math.Sqrt(n));
            double b = 0.5;
            while (Math.Sqrt(b) != Math.Floor(Math.Sqrt(b)))
            {
                i++;
                b = ((a + i) * (a + i)) - n;
            }

            b = Math.Sqrt(b);
            p = (int)(a + b + i);
            q = (int)(a - b + i);
            return 0;
        }
    }
}
