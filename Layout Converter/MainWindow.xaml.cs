using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Layout_Converter.Enums;
using Layout_Converter.Folding;
using Layout_Converter.Helper;
using Layout_Converter.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Layout_Converter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members
        private FoldingManager textResultFoldingManager;
        private FoldingManager textXMLFoldingManager;
        private Layout rootLayout = null;
        private BraceFoldingStrategy braceFoldingStrategy = new BraceFoldingStrategy();
        private XmlFoldingStrategy xmlFoldingStrategy = new XmlFoldingStrategy();

        private readonly List<Rule> defaultRuleSet = new List<Rule>()
        {
            new Rule() { Search = "AndroidX.Constraintlayout.Widget.", Replace = string.Empty},
            new Rule() { Search = "AndroidX.Appcompat.Widget.", Replace = string.Empty},
            new Rule() { Search = "Android.", Replace = "A."},
        };
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            // Initalize syntax highlighting
            TextAXML.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML");
            TextAXML.ShowLineNumbers = true;
            TextResult.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            TextResult.ShowLineNumbers = true;
            TextResult.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.CSharp.CSharpIndentationStrategy(TextResult.Options);

            // TextChanged (Conversation & Folding)
            TextResult.TextChanged += TextResult_TextChanged;
            TextAXML.TextChanged += TextAXML_TextChanged;
        }

        #region TextChanged/Folding

        private void TextResult_TextChanged(object sender, EventArgs e)
        {
            if (textResultFoldingManager != null)
                FoldingManager.Uninstall(textResultFoldingManager);

            textResultFoldingManager = FoldingManager.Install(TextResult.TextArea);
            braceFoldingStrategy.UpdateFoldings(textResultFoldingManager, TextResult.Document);
        }

        private void TextAXML_TextChanged(object sender, EventArgs e)
        {
            if (textXMLFoldingManager != null)
                FoldingManager.Uninstall(textXMLFoldingManager);

            GenerateCode(TextAXML.Text);
            textXMLFoldingManager = FoldingManager.Install(TextAXML.TextArea);
            xmlFoldingStrategy.UpdateFoldings(textXMLFoldingManager, TextAXML.Document);
        }

        #endregion

        #region Menu Buttons

        private void MenuButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog() { Filter = "Layout Files|*.axml;*.xml;"  })
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        TextAXML.Text = System.IO.File.ReadAllText(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to open file: {ex.Message}");
                    }
                }
            }
        }

        private void MenuButtonSave_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog() { Filter = "C# Class|*.cs;" })
            {
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        System.IO.File.WriteAllText(sfd.FileName, rootLayout.GenerateLayoutCode().ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save file: {ex.Message}");
                    }
                }
            }
        }

        private void MenuButtonCopyToClipboardComplete_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard(ClipboardAction.Complete);
        }

        private void MenuButtonCopyToClipboardConstraintSets_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard(ClipboardAction.ConstraintSet);
        }

        private void MenuButtonCopyToClipboardVariables_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard(ClipboardAction.VariableDeclarations);
        }

        private void MenuButtonCopyToClipboardViewDef_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard(ClipboardAction.ViewDefinitions);
        }

        private void MenuButtonLoadEx1_Click(object sender, RoutedEventArgs e)
        {
            LoadExampleFile("sign");
        }

        private void MenuButtonLoadEx2_Click(object sender, RoutedEventArgs e)
        {
            LoadExampleFile("sub");
        }

        private void MenuButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        #endregion

        #region Methods

        public void GenerateCode(string content)
        {
            try
            {
                View.ResetUniqueCounters();
                rootLayout = Converter.ParseXML(content);
                string code = rootLayout.GenerateLayoutCode().GenerateCode("MyProject", "MyClass", defaultRuleSet);

                TextResult.Text = code;
                StatusLabel.Text = "Status: OK";
            }
            catch (Exception ex)
            {
                // Display error message in the bottom status bar
                StatusLabel.Text = $"Status: Failed ({ex.Message})";
                TextResult.Clear();
            }
        }

        private void LoadExampleFile(string fileName)
        {
            try
            {
                TextAXML.Text = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Examples\{fileName}.xml"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load example file: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }   

        public void CopyToClipboard(ClipboardAction action)
        {
            Code code = rootLayout?.GenerateLayoutCode();
            if (code == null)
                return;

            string clipbboardText = string.Empty;
            switch (action)
            {
                case ClipboardAction.Complete: clipbboardText = code.ToString(); break;
                case ClipboardAction.ConstraintSet: clipbboardText = string.Join("\n", code.ConstraintSets); break;
                case ClipboardAction.VariableDeclarations:
                    {
                        foreach (var v in code.Variables)
                            clipbboardText += $"private {v} = null; \n";
                    }
                    break;
                case ClipboardAction.ViewDefinitions: clipbboardText = code.ViewCode; break;
            }

            if (!string.IsNullOrEmpty(clipbboardText))
                Clipboard.SetText(clipbboardText.ApplyRules(defaultRuleSet));
        }
        #endregion
    }
}
