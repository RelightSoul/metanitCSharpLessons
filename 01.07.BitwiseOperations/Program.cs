﻿//Логические операции
// Особый класс операций представляют поразрядные операции. Они выполняются над отдельными
// разрядами числа. В этом плане числа рассматриваются в двоичном представлении, например,
// 2 в двоичном представлении 10 и имеет два разряда, число 7 - 111 и имеет три разряда.

//&(логическое умножение)
int x1 = 2;  //010
int y1 = 5;  //101
Console.WriteLine(x1&y1);  //0   (0*1,1*0,0*1)  000=0

int x2 = 4; //100
int y2 = 5; //101
Console.WriteLine(x2&y2);  //4   (1*1,0*0,0*1)  100=4

// | (логическое сложение)

int j1 = 2; //010
int n1 = 5; //101
Console.WriteLine(j1 | n1); // выведет 7 - 111
int j2 = 4; //100
int n2 = 5; //101
Console.WriteLine(j2 | n2); // выведет 5 - 101

// ^ (логическое исключающее ИЛИ)
int x = 45; // Значение, которое надо зашифровать - в двоичной форме 101101
int key = 102; //Пусть это будет ключ - в двоичной форме 1100110

int encrypt = x ^ key; //Результатом будет число 1001011 или 75
Console.WriteLine($"Зашифрованное число: {encrypt}");

int decrypt = encrypt ^ key; // Результатом будет исходное число 45
Console.WriteLine($"Расшифрованное число: {decrypt}");

// ~ (логическое отрицание или инверсия)  -  инвертирует все разряды
int l = 12;                 // 00001100
Console.WriteLine(~l);      // 11110011   или -13


// Операции сдвига

//x << y - сдвигает число x влево на y разрядов. Например, 4<<1 сдвигает число 4
//(которое в двоичном представлении 100) на один разряд влево, то есть в итоге
//получается 1000 или число 8 в десятичном представлении.

//x>>y - сдвигает число x вправо на y разрядов. Например, 16>>1 сдвигает число 16
//(которое в двоичном представлении 10000) на один разряд вправо, то есть в итоге
//получается 1000 или число 8 в десятичном представлении.