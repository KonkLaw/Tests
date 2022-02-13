using System;

namespace TestDotNet.Utils;

static class SickSh_tApi
{
    public static unsafe TTo Cast<TFrom, TTo>(TFrom origin)
    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        TTo placeholder = default;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        TypedReference trPla = __makeref(placeholder);
        TypedReference trOr = __makeref(origin);
        *(void**)&trPla = *(void**)&trOr;
        return __refvalue(trPla, TTo);
    }
}