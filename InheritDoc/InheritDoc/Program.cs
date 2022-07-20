// See https://aka.ms/new-console-template for more information

using InheritDoc;

Console.WriteLine("Hello, World!");
new ChineseCanSayHello().SayHello();

ICanSayHello i = new JapaneseCanSayHello();
i.SayHello();