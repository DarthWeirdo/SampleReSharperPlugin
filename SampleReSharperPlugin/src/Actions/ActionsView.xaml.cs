using JetBrains.UI.Wpf;

namespace SampleReSharperPlugin
{
    [View]
    public partial class ActionsView : IView<ActionsViewModel>
    {
        public ActionsView()
        {
            InitializeComponent();
        }
    }
}
