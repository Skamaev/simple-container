using System;
using System.Reflection;

namespace SimpleContainer.Hosting
{
	public class HostingEnvironment
	{
		private readonly IInheritanceHierarchy inheritors;
		private readonly IContainerConfiguration configuration;

		public HostingEnvironment(IInheritanceHierarchy inheritors, IContainerConfiguration configuration)
		{
			this.inheritors = inheritors;
			this.configuration = configuration;
		}

		public SimpleContainer CreateContainer(Action<ContainerConfigurationBuilder> configure)
		{
			var configurationBuilder = new ContainerConfigurationBuilder();
			configure(configurationBuilder);
			return new SimpleContainer(new MergedConfiguration(configuration, configurationBuilder.Build()), inheritors);
		}

		public ContainerHost CreateHost(Assembly primaryAssembly)
		{
			return new ContainerHost(inheritors, configuration, primaryAssembly);
		}

		//internal-������� (FactoryConfigurator, GenericConfigurator) ������ ���������, �.�.
		//�� �������� ����� �������� - ���� ��������� ������ ��� ���� �� ���� �������.

		//�����������, ��������� ������ ������������

		//���� ���������� �������������� ���� ���������

		//�� ������ ������ GetHost � ������� �������� ������ ��������� ����� �������������� ����� �������
		//�� ������ �� �������������� PrimaryAssembly
	}
}