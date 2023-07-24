using FwaEu.Fwamework.Temporal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.BackgroundTasks
{
	public interface IScheduledBackgroundTaskFactory
	{
		/// <summary>
		/// Used to (sometimes) create a task which will cleanup, free resources, etc.
		/// </summary>
		/// <returns>Returns null if nothing to do.</returns>
		Task<ITaskStartParameters> GetScheduledTaskStartParametersAsync(IServiceProvider serviceProvider);
	}

	public class ScheduledBackgroundTaskFactory<TTask> : IScheduledBackgroundTaskFactory
		where TTask : IBackgroundTask
	{
		private readonly string _taskName;
		private readonly TimeSpan _regularity;

		private DateTime? _lastTaskCreation;

		public ScheduledBackgroundTaskFactory(string taskName, TimeSpan regularity)
		{
			this._regularity = regularity;

			this._taskName = taskName
				?? throw new ArgumentNullException(nameof(taskName));
		}

		public Task<ITaskStartParameters> GetScheduledTaskStartParametersAsync(IServiceProvider serviceProvider)
		{
			var now = serviceProvider.GetRequiredService<ICurrentDateTime>().Now;

			if (this._lastTaskCreation == null
				|| (now - this._lastTaskCreation.Value) > this._regularity)
			{
				this._lastTaskCreation = now;
				return Task.FromResult<ITaskStartParameters>(new TaskStartParameters(this._taskName, null));
			}

			return Task.FromResult<ITaskStartParameters>(null);
		}
	}
}
