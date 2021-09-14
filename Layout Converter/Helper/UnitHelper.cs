using Layout_Converter.Enums;
using System.Linq;

namespace Layout_Converter.Helper
{
    public static class UnitHelper
    {
        private static readonly string[] units = new string[] { "dp", "px", "sp" };
        private static readonly ComplexUnitType[] match = new ComplexUnitType[] { ComplexUnitType.Dip, ComplexUnitType.Px, ComplexUnitType.Sp };

        public static bool ParseNumber(string input, out float value, out ComplexUnitType complexUnitType)
        {
            complexUnitType = ComplexUnitType.Px; // default or not specified
            value = 0; // default

            if (units.Any(u => input.Contains(u)))
            {
                string newInput = input;
                int counter = 0;
                foreach (var u in units)
                {
                    newInput = newInput.Replace(u, string.Empty);

                    if (newInput != input)
                    {
                        complexUnitType = match[counter];
                        if (float.TryParse(newInput, out value))
                            return true;

                        break;
                    }

                    counter++;
                }
                    
            }

            return false;
        }  
    }
}
