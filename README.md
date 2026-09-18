# Movement Lab

A Unity 6 third-person movement prototype focused on responsive momentum, traversal, stamina, crouching/sliding, wall bounces, air control, and live movement tuning.

## Unity version
This repository is pinned to **Unity 6.3 LTS (6000.3.9f1)**. Use this editor version for the project unless the repository is intentionally upgraded.

## Repository workflow
- `main` is the stable branch.
- New Movement Lab changes should be developed on feature/version branches and merged back through pull requests.
- Commit `Assets/`, `Packages/`, and `ProjectSettings/`.
- Do not commit generated Unity folders such as `Library/`, `Temp/`, `Logs/`, `Obj/`, or local builds.
- Large binary assets are configured for Git LFS through `.gitattributes`.

## Current prototype
The local prototype lineage reached v0.7 Responsive Momentum before migration to GitHub. The next step is importing the Unity project itself into this repository while preserving its project/package settings.
