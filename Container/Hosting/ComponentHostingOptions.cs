using System;
using System.Collections.Generic;
using SimpleContainer.Reflection;

namespace SimpleContainer.Hosting
{
	public class Application
	{
		public string HostName { get; private set; }

		public void Run()
		{
			//���� ������ �������� �������� ����� - ����� Main � ������� ���������� �������
			//����� �������� ��� ������ ������
			//	������ ������� ������������ ���������� IRunnable
			//	���� ���-�� �������� IsBackground, �� ������� ����� ������ �������� �� IShutdownCoordinator.Task.WaitOne();

			//������, ����� ����������, ��������������� � ���� ������ � IComponent-���.
			//������ ��� ���������� ������ ������� � ������ BootstrapPoller-�.

			//�� ���� ��� ���������� ������ ��������������� SimpleContainer � ����� � ���������� ������ ��������� IComponent-�
			//��� ������ ������������ IDisposable
			//��� ���������� ����� ����� ������� ��� - ����� ��������� ������ ����� ����, ����� ������� ��� IComponent-�, �������
			//������� �� ����� ��� ��������. ���� ����������� ����, ��� ����������� IComponent �� �������, �.�. ��� � ������ ����������
			//����� �����������

			//� ��� ����� �� �������, ��� ����� ��������� ���������� �� ������ PrimaryAssembly
			//��� ���������� ���������� � ������� ��� ����� ����������.
			//������ ������� �������� �� ��������� ?
			//����� ������ ��� ���������� ���������� ContainerComponent
		}
	}

	//HostingEnvironment

	//ContainerComponent

	//Application

	//public class ContainerComponent
	//{
	//	public IContainer GetContainer()
	//	{
	//		//������������ ���������� ���������� ������ ���������� instanceCache-�

	//		//��� � ���� ������ ����� ����� ??

	//		//����� ��������� ������ ���� � ����� ����� ?

	//		//��� �� ������� �� IShutdownCoordinator ?

	//		//� � �����, �����, ��� ���� ������ ������ ������� ������ IComponent-� ?
	//	}
	//}
	public class ComponentHostingOptions
	{
		internal IComponent Component;

		internal ComponentHostingOptions(IComponent component)
		{
			Component = component;
		}

		internal void Initialize()
		{
			Component.Initialize(this);
		}

		internal void Stop()
		{
			if (OnStop == null)
				return;
			try
			{
				OnStop();
			}
			catch (Exception e)
			{
				var message = string.Format("error stopping component [{0}]", Component.GetType().FormatName());
				throw new SimpleContainerException(message, e);
			}
		}

		public bool IsBackground { get; set; }
		public Action OnStop;
	}
}