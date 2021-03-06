﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace redisSpeedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Redis 写入性能测试");
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            var db = redis.GetDatabase();
            var count = 10000;
            var ts = SingleSet(db, count);
            Console.WriteLine($"\t单条写入{count}条耗时：{ts}");

            ts = BatchSet(db, count);
            Console.WriteLine($"\t批量写入{count}条耗时：{ts}");
            Console.WriteLine();

            Console.WriteLine("Redis 读取性能测试");

            ts = SingleGet(db, count);
            Console.WriteLine($"\t单条读取{count}条耗时：{ts}");

            ts = BatchGet(db, count);
            Console.WriteLine($"\t批量读取{count}条耗时：{ts}");
        }

        static TimeSpan SingleSet(IDatabase db, int count)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                db.StringSet($"foo{i}", $"bar-{i}");
            }
            sw.Stop();
            //Console.WriteLine($"foo{count - 1}={{0}}", db.StringGet($"foo{count - 1}"));

            return sw.Elapsed;
        }

        static TimeSpan BatchSet(IDatabase db, int count)
        {
            var sw = Stopwatch.StartNew();

            int offset = 0;
            int block = 100;
            while (offset < count)
            {
                int size = count - offset >= block ? block : count - offset;
                var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[size];

                for (int i = 0; i < size; i++)
                {
                    keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>($"batchfoo{offset + i}", $"bar-{offset + i}");
                }
                db.StringSet(keyValuePair);
                offset += size;
                //Console.WriteLine($"写入{offset}/{count}");
            }
            sw.Stop();

            //Console.WriteLine($"batchfoo{count - 1}={{0}}", db.StringGet($"foo{count - 1}"));

            return sw.Elapsed;
        }

        static TimeSpan SingleGet(IDatabase db, int count)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                string value = db.StringGet($"foo{i}");
            }
            //Console.WriteLine($"foo{count - 1}={{0}}", db.StringGet($"foo{count - 1}"));

            sw.Stop();
            return sw.Elapsed;
        }

        static TimeSpan BatchGet(IDatabase db, int count)
        {
            var batch = db.CreateBatch();

            var sw = Stopwatch.StartNew();

            int offset = 0;
            int block = 100;
            while (offset < count)
            {
                List<Task<RedisValue>> valueList = new List<Task<RedisValue>>();

                int size = count - offset >= block ? block : count - offset;
                for (int i = 0; i < size; i++)
                {
                    string key = $"batchfoo{offset + i}";
                    Task<RedisValue> tres = batch.StringGetAsync(key);
                    valueList.Add(tres);
                }
                batch.Execute();

                foreach (var val in valueList)
                {
                    var value = val.Result.ToString();
                }
                offset += size;
            }
            sw.Stop();

            return sw.Elapsed;
        }
    }
}
