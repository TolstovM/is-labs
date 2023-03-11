using System;

namespace lab1
{
    class Program
    {
        // Объявление констант
        const int N = 8; // Число раундов
        const UInt16 F16 = 0xFFFF; // 16 разрядное число
        const int razmer_K = 64;

        // Исходное сообщение
        static UInt64 msg = 0x123456789ABCDEF0;
        static UInt64 K = 0x96EA704CFB1CF672;  // исходный ключ (64 битный)

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

                // Вывод подблоков на входе раунда (для отладки)
                Console.WriteLine("in {0} z1 = {1:X}; z2 = {2:X}; z3 = {3:X}; z4 = {4:X}", i, block1, block2, block3, block4);

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
                Console.WriteLine("in {0} z1 = {1:X}; z2 = {2:X}; z3 = {3:X}; z4 = {4:X}", i, block1, block2, block3, block4);

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

                Console.WriteLine("in {0} z1 = {1:X}; z2 = {2:X}; z3 = {3:X}; z4 = {4:X}", i, block1, block2, block3, block4);

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
                Console.WriteLine("in {0} z1 = {1:X}; z2 = {2:X}; z3 = {3:X}; z4 = {4:X}", i, block1, block2, block3, block4);

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
            // Исходное сообщение
            Console.WriteLine("{0:X}", msg);

            // Зашифрованное сообщение
            UInt64 c_msg = encode(msg);
            Console.WriteLine("{0:X}", c_msg);

            // Расшифрованное сообщение
            UInt64 msg_ = decode(c_msg);
            Console.WriteLine("{0:X}", msg_);
        }
    }
}
