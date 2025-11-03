using System.Collections.Generic;

namespace Stalker2ModManager.Models
{
    public class VortexSnapshot
    {
        public List<string> Files { get; set; } = new List<string>();
    }

    // Альтернативная структура для snapshot.json
    public class VortexSnapshotRoot
    {
        public List<string> Files { get; set; } = new List<string>();
    }
}

