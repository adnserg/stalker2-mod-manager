🌐 Languages: [English(current)](README.md) | [Русский](README.ru.md)

# S.T.A.L.K.E.R. 2 Mod Manager

A mod management application for S.T.A.L.K.E.R. 2: Heart of Chornobyl.

## Features

1. **Load Mods** - Load mod list from Vortex mods folder
2. **Mod Sorting** - Change mod order (affects folder prefixes: AAA-, AAB-, AAC-, etc.)
3. **Save/Load Configuration** - Save current mod list and their settings (separate files for paths and mod order)
4. **Enable/Disable Mods** - Control which mods will be installed
5. **Install Mods** - Automatically create folders with prefixes and copy mod files
6. **Drag & Drop Reordering** - Visually drag mods to reorder them in the list
7. **Export/Import Mod Order** - Share your mod order with others
8. **Advanced Sorting** - Sort mods according to external JSON/TXT files (e.g., Vortex snapshot)
9. **Smart Installation** - Only copies changed or missing files, skipping identical files
10. **Window Size Persistence** - Remembers window size between sessions
11. **Logging** - Comprehensive logging to `app.log` file
12. **Multi-language Support** - Switch between English and Russian

## Usage

1. **Set Paths:**
   - **Vortex Path** - Path to Vortex mods folder (e.g., `E:\Vortex Mods\stalker2heartofchornobyl`)
   - **Target Path** - Path to `~mods` folder (e.g., `E:\SteamLibrary\steamapps\common\S.T.A.L.K.E.R. 2 Heart of Chornobyl\Stalker2\Content\Paks\~mods`)

2. **Load Mods:**
   - Click "Load Mods" to load mod list from Vortex folder

3. **Configure Mods:**
   - Use checkboxes to enable/disable mods
   - Drag and drop mods in the list to reorder them
   - Or use "Move Up" and "Move Down" buttons to change mod order

4. **Save Configuration:**
   - Click "Save Config" to save current settings (paths and mod order are saved separately)

5. **Export/Import Mod Order:**
   - Click "Export Order" to save mod order to a JSON file
   - Click "Import Order" to load mod order from a JSON file

6. **Advanced Options:**
   - Click "Advanced" to open advanced options window
   - Enable "Sort mods according to JSON/TXT file"
   - Select a JSON or TXT file (e.g., Vortex snapshot.json or custom order file)
   - The mod order will be sorted according to the file

7. **Install Mods:**
   - Click "Install Mods" to install all enabled mods to the ~mods folder
   - The application will:
     - Check each file before copying
     - Skip files that are already identical (by size and modification date)
     - Only copy changed or missing files
     - Remove unused mod folders

## Folder Format

Mods are installed in folders with prefixes:
- `AAA-ModName` (first mod)
- `AAB-ModName` (second mod)
- `AAC-ModName` (third mod)
- etc.

Prefix order: AAA, AAB, AAC, ..., AAZ, ABA, ABB, ..., ABZ, ACA, etc.

## Configuration Files

The application uses two separate configuration files:
- `paths_config.json` - Stores Vortex path, target path, and window size
- `mods_order.json` - Stores mod order and enabled status (can be exported/imported)

## Language Support

The application supports English and Russian languages. You can switch languages using the menu (when available) or by changing the language in settings.

## Requirements

- .NET 9.0 Windows Runtime
- Windows 10/11

## Installation

### Option 1: Use ready-to-run executable (recommended)

If you don't want to build the project from source, you can download a ready-to-use `.exe` from the **Releases** page:

- Download the latest version from [`Releases`](https://github.com/adnserg/stalker2-vortex-helper/releases).
- Place the `.exe` file in any convenient folder (for example next to your `~mods` folder).
- Run the `.exe` file.

### Option 2: Build from source

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run
```

Or run the compiled `.exe` file from the `bin/Debug/net9.0-windows/` folder.

## Logging

All application actions are logged to `app.log` file in the application directory. Log levels:
- Debug - Detailed information for debugging
- Info - General information
- Success - Successful operations
- Warning - Warning messages
- Error - Error messages