using JetBrains.DocumentManagers;
using JetBrains.DocumentManagers.Transactions;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace SampleReSharperPlugin
{
    class PsiNavigationHelper
    {
        private readonly DocumentManager _documentManager;
        private readonly ITextControlManager _textControlManager;

        public PsiNavigationHelper(DocumentManager documentManager, ITextControlManager textControlManager)
        {
            _documentManager = documentManager;
            _textControlManager = textControlManager;
        }

        public ITreeNode GetTreeNodeUnderCaret()
        {
            var textControl = _textControlManager.LastFocusedTextControl.Value;
            if (textControl == null)
                return null;

            var projectFile = _documentManager.TryGetProjectFile(textControl.Document);
            if (projectFile == null)
                return null;

            var range = new TextRange(textControl.Caret.Offset());

            var psiSourceFile = projectFile.ToSourceFile().NotNull("File is null");

            var documentRange = range.CreateDocumentRange(projectFile);
            var file = psiSourceFile.GetPsiFile(psiSourceFile.PrimaryPsiLanguage, documentRange);

            var element = file?.FindNodeAt(documentRange);
            return element;
        }
    }
}
