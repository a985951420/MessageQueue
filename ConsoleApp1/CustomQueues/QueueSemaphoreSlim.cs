using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class QueueSemaphoreSlim<TQueue> : IQueueSemaphoreSlim<TQueue>, IDisposable
{
    private static readonly object _MonitorRun = new object();

    /// <summary>
    /// 队列总数
    /// </summary>
    public int QueueCount
    {
        get
        {
            if (_Queue != null)
            {
                return _Queue.Count;
            }
            return 0;
        }
    }

    /// <summary>
    /// 初始化一个新的实例QueueSemaphoreSlim{TQueue}
    /// </summary>
    /// <param name="minSemaphoreSlim">The minSemaphoreSlim</param>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:27
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public QueueSemaphoreSlim(int minSemaphoreSlim)
    {
        _minSemaphoreSlim = minSemaphoreSlim;
        InitQueue();
    }

    /// <summary>
    /// 初始化一个新的实例QueueSemaphoreSlim{TQueue}
    /// </summary>
    /// <param name="minSemaphoreSlim">The minSemaphoreSlim</param>
    /// <param name="maxSemaphoreSlim">The maxSemaphoreSlim</param>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:27
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public QueueSemaphoreSlim(int minSemaphoreSlim, int maxSemaphoreSlim)
    {
        _minSemaphoreSlim = minSemaphoreSlim;
        _maxSemaphoreSlim = maxSemaphoreSlim;
        InitQueue();
    }

    /// <summary>
    /// 初始化队列
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:27
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:26
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected override void InitQueue()
    {
        _SemaphoreSlim = new SemaphoreSlim(_minSemaphoreSlim, _maxSemaphoreSlim);
        _Queue = new ConcurrentQueue<TQueue>();
        _CancellSource = new CancellationTokenSource();
        _ScheduledTask = AssignmentTask();
        _ScheduledTask.Start();
    }

    /// <summary>
    /// 添加消息队列
    /// </summary>
    /// <param name="queue">The queue</param>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:34
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:08
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public override void AddTQuequ(TQueue queue)
    {
        _Queue.Enqueue(queue);
        _SemaphoreSlim.Release();
    }

    Task AssignmentTask()
    {
        return new Task(delegate ()
         {
             while (true)
             {
                 Console.WriteLine("AssignmentTask 任务运行中！");
                 if (_CancellToken.IsCancellationRequested)
                 {
                     Console.WriteLine("AssignmentTask 取消！");
                     break;
                 }
                 try
                 {
                     if (_Queue.TryDequeue(out TQueue item))
                         OnQueueCallBack(item);
                     else
                     {
                         if (_Queue.Count <= 0)
                         {
                             Console.WriteLine("AssignmentTask 停顿中！");
                             _SemaphoreSlim.Wait();
                         }
                     }
                 }
                 catch (Exception ex) { OnErrorCallBack(ex); }
             }
         }, _CancellToken);
    }

    /// <summary>
    /// 运行队列
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 15:22
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:29
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public override void RunQueue()
    {
        try
        {
            Monitor.Enter(_MonitorRun);
            if (!_IsStart)
            {
                _IsStart = true;
                OnStartCallBack();
            }
        }
        catch (Exception ex) { OnErrorCallBack(ex); }
        finally
        {
            Monitor.Exit(_MonitorRun);
        }
    }

    /// <summary>
    /// 队列停顿
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 15:22
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:32
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public override void StopQueue()
    {
        try
        {
            Monitor.Enter(_MonitorRun);
            _CancellSource.Cancel();
            OnStopCallBack();
        }
        catch (Exception ex) { OnErrorCallBack(ex); }
        finally
        {
            Monitor.Exit(_MonitorRun);
        }
    }
    /// <summary>
    /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:43
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public void Dispose()
    {
        try
        {
            _ScheduledTask.Dispose();
        }
        catch (Exception ex) { OnErrorCallBack(ex); }
        _ScheduledTask = null;
        _SemaphoreSlim = null;
        _CancellSource = null;
        _Queue = null;
    }
}
