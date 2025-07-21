# GitHub Copilot Instructions for RimWorld Modding Project

Welcome to the ArchotechPlus RimWorld mod project! This document provides guidance on using GitHub Copilot to assist with development in this mod, leveraging its capabilities to streamline the coding process while adhering to project standards.

## Mod Overview and Purpose

ArchotechPlus is a RimWorld mod designed to enhance gameplay with advanced technology and implants that further push the boundaries of the game's existing bionic systems. The mod introduces new classes and features aimed at enhancing player interaction with advanced medical and technological systems within the game.

## Key Features and Systems

1. **Advanced Implants**: Enables installation of high-tech implants that provide unique abilities and enhancements to pawns.
2. **Target Following System**: Implements a component that allows objects or pawns to follow designated targets, increasing tactical options.
3. **Resurrection System**: Introduces a system to resurrect fallen comrades, conditioned by specific requirements.
4. **Regeneration Mechanism**: Provides a way for pawns to naturally heal and regenerate over time, simulating advanced medical technology.

## Coding Patterns and Conventions

- **Naming Conventions**: Classes and methods are named using PascalCase. Private fields usually start with an underscore followed by camelCase. 
- **Class Structure**: Classes are grouped by functionality, such as components (`Comp*`) and their properties (`CompProperties_*`).
- **Modular Design**: Logic is separated into smaller methods for clarity and maintainability. This allows for easier patching and extending in the future.

## XML Integration

While the summary does not include specific XML content, XML files typically define Defs for items, hediffs, and other game elements that are integrated with C# code through properties and components. XML files should be structured clearly and documented to ensure seamless integration with the code. Attributes and their relationships should be reflected accurately in the C# classes.

## Harmony Patching

Harmony is used for method patching to add, modify, or completely override existing game logic without directly altering game files.

- **Patch Types**: Identify method entry and exit points using `Prefix` and `Postfix` patches.
- **Selective Patching**: Use Transpilers sparingly for changes that cannot be achieved with other patch types.
- **Patch Safety**: Ensure patches check for null references and are reversible if needed.

## Suggestions for Copilot

- **Auto-complete with Class Templates**: Leverage Copilot for automatically generating class and method stubs according to the project’s conventions.
- **Code Suggestions for Conditions and Logic**: Utilize Copilot to suggest efficient conditional checks and loops, especially within methods like `ResurrectionConditionsMet()` and `IsPawnInjured()`.
- **XML Code Integration**: Use Copilot to generate C# property accessors and mutators for XML-defined attributes.
- **Harmony Patch Templates**: Encourage Copilot to provide boilerplate code for common Harmony patches, ensuring adherence to safe patching practices.

By using these guidelines with Copilot, developers can enhance their productivity while maintaining high code quality and consistency throughout the ArchotechPlus mod project.
