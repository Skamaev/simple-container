using System;
using System.Collections.Generic;

namespace SimpleContainer.Configuration
{
	//������� ����������������� ���, ����� ���� ��� GenericConfigurator-�
	//��������, ����� � generic-��� ���������� ����� �����
	internal class FilteredContainerConfiguration : IConfigurationRegistry
	{
		private readonly IConfigurationRegistry parent;
		private readonly IDictionary<Type, ServiceConfiguration> filteredCache = new Dictionary<Type, ServiceConfiguration>();
		private readonly Func<Type, bool> filter;

		public FilteredContainerConfiguration(IConfigurationRegistry parent, Func<Type, bool> filter)
		{
			this.parent = parent;
			this.filter = filter;
		}

		public ServiceConfiguration GetConfiguration(Type type, List<string> contracts)
		{
			ServiceConfiguration result;
			if (!filteredCache.TryGetValue(type, out result))
			{
				result = parent.GetConfiguration(type, contracts);
				if (result != null)
					result = result.CloneWithFilter(filter);
				filteredCache.Add(type, result);
			}
			return result;
		}

		public List<string> GetContractsUnionOrNull(string contract)
		{
			return parent.GetContractsUnionOrNull(contract);
		}
	}
}