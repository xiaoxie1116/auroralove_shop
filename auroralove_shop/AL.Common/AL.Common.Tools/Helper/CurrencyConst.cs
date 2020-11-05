using System;
using System.Collections.Generic;
using System.Text;

namespace AL.Common.Tools.Helper
{
    public static class CurrencyConst
    {
        public static readonly Dictionary<string, string> DicCurrency = new Dictionary<String, String>
        {
            {"CNY","¥" },
            {"USD","$" },
            {"EUR","€" },
            {"GBP","￡" }
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string GetCurrency(string currency)
        {
            if (string.IsNullOrEmpty(currency))
            {
                return "";
            }
            string labelCurrency = "";
            var bok = DicCurrency.TryGetValue(currency, out labelCurrency);
            if (bok)
            {
                return labelCurrency;
            }
            return currency;
        }
    }
}
