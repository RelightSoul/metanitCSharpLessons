﻿//  Параллельное программирование и библиотека TPL
//  Задачи и класс Task

//  В эпоху многоядерных машин, которые позволяют параллельно выполнять сразу несколько процессов, стандартных
//  средств работы с потоками в .NET уже оказалось недостаточно. Поэтому во фреймворк .NET была добавлена
//  библиотека параллельных задач TPL (Task Parallel Library), основной функционал которой располагается в
//  пространстве имен System.Threading.Tasks. Данная библиотека упрощает работу с многопроцессорными, многоядерными
//  система. Кроме того, она упрощает работу по созданию новых потоков. Поэтому обычно рекомендуется использовать
//  именно TPL и ее классы для создания многопоточных приложений, хотя стандартные средства и класс Thread
//  по-прежнему находят широкое применение.

//  В основе библиотеки TPL лежит концепция задач, каждая из которых описывает отдельную продолжительную операцию.
//  В библиотеке классов .NET задача представлена специальным классом - классом Task, который находится в
//  пространстве имен System.Threading.Tasks. Данный класс описывает отдельную задачу, которая запускается
//  асинхронно в одном из потоков из пула потоков. Хотя ее также можно запускать синхронно в текущем потоке.

//  Для определения и запуска задачи можно использовать различные способы.

//  Первый способ создание объекта Task и вызов у него метода Start:
Task task1 = new Task(() => Console.WriteLine("Task1"));
task1.Start();
//  В качестве параметра объект Task принимает делегат Action, то есть мы можем передать любое действие,
//  которое соответствует данному делегату, например, лямбда-выражение, как в данном случае, или ссылку
//  на какой-либо метод. То есть в данном случае при выполнении задачи на консоль будет выводиться строка
//  "Hello Task!".

//  А метод Start() собственно запускает задачу.

//  Второй способ заключается в использовании статического метода Task.Factory.StartNew(). Этот метод также
//  в качестве параметра принимает делегат Action, который указывает, какое действие будет выполняться. При
//  этом этот метод сразу же запускает задачу:
Task task2 = Task.Factory.StartNew(() => Console.WriteLine("Task2"));
//  В качестве результата метод возвращает запущенную задачу.

//  Третий способ определения и запуска задач представляет использование статического метода Task.Run():
Task task3 = Task.Run(() => Console.WriteLine("Task3"));
//  Метод Task.Run() также в качестве параметра может принимать делегат Action - выполняемое действие и
//  возвращает объект Task.

//  Итак, в данном коде задачи создаются и запускаются, но при выполнении приложения на консоли мы можем не
//  увидеть ничего. Почему? Потому что когда поток задачи запускается из основного потока программы - потока
//  метода Main, приложение может завершить выполнение до того, как все три или даже хотя бы одна из трех задач
//  начнет выполнение. Чтобы этого не произошло, мы можем программным образом ожидать завершения задачи.

#region Ожидание завершения задачи
//  Чтобы приложение ожидало завершения задачи, можно использовать метод Wait() объекта Task:
task1.Wait();   // ожидаем завершения задачи task1
task2.Wait();   // ожидаем завершения задачи task2
task3.Wait();   // ожидаем завершения задачи task3
//  Консольный вывод не детерминирован, поскольку задачи не выполняются последовательно. Первая запущенная
//  задача может завершить свое выполнение после последней задачи.
Console.WriteLine("------------------");
//  Стоит отметить, что метод Wait() блокирует вызывающий поток, в котором запущена задача, пока эта задача
//  не завершит свое выполнение. Например:
Console.WriteLine("Main Start");
//  Создаём задачу
Task task4 = new Task(() =>
{
    Console.WriteLine("Task Start");
    Thread.Sleep(1000);   // задержка на 1 секунду - имитация долгой работы
    Console.WriteLine("Task End");
});
task4.Start();
task4.Wait();
Console.WriteLine("Main End");
//  Для эмуляции долговременной работы здесь в задаче task1 устанавливается задержка на 1 секунду. В итоге,
//  когда выполнение дойдет до вызова task1.Wait() основной поток остановит свое выполнение и будет ждать
//  завершения задачи. И мы получим следующий консольный вывод:

//  Если подобное поведение не принципиально, то ожидание завершения задачи можно поместить в конец метода Main:
//      task4.Wait();
//  В этом случае приложение все равно будет ждать завершения задачи, однако другие синхронные действия в основном
//  потоке не будут блокироваться и ожидать завершения задачи.
#endregion

#region Синхронный запуск задачи
//  По умолчанию задачи запускаются асинхронно. Однако с помощью метода RunSynchronously() можно запускать
//  синхронно:
Console.WriteLine("Main Starts");
// создаем задачу
Task task5 = new Task(() =>
{
    Console.WriteLine("Task Starts");
    Thread.Sleep(1000);
    Console.WriteLine("Task Ends");
});
task5.RunSynchronously(); // запускаем задачу синхронно
Console.WriteLine("Main Ends"); // этот вызов ждет завершения задачи task5
#endregion

#region Свойства класса Task
//  Класс Task имеет ряд свойств, с помощью которых мы можем получить информацию об объекте. Некоторые из них:

//  AsyncState: возвращает объект состояния задачи

//  CurrentId: возвращает идентификатор текущей задачи (статическое свойство)

//  Id: возвращает идентификатор текущей задачи

//  Exception: возвращает объект исключения, возникшего при выполнении задачи

//  Status: возвращает статус задачи. Представляет перечисление System.Threading.Tasks.TaskStatus, которое
//  имеет следующие значения:
//          Canceled: задача отменена
//          Created: задача создана, но еще не запущена
//          Faulted: в процессе работы задачи произошло исключение
//          RanToCompletion: задача успешно завершена
//          Running: задача запущена, но еще не завершена
//          WaitingForActivation: задача ожидает активации и постановки в график выполнения
//          WaitingForChildrenToComplete: задача завершена и теперь ожидает заврешения прикрепленных к
//          ней дочерних задач
//          WaitingToRun: задача поставлена в график выполнения, но еще не начала свое выполнение

//  IsCompleted: возвращает true, если задача завершена

//  IsCanceled: возвращает true, если задача была отменена

//  IsFaulted: возвращает true, если задача завершилась при возникновении исключения

//  IsCompletedSuccessfully: возвращает true, если задача завершилась успешно

// получаем информацию о задаче
Console.WriteLine($"Task ID = {task4.Id}");
Console.WriteLine($"Task is Completed = {task4.IsCompleted}");
Console.WriteLine($"Task Status = {task4.Status}");
#endregion