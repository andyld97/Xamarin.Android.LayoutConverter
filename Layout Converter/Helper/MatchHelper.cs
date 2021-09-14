namespace Layout_Converter.Helper
{
    public static class MatchHelper
    {
        public static string MatchLayoutParams(string value, string layoutParamsType)
        {
            string result = value; // ensure not to return null

            if (value == "wrap_content")
                result = $"{layoutParamsType}.LayoutParams.WrapContent";
            else if (value == "match_parent")
                result = $"{layoutParamsType}.LayoutParams.MatchParent";
            else if (value == "match_constraint" || (value == "0dp" && layoutParamsType.Contains("ConstraintLayout")))
                result = $"{layoutParamsType}.LayoutParams.MatchConstraint";
            else if (value == "horizontal")
                result = "ConstraintSet.Horizontal";
            else if (value == "vertical")
                result = "ConstraintSet.Vertical";

            // ToDO: *** Add more cases

            else
            {
                // It may be something like 15dp
                if (UnitHelper.ParseNumber(value, out float number, out Enums.ComplexUnitType complexUnitType))
                {
                    return $"(int)Android.Util.TypedValue.ApplyDimension(Android.Util.ComplexUnitType.{complexUnitType}, {number}f, context.Resources.DisplayMetrics)";
                }
            }

            return result;
        }
    }
}
