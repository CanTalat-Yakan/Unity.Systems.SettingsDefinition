using UnityEngine;

namespace UnityEssentials.Samples
{
    public class SettingsDefinitionSample : MonoBehaviour
    {
        // Like SettingsProfileSample: keep a static reference around.
        public static readonly SettingsDefinition Definition = SettingsDefinition.GetOrCreate("Settings");

        private void Awake()
        {
            UseDefinition();
        }

        private static void UseDefinition()
        {
            Definition
                .SetSlider("Controls/Mouse/Sensitivity", min: 0, max: 100, step: 5, @default: 50)
                .SetTooltip("Set mouse sensitivity")
                .SetOrder(10);

            Definition
                .SetOptions("Controls/Input", new[] { "Gamepad", "Keyboard" }, @default: 1)
                .SetTooltip("Select preferred input")
                .SetOrder(9);

            Definition
                .SetToggle("Graphics/VSync", @default: true)
                .SetTooltip("Synchronize frame output to display refresh.")
                .SetOrder(20);

            // Read access.
            var catalog = Definition.Value;
            foreach (var kv in catalog)
                Debug.Log($"Definition: {kv.Key} -> {kv.Value.Label} ({kv.Value.Type})");
        }

        private void OnApplicationQuit()
        {
            Definition.SaveIfDirty();
        }
    }
}
