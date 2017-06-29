using System;
using System.Windows.Input;
using JetBrains.Common.Util;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Navigation;
using JetBrains.Util;

namespace SampleReSharperPlugin
{
    class NavigateByReference : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var solutionStateTracker = SolutionStateTracker.Instance;
            var solution = solutionStateTracker.Solution;
            var detector = solution?.GetComponent<NodeUnderCaretDetector>();
            detector?.NavigateToFirstReferencedElement();
        }

        public event EventHandler CanExecuteChanged;
    }
}