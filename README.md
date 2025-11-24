# Godot .NET Project

This project is a game developed using the **Godot Engine** with C# integration.

## 🛠 Technical Overview

The project is built on a the following stack:
*   **Engine:** Godot 4 (using Godot .NET SDK `4.5.0`)
*   **Framework:** .NET 8.0
*   **Language:** C# 12.0

## ✨ Key Features

### Technical Features
*   **High Performance:** Utilizes the latest .NET 8 runtime for optimized script execution.
*   **Modern Syntax:** leveraging C# 12 features (primary constructors, collection expressions, etc.) for cleaner code.
*   **Type Safety:** Full static typing support across the codebase.

### Gameplay Features
*(TODO: Add your specific game mechanics here. Examples below:)*
*   **Static Loot Creation** Create loot items that are static according to your specifications.
*   **Procedural Loot Generation** Generate loot procedurally.
*   **Configurable** where possible there are configurable/exposed parameters in the UI for items to customize the inventory to your particular project.
*   **UI System:** Contains UI nodes that can be used out of the box, as wel as example scripts and scenes to configure and use them.
*   **Random Number Singleton** Includes an autoload singleton that is used for RandomNumber generation for procedural generation. You can mark this with a fixed seed for reproducibility and/or testing or seeded runs.
*   **Inventory Events Singleton** Includes an autoload for sending inventory events between system.

## 🚀 Getting Started

1.  Ensure you have **Godot 4.x (.NET edition)** installed.
2.  Ensure you have the **.NET 8.0 SDK** installed.
3.  Clone the repository.
4.  Open the `project.godot` file in the Godot Editor to import assets.
5.  Open the solution (`.sln`) in **JetBrains Rider** to edit code.

## 🧪 Testing

None at this time - all manual!
