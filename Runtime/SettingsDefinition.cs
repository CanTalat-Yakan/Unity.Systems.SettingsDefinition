using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace UnityEssentials
{
    [Serializable]
    public class SettingsDefinitionBase : SerializedDictionary<string, SettingsMetaData> { }

    public sealed class SettingsDefinition : SettingsProfile<SettingsDefinitionBase>
    {
        public new static SettingsDefinition GetOrCreate(string name = "Default") =>
            SettingsDefinitionRegistry.GetOrCreate(name, n => new SettingsDefinition(n));

        public SettingsDefinition(string name) : base(name) { }

        public new SettingsDefinitionBase GetValue(bool markDirty = true, bool notify = true) =>
            base.GetValue(markDirty, notify);

        public new SettingsDefinitionBase Load() =>
            base.Load();

        public SettingsDefinitionBuilder SetSlider(string key, float min, float max, float step, float? @default = null, string unit = null)
        {
            var builder = GetOrCreateSetting(key, SettingsValueType.Float);
            if (@default.HasValue)
                builder.SetDefault(JToken.FromObject(@default.Value));
            return builder.SetSlider(min, max, step, unit);
        }

        public SettingsDefinitionBuilder SetIntSlider(string key, int min, int max, int step, int? @default = null, string unit = null)
        {
            var builder = GetOrCreateSetting(key, SettingsValueType.Int);
            if (@default.HasValue)
                builder.SetDefault(JToken.FromObject(@default.Value));
            return builder.SetSlider(min, max, step, unit);
        }

        public SettingsDefinitionBuilder SetToggle(string key, bool? @default = null)
        {
            var builder = GetOrCreateSetting(key, SettingsValueType.Bool);
            if (@default.HasValue)
                builder.SetDefault(JToken.FromObject(@default.Value));
            return builder.SetToggle();
        }

        public SettingsDefinitionBuilder SetOptions(string key, IEnumerable<string> options, string @default = null, bool reverseOrder = false)
        {
            var builder = GetOrCreateSetting(key, SettingsValueType.Enum);
            if (@default != null)
                builder.SetDefault(JToken.FromObject(@default));
            return builder.SetOptions(options, reverseOrder);
        }

        public SettingsDefinitionBuilder GetOrCreateSetting(string key, SettingsValueType type)
        {
            if (string.IsNullOrWhiteSpace(key)) key = string.Empty;
            var writableCatalog = GetValue(markDirty: true, notify: false);

            SettingsMetaData def;
            if (!writableCatalog.TryGetValue(key, out def) || def == null)
            {
                def = new SettingsMetaData() { Key = key, Type = type };
                writableCatalog[key] = def;
            }

            def.Key = key;
            def.Type = type;
            def.Validate();

            return new SettingsDefinitionBuilder(def);
        }
    }
}
