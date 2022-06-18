﻿// Кроме стандартных значений типа чисел, строк, язык C# имеет специальное значение - null,
// которое фактически указывает на отсутствие значения как такового, отсутствие данных.
// До сих пор значение null выступает как значение по умолчанию для ссылочных типов.

//  До версии C# 8.0 всем ссылочным типам спокойно можно было присваивать значение null:

string name = null;
Console.WriteLine(name);

// Но начиная с версии C# 8.0 в язык была введена концепция ссылочных nullable-типов (nullable reference types)
// и nullable aware context - nullable-контекст, в котором можно использовать ссылочные nullable-типы.

string? name2 = null;
Console.WriteLine(name2);
//  Чтобы определить переменную/параметр ссылочного типа, как переменную/параметр, которым можно
//  присваивать значение null, после названия типа указывается знак вопроса ?

//  Зачем нужно это значение null? Стандартный пример - работа с базой данных, которая может содержать
//  значения null. И мы можем заранее не знать, что мы получим из базы данных - какое-то определенное
//  значение или же null.

//  Для nullable-контекста характерны следующие особенности:

//  Переменную ссылочного типа следует инициализировать конкретным значением, ей не следует
//  присваивать значение null

//  Переменной ссылочного nullable-типа можно присвоить значение null, но перед использование
//  необходимо проверять ее на значение null.

//  Начиная с .NET 6 и C# 10 nullable-контекст по умолчанию распространяется на все файлы кода в проекта.
//  Например, если мы наберем в Visual Studio 2022 для проекта .NET 6 предыдущий пример,
//  то мы столкнемся с предупреждением:
string name3 = null;

//  В директории Edit Project File(Изменить фаул проекта) мы может найти строчку:

//              < Nullable > enable </ Nullable >     
//  enable указывает, что эта nullable-контекст будет распространяться на весь проект

//  Отключив nullable-контекст, мы больше не сможем использовать в файлах кода в проекте ссылочные
//  nullable-типы и соответственно воспользоваться встроенным статическим анализом потенциально
//  опасных ситуаций, где можно столкнуться с NullReferenceException.
//  Это значит что нам нужно тщательнее заботиться об опасных местах кода и указывать условия и Exception,
//  чтобы не получать вылеты во время выполнения. 

#region nullable-контекст на уровне участка кода
//  Мы также можем включить nullable-контекст на урове отдельных участков кода с помощью директивы
//  #nullable enable. Допустим, глобально у нас отключен nullable-контекст
//      <Nullable>disable</Nullable>

#nullable enable // включаем nullable-контекст на уровне файла
string? name4 = null;
PrintUpper(name4);
void PrintUpper(string? text)
{
    Console.WriteLine(text.ToUpper());
}
#endregion

#region  Оператор ! (null-forgiving operator)
//  Оператор!(null - forgiving operator) позволяет указать, что переменная ссылочного типа не равна null
string? name5 = null;

PrintUpper2(name5!);

void PrintUpper2(string text)
{
    if (text == null) Console.WriteLine("null");
    else Console.WriteLine(text.ToUpper());
}
#endregion

#region Исключение кода из nullable-контекста
//       <Nullable>enable</Nullable>
#nullable disable
string text = null; // здесь nullable-контекст не действует
#nullable restore
#endregion