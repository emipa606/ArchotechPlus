# GitHub Copilot Instructions for Archotech+ Mod

This document provides guidelines and instructions for using GitHub Copilot for the Archotech+ mod for RimWorld. The Archotech+ mod is a lightweight expansion that adds four new archotech implants that are rewarded from quests or tasks. These implants are balanced around vanilla and royalty implants and while they are powerful, they will not make any pawn invincible.

## Mod Overview and Purpose

Archotech+ is a mod that introduces new archotech implants to the game RimWorld. These implants can be obtained through quests or exotic merchants and can provide various benefits to pawns, such as healing missing body parts, scars, permanent damage, and age-related chronic conditions. The mod also includes a regenerator that can resurrect dead pawns.

## Key Features and Systems

- Four new archotech implants: These implants can be obtained through quests or exotic merchants and provide various benefits to pawns.
- Regenerator: The regenerator can heal missing body parts, scars, permanent damage, and age-related chronic conditions. It can also resurrect dead pawns.
- De-Aging option: An option to turn off the De-Aging feature.
- Harmony patching: The mod uses Harmony to patch the game's code.

## Coding Patterns and Conventions

- Use meaningful and descriptive variable and method names.
- Follow the RimWorld coding conventions and best practices.
- Use comments to explain complex code or logic.

## XML Integration

The Archotech+ mod uses XML files to define game objects such as hediffs, thingDefs, and other game entities. When using GitHub Copilot, you can generate XML code for new game objects or modify existing ones. Here are some tips for working with XML in the Archotech+ mod:

- Use the existing XML files as a reference for how to define new game objects.
- Use the `<defName>` attribute to uniquely identify each game object.
- Use the `<description>` attribute to provide a description of the game object.
- Use the `<statBases>` and `<statFactors>` attributes to define the stats and factors of the game object.

## Harmony Patching

The Archotech+ mod uses Harmony to patch the game's code. When using GitHub Copilot to generate Harmony patches, you can follow these guidelines:

- Use the `[HarmonyPatch]` attribute to specify the method to patch.
- Use the `[HarmonyPrefix]`, `[HarmonyPostfix]`, or `[HarmonyTranspiler]` attributes to specify the patch type.
- Use the `__instance` parameter to access the instance of the patched method.
- Use the `__result` parameter to modify the return value of the patched method.

## Suggestions for Copilot

Here are some suggestions for using GitHub Copilot to work on the Archotech+ mod:

- Use Copilot to generate XML code for new game objects or modify existing ones.
- Use Copilot to generate Harmony patches for modifying the game's code.
- Use Copilot to generate code for new features or systems, such as new implants or effects.
- Use Copilot to generate code for bug fixes or improvements to existing features or systems.
- Use Copilot to generate code for refactoring or optimizing existing code.

By following these guidelines and using GitHub Copilot effectively, you can help improve the Archotech+ mod and make it even more enjoyable for players.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

