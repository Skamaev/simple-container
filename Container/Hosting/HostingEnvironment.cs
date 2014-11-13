using System;
using System.Linq;
using System.Reflection;
using SimpleContainer.Helpers;

namespace SimpleContainer.Hosting
{
	public class HostingEnvironment
	{
		private readonly IInheritanceHierarchy hierarchy;
		private readonly IContainerConfiguration configuration;
		private readonly Func<AssemblyName, bool> assemblyFilter;

		public HostingEnvironment(IInheritanceHierarchy hierarchy, IContainerConfiguration configuration,
			Func<AssemblyName, bool> assemblyFilter)
		{
			this.hierarchy = hierarchy;
			this.configuration = configuration;
			this.assemblyFilter = assemblyFilter;
		}

		public SimpleContainer CreateContainer(Action<ContainerConfigurationBuilder> configure)
		{
			var configurationBuilder = new ContainerConfigurationBuilder();
			configure(configurationBuilder);
			return new SimpleContainer(new MergedConfiguration(configuration, configurationBuilder.Build()), hierarchy);
		}

		public ContainerHost CreateHost(Assembly primaryAssembly)
		{
			var targetAssemblies = Utils.Closure(primaryAssembly,
				a => a.GetReferencedAssemblies()
					.Concat(a.GetCustomAttributes<ContainerReferenceAttribute>().Select(x => new AssemblyName(x.AssemblyName)))
					.Where(assemblyFilter).Select(Assembly.Load))
				.ToSet();
			var restrictedHierarchy = new AssembliesRestrictedInheritanceHierarchy(targetAssemblies, hierarchy);
			return new ContainerHost(restrictedHierarchy, configuration);
		}

		//internal-������� (FactoryConfigurator, GenericConfigurator) ������ ���������, �.�.
		//�� �������� ����� �������� - ���� ��������� ������ ��� ���� �� ���� �������.

		//�����������, ��������� ������ ������������

		//���� ���������� �������������� ���� ���������

		//�� ������ ������ GetHost � ������� �������� ������ ��������� ����� �������������� ����� �������
		//�� ������ �� �������������� PrimaryAssembly
	}
}