// <copyright file="TaskHelpers.cs" company="eVote">
//   Copyright © eVote
// </copyright>
namespace ProGaudi.Tarantool.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class TaskHelpers
    {
        public static async Task<T> WithCancellation<T>(
            this Task<T> task,
            CancellationToken cancellationToken)
        {
            var cancellationCompletionSource = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(() => cancellationCompletionSource.TrySetResult(true)))
            {
                if (task != await Task.WhenAny(task, cancellationCompletionSource.Task))
                {
                    throw new OperationCanceledException("Task was cancelled by timeout", cancellationToken);
                }
            }

            return await task;
        }
    }
}