# 简介
本项目简单测试了Redis的单条读写和批量读写耗时差异。

# 测试环境
* Dotnet Core 2.0.0
* Redis 4.0.1
* StackExchange.Redis 1.2.6
* Docker 17.06.0-ce
* MacBook Pro; 2.6 GHz Intel Core i5; 8 GB 1600 MHz DDR3

Redis运行在Docker中，测试代码为Console程序，都在本机运行。测试结果的耗时没有包含网络通信的开销，所以结果仅供Redis和应用都在一台计算机的情况下参考。需要跨机器查询的请自行下载代码测试。

# 测试内容
此次测试为单线程连续写入10000条数据，分为单条写入和批量写入两种方式，批量写入为每次写入100条。写入完成后同样方式测试读取耗时。

# 执行测试

## 启动redis  
redis只用于测试，启动容器时我加了`--rm`参数，测试完成容器自动删除，写入的数据不会保留在你的计算机里。
```
docker run --rm -p 6379:6379 --name redis redis
```

## 运行测试代码
```sh
git clone https://github.com/scfido/redisSpeedTest.git
cd redisSpeedTest
dotnet run
```

# 测试结果
在我的机器上测试结果如下
```
Redis 写入性能测试
        单条写入10000条耗时：00:00:03.9709090
        批量写入10000条耗时：00:00:00.0855790

Redis 读取性能测试
        单条读取10000条耗时：00:00:03.9316330
        批量读取10000条耗时：00:00:00.1684330
```