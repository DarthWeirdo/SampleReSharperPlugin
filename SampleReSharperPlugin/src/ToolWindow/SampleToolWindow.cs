using System;
using System.Windows.Controls;
using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.Threading;
using JetBrains.UI.Application;
using JetBrains.UI.Controls;
using JetBrains.UI.CrossFramework;
using JetBrains.UI.ToolWindowManagement;
using JetBrains.Util;

namespace SampleReSharperPlugin
{
    public class SampleToolWindow
    {
        private readonly TabbedToolWindowClass _toolWindowClass;
        private readonly ToolWindowInstance _toolWindowInstance;        

        public SampleToolWindow(Lifetime lifetime, ToolWindowManager toolWindowManager,
            SampleToolWindowDescriptor sampleToolWindowDescriptor, ISettingsStore settingsStore)
        {
            _toolWindowClass = toolWindowManager.Classes[sampleToolWindowDescriptor] as TabbedToolWindowClass;
            if (_toolWindowClass == null)
                throw new ApplicationException("ToolWindowClass");                        

            _toolWindowInstance = _toolWindowClass.RegisterInstance(lifetime, "Sample Tool Window", null,
                (lt, twi) =>
                {
                    twi.QueryClose.Value = true;
                    
                    var myToolPanel = new ToolPanel();

                    // Options 
                    var optionsPageViewModel = new OptionsPageViewModel(lt, settingsStore);
                    var optionsPageView = new OptionsPageView {DataContext = optionsPageViewModel};
                    var optionsPageViewTab = new TabItem
                    {
                        Content = optionsPageView,
                        Header = optionsPageViewModel.Name                        
                    };
                    myToolPanel.tabControl.Items.Add(optionsPageViewTab);

                    // Actions
                    var actionsViewModel = new ActionsViewModel(lt);
                    var actionsView = new ActionsView {DataContext = actionsViewModel};
                    var actionsViewTab = new TabItem
                    {
                        Content = actionsView,
                        Header = actionsViewModel.Name
                    };
                    myToolPanel.tabControl.Items.Add(actionsViewTab);

                    return new EitherControl(lt, myToolPanel);                    
                });
        }

        public void Show()
        {            
            _toolWindowClass.Show();
        }



    }
}