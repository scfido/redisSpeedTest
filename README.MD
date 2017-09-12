# 简介
本项目简单测试了Redis的写入性能。

# 测试环境
* Dotnet Core 2.0.0
* Redis 4.0.1
* StackExchange.Redis 1.2.6
* MacBook Pro; 2.6 GHz Intel Core i5; 8 GB 1600 MHz DDR3

Redis运行在Docker中，测试代码为Console程序，都在本机运行。

# 测试内容
此次测试为单线程连续写入10000条数据，分为单条写入和批量写入两种方式，批量写入为每次写入100条。

# 执行测试

```sh
git clone https://github.com/scfido/redisSpeedTest.git
cd redisSpeedTest
dotnet run
```

# 测试结果
在我的机器上测试结果如下
```
单条写入10000条耗时：00:00:03.3915910
批量写入10000条耗时：00:00:00.0892320
```