namespace Layout_Converter.Enums
{
    //
    // Zusammenfassung:
    //     Enumerates values returned by several methods of Android.Util.ComplexUnitType
    //     and taken as a parameter of the F:Android.Util.TypedValue.ApplyDimension, and
    //     F:Android.Widget.TextView.SetTextSize members.
    //
    // Hinweise:
    //     Portions of this page are modifications based on work created and shared by the
    //     Android Open Source Project and used according to terms described in the Creative
    //     Commons 2.5 Attribution License.
    public enum ComplexUnitType
    {
        //
        // Zusammenfassung:
        //     Android.Util.DataType.Fractioncomplex unit: A basic fraction of the overall size.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Fraction = 0,
        //
        // Zusammenfassung:
        //     Android.Util.DataType.Dimensioncomplex unit: Value is raw pixels.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Px = 0,
        //
        // Zusammenfassung:
        //     Complex data: bit location of unit information.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Shift = 0,
        //
        // Zusammenfassung:
        //     Android.Util.DataType.Dimensioncomplex unit: Value is Device Independent Pixels.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Dip = 1,
        //
        // Zusammenfassung:
        //     Android.Util.DataType.Fractioncomplex unit: A fraction of the parent size.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        FractionParent = 1,
        //
        // Zusammenfassung:
        //     Android.Util.DataType.Dimensioncomplex unit: Value is a scaled pixel.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Sp = 2,
        //
        // Zusammenfassung:
        //     Android.Util.DataType.Dimensioncomplex unit: Value is in points.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Pt = 3,
        //
        // Zusammenfassung:
        //     Android.Util.DataType.Dimensioncomplex unit: Value is in inches.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        In = 4,
        //
        // Zusammenfassung:
        //     Android.Util.DataType.Dimensioncomplex unit: Value is in millimeters.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Mm = 5,
        //
        // Zusammenfassung:
        //     Complex data: mask to extract unit information (after shifting by Android.Util.ComplexUnitType.Shift).
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Mask = 15
    }



    //
    // Zusammenfassung:
    //     Enumerates values returned by several types and taken as a parameter of several
    //     types.
    //
    // Hinweise:
    //     Portions of this page are modifications based on work created and shared by the
    //     Android Open Source Project and used according to terms described in the Creative
    //     Commons 2.5 Attribution License.
    public enum TypefaceStyle
    {
        //
        // Zusammenfassung:
        //     To be added.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Normal = 0,
        //
        // Zusammenfassung:
        //     To be added.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Bold = 1,
        //
        // Zusammenfassung:
        //     To be added.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        Italic = 2,
        //
        // Zusammenfassung:
        //     To be added.
        //
        // Hinweise:
        //     Portions of this page are modifications based on work created and shared by the
        //     Android Open Source Project and used according to terms described in the Creative
        //     Commons 2.5 Attribution License.
        BoldItalic = 3
    }


    public enum ClipboardAction
    {
        Complete,
        ConstraintSet,
        ViewDefinitions,
        VariableDeclarations
    }
}
