# Release Notes - v1.0.0

## S.T.A.L.K.E.R. 2 Mod Manager - First Release

### Features

#### Core Functionality
- **Mod Loading** - Load mod list from Vortex mods folder
- **Mod Sorting** - Change mod order (affects folder prefixes: AAA-, AAB-, AAC-, etc.)
- **Enable/Disable Mods** - Control which mods will be installed
- **Install Mods** - Automatically create folders with prefixes and copy mod files
- **Smart Installation** - Only copies changed or missing files, skipping identical files

#### User Interface
- **Drag & Drop Reordering** - Visually drag mods to reorder them in the list with smooth auto-scrolling
- **Visual Feedback** - Insertion line indicator shows where mods will be placed when dropped
- **Window Resizing** - Resizable window with visual resize grips
- **Window Size Persistence** - Remembers window size between sessions
- **Column Headers** - Clear column headers for mod list (Name, Order, Target Folder)
- **Modern UI** - Clean and intuitive interface design

#### Configuration
- **Save/Load Configuration** - Save current mod list and their settings (separate files for paths and mod order)
- **Export/Import Mod Order** - Share your mod order with others via JSON files
- **Advanced Sorting** - Sort mods according to external JSON/TXT files (e.g., Vortex snapshot)

#### Internationalization
- **Multi-language Support** - Switch between English and Russian languages
- **Localized UI** - All UI elements are translated

#### Utilities
- **Comprehensive Logging** - All application actions logged to `app.log` file
- **Error Handling** - Robust error handling with detailed logging

### Technical Details

- **Framework**: .NET 9.0 Windows Runtime
- **UI Framework**: WPF (Windows Presentation Foundation)
- **Dependencies**: Newtonsoft.Json 13.0.3
- **Platform**: Windows 10/11

### Installation

1. Download the release from GitHub
2. Extract the application
3. Run `Stalker2ModManager.exe`
4. Configure paths (Vortex path and Target path)
5. Load mods and start managing!

### Usage

See [README.md](README.md) for detailed usage instructions.

