using Layout_Converter.Enums;
using Layout_Converter.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Layout_Converter.Model
{
    public class View
    {
        private string name = string.Empty;
        private string type = string.Empty;

        public Layout Parent { get; set; }

        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        private static readonly string[] IGNORE_VIEW_ATTRIBUTES = new[] { "tools", "app", "context", "android", "baselineAligned", "srcCompat", "src" };

        private static readonly string[] LAYOUT_ATTRIBUTES = new string[]
        {
            "layout_gravitiy",          
            "layout_margin",
            "layout_marginBottom",
            "layout_marginEnd",
            "layout_marginLeft",
            "layout_marginRight",
            "layout_marginStart",
            "layout_marginTop",
            "layout_weight",
            "padding",
            "paddingBottom",
            "paddingEnd",
            "paddingLeft",
            "paddingRight",
            "paddingStart",
            "paddingTop",
            "orientation",
            "layout_constraintGuide_percent"
        };

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    GetUniqueVariableName(DefiniedType, out name);

                return name;
            }
            set
            {
                if (value != name)
                    name = value;
            }
        }

        public string DefiniedType
        {
            get => type;
            set
            {
                if (value != type)
                    type = value.ApplyCSharpConvention();
            }
        }

        public void ApplyAttributes(List<XAttribute> xAttributes)
        {
            foreach (var attrib in xAttributes)
            {
                string name = attrib.Name.LocalName;

                // Apply attribute ignore filter
                if (IGNORE_VIEW_ATTRIBUTES.Contains(name))
                    continue;

                if (name == "id")
                    Name = attrib.Value.RemoveIDPrefix();
                else
                {
                    // e.g. tools:text="bla" and android:text="bla"
                    if (Attributes.ContainsKey(name))
                        Attributes[name] = attrib.Value.RemoveIDPrefix();
                    else 
                        Attributes.Add(name, attrib.Value.RemoveIDPrefix());
                }
            }
        }

        public string GenerateViewCode(List<string> variables)
        {
            string code = string.Empty;

            // Define 
            code += $"{Name} = new {DefiniedType}(context);\n";
            variables.Add($"{DefiniedType} {Name}");

            // Apply attributes
            foreach (var attrib in Attributes)
            {
                // e.g. layout_width cannot be set directly
                if (attrib.Key.StartsWith("layout_")|| attrib.Key == "padding")
                    continue;

                if (DefiniedType.ToLower().Contains("guideline") && attrib.Key == "orientation")
                    continue;

                string subCode = string.Empty;
                if (attrib.Key == "text")
                    subCode = $"{Name}.Text = \"{attrib.Value}\";";
                else if (attrib.Key == "textSize")
                {
                    if (UnitHelper.ParseNumber(attrib.Value, out float value, out ComplexUnitType type))
                    {
                        string typeDef = $"Android.Util.ComplexUnitType.{type}";
                        subCode = $"{Name}.SetTextSize({typeDef}, {value});";
                    }
                    else
                    {
                        // Fehler
                        throw new ArgumentException($"Failed to parse unit: {Name}! {attrib.Key} = {attrib.Value}");
                    }
                }
                else if (attrib.Key == "textStyle")
                {
                    // ToDo: *** attrib.Value can be also like bold|italic
                    string value = "Normal";
                    if (attrib.Value.Contains("bold"))
                        value = "Bold";
                    else if (attrib.Value.Contains("italic"))
                        value = "Italic";

                    subCode = $"{Name}.SetTypeface({Name}.Typeface, TypefaceStyle.{value});";
                }
                else if (attrib.Key == "background")
                {
                    subCode = $"{Name}.Background = new ColorDrawable(Color.ParseColor(\"{attrib.Value}\"));";
                }
                else if (attrib.Key == "foreground")
                {
                    subCode = $"{Name}.Foreground = Color.ParseColor(\"{attrib.Value}\"));";
                }
                else if (attrib.Key == "textColor")
                {
                    subCode = $"{Name}.TextColor = Color.ParseColor(\"{attrib.Value}\"));";
                }
                else if (attrib.Key == "orientation")
                {
                    string orientation = $"Android.Widget.Orientation.{attrib.Value.ApplyCSharpConvention()}";
                    subCode = $"{Name}.Orientation = {orientation};";
                }
                else if (attrib.Key == "visibility")
                {
                    subCode = $"{Name}.Visibility = Android.Views.ViewStates.{attrib.Value.ApplyCSharpConvention()};";
                }
                else if (attrib.Key == "scaleType")
                {
                    subCode = $"{Name}.SetScaleType(ImageView.ScaleType.{attrib.Value.ApplyCSharpConvention()});";
                }
                else if (attrib.Key == "gravity")
                {
                    string[] flags = null;
                    //  A.Views.GravityFlags.CenterVertical
                    if (!attrib.Value.Contains("|"))
                        flags = new[] { attrib.Value.RemoveUnderscore() };
                    else
                    {
                        string[] temp = attrib.Value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> items = new List<string>();
                        foreach (var item in temp)
                            items.Add(item.RemoveUnderscore());

                        flags = items.ToArray();
                    }

                    subCode = $"{Name}.Gravity = ";

                    if (flags.Length == 1)
                        subCode += $"A.Views.GravityFlags.{flags[0]};";
                    else
                    {
                        bool start = false;
                        foreach (var flag in flags)
                        {
                            if (start)
                                subCode += " | ";

                            subCode += $"A.Views.GravityFlags.{flag}";
                            start = true;
                        }
                        subCode += ";";
                    }
                }

                /* TODO: ****
                 *  Weight = ,
                 *  SetAutoSizeTextTypeUniformWithConfiguration
                */

                if (string.IsNullOrEmpty(subCode))
                {
                    string attribValueL = attrib.Value.ToLower().Trim();
                    if (attribValueL == "false")
                        code += $"{Name}.{attrib.Key.ApplyCSharpConvention()} = false; \n";
                    else if (attribValueL == "true")
                        code += $"{Name}.{attrib.Key.ApplyCSharpConvention()} = true; \n";
                    else
                        code += $"{Name}.{attrib.Key.ApplyCSharpConvention()} = \"{attrib.Value}\"; \n";
                }
                else
                    code += $"{subCode}\n";
            }

            // Apply layout
            // 1) Get type of layout params
            string layoutParamsType = "LinearLayout"; // default

            if (this is Layout l && l.Name.Contains("Layout"))
                layoutParamsType = DefiniedType;
            else if (Parent != null)
                layoutParamsType = Parent?.DefiniedType;

            // 2) Get width and height
            string layout_width = MatchHelper.MatchLayoutParams(Attributes["layout_width"], layoutParamsType);
            string layout_height = MatchHelper.MatchLayoutParams(Attributes["layout_height"], layoutParamsType);

            string layoutParamsCode = $"{Name}.LayoutParameters = new {layoutParamsType}.LayoutParams({layout_width}, {layout_height})";

            if (LAYOUT_ATTRIBUTES.Any(p => Attributes.ContainsKey(p) && (!DefiniedType.Contains("guideline") && p != "orientation")))
            {
                layoutParamsCode += "\n{\n";

                foreach (var attrb in LAYOUT_ATTRIBUTES.Where(p => Attributes.ContainsKey(p)))
                {
                    string name = attrb.Replace("layout_", string.Empty).ApplyCSharpConvention();
                    string value = MatchHelper.MatchLayoutParams(Attributes[attrb], string.Empty);

                    if (name == "MarginLeft")
                        name = "LeftMargin";
                    else if (name == "MarginRight")
                        name = "RightMargin";
                    else if (name == "MarginBottom")
                        name = "BottomMargin";
                    else if (name == "MarginTop")
                        name = "TopMargin";

                    if (attrb == "layout_constraintGuide_percent")
                    {
                        name = "GuidePercent";
                        value = $"{Attributes[attrb]}f";
                    }

                    layoutParamsCode += $"     {name} = {value},\n";
                }

                layoutParamsCode += "};\n";
            }
            else
                layoutParamsCode += ";\n";
            
            code += layoutParamsCode;
            code += $"{Name}.Id = View.GenerateViewId();";

            if (Parent != null)
                code += "\n";

            return code;
        }

        public bool HasConstraints => Attributes.Any(p => p.Key.Contains("layout_constraint"));

        public override string ToString()
        {
            return $"{Name}";
        }

        #region Static
        private static int uniqueCounter = 1;
        private static int constraintSetCounter = 1;

        public static bool GetUniqueVariableName(string type, out string result)
        {
            if (!type.Contains("."))
            {
                result = $"{type}{uniqueCounter++}".ApplyCSharpConvention(false);
                return true;
            }         

            string lastSegment = type.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if (!string.IsNullOrEmpty(lastSegment))
            {
                result = $"{lastSegment}{uniqueCounter++}".ApplyCSharpConvention(false);
                return true;
            }

            result = string.Empty;
            return false;
        }

        /// <summary>
        /// Should be called before each code generation
        /// </summary>
        public static void ResetUniqueCounters()
        {
            uniqueCounter = 1;
            constraintSetCounter = 1;
        }

        public static string GenerateContraintSetName()
        {
            return $"cs{constraintSetCounter++}";
        }

        #endregion
    }
}