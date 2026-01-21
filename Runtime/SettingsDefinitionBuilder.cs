using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace UnityEssentials
{
    /// <summary>
    /// Fluent builder for a single entry inside a <see cref="SettingsDefinitionBase"/>.
    /// Instances are lightweight wrappers around the underlying <see cref="SettingsMetaData"/>.
    /// </summary>
    public readonly struct SettingsDefinitionBuilder
    {
        private readonly SettingsMetaData _metadata;

        internal SettingsDefinitionBuilder(SettingsMetaData metadata) => _metadata = metadata;

        public SettingsDefinitionBuilder SetLabel(string label)
        {
            _metadata.Label = label;
            return this;
        }

        public SettingsDefinitionBuilder SetTooltip(string tooltip)
        {
            _metadata.Tooltip = tooltip;
            return this;
        }

        public SettingsDefinitionBuilder SetOrder(int order)
        {
            _metadata.Order = order;
            return this;
        }

        public SettingsDefinitionBuilder SetFlags(SettingsFlags flags)
        {
            _metadata.Flags = flags;
            return this;
        }
        
        public SettingsDefinitionBuilder SetSlider(float min, float max, float step, string unit = null)
        {
            _metadata.Ui ??= new SettingsUiDefinition();
            _metadata.Ui.Control = SettingsUiControl.Slider;
            _metadata.Ui.Min = min;
            _metadata.Ui.Max = max;
            _metadata.Ui.Step = step;
            if (unit != null) _metadata.Ui.Unit = unit;
            return this;
        }

        public SettingsDefinitionBuilder SetOptions(IEnumerable<string> options, bool reverseOrder = false)
        {
            _metadata.Ui ??= new SettingsUiDefinition();
            _metadata.Ui.Control = SettingsUiControl.Dropdown;
            _metadata.Ui.Options = options == null ? new List<string>() : new List<string>(options);
            _metadata.Ui.ReverseOrder = reverseOrder;
            return this;
        }

        public SettingsDefinitionBuilder SetToggle()
        {
            _metadata.Ui ??= new SettingsUiDefinition();
            _metadata.Ui.Control = SettingsUiControl.Toggle;
            return this;
        }

        public SettingsDefinitionBuilder SetDefault(JToken value)
        {
            _metadata.Default = value;
            return this;
        }
    }
}
