﻿//  Дополнительные классы и структуры .NET
//  Отложенная инициализация и тип Lazy

//  Приложение может использовать множество классов и объектов. Однако есть вероятность,
//  что не все создаваемые объекты будут использованы. Особенно это касается больших приложений. Например:
using System;

class Reader
{
    Library library = new Library();

    public void ReadBook()
    {
        library.GetBook();
        Console.WriteLine("Читаем книгу");
    }

    public void ReadEBook()
    {
        Console.WriteLine("Читаем электронную книгу");
    }
}
class Library
{
    private string[] books = new string[99];

    public void GetBook()
    {
        Console.WriteLine("Выдаём книгу читателю");
    }
}
//  Есть класс Library, представляющий библиотеку и хранящий некоторый набор книг в виде массива.
//  Есть класс читателя Reader, который хранит ссылку на объект библиотеки, в которой он записан.
//  У читателя определено два метода: для чтения электронной книги и для чтения обычной книги.
//  Для чтения обычной книги необходимо обратиться к методу класса Library, чтобы получить эту книгу.

//  Но что если читателю вообще не придется читать обычную книгу, а только электронные, например,
//  в следующем случае:
class Program
{
    static void Main(string[] args)
    {
        // ------ Без Lazy
        Reader reader = new Reader();
        reader.ReadEBook();

        // ------ С Lazy
        LazyReader lReader = new LazyReader();
        lReader.ReadEBook();
        lReader.ReadBook();
        //  Непосредственно объект Library задействуется здесь только на третьей строке в методе
        //  reader.ReadBook(), который вызывает в свою очередь метод library.Value.GetBook(). Поэтому
        //  вплоть до третьей строки объект Library, используемый читателем, не будет создан. Если мы
        //  не будем применять в программе метод reader.ReadBook(), то объект библиотеки тогда вобще
        //  не будет создан, и мы избежим лишних затрат памяти. Таким образом, Lazy<T> гарантирует нам,
        //  что объект будет создан только тогда, когда в нем есть необходимость.
    }
}
//  В этом случае объект library в классе читателя никак не будет использоваться и будет только занимать
//  место памяти. Хотя надобности в нем не будет.

//  Для подобных случаев в .NET определен специальный класс Lazy<T>. Изменим класс читателя следующим образом:
class LazyReader
{
    Lazy<Library> lazyLibrary = new Lazy<Library>();
    public void ReadBook()
    {
        lazyLibrary.Value.GetBook();
        Console.WriteLine("Читаем книгу");
    }

    public void ReadEBook()
    {
        Console.WriteLine("Читаем электронную книгу");
    }
}
//  Класс Library остается прежнем. Но теперь класс читателя содержит ссылку на библиотеку в виде объекта
//  Lazy<Library>. А чтобы обратиться к самой библиотеке и ее методам, надо использовать выражение
//  library.Value - это и есть объект Library.