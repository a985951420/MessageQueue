using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public abstract class IQueueSemaphoreSlim<TQueue>
{
    /// <summary>
    /// 线程任务
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:32
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected Task _ScheduledTask { get; set; }
    /// <summary>
    /// 任务取消标记
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:38
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected CancellationTokenSource _CancellSource { get; set; }
    /// <summary>
    /// 取消标记
    /// </summary>
    protected CancellationToken _CancellToken
    {
        get
        {
            if (_CancellSource != null)
                return _CancellSource.Token;
            return CancellationToken.None;
        }
    }
    /// <summary>
    /// 是否已完成取消
    /// </summary>
    protected bool IsCancell { get; set; }
    /// <summary>
    /// 是否已开启
    /// </summary>
    protected bool _IsStart { get; set; }
    /// <summary>
    /// 最小并行线程
    /// </summary>
    protected int _minSemaphoreSlim = 0;
    /// <summary>
    /// 最大并行线程
    /// </summary>
    protected int _maxSemaphoreSlim = int.MaxValue;

    /// <summary>
    /// 消息队列
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:08
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected ConcurrentQueue<TQueue> _Queue { get; set; }
    /// <summary>
    /// 对可同时访问资源或资源池的线程数加以限制的 Semaphore 的轻量替代。
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:11
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected SemaphoreSlim _SemaphoreSlim { get; set; }
    /// <summary>
    /// 初始化队列
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:26
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    abstract protected void InitQueue();
    /// <summary>
    /// 添加消息队列
    /// </summary>
    /// <typeparam name="TQueue">The type of the queue.</typeparam>
    /// <param name="queue">The queue</param>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:08
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    abstract public void AddTQuequ(TQueue queue);
    /// <summary>
    /// 运行队列
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:29
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    abstract public void RunQueue();
    /// <summary>
    /// 队列停止
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:32
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    abstract public void StopQueue();
    #region 事件
    /// <summary>
    /// 停止事件
    /// </summary>
    /// <returns>
    /// 返回值：EventArgs
    /// </returns>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:46
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public event EventHandler StopCallBack;
    /// <summary>
    /// 开始事件
    /// </summary>
    /// <returns>
    /// 返回值：EventArgs
    /// </returns>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:46
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public event EventHandler StartCallBack;
    /// <summary>
    /// 队列取出事件
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 14:59
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public event EventHandler QueueCallBack;
    /// <summary>
    /// 错误回调
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 15:16
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    public event EventHandler ErrorCallBack;

    /// <summary>
    /// 停止回调
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 15:04
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected void OnStopCallBack()
    {
        StopCallBack?.Invoke(null, null);
    }

    /// <summary>
    /// 停止回调
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 15:04
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected void OnQueueCallBack(TQueue queue)
    {
        QueueCallBack?.Invoke(queue, null);
    }

    /// <summary>
    /// 启动回调
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 15:04
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected void OnStartCallBack()
    {
        StartCallBack?.Invoke(null, null);
    }

    /// <summary>
    /// 错误回调事件
    /// </summary>
    /// 创建者：万浩
    /// 创建日期：2017/12/05 15:17
    /// 修改者：
    /// 修改时间：
    /// ----------------------------------------------------------------------------------------
    protected void OnErrorCallBack(Exception exception)
    {
        ErrorCallBack?.Invoke(exception, null);
    }
    #endregion
}
