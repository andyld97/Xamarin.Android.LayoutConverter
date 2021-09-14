using Layout_Converter.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Layout_Converter.Model
{
    public class Code
    {
        public string ViewCode { get; set; }

        public List<string> ConstraintSets { get; set; }

        public List<string> Variables { get; set; } = new List<string>();

        public static List<string> UsingStatements { get; set; } = new List<string>()
        {
            "using Android.View;",
            "using Android.Support.Constraint;",
            "using Android.Graphics;",
            "using Android.Support.V7.Widget;",
            "using Android.Widget;",
            "using A = Android;"
        };

        public string GenerateCode(string namespaceName = "Layout_Converter", string className = "LayoutBehind", List<Rule> rules = null)
        {
            string finalCode = string.Empty;
            string n = "\n";
            string nn = $"{n}{n}";

            // 1) Using
            foreach (var statement in UsingStatements.OrderBy(p => p))
                finalCode += statement + n;

            // 2) Namespace
            finalCode += n;
            finalCode += $"namespace {namespaceName}";
            finalCode += n + "{" + n;

            // 3) Class
            finalCode += $"public class {className}".ApplySpacing(1);
            finalCode += n + "{".ApplySpacing(1) + n;
            foreach (var variable in Variables)
                finalCode += $"private {variable} = null;".ApplyRules(rules).ApplySpacing(2) + n;

            string constraintSets = string.Empty;
            foreach (var cSet in ConstraintSets)
                constraintSets += $"{cSet}" + n + n;

            // 4) Ctor + Methods
            finalCode += "\n";
            finalCode += $"public {className}(Context context)".ApplySpacing(2) + n;
            finalCode += "{".ApplySpacing(2) + n;
            finalCode += "InitalizeComponents(context);".ApplySpacing(3) + n;
            finalCode += "InitalizeConstraints();".ApplySpacing(3) + n;
            finalCode += "}".ApplySpacing(2) + n;
            finalCode += "\n";
            finalCode += "public void InitalizeComponents(Context context)".ApplySpacing(2) + n;
            finalCode += "{".ApplySpacing(2) + n;
            finalCode += ViewCode.ApplyRules(rules).ApplySpacing(3);
            finalCode += "}".ApplySpacing(2) + nn;
            finalCode += "public void InitalizeConstraints()".ApplySpacing(2) + n;
            finalCode += "{".ApplySpacing(2) + n;
            finalCode += constraintSets.ApplyRules(rules).ApplySpacing(3);
            finalCode += "}".ApplySpacing(2) + nn;
            finalCode += "}".ApplySpacing(1) + n;
            finalCode += "}";

            // finalCode = finalCode.Replace(n, System.Environment.NewLine);

            return finalCode;
        }

        public override string ToString()
        {
            return GenerateCode();
        }
    }
}
