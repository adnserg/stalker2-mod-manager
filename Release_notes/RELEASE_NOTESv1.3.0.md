🌐 Languages: [English(current)](RELEASE_NOTESv1.3.0.md) | [Русский](RELEASE_NOTESv1.3.0.ru.md)

# Release Notes - v1.3.0

## S.T.A.L.K.E.R. 2 Mod Manager - Update Release

### Major Changes

#### New Features

##### Settings Reorganization
- **Consolidated Settings** - All settings moved to Additional Options window (Tools → Advanced)
- **Localization Settings** - Load Custom Localization and Reset Localization now in Additional Options
- **Settings Menu Removed** - Settings menu removed from main window for better organization
- **Improved UI Organization** - Better structured Additional Options window with clear sections

##### Game Path Validation
- **Validate Game Path Option** - New setting to enable/disable game path validation (enabled by default)
- **Flexible Path Handling** - Option to skip path validation for custom installations
- **Better User Control** - Users can disable validation if needed

##### Xbox/Windows Store Support
- **Xbox Version Support** - Added support for Xbox/Windows Store version of the game
- **WinGDK Executable Detection** - Automatically detects `Stalker2-WinGDK-Shipping.exe` in WinGDK folder
- **Cross-Platform Compatibility** - Works with both Steam and Xbox/Windows Store versions

##### Quick Access Section
- **Open Application Folder** - Quick button to open the application installation folder
- **Open Game Root Folder** - Quick button to open Stalker 2 root folder
- **Open ~mods Folder** - Quick button to open the ~mods folder (creates if missing)
- **Smart Button States** - Game-related buttons are disabled when path is not set

##### DLC Mod Loader Improvements
- **Direct Path Usage** - DLC Mod Loader now uses current TargetPath from UI instead of config
- **No Config Save Required** - Path is available immediately without saving config
- **Better Error Handling** - Improved error logging for temporary folder cleanup
- **Automatic Cleanup** - Temporary folders are properly cleaned up after execution

### Technical Improvements

- **Enhanced Localization** - Added localization for all Additional Options window elements
  - Section headers: Mod Order, Settings, Localization, Quick Access
  - All buttons and checkboxes fully localized
  - Support for English, Russian, and French
- **Better Error Logging** - All errors are now properly logged instead of being silently ignored
- **Improved Code Quality** - Better error handling and logging throughout the application
- **UI Improvements** - Better organized settings window with clear sections

### Bug Fixes

- **DLC Mod Loader Path Issue** - Fixed issue where DLC Mod Loader couldn't find game path until config was saved
- **ValidateGamePath Fixes** - Fixed validation logic for better compatibility
- **Settings Layout** - Fixed ConsiderModVersion checkbox layout in Additional Options window

### Installation

1. Download the v1.3.0 release from GitHub
2. Extract the application
3. Run `Stalker2ModManager.exe`
4. Enjoy the new features!

### Usage

See [README.md](README.md) for detailed usage instructions.

