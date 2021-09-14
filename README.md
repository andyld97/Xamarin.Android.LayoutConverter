# Xamarin.Android.LayoutConverter

## Why is this necessary?

Normally it is not necessary to declare Android layouts via codebehind, because after all there are *.axml files that can be easily edited with the layout editor. However, in some cases it may be useful to define a layout via codebehind.

Example:
If you develop a plugin DLL for a Xamarin.Android app, you cannot use the usual resources/assets there, as these are not loaded correctly at runtime while the assembly is being loaded, i.e. you cannot use layout files there either. 

## How it works

The Android layout you want to use can be created normally with Android Studio (works best there) or with Xamarin.Android Designer and then you can specify the file in LayoutConverter or paste it by text and you will get the generated C# code. 

The code should compile in most cases, but usually needs some rework, because not all features are supported yet, e.g. 
- ```txtMoves.Text = "@string/moves"; ```
- ```txtMoves.TextAlignment = "gravity"; ```
 
If you want, you are welcome to implement further properties, but I will also extend the program from time to time.

## Features
- :heavy_check_mark: ConstraintLayouts are supported using ConstraintSets!
- :heavy_check_mark: Nested layouts will also work!
- :heavy_check_mark: Copy parts or everything of the generated code to the clipboard or you can export it as a C#-File!
- :heavy_check_mark: Example files avaiable in the menu!
