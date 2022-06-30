﻿//  Финализируемые объекты

//  Большинство объектов, используемых в программах на C#, относятся к управляемым или managed-коду. Такие
//  объекты управляются CLR и легко очищаются сборщиком мусора. Однако вместе с тем встречаются также и
//  такие объекты, которые задействуют неуправляемые объекты (подключения к файлам, базам данных, сетевые
//  подключения и т.д.). Такие неуправляемые объекты обращаются к API операционной системы. Сборщик мусора
//  может справиться с управляемыми объектами, однако он не знает, как удалять неуправляемые объекты. В этом
//  случае разработчик должен сам реализовывать механизмы очистки на уровне программного кода.

//  Освобождение неуправляемых ресурсов подразумевает реализацию одного из двух механизмов:
//  Создание деструктора
//  Реализация классом интерфейса System.IDisposable

#region Создание деструкторов
//  Если вы вдруг программировали на языке C++, то наверное уже знакомы с концепцией деструкторов. Метод
//  деструктора носит имя класса (как и конструктор), перед которым стоит знак тильды (~).

//  Деструкторы можно определить только в классах. Деструктор в отличие от конструктора не может иметь
//  модификаторов доступа и параметры. При этом каждый класс может иметь только один деструктор.class Person
class Person
{
    public string Name { get; }
    public Person(string name) => Name = name;

    ~Person()
    {
        Console.WriteLine($"{Name} has deleted");
    }
}
//  В данном случае в деструкторе в целях демонстрации просто выводится строка на консоль, которая уведомляет,
//  что объект удален. Но в реальных программах в деструктор вкладывается логика освобождения неуправляемых
//  ресурсов.

//  Однако на деле при очистке сборщик мусора вызывает не деструктор, а метод Finalize. Все потому, что
//  компилятор C# компилирует деструктор в конструкцию, которая эквивалентна следующей:
//  protected override void Finalize()
//  {
//       try
//       {
//          // здесь идут инструкции деструктора
//       }
//       finally
//       {
//           base.Finalize();
//       }
//  }
//  Метод Finalize уже определен в базовом для всех типов классе Object, однако данный метод нельзя так просто
//  переопределить. И фактическая его реализация происходит через создание деструктора.
#endregion

#region Интерфейс IDisposable
//  Интерфейс IDisposable объявляет один единственный метод Dispose, в котором при реализации интерфейса в
//  классе должно происходить освобождение неуправляемых ресурсов. Например:
class Program
{
    static void Main(string[] args)
    {
        Test();

        void Test()
        {
            Person2? tom = null;
            try
            {
                tom = new Person2("Tom");
            }
            finally
            {
                tom?.Dispose();
            }
        }
    }
}
public class Person2 : IDisposable
{
    public string Name { get; }
    public Person2(string name) => Name = name;

    public void Dispose()
    {
        Console.WriteLine($"{Name} has been disposed");
    }
}
//  В данном коде используется конструкция try...finally. По сути эта конструкция по функционалу в общем
//  эквивалентна следующим двум строкам кода:
//          Person tom = new Person("Tom");
//          tom.Dispose();
//  Но конструкцию try...finally предпочтительнее использовать при вызове метода Dispose, так как она
//  гарантирует, что даже в случае возникновения исключения произойдет освобождение ресурсов в методе Dispose.
#endregion

#region Комбинирование подходов
//  Мы рассмотрели два подхода. Какой же из них лучше? С одной стороны, метод Dispose позволяет в любой
//  момент времени вызвать освобождение связанных ресурсов, а с другой - программист, использующий наш класс,
//  может забыть поставить в коде вызов метода Dispose. В общем бывают различные ситуации. И чтобы сочетать
//  плюсы обоих подходов мы можем использовать комбинированный подход. Microsoft предлагает нам использовать
//  следующий формализованный шаблон:
public class SomeClass : IDisposable
{
    private bool disposed = false;

    // реализация интерфейса IDisposable.
    public void Dispose()
    {
        // освобождаем неуправляемые ресурсы
        Dispose(true);
        // подавляем финализацию
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;
        if (disposing)
        {
            // Освобождаем управляемые ресурсы
        }
        // освобождаем неуправляемые объекты
        disposed = true;
    }

    // Деструктор
    ~SomeClass()
    {
        Dispose(false);
    }
}
//  Логика очистки реализуется перегруженной версией метода Dispose(bool disposing). Если параметр disposing
//  имеет значение true, то данный метод вызывается из публичного метода Dispose, если false - то из деструктора.

//  При вызове деструктора в качестве параметра disposing передается значение false, чтобы избежать очистки
//  управляемых ресурсов, так как мы не можем быть уверенными в их состоянии, что они до сих пор находятся
//  в памяти. И в этом случае остается полагаться на деструкторы этих ресурсов. Ну и в обоих случаях
//  освобождаются неуправляемые ресурсы.

//  Еще один важный момент - вызов в методе Dispose метода GC.SuppressFinalize(this). GC.SuppressFinalize не
//  позволяет системе выполнить метод Finalize для данного объекта. Если же в классе деструктор не определен,
//  то вызов этого метода не будет иметь никакого эффекта.

//  Таким образом, даже если разработчик не использует в программе метод Dispose, все равно произойдет очистка
//  и освобождение ресурсов.
#endregion

#region Общие рекомендации по использованию Finalize и Dispose
//  1.  Деструктор следует реализовывать только у тех объектов, которым он действительно необходим, так как метод
//  Finalize оказывает сильное влияние на производительность

//  2.  После вызова метода Dispose необходимо блокировать у объекта вызов метода Finalize с помощью GC.SuppressFinalize

//  3.  При создании производных классов от базовых, которые реализуют интерфейс IDisposable, следует также
//  вызывать метод Dispose базового класса:
//   public class Derived : Base
//   {
//       private bool IsDisposed = false;

//       protected override void Dispose(bool disposing)
//       {
//           if (IsDisposed) return;
//           if (disposing)
//           {
//               // Освобождение управляемых ресурсов
//           }
//           IsDisposed = true;
//           // Обращение к методу Dispose базового класса
//           base.Dispose(disposing);
//       }
//    }
//  4.  Отдавайте предпочтение комбинированному шаблону, реализующему как метод Dispose, так и деструктор
#endregion