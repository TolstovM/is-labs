using System;
using System.Collections.Generic;

namespace lab3
{
    class Program
    {
        const int M = 430;        
        const int C = 2531;        
        const int p = 11979;        
        const float R0 = 0.51111111f;

        /// Генерация псевдо-случайной последовательности (ПСП)
        static List<float> gen(ref int base_len, ref int rep_len)
        {
            List<float> R = new List<float>();        // массив случайных чисел
            R.Add(R0);        // помещаем в него начальный элемент
            Dictionary<float, int> R_uniq = new Dictionary<float, int>(); // множество уникальных элементов для контроля повторяемости
            R_uniq.Add(R0, 0); // фиксируем индекс начального элемента

            int l = 0;
            int L = 0;       // периоды - l - длина повторяющейся части последовательности; L - номер элемента, с которого начинаются повторения
            for (int i = 1; ; i++)
            {
                // Генерация очередного случайного числа (r_) на основе предыдущего (R.back())
                float r_ = ((int)(R[R.Count - 1] * M + C)) % p;    // новый элемент

                // Проверяем повторение
                if (R_uniq.ContainsKey(r_))   // элемент уже содержится в сгенерированных
                {
                    L = (int)R_uniq.Count;     // общее число неповторяющихся элементов (базовая длина последовательности)
                    l = L - R_uniq[r_];         // длина повторяющейся подпоследовательности (базовая_длина - первый_индекс_повторённого_элемента (R_uniq[r_]))
                    break;
                }
                else
                {
                    R.Add(r_);        // добавляем в ПСП
                    R_uniq.Add(r_, i);
                }
            }
            base_len = L;
            rep_len = l;

            return R;
        }


        static void Main(string[] args)
        {
            int l = 0;
            int L = 0;
            List<float> R = gen(ref L, ref l);

            Console.WriteLine("Element number = " + R.Count + "\nAperiodic length (L) = " + L + "\nl = " + l);
        }
    }
}
