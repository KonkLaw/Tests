using System;

namespace Tests
{
	static class SickSh_tApi
	{
		public static unsafe TTo Cast<TFrom, TTo>(TFrom origin)
		{
			TTo placeholder = default(TTo);
			TypedReference trPla = __makeref(placeholder);
			TypedReference trOr = __makeref(origin);
			*(void**)&trPla = *(void**)&trOr;
			return __refvalue(trPla, TTo);
		}
	}
}
