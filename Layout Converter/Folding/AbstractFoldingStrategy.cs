using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using System.Collections.Generic;

namespace Layout_Converter.Folding
{
    public abstract class AbstractFoldingStrategy
    {
        public void UpdateFoldings(FoldingManager manager, TextDocument document)
        {
            int firstErrorOffset;
            IEnumerable<NewFolding> newFoldings = CreateNewFoldings(document, out firstErrorOffset);
            manager.UpdateFoldings(newFoldings, firstErrorOffset);
        }

        public abstract IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset);
    }
}
