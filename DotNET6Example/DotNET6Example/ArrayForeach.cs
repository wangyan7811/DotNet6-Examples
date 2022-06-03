namespace DotNET6Example
{
    internal class ArrayForeach
    {
        /// <summary>
        /// 通过传入不同的action就可以完成对数组不同的操作
        /// </summary>
        /// <param name="action"></param>
        public void Demo(Action<int> action)
        {
            int[] arr = {1, 2, 3, 4, 5,};
            Array.ForEach(arr, action);
        }

        public void Test()
        {
            Demo((x) => { Console.WriteLine($@"我是：{x}");});

            Demo((x) => { Console.WriteLine($@"我{x}岁了"); });
        }
    }
}
