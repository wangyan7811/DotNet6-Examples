namespace InheritDoc
{
    /// <inheritdoc cref="ICanSayHello"/>
    internal class ChineseCanSayHello:ICanSayHello
    {
       
        /// <inheritdoc />
        public void SayHello()
        {
            Console.WriteLine("你好啊朋友，吃了吗");
        }
    }
}
