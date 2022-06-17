namespace DeadLock
{
    internal class SynchroThis
    {
        private int i = 0;

        /// <summary>
        /// lock(this)不够健壮
        /// </summary>
        /// <param name="state"></param>
        public void Work(Object state)
        {
            lock (this)
            {
                Console.WriteLine($@"i的值为{i.ToString()}");
                i++;
                Thread.Sleep(200);
                Console.WriteLine($@"i自增，i的值为{i.ToString()}");
            }
        }
    }
}
