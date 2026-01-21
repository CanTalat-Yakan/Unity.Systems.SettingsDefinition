# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials entry package via Unity's Package Manager, then install modules from the Tools menu.

- Add the entry package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

---

# Settings Definition

> Quick overview: A JSON-backed *catalog of setting metadata* (labels, tooltips, defaults, UI hints) keyed by reference path like `Controls/Mouse/Sensitivity`.

`SettingsDefinition` is not a “settings values” store. It’s the metadata side: it describes what settings exist and how to present them.

Internally it is implemented as a specialized `SettingsProfile<SettingsDefinitionBase>`:

- `SettingsDefinitionBase` is a `SerializedDictionary<string, SettingsMetaData>`.
- Key = reference path (hierarchical key) e.g. `Graphics/VSync`.
- Value = `SettingsMetaData` (type, label, tooltip, default, UI definition like slider/dropdown).
- Persistence is handled by the shared Serializer package (Json.NET + envelope + atomic writes).

## Features

- Fluent authoring API
  - `SetSlider(key, min, max, step)`
  - `SetOptions(key, options)`
  - `SetToggle(key)`
  - Chain common metadata: `.SetOrder(...)`, `.SetTooltip(...)`, `.SetLabel(...)`, `.SetDefault(...)`
- Hierarchical keys
  - Keys use `/` separators (e.g. `Controls/Mouse/Sensitivity`) so UIs can build category trees.
- JSON persistence (via `SettingsProfile`)
  - Load on demand, save explicitly via `Save()` / `SaveIfDirty()`.
- Optional normalization
  - `SettingsMetaData.Validate()` auto-fills a display label from the last key segment (e.g. `MasterVolume` → `Master Volume`).

## Requirements

- Unity 6000.0+
- Runtime module
- Depends on `UnityEssentials.SettingsProfile` and `UnityEssentials.Serializer`

## Usage

### 1) Create / edit definitions

```csharp
using UnityEssentials;

var definition = SettingsDefinition.GetOrCreate("Settings");

// Slider definition

definition
    .SetSlider("Controls/Mouse/Sensitivity", min: 0, max: 100, step: 5)
    .SetOrder(10)
    .SetTooltip("Set mouse sensitivity");

// Dropdown definition

definition
    .SetOptions("Controls/Input", new[] { "Gamepad", "Keyboard" }, @default: "Keyboard")
    .SetOrder(9)
    .SetTooltip("Select preferred input");

// Toggle definition

definition
    .SetToggle("Graphics/VSync", @default: true)
    .SetOrder(20)
    .SetTooltip("Synchronize frame output to display refresh.");

definition.SaveIfDirty();
```

### 2) Read definitions

```csharp
var definition = SettingsDefinition.GetOrCreate("Settings");
var catalog = definition.Value; // loads on demand

foreach (var kv in catalog)
{
    var key = kv.Key;
    var meta = kv.Value;
    UnityEngine.Debug.Log($"{key}: {meta.Label} ({meta.Type})");
}
```

## Persistence / File location

SettingsDefinition uses the same storage as SettingsProfile:

- `Path.Combine(Application.dataPath, "..", "Resources", "{Name}.json")`

This is **project-folder storage**, not `Application.persistentDataPath`.

## Notes / gotchas

- Definitions don’t auto-save. Call `Save()` / `SaveIfDirty()` at a time you control.
- Dirty tracking is driven by dictionary changes. If you mutate a `SettingsMetaData` instance directly without re-assigning it into the dictionary, no dictionary change event occurs.
  - Preferred: use the fluent API (it goes through `GetValue(markDirty:true)` and dictionary writes).

## Files in this package

- `Runtime/SettingsDefinition.cs` – `SettingsDefinition` wrapper + fluent authoring API
- `Runtime/SettingsContracts.cs` – `SettingsMetaData`, `SettingsUiDefinition`, enums
- `Runtime/SettingsDefinitionBuilder.cs` – fluent builder for a single entry

## Tags

unity, settings, metadata, catalog, json, serializer
