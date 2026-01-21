using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace UnityEssentials
{
    public enum SettingsValueType
    {
        Bool,
        Int,
        Float,
        String,
        Enum,
        Color,
        Json
    }

    [Flags]
    public enum SettingsFlags
    {
        None = 0,
        RestartRequired = 1 << 0,
        Hidden = 1 << 1,
        Advanced = 1 << 2,
        ReadOnly = 1 << 3,
    }

    public enum SettingsUiControl
    {
        Auto,
        Slider,
        Dropdown,
        Toggle,
        InputField,
    }

    [Serializable]
    public sealed class SettingsUiDefinition : ISettingsValidate
    {
        public SettingsUiControl Control = SettingsUiControl.Auto;

        // Common
        public string Unit;

        // Slider
        public float? Min;
        public float? Max;
        public float? Step;

        // Dropdown
        public List<string> Options;
        public bool ReverseOrder;

        public void Validate()
        {
            Unit ??= string.Empty;
            Options ??= new List<string>();
        }
    }
    
    

    [Serializable]
    public class SettingsMetaData : ISettingsValidate
    {
        /// <summary>Hierarchical key path for the setting (e.g. "Controls/Mouse/Sensitivity").</summary>
        public string Key;

        public SettingsValueType Type;

        /// <summary>Display label. If empty, derived from <see cref="Key"/>'s last segment.</summary>
        public string Label;

        public string Tooltip;

        /// <summary>
        /// Default value for the setting.
        /// Stored as JSON token so it can represent bool/int/float/string/enums/arrays.
        /// </summary>
        public JToken Default;

        public SettingsUiDefinition Ui;

        public int Order;

        public SettingsFlags Flags;

        /// <summary>Optional stable identifier separate from <see cref="Key"/>.</summary>
        public string Id;

        public void Validate()
        {
            Key = string.IsNullOrWhiteSpace(Key) ? string.Empty : Key.Trim();
            Id = string.IsNullOrWhiteSpace(Id) ? null : Id.Trim();

            Label = string.IsNullOrWhiteSpace(Label)
                ? SerializerUtility.LabelizePathSegment(SerializerUtility.GetLastSegment(Key))
                : Label;

            Tooltip ??= string.Empty;
            Ui ??= new SettingsUiDefinition();
            Ui.Validate();
        }
    }
}
