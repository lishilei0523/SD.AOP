using SD.AOP.Standard.Tests.StubEntities;
using System;

namespace SD.AOP.Standard.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Student student = new Student("Tom", true, 20);
            student.UpdateInfo(Guid.NewGuid(), null, false, 25);

            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}
