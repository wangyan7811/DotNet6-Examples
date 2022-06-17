// See https://aka.ms/new-console-template for more information

using DeadLock;

Console.WriteLine("Hello, World!");

SynchroThis st = new SynchroThis();
Monitor.Enter(st);
// 对象本身已经被锁 ，所以Work中lock一直无法等待到锁释放，发生死锁

//Monitor.Exit(st);提前释放锁

Thread t = new Thread(st.Work);
t.Start();
t.Join();
//程序不会执行到这里
Console.WriteLine("程序结束");