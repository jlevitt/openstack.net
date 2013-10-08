namespace net.openstack.Core
{
#if NET35
    using System;
#endif
    using System.Threading.Tasks;

    /// <summary>
    /// Provides extension methods to <see cref="Task"/> and <see cref="Task{TResult}"/> instances
    /// for use within the openstack.net library.
    /// </summary>
    internal static class InternalTaskExtensions
    {
        /// <summary>
        /// Gets a completed <see cref="Task"/>.
        /// </summary>
        /// <returns>A completed <see cref="Task"/>.</returns>
        public static Task CompletedTask()
        {
            return CompletedTaskHolder.Default;
        }

        /// <summary>
        /// Gets a completed <see cref="Task{TResult}"/> with the specified result.
        /// </summary>
        /// <typeparam name="TResult">The task result type.</typeparam>
        /// <param name="result">The result of the completed task.</param>
        /// <returns>A completed <see cref="Task{TResult}"/>, whose <see cref="Task{TResult}.Result"/> property returns the specified <paramref name="result"/>.</returns>
        public static Task<TResult> CompletedTask<TResult>(TResult result)
        {
            TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
            completionSource.SetResult(result);
            return completionSource.Task;
        }

#if NET35
        /// <summary>
        /// Immediately propagates any exceptions thrown by the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <exception cref="ObjectDisposedException">If <paramref name="task"/> is been disposed.</exception>
        /// <exception cref="AggregateException">
        /// If <paramref name="task"/> was cancelled.
        /// <para>-or-</para>
        /// <para>If an exception was thrown during the execution of <paramref name="task"/>.</para>
        /// </exception>
        public static void PropagateExceptions(this Task task)
        {
            if (!task.IsCompleted)
                throw new InvalidOperationException("The task has not completed.");
            if (task.IsFaulted)
                task.Wait();
        }
#endif

        private static class CompletedTaskHolder
        {
            public static readonly Task Default;

            static CompletedTaskHolder()
            {
                Default = CompletedTaskHolder<object>.Default;
            }
        }

        private static class CompletedTaskHolder<T>
        {
            public static readonly Task<T> Default;

            static CompletedTaskHolder()
            {
                TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
                completionSource.SetResult(default(T));
                Default = completionSource.Task;
            }
        }
    }
}
