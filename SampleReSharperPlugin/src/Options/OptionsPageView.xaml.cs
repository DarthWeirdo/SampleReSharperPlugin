﻿using JetBrains.UI.Wpf;

namespace SampleReSharperPlugin
{
    [View]
    public partial class OptionsPageView : IView<OptionsPageViewModel>
    {
        public OptionsPageView()
        {
            InitializeComponent();
        }
    }
}
