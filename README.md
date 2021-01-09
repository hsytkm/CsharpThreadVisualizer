# C# Thread Visualizer

Created in 2021/01



### Environment

.NET 5 + C#9.0 + WPF



## English

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



![Threads.png](https://github.com/hsytkm/CsharpThreadVisualizer/blob/master/Threads.png)



## Japanese

### 目的

大量のタスクを実行をしたとき、どのように スレッドが使用されるかを視覚化したかった。



### これは何？

大量のタスクを実行しグラフ化した。

- 横軸：経過時間(ミリ秒)
- 縦軸：スレッドID (`Thread.CurrentThread.ManagedThreadId`)

タスクは UI で指定した 時間だけスレッドを止めるだけ。`Thread.Sleep()`



### 実行結果

実行スレッド数が、タスクマネージャの論理プロセッサ数を超えることがあった。

なぜ？ タスクの中身が `Thread.Sleep` メソッドだから？



![Threads.png](https://github.com/hsytkm/CsharpThreadVisualizer/blob/master/Threads.png)



EOF