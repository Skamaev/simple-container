using System.Collections.Generic;
using SimpleContainer.Configuration;
using SimpleContainer.Helpers;

namespace SimpleContainer.Implementation
{
	internal class ContractsList
	{
		private List<string> contracts = new List<string>();
		private string[] contractsArray = InternalHelpers.emptyStrings;

		public int Match(List<string> chain)
		{
			int i = 0, j = 0;
			while (true)
			{
				if (i >= chain.Count)
					return j;
				if (j >= contracts.Count)
					return -1;
				if (Equals(chain[i], contracts[j]))
					i++;
				j++;
			}
		}

		public string[] Snapshot()
		{
			return contractsArray ?? (contractsArray = contracts.ToArray());
		}

		public List<string> Replace(List<string> newContracts)
		{
			var result = contracts;
			contracts = newContracts;
			contractsArray = null;
			return result;
		}

		public void Restore(List<string> newContracts)
		{
			contracts = newContracts;
			contractsArray = null;
		}

		public ActionResult Push(string[] newContracts)
		{
			var pushedCount = 0;
			foreach (var newContract in newContracts)
			{
				foreach (var c in contracts)
					if (newContract.EqualsIgnoringCase(c))
					{
						const string messageFormat = "contract [{0}] already declared, all declared contracts [{1}]";
						var message = string.Format(messageFormat, newContract, InternalHelpers.FormatContractsKey(contracts));
						contracts.RemoveLast(pushedCount);
						return Result.Fail(message);
					}
				contracts.Add(newContract);
				pushedCount++;
			}
			if (pushedCount > 0)
				contractsArray = null;
			return Result.Ok();
		}

		public void PushNoCheck(string[] newContracts)
		{
			contracts.AddRange(newContracts);
			if (newContracts.Length > 0)
				contractsArray = null;
		}

		public void RemoveLast(int count)
		{
			contracts.RemoveLast(count);
			if (count > 0)
				contractsArray = null;
		}

		public string[] PopMany(int count)
		{
			var result = contracts.PopMany(count);
			if (count > 0)
				contractsArray = null;
			return result;
		}

		public string[][] TryExpandUnions(ConfigurationRegistry configuration)
		{
			string[][] result = null;
			var startIndex = 0;
			for (var i = 0; i < contracts.Count; i++)
			{
				var contract = contracts[i];
				var union = configuration.GetContractsUnionOrNull(contract);
				if (union == null)
				{
					if (result != null)
						result[i - startIndex] = new[] {contract};
				}
				else
				{
					if (result == null)
					{
						startIndex = i;
						result = new string[contracts.Count - startIndex][];
					}
					result[i - startIndex] = union.ToArray();
				}
			}
			return result;
		}
	}
}