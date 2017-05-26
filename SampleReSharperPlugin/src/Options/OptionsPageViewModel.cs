﻿using JetBrains.Application.Settings;
using JetBrains.DataFlow;
using JetBrains.UI.Wpf;

namespace SampleReSharperPlugin
{
    public class OptionsPageViewModel: AAutomation    
    {
        public IProperty<string> Text { get; set; }

        public string Name => "Options";

        public OptionsPageViewModel(Lifetime lifetime, ISettingsStore settingsStore)
        {
            Text = new Property<string>(lifetime, "OptionsExampleViewModel.Text");

            var checkMeOption =
                settingsStore.BindToContextLive(lifetime, ContextRange.ApplicationWide)
                    .GetValueProperty(lifetime, (MySettingsKey key) => key.CheckMe);

            checkMeOption.Change.Advise_HasNew(lifetime, v =>
            {
                Text.Value = v.New ? "checked" : "not checked";
            });
        }
    }
}
