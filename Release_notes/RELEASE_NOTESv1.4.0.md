🌐 Languages: [English(current)](RELEASE_NOTESv1.4.0.md) | [Русский](RELEASE_NOTESv1.4.0.ru.md)

# Release Notes - v1.4.0

## S.T.A.L.K.E.R. 2 Mod Manager - Update Release

### Major Changes

#### New Features

##### Per-File Mod Management
- **Mod Files Window** - New window to display and manage individual mod files
- **File Selection** - Select which files to include/exclude from each mod
- **Checkbox Interface** - All files shown with checkboxes (enabled by default)
- **Select All/Deselect All** - Quick buttons to manage all files at once
- **Visual Indicator** - Warning icon (⚠) shows mods with disabled files in the main list
- **Double-Click Support** - Double-click a mod to open its files window
- **Open Mod Files Button** - Dedicated button to open the mod files window

##### File State Persistence
- **Automatic Save/Load** - File states are automatically saved in `mods_order.json`
- **State Restoration** - File states are restored when loading configuration
- **Auto-Load on Startup** - Mods and file states are automatically loaded when application starts
- **Config Auto-Save** - Configuration and mod order are automatically saved on application close

##### Installation Improvements
- **Selective File Copying** - Only enabled files are copied during mod installation
- **Disabled File Removal** - Disabled files are automatically removed from target folder if present
- **Smart File Management** - Efficient handling of file states during installation process

### Technical Improvements

- **Unified Load Method** - Created `LoadModsFromPath()` method used for both manual loading and auto-load on startup
- **Enhanced Data Model** - Added `FileStates` dictionary to `ModInfo` class
- **Improved File Tracking** - Better tracking of enabled/disabled files per mod
- **Localization Support** - Added English and Russian strings for new features
  - "Mod Files", "Select All", "Deselect All", "Open Mod Files", "Some Files Disabled"

### Bug Fixes

- **Mod Name Normalization** - Fixed `NormalizeOrderName()` to correctly extract base mod name before version ID
  - Now correctly handles formats like "Loader v2.0.1 - Steam-664-2-0-1-1737163600"
  - Extracts "Loader v2.0.1 - Steam" instead of including version identifiers
  - Improves mod matching when "Consider mod versions" is disabled

### Installation

1. Download the v1.4.0 release from GitHub
2. Extract the application
3. Run `Stalker2ModManager.exe`
4. Enjoy the new features!

### Usage

- **To manage mod files**: Double-click a mod in the list or select it and click "Open Mod Files"
- **To select files**: Use checkboxes in the Mod Files window to enable/disable individual files
- **File states are saved automatically** when you close the Mod Files window
- **Mods auto-load on startup** if Vortex path is saved in configuration

See [README.md](README.md) for detailed usage instructions.

