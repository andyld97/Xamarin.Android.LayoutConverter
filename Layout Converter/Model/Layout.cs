using Layout_Converter.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Layout_Converter.Model
{
    public class Layout : View
    {
        public List<View> Children { get; set; } = new List<View>();

        public Layout()
        {
     
        }

        public Code GenerateLayoutCode(List<string> variables = null)
        {
            if (variables == null)
                variables = new List<string>();

            List<string> constraintSets = new List<string>();
            string n = System.Environment.NewLine;
            string nn = $"{n}{n}";

            string code = string.Empty;
            code += GenerateViewCode(variables) + nn;

            foreach (var child in Children)
            {
                if (child is Layout l)
                {
                    var res = l.GenerateLayoutCode(variables);
                    constraintSets.AddRange(res.ConstraintSets);
                    code += res.ViewCode;
                }
                else if (child is View)
                    code += child.GenerateViewCode(variables);

                code += $"{Name}.AddView({child.Name});{nn}";
            }

            if (DefiniedType.Contains("ConstraintLayout"))
            {
                // Build the apporpirate ConstraintSet
                string ccst = string.Empty;

                string cs = View.GenerateContraintSetName();

                variables.Add($"ConstraintSet {cs}");
                ccst += $"{cs} = new ConstraintSet();\n";
                ccst += $"{cs}.Clone({Name});\n";

                // Add guidlines
                foreach (var guideline in Children.Where(p => p.DefiniedType.ToLower().Contains("guideline")))
                {
                    var percent = guideline.Attributes["layout_constraintGuide_percent"];
                    var orientation = guideline.Attributes["orientation"].ApplyCSharpConvention();

                    ccst += $"{cs}.Create({guideline.Name}.Id, ConstraintLayout.LayoutParams.{orientation});\n";
                    ccst += $"{cs}.SetGuidelinePercent({guideline.Name}.Id, {percent}f); \n\n";
                }

                ccst += "\n";

                // Add views
                foreach (var childView in Children.Where(p => !p.DefiniedType.ToLower().Contains("guideline") && p.HasConstraints))
                {
                    var constraints = childView.Attributes.Where(p => p.Key.Contains("layout_constraint")).ToList();
                    foreach (var constraint in constraints)
                    {
                        // Generate constraint line
                        string[] parts = constraint.Key.Split(new string[] { "_" }, System.StringSplitOptions.RemoveEmptyEntries);
                        // parts[0] := layout
                        // parts[1] := contraintStart
                        // parts[2] := toStartOf

                        string firstParameter = parts[1].Replace("constraint", string.Empty);
                        string secondParameter = parts[2].Substring(2, parts[2].Length - 2).Replace("Of",string.Empty);
                        string targetView = "ConstraintSet.ParentId";
                        if (constraint.Value != "parent")
                            targetView = $"{constraint.Value}.Id";

                        ccst += $"{cs}.Connect({childView.Name}.Id, ConstraintSet.{firstParameter}, {targetView}, ConstraintSet.{secondParameter});\n";
                    }
                    ccst += "\n";
                }

                ccst += $"{cs}.ApplyTo({Name});\n";
                constraintSets.Add(ccst);
            }

            // Order variables
            variables = variables.OrderBy(p => p).ToList();
            return new Code() { ViewCode = code, Variables = variables, ConstraintSets = constraintSets };
        }
    }
}