# Accessible Wayfinding Framework for Unity

A modular, open-source framework for Unity that simplifies the implementation of accessible navigation features in games. It provides a centralised system for managing wayfinding cues, allowing players to customise their experience based on sensory, cognitive, or motor needs. The framework is lightweight, leveraging Unity's built-in tools, such as NavMesh, for efficiency.

## Overview

- [Motivation](#motivation)
- [Installation](#installation)
- [Getting Started](#getting-started)
- [Extending the Framework](#extending-the-framework)
- [Contributing](#contributing)
- [License](#license)
- [Issues](#issues)
- [Author](#author)

## Motivation

Video games should be enjoyable for everyone, but navigation can pose significant barriers for players with disabilities. This framework bridges the gap by offering easy-to-integrate tools that enhance wayfinding accessibility, making it simpler for indie developers and students to create inclusive experiences. Inspired by the need for better developer resources in response to growing accessibility standards, it empowers you to build games that reach a wider audience without compromising core design.

## Installation

### Adding the Package

1. Open the Unity Editor and navigate to **Window > Package Manager**.
2. Click the **+** icon in the top-left corner and select **Add package from git URL...**.
3. Enter the following URL: `https://github.com/Rythiq/AccessibleWayfinding.git`.
4. The package will appear in the Package Manager. Select it to install.

### Importing Samples (Optional)

Once the package is installed:
1. In the Package Manager, select the Accessible Wayfinding Framework.
2. Click **Samples** and import the desired assets, such as the demo scene or example settings menu prefab.

This will add optional assets to your project's `/Assets/Samples/AccessibleWayfinding` folder for quick testing and reference.

## Getting Started

To set up the framework in a scene:

1. Create an empty GameObject in your initial scene and attach the `WayfindingManager` script (a MonoBehaviour Singleton).
2. Assign the player's `Transform` to the manager's `playerTransform` field in the Inspector or via code.
3. Implement your game's objectives using the `IObjective` and `IAction` interfaces. For simple cases, use the provided `SingleActionObjective` and `ReachPositionAction`.
   - Example: Create a `SingleActionObjective` with a `ReachPositionAction` pointing to a target Transform and a description.
4. Register cue implementations (e.g., `VisualPathCue`, `AudioPathCue`) by attaching them to GameObjects and calling `WayfindingManager.Instance.RegisterCue(this)` in their `Start()` method.
5. Provide player settings via `IWayfindingAccessibilitySettings`. Use the included `MinimalWayfindingAccessibilitySettings` prefab for a basic UI menu with toggles for cue categories.
6. Call `WayfindingManager.Instance.SetObjective(yourObjective)` to activate cues based on the current action and settings.

The framework will handle cue activation/deactivation automatically as objectives change.

## Extending the Framework

The framework is designed for easy extension through the `ICue` interface. Here's an example of creating a custom haptic cue:

1. Create a new C# script `HapticFeedbackCue.cs` that inherits from `MonoBehaviour` and implements `ICue`.

```csharp
using UnityEngine;
using AccessibleWayfinding;

public class HapticFeedbackCue : MonoBehaviour, ICue // this cue would be directly attached to the player
{
    public CueType Type => CueType.Haptic; // Or assign None if you want Activate() to always be called
    public bool IsActive { get; private set; }

    private Transform target; // only if your cue needs one

    public void Activate(Transform target) // If you used the None CueType, you will have to use custom evaluation here. You can get the current settings from WayfindingManager.Instance.wayfindingAccessibilitySettings and query your required information.
    {
        this.target = target; // if your cue doesn't need a target, feel free to ignore it...
        IsActive = true; // if custom type, decide this by querying settings
    }

    public void Deactivate() 
    {
        // Stop any vibrations
        IsActive = false;
    }

    void Update()
    {
        if (!IsActive) return;
        // example logic for looking at a target
        Vector3 direction = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, direction);

        if (dot > 0.9f) // Threshold for facing the target
        {
            // Trigger controller vibration (using Unity's Input System or platform API)
        }
    }

    void Start()
    {
        WayfindingManager.Instance.RegisterCue(this);
    }
}
```

2. Attach the script to a GameObject (e.g., the player).
3. For custom settings, extend `IWayfindingAccessibilitySettings` with a new bool (e.g., `EnabledMyNewTypeCues`) and check it in `Activate()`.

This modular approach lets you add new cues without modifying the core system.

## Contributing
Contributions are welcome to expand cues, improve integration, or add support for new types of impairments. Clone the repo, make your changes, and submit a pull request. Let's collaborate to evolve this framework and help developers create accessible games for all.

## License

This project is licensed under the GNU Lesser General Public License (LGPL). See the [LICENSE](LICENSE) file for details.

## Issues

If you encounter bugs or have feature requests, please open an issue on GitHub.

## Author

Developed by Werner Richter as part of a Bachelor's thesis in Informatics: Games Engineering at the Technical University of Munich (TUM).
