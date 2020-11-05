using System;
using System.Collections.Generic;
using CSRedis;
using AL.Common.Base;
using AL.Common.Data;
using AL.Common.Tools.Redis;

namespace HC.Core.Centre
{
    class Program
    {
        static void Main(string[] args)
        {
            //var redis = new CSRedisClient("172.17.17.9:6379");
            ////初始化
            //RedisHelper.Initialization(redis);
            //var result = RedisHelper.Get($"{RedisConsts.UserToken}:{30}:a26fe79a-16a2-4d32-9b89-c0fbcb64a38e");
            ////RedisHelper.Expire();
            //ITest test;
            //var str = Console.ReadLine();
            //if (str == "a")
            //{
            //    test = new TestA();
            //}
            //else
            //{
            //    test = new TestB();
            //}
            //Console.WriteLine(test.GetStr());
            Console.WriteLine("20200925135003-00000000-0000-0000-0000-000000000000".Length);
            Console.WriteLine("Hello World!");
        }
    }

    interface ITest
    {
        public string GetStr();
    }

    public class TestA : ITest
    {
        public string GetStr()
        {
            return "TestA";
        }
    }

    public class TestB : ITest
    {
        public string GetStr()
        {
            return "TestB";
        }
    }
}
