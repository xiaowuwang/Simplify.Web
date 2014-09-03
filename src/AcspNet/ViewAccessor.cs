﻿using AcspNet.Core;
using Simplify.DI;

namespace AcspNet
{
	/// <summary>
	/// View accessor base class
	/// </summary>
	public abstract class ViewAccessor
	{
		internal virtual IViewFactory ViewFactory { get; set; }
		internal virtual IDIContainerProvider ContainerProvider { get; set; }
		
		/// <summary>
		/// Gets view instance
		/// </summary>
		/// <typeparam name="T">View instance to get</typeparam>
		/// <returns>View instance</returns>
		public virtual T GetView<T>()
			where T : View
		{
			var type = typeof(T);

			return (T)ViewFactory.CreateView(type, ContainerProvider);
		}
	}
}
