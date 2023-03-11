using System;

namespace lab1
{
    class Program
    {
        // Объявление констант
        const int N = 8; // Число раундов
        const UInt16 F16 = 0xFFFF; // 16 разрядное число
        const int razmer_K = 64;

        static UInt64 K = 0x96EA704CFB1CF672;  // исходный ключ (64 битный)
        // Для реализации различных режимов шифрования (лаб 2)
        // Исходное сообщение (1й,2й блоки одинаковы, равно как и 3й с 4м)
        static UInt64[] msg = { 0x123456789ABCDEF0, 0x123456789ABCDEF0, 0x1FBA85C953ABCFD0, 0x1FBA85C953ABCFD0 };
        // Вектор для дополнительной шифровки первого блока сообщения (в режимах CBC и OFB)
        static UInt64 IV = 0x18FD47203C7A23BC;  // инициализационный вектор
        const int B = 4; // число блоков в исходном сообщении

        // Циклический сдвиг вправо для 32 бит
        static UInt16 cyclicShiftRight16(UInt16 x, int t)
        {
            return ((UInt16)((x >> t) | (x << (16 - t))));
        }

        // Циклический сдвиг вправо для 64 бит
        static UInt64 cyclicShiftRight64(UInt64 x, int t)
        {
            return ((x >> t) | (x << (64 - t)));
        }

        // Циклический сдвиг влево для 16 бит
        static UInt16 cyclicShiftLeft16(UInt16 x, int t)
        {
            return ((UInt16)((x << t) | (x >> (16 - t))));
        }

        // Циклический сдвиг влево для 64 бит
        static UInt64 cyclicShiftLeft64(UInt64 x, int t)
        {
            return ((x << t) | (x >> (64 - t)));
        }

        // Генерация 16 разрядного ключа на i-м раунде из исходного 64-разрядного
        static UInt16 getKey16(int i)
        {
            return (UInt16)cyclicShiftRight64(K, i * 4);	// циклический сдвиг на 8 бит и обрезка правых 48 бит
        }

        // Образующая функция - функция, шифрующая половину блока polblok ключом K_i на i-м раунде
        static UInt16 F(UInt16 polblok, UInt16 K_i)
        {
            UInt16 f1 = cyclicShiftLeft16(polblok, 7);
            UInt16 f2 = (UInt16)(cyclicShiftRight16(K_i, 7) | polblok);
            UInt16 f3 = cyclicShiftLeft16(f2, 7);
            return (UInt16)(f1 ^ f2 ^ f3);
        }

        // Шифрование 64 разрядного блока
        static UInt64 encode(UInt64 block)
        {
            UInt16 block1 = (UInt16)((block >> 48) & F16);
            UInt16 block2 = (UInt16)((block >> 32) & F16);
            UInt16 block3 = (UInt16)((block >> 16) & F16);
            UInt16 block4 = (UInt16)(block & F16);

            // Выполняются 8 раундов шифрования
            for (int i = 0; i < N; i++)
            {
                UInt16 key = getKey16(i); // генерация ключа для i-го раунда
                // На i-м раунде значения подблоков изменяются	
                UInt16 block1_i = block1;
                UInt16 block2_i = (UInt16)(block2 ^ F(block1, key));
                UInt16 block3_i = (UInt16)(block3 ^ F(block1, key));
                UInt16 block4_i = (UInt16)(block4 ^ F(block1, key));


                // Если раунд не последний, то
                if (i < N - 1)
                {
                    block1 = block2_i;
                    block2 = block3_i;
                    block3 = block4_i;
                    block4 = block1_i;
                }
                else // После последнего раунда блоки не меняются местами
                {
                    block1 = block1_i;
                    block2 = block2_i;
                    block3 = block3_i;
                    block4 = block4_i;
                }

            }

            // После всех раундов шифрования объединяем в один большой шифрованный блок (64 битный)
            UInt64 encodedBlock = block1;
            encodedBlock = (encodedBlock << 16) | (UInt16)(block2 & F16);
            encodedBlock = (encodedBlock << 16) | (UInt16)(block3 & F16);
            encodedBlock = (encodedBlock << 16) | (UInt16)(block4 & F16);
            // Возвращаем зашифрованный блок
            return encodedBlock;
        }

        // Расшифровка 64 разрядного блока
        static UInt64 decode(UInt64 block)
        {
            UInt16 block1 = (UInt16)((block >> 48) & F16);
            UInt16 block2 = (UInt16)((block >> 32) & F16);
            UInt16 block3 = (UInt16)((block >> 16) & F16);
            UInt16 block4 = (UInt16)(block & F16);

            // Выполняются 8 раундов шифрования
            for (int i = N - 1; i >= 0; i--)
            {
                UInt16 key = getKey16(i); // генерация ключа для i-го раунда
                // На i-м раунде значения подблоков изменяются	
                UInt16 block1_i = block1;
                UInt16 block2_i = (UInt16)(block2 ^ F(block1, key));
                UInt16 block3_i = (UInt16)(block3 ^ F(block1, key));
                UInt16 block4_i = (UInt16)(block4 ^ F(block1, key));


                // Если раунд не последний, то
                if (i > 0)
                {
                    block1 = block4_i;
                    block2 = block1_i;
                    block3 = block2_i;
                    block4 = block3_i;
                }
                else // После последнего раунда блоки не меняются местами
                {
                    block1 = block1_i;
                    block2 = block2_i;
                    block3 = block3_i;
                    block4 = block4_i;
                }

            }

            // После всех раундов шифрования объединяем левый и правый подблоки в один большой шифрованный блок (64 битный)
            UInt64 decodedBlock = block1;
            decodedBlock = (decodedBlock << 16) | (UInt16)(block2 & F16);
            decodedBlock = (decodedBlock << 16) | (UInt16)(block3 & F16);
            decodedBlock = (decodedBlock << 16) | (UInt16)(block4 & F16);
            // Возвращаем зашифрованный блок
            return decodedBlock;
        }

        static void Main(string[] args)
        {
            // Отображаем на консоли исходный ключ K (и IV) для зашифровки и исходное сообщение text (все это объявлено в первых строках)
            Console.WriteLine("Init Key {0:X}", K);     // большой ключ K (64 бит) из битов которого создаются маленькие ключи K_i (по 32 бита)

            Console.WriteLine("Init V {0:X}", IV);      // дополнительный ключ (вектор) для шифровки первого блока сообщения врежимах CBC и OFB (64 бит)

            // Вывод блоков сообщения до шифрования
            Console.WriteLine("Text (message blocks)");
            for (int b = 0; b < B; b++)
                Console.Write("{0:X} ", msg[b]);    // выводим очередной блок сообщения

            // 1. Шифрование

            // 1.1. Шифрование в режиме ECB (электронная кодовая книга)
            UInt64[] msg_ecb = new UInt64[B];
            Console.WriteLine("\nShifr ECB:");

            // Шифрование последовательно каждого блока без дополнительных преобразований
            for (int b = 0; b < B; b++)
            {
                msg_ecb[b] = encode(msg[b]);     // шифруем блок
                Console.Write("{0:X} ", msg_ecb[b]);    // выводим очередной блок сообщения	// выводим зашифрованный блок на консоль
                                                        // В зашифрованном тексте 1й и 2й блоки одинаковы (3й с 4м тоже) как и в исходном сообщении - это недостаток режима ECB
            }

            // 1.2. Шифрование в режиме CBC (режим сцепления блоков шифротекста)
            UInt64[] msg_cbc = new UInt64[B];
            Console.WriteLine("\nShifr CBC:");
            // Первый блок сообщения xor'ится с IV перед шифрованием:
            UInt64 blok = msg[0] ^ IV;
            msg_cbc[0] = encode(blok); // шифруем блок
            Console.Write("{0:X} ", msg_cbc[0]);    // выводим зашифрованный первый блок на консоль

            // Каждый последующий блок перед шифрованием xor'ится с предыдущим зашифрованным блоком:
            for (int b = 1; b < B; b++)
            {
                blok = msg[b] ^ msg_cbc[b - 1]; // xor с предыдущим зашифрованным
                msg_cbc[b] = encode(blok); // шифруем блок
                Console.Write("{0:X} ", msg_cbc[b]);    // выводим зашифрованный блок на консоль
                                                        // В зашифрованном тексте все блоки будут разными, не смотря на то что в исходном сообщении они повторялись
            }

            // 2. Расшифрование
            // 2.1. Расшифровка в режиме ECB (электронная кодовая книга)
            UInt64 msg_b;   // блок расшифрованного текста
            Console.WriteLine("\nText ECB:");
            // Расшифровка последовательно каждого блока без дополнительных преобразований
            for (int b = 0; b < B; b++)
            {
                msg_b = decode(msg_ecb[b]);   // расшифровка блока
                Console.Write("{0:X} ", msg_b);     // выводим расшифрованный блок на консоль
            }

            // 2.2. Расшифровка в режиме CBC (режим сцепления блоков шифротекста)
            Console.WriteLine("\nText CBC:");
            // Первый блок сообщения xor'ится с IV после расшифровки:
            msg_b = decode(msg_cbc[0]);   // расшифровка блока
            msg_b ^= IV; // xor'им с IV после расшифровки
            Console.Write("{0:X} ", msg_b); // выводим расшифрованный первый блок на консоль
                                            // Каждый последующий блок после расшифровки xor'ится с предыдущим зашифрованным блоком:
            for (int b = 1; b < B; b++)
            {
                msg_b = decode(msg_cbc[b]);   // расшифровка блока
                msg_b ^= msg_cbc[b - 1];        // xor с предыдущим зашифрованным
                Console.Write("{0:X} ", msg_b); // выводим расшифрованный блок на консоль
            }
        }
    }
}
