using System;
using System.Numerics;
using System.Text;

namespace lab4
{
    class Program
    {

        // Функция для преобразования пары чисел в коды символов
        static String parse_str(String str)
        {
            StringBuilder text = new StringBuilder();
            Byte[] bytes = new Byte[str.Length / 2];
            for (int i = 0; i < str.Length / 2; i++)
            {
                String num = str.Substring(2 * i, 2);
                bytes[i] = Byte.Parse(num);
            }
            return ASCIIEncoding.ASCII.GetString(bytes);
        }

        static void Main(string[] args)
        {
            // Заданы
            BigInteger e = 17731;
            BigInteger n = 471120228690799;
            // Вычисляем онлайн калькулятором wolframalpha
            BigInteger p = 2432119;
            BigInteger q = 193707721;
            // На их основе вычисляем функцию Эйлера
            BigInteger phi = (p - 1) * (q - 1);
            Console.WriteLine("phi = {0}", phi);
            // Вычисляем d на основе расширенного алгоритма Евклида
            // (реализуйте самостоятельно)
            BigInteger d = 14029179244651; // обратный элемент для e по модулю phi
            Console.WriteLine("d = {0}", d);
            // Формируем массив строк зашифрованного сообщения
            String[] c_text = "89847564151340,187749689769452,346266984369652,39101417375771,264912762521964".Split(',');
            StringBuilder text = new StringBuilder();
            // Для каждой зашифрованной строки
            foreach (String c_str in c_text)
            {
                BigInteger c_msg = UInt64.Parse(c_str); // преобразуем в большое целое число
                BigInteger msg = BigInteger.ModPow(c_msg, d, n); // дешифруем
                text.Append(msg.ToString()); // дописываем дешифрованное число в строку
            }
            // Получаем строку чисел, соответствующую расшифрованному сообщению
            String str = text.ToString();
            // Преобразуем пары чисел в коды символов и выводим результат
            Console.WriteLine(parse_str(str));
            Console.WriteLine(BigInteger.ModPow(new BigInteger(90786575797765), e, n));
        }
    }
}
