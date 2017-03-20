using System;

namespace RESTWebApplication1.Business
{
	public static class GetTodaysrPricing
	{
		private static int _firstCutoffMinutes = 120;
		private static int _secondCutoffMinutes = 600;
		public static int GetCostInCents(TimeSpan ts)
		{
			int answer = 500;
			if (ts.TotalMinutes > _firstCutoffMinutes)
			{ answer = 1000; }
			if (ts.TotalMinutes > _secondCutoffMinutes)
			{ answer = 1500; }
			return answer;
		}
	}
}