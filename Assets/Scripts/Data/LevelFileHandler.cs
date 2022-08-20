using Assets.Scripts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class LevelFileHandler
    {
        private const string LevelDir = "levels";
        private void EnsureLevelFolderExists() => FileHandler.CreateDir("levels");

        public LevelFileHandler()
        {
            EnsureLevelFolderExists();
        }

        public void SaveLevel(GameMap map, string mapName = null)
        {
            var fileName = mapName ?? $"map-{DateTime.Now.ToString("dd-mm-yy")}";
            fileName += ".json";
            var relPath = Path.Combine(LevelDir, fileName);
            FileHandler.SaveToJSON(map, relPath);
        }

        public GameMap LoadLevel(string levelName)
        {
            var path = levelName;
            if (!Path.HasExtension(path))
                path += ".json";

            return FileHandler.ReadFromJSON<GameMap>(Path.Combine(LevelDir, path));
        }

        public string[] FetchLevels() => FileHandler.GetFiles(LevelDir);
    }
}
