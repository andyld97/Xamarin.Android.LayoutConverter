using Layout_Converter.Model;
using System;
using System.Collections.Generic;

namespace Layout_Converter.Helper
{
    public static class StringHelper
    {
        public static string RemoveIDPrefix(this string value, string prefix = "@+id/")
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return value.Replace(prefix, string.Empty).Trim();
        }

        public static string ApplySpacing(this string input, int nSpacing)
        {
            string spacing = "    ";
            string doubleSpacing = spacing + spacing;

            if (!input.Contains("\n"))
            {
                string spc = string.Empty;
                for (int i = 0; i < nSpacing; i++)
                    spc += spacing;

                return $"{spc}{input}";
            }

            string result = string.Empty;
            foreach (var line in input.Split("\n".ToCharArray()))
                result += line.ApplySpacing(nSpacing) + "\n";

            return result;
        }

        public static string RemoveUnderscore(this string value)
        {
            if (!value.Contains("_"))
                return value.ApplyCSharpConvention();

            string[] items = value.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            string result = string.Empty;

            foreach (var item in items)
                result += item.ApplyCSharpConvention();

            return result;
        }

        public static string ApplyCSharpConvention(this string value, bool toUpper = true)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (!value.Contains("."))
            {
                if (value.Length == 1)
                {
                    if (toUpper)
                        return value[0].ToString().ToUpper();
                    else
                        return value[0].ToString().ToLower();
                }

                string firstChar = value[0].ToString().ToUpper();
                if (!toUpper)
                    firstChar = firstChar.ToLower();

                return firstChar + value.Substring(1, value.Length - 1);
            }
            else
            {
                string result = string.Empty;
                foreach (var seg in value.Split(new string[] { "."} ,StringSplitOptions.RemoveEmptyEntries))
                {
                    result +=$"{seg.ApplyCSharpConvention()}.";
                }

                if (result.EndsWith("."))
                    result = result.Substring(0, result.Length - 1);

                return result.Replace("Androidx", "AndroidX");
            }

            return value;
        }

        public static string ApplyRules(this string input, List<Rule> rules)
        {
            if (rules == null)
                return input;

            return Rule.ApplyRules(input, rules);
        }
    }
}
