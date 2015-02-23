﻿using System;

namespace SimpleContainer.Helpers
{
	internal static class StringHelpers
	{
		public static bool EqualsIgnoringCase(this string s1, string s2)
		{
			return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
		}
	}
}