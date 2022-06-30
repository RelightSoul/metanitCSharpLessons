﻿//  Dynamic Language Runtime
//  DLR в C#. Ключевое слово dynamic

//  Хотя C# относится к статически типизированным языкам, в последних версиях языка были добавлены некоторые
//  динамические возможности. Так, начиная с .NET 4.0 была добавлена новая функциональность под названием DLR
//  (Dynamic Language Runtime). DLR представляет среду выполнения динамических языков, например, таких языков
//  как IronPython и IronRuby.

//Чтобы понять значение данного нововведение, нужно осознавать разичие между языками со статической и динамической
//типизицией. В языках со статической типизацией выявление всех типов и их членов - свойств и методов происходит
//на этапе компиляции. А в динамических языках системе ничего не известно о свойствах и методах типов вплоть до
//выполнения.

//Благодаря этой среде DLR C# может создавать динамические объекты, члены которых выявляются на этапе выполнения
//программы, и использовать их вместе с традиционными объектами со статической типизацией.

//Ключевым моментом использования DLR в C# является применение типов dynamic. Это ключевое слово позволяет
//опустить проверку типов во время компиляции. Кроме того, объекты, объявленные как dynamic, могут в течение
//работы программы менять свой тип. 

dynamic obj = 3;        // здесь obj - целочисленное int
Console.WriteLine(obj); // 3

obj = "Hello world";    // obj - строка
Console.WriteLine(obj); // Hello world

obj = new Person("Tom", 37); // obj - объект Person
Console.WriteLine(obj);      // Name: Tom  Age: 37
//  Несмотря на то, что переменная x меняет тип своего значения несколько раз, данный код будет нормально
//  работать. В этом использование типов dynamic отличается от применения ключевого слова var. Для переменной,
//  объявленной с помощью ключевого слова var, тип выводится во время компиляции и затем во время выполнения
//  больше не меняется.

//  Также можно найти общее между использованием dynamic и типом object. Если в предыдущем примере мы заменим
//  dynamic на object: object x = 3;, то результат будет тот же. Однако и тут есть различия. Например:
object obj33 = 24;
dynamic dyn = 24;
// obj33 += 5;  // так нельзя
dyn += 5;    // а так можно

//  На строке obj += 4; мы увидим ошибку, так как операция += не может быть применена к типам object и int.
//  С переменной, объявленной как dynamic, это пройдет, так как ее тип будет известен только во время выполнения.

//  Еще одна отличительная особенность использования dynamic состоит в том, что это ключевое слово применяется
//  не только к переменным, но и к свойствам и методам. Например:
dynamic tom = new Person("Tom", 22);
Console.WriteLine(tom);
Console.WriteLine(tom.GetSalary(28, "int"));

dynamic bob = new Person("Bob", "twenty-two");
Console.WriteLine(bob);
Console.WriteLine(bob.GetSalary("twenty-eight", "string"));
class Person
{
    public string Name { get; }
    public dynamic Age { get; set; }
    public Person(string name, dynamic age)
    {
        Name = name; Age = age;
    }

    // выводим зарплату в зависимости от переданного формата
    public dynamic GetSalary(dynamic value, string format)
    {
        if (format == "string") return $"{value} euro";
        else if (format == "int") return value;
        else return 0.0;
    }

    public override string ToString() => $"Name: {Name}  Age: {Age}";
}
//  В классе Person определено динамическое свойство Age, поэтому при задании значения этому свойству мы
//  можем написать и person.Age=22, и person.Age="twenty-two". Оба варианта будут допустимыми. А через
//  параметр age в конструкторе этому свойству можно передать любое значение.

//  Также есть метод GetSalary, который возвращает значение dynamic. Например, в зависимости от параметра мы
//  можем вернуть или строковое представление суммы дохода или численное. Также метод принимает dynamic в
//  качестве параметра. Таким образом, мы можем передать в качестве значения дохода как целое, так и дробное
//  число или строку