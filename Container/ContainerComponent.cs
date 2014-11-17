using System;
using System.Collections.Generic;
using System.Linq;
using SimpleContainer.Hosting;
using SimpleContainer.Reflection;

namespace SimpleContainer
{
	public class ContainerComponent : IDisposable
	{
		private readonly SimpleContainer container;
		private readonly ServiceName name;
		private readonly List<ComponentHostingOptions> components = new List<ComponentHostingOptions>();
		private bool entryPointCreated;

		public ContainerComponent(SimpleContainer container, ServiceName name)
		{
			this.container = container;
			this.name = name;
			container.OnResolve += delegate(ContainerService service)
			{
				if (!typeof (IComponent).IsAssignableFrom(service.type))
					return;
				if (entryPointCreated)
				{
					const string messageFormat = "can't create type [{0}] because it implements IComponent, " +
					                             "but entry point [{1}] have already been created";
					var message = string.Format(messageFormat, service.type.FormatName(), name.type.FormatName());
					throw new InvalidOperationException(message);
				}

				//top sort �� ���� ����������. ���� ����������� ������� DependsOnServiceAttribute, �������� ������� top sort ������.
				components.Add(new ComponentHostingOptions((IComponent) service.SingleInstance()));
			};
		}

		public object CreateEntryPoint()
		{
			if (entryPointCreated)
				throw new InvalidOperationException("entry point already created");
			var entryPoint = container.Get(name.type, name.contract);
			entryPointCreated = true;
			foreach (var componentOptions in components.AsEnumerable().Reverse())
				componentOptions.Initialize();
			return entryPoint;
		}

		public void Dispose()
		{
			var exceptions = new List<Exception>();
			foreach (var hostingOptions in components)
			{
				try
				{
					hostingOptions.Stop();
				}
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}
			if (exceptions.Count > 0)
				throw new AggregateException("error stopping components", exceptions);
		}
	}
}