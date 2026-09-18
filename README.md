# Movement Lab

A Unity 6 third-person movement prototype focused on responsive momentum, traversal, stamina, crouching/sliding, wall bounces, air control, and live movement tuning.

## Unity version
This repository is pinned to **Unity 6000.4.6f1**, matching the editor installed for this project. Keep this exact version unless we intentionally upgrade it.

## Project setup
- Input System is declared in `Packages/manifest.json`.
- The built-in Animation module is explicitly declared so Animator support persists between pulls/clones.
- `Assets/`, `Packages/`, and `ProjectSettings/` are version controlled.
- Unity-generated `Library/`, `Temp/`, `Logs/`, `Obj/`, and build folders are ignored.
- Large binary art/audio assets are configured for Git LFS.

## Current baseline
The GitHub baseline is **v0.7 Responsive Momentum**. It includes speed-sensitive ground braking, stronger initial acceleration, visible crouching via a separate Player Visual child, three-bar stamina HUD support, L3 dodge, wall bounce forward carry, air control, and the generated movement test arena.

In Unity, use **Movement Prototype > Build Complete Test Arena** to regenerate the test scene after scene-builder changes.

## Workflow
`main` is the canonical project. Future gameplay changes should be made from this repository rather than from replacement ZIP projects.
