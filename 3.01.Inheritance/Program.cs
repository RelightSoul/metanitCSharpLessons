﻿// Наследование (inheritance) является одним из ключевых моментов ООП. Благодаря наследованию
// один класс может унаследовать функциональность другого класса.

class Person
{
    private string _name = "";
    public string Name
    {
        get { return _name; }
        set { _name = value;}
    }

    public Person(string name)
    {
        Name = name;
    }
    public Person() { }

    public void Print()
    {
        Console.WriteLine(Name);
    }
}

//  Вдруг нам потребовался класс, описывающий сотрудника предприятия - класс Employee. Поскольку
//  этот класс будет реализовывать тот же функционал, что и класс Person, так как сотрудник - это
//  также и человек, то было бы рационально сделать класс Employee производным (или наследником,
//  или подклассом) от класса Person, который, в свою очередь, называется базовым классом или
//  родителем (или суперклассом):

class Employee : Person
{
    public void PrintName()
    {
        Console.WriteLine(Name);
        // Console.WriteLine(_name);   нет доступа
    }

}

class Program
{
    static void Main(string[] args)
    {
        Person person = new Person { Name = "Tom"};
        person.Print();
        person = new Employee { Name = "Sam"};
        person.Print();

        Employee2 emp = new Employee2("Mark", "BMW");
    }
}

//  Все классы по умолчанию могут наследоваться. Однако здесь есть ряд ограничений:

//  Не поддерживается множественное наследование, класс может наследоваться только от одного класса.

//  При создании производного класса надо учитывать тип доступа к базовому классу - тип доступа
//  к производному классу должен быть таким же, как и у базового класса, или более строгим.
//  То есть, если базовый класс у нас имеет тип доступа internal, то производный класс может
//  иметь тип доступа internal или private, но не public.

//  Однако следует также учитывать, что если базовый и производный класс находятся в разных сборках
//  (проектах), то в этом случае производый класс может наследовать только от класса, который имеет
//  модификатор public.

//  Если класс объявлен с модификатором sealed, то от этого класса нельзя наследовать
//  и создавать производные классы.

#region Доступ к членам базового класса из класса-наследника
//   производный класс может иметь доступ только к тем членам базового класса, которые определены
//   с модификаторами private protected (если базовый и производный класс находятся в одной сборке),
//   public, internal (если базовый и производный класс находятся в одной сборке), protected и
//   protected internal.
#endregion

#region Ключевое слово base
//  С помощью ключевого слова base мы можем обратиться к базовому классу. В нашем случае в конструкторе
//  класса Employee2 нам надо установить имя и компанию. Но имя мы передаем на установку в конструктор
//  базового класса, то есть в конструктор класса Person, с помощью выражения base(name).
class Employee2 : Person
{
    public string Company { get; set; }
    public Employee2(string name, string company)
        : base(name)
    {
        Company = company;
    }
}
#endregion

#region Конструкторы в производных классах
//  Конструкторы не передаются производному классу при наследовании. И если в базовом классе
//  не определен конструктор по умолчанию без параметров, а только конструкторы с параметрами
//  (как в случае с базовым классом Person), то в производном классе мы обязательно должны
//  вызвать один из этих конструкторов через ключевое слово base.
#endregion

#region Конструкторы в производных классах
//  При вызове конструктора класса сначала отрабатывают конструкторы базовых классов и только
//  затем конструкторы производных.
#endregion
