﻿using System;
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
                    
                    var toolPanel = new ToolPanel();

                    // Tool Window
                    var toolwndView = new ToolWindowView();
                    var toolwndViewTab = new TabItem
                    {
                        Content = toolwndView,
                        Header = toolwndView.Name
                    };
                    toolPanel.tabControl.Items.Add(toolwndViewTab);

                    // Options 
                    var optionsPageViewModel = new OptionsPageViewModel(lt, settingsStore);
                    var optionsPageView = new OptionsPageView {DataContext = optionsPageViewModel};
                    var optionsPageViewTab = new TabItem
                    {
                        Content = optionsPageView,
                        Header = optionsPageView.Name                        
                    };
                    toolPanel.tabControl.Items.Add(optionsPageViewTab);

                    // Actions
                    var actionsViewModel = new ActionsViewModel(lt);
                    var actionsView = new ActionsView {DataContext = actionsViewModel};
                    var actionsViewTab = new TabItem
                    {
                        Content = actionsView,
                        Header = actionsView.Name
                    };
                    toolPanel.tabControl.Items.Add(actionsViewTab);

                    // Solution Component
                    var solcompView = new SolutionComponentView();
                    var solcompViewTab = new TabItem
                    {
                        Content = solcompView,
                        Header = solcompView.Name
                    };
                    toolPanel.tabControl.Items.Add(solcompViewTab);

                    // Signals
                    var sigViewModel = new SignalsViewModel(lt);
                    var sigView = new SignalsView {DataContext = sigViewModel};
                    var sigViewTab = new TabItem
                    {
                        Content = sigView,
                        Header = sigView.Name
                    };
                    toolPanel.tabControl.Items.Add(sigViewTab);

                    // IProperty
                    var propViewModel = new PropertyViewModel(lt);
                    var propView = new PropertyView { DataContext = propViewModel };
                    var propViewTab = new TabItem
                    {
                        Content = propView,
                        Header = propView.Name
                    };
                    toolPanel.tabControl.Items.Add(propViewTab);

                    return new EitherControl(lt, toolPanel);                    
                });
        }

        public void Show()
        {            
            _toolWindowClass.Show();
        }



    }
}