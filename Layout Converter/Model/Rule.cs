using System.Collections.Generic;

namespace Layout_Converter.Model
{
    public class Rule
    {
        public string Search { get; set; }

        public string Replace { get; set; }

        public bool IsEnabled { get; set; } = true;

        public Rule() { }

        public Rule(string search, string repalce)
        {
            this.Search = search;
            this.Replace = repalce;
        }

        public string Apply(string input)
        {
            if (!IsEnabled)
                return input;

            return input.Replace(Search, Replace);
        }

        public static string ApplyRules(string input, List<Rule> rules)
        {
            string value = input;

            foreach (var rule in rules)
                value = rule.Apply(value);

            return value;
        }
    }
}
