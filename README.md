# C# Thread Visualizer

Created in 2021/01



### 環境

.NET 5 + C#9.0 + WPF



### 目的

大量のタスクを実行をしたとき、どのように スレッドが使用されるかを視覚化したかった。



### これは何？

大量のタスクを実行しグラフ化した。

- 横軸：経過時間(ミリ秒)
- 縦軸：スレッドID (`Thread.CurrentThread.ManagedThreadId`)

タスクは UI で指定した 時間だけスレッドを止めるだけ。`Thread.Sleep()`



### 実行結果

1. 100msec `Thread.Sleep` する `Task` を ほぼ同時に 100個 実行

![Threads1.png](https://github.com/hsytkm/CsharpThreadVisualizer/blob/master/Threads1.png)

2. **1sec** `Thread.Sleep` する `Task` を ほぼ同時に 100個 実行

![Threads2.png](https://github.com/hsytkm/CsharpThreadVisualizer/blob/master/Threads2.png)

3. **1sec** `Thread.Sleep` する `Task` を ほぼ同時に **1000個** 実行

![Threads3.png](https://github.com/hsytkm/CsharpThreadVisualizer/blob/master/Threads3.png)



### 実行結果への疑問

実行スレッド数が、タスクマネージャの論理プロセッサ数を超えることがあった。 なぜ？ タスクの中身が `Thread.Sleep` メソッドだから？



**以下、翌日に追記**

1. まず `Thread.Sleep` の待機中は **別スレッドに制御を渡してる**。

   [Thread.Sleep メソッド - MSDocs](https://docs.microsoft.com/ja-jp/dotnet/standard/threading/pausing-and-resuming-threads)

   - `Thread.Sleep` メソッドをコールすると、スレッドは 実行待機状態 となる。
   - 残りの時間は、別のスレッドに渡される。
   - 指定時間が経過すると、スレッドは 実行状態 に再開される。

2. また、スレッドプールの最小値（論理プロセッサ数）を超えるスレッドが作成されることはある。

   [マネージド スレッド プール - MSDocs](https://docs.microsoft.com/ja-jp/dotnet/standard/threading/the-managed-thread-pool)

   >  スレッド プールの最小値に達すると、**追加のスレッドが作成されるか**、いくつかのタスクが完了するまで待機状態になります。

3. と言うことで、**論理プロセッサ数を超えるスレッド が動作することもある**。正確には、実行状態 になってるのは 論理プロセッサ数 ってことなのかな？

4. `Task` の中で `Thread.Sleep` ではなく、ループでひたすら加算（待ちでなく計算）にしてみたけど、論理プロセッサ数 を超えるスレッドが動作した。 なので `Thread.Sleep` だけの現象（待機中に別のスレッドに制御渡すから）ではないっぽい。

   

### まとめ

よく分からない部分もあるけど、.NET が 良しなに してくれてるっぽいので、深く考えずに任しておけば良いかなぁ って気がしてきた。









## English (Give up on the way)

### Purpose

I want to visualize how threads are used when invoking a lot of Tasks.



### What is this?

Invoke a lot of Tasks and graphed them.

- Horizontal axis : Elapsed time (msec)

- Vertical axis : Thread Id (`Thread.CurrentThread.ManagedThreadId`)

Tasks just stops for the time specified in the UI.  `Thread.Sleep()`



### Result

The number of execution threads sometimes exceeded the number of logical processors in Task Manager.

Why? Because the contents of the Task is `Thread.Sleep` method.



EOF