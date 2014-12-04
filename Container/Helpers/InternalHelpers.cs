﻿using System;
using System.Collections.Generic;

namespace SimpleContainer.Helpers
{
	internal static class InternalHelpers
	{
		//todo утащить во что-нить типа ContractsSet
		public static string FormatContractsKey(IEnumerable<string> contracts)
		{
			return string.Join("->", contracts);
		}

		public static string ByNameDependencyKey(string name)
		{
			return "name=" + name;
		}

		public static string ByTypeDependencyKey(Type type)
		{
			return "type=" + type.FormatName();
		}
	}
}