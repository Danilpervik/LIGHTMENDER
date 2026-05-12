using WinFormsApp1.Model.Entities;
using static WinFormsApp1.Model.Entities.Platform;

namespace WinFormsApp1.Model.World;

public class LevelLoader
{   
    public string LevelsFolder { get; set; } = "Levels";
    public Level LoadLevel(int levelIndex)
    {   
        if (levelIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(levelIndex), "Номер уровня не может быть отрицательным");
        int totalLevels = Directory.GetFiles(LevelsFolder, "*.txt").Length;
        if (levelIndex >= totalLevels)
            throw new ArgumentOutOfRangeException(nameof(levelIndex), "Номер уровня превышает доступные уровни.");
        var levelFilePath = string.Format("{0}/level{1}.txt", LevelsFolder, levelIndex);
        if (!File.Exists(levelFilePath))
        {
            throw new FileNotFoundException($"Файл уровня не найден: {levelFilePath}");
        }
        var level = new Level();
        var lines = File.ReadAllLines(levelFilePath);
        var linesWithoutCommentsAndWhitespace = lines.
            Select(line => line.Trim()).
            Where(x => x[0] != '#')
            .Where(x => !string.IsNullOrWhiteSpace(x));
        foreach (var line in linesWithoutCommentsAndWhitespace)
        {   
            var tokens = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            switch (tokens[0].ToLower())
            {
                case "platform":
                    var typeValue = int.Parse(tokens[5]);
                    var platformType = (PlatformType)typeValue;
                    var platform = new Platform
                    (
                        int.Parse(tokens[1]),
                        int.Parse(tokens[2]),
                        int.Parse(tokens[3]),
                        int.Parse(tokens[4]),
                        platformType
                    );
                    level.AddPlatform(platform);
                    break;
                case "enemy":
                    var enemy = new Enemy
                    (
                        int.Parse(tokens[1]),
                        int.Parse(tokens[2]),
                        int.Parse(tokens[3]),
                        int.Parse(tokens[4]),
                        int.Parse(tokens[5]),
                        int.Parse(tokens[6])
                    );
                    level.AddEnemy(enemy);
                    break;
                case "energy_orb":
                    var energyOrb = new EnergyOrb
                    (
                        int.Parse(tokens[1]),
                        int.Parse(tokens[2]),
                        int.Parse(tokens[3]),
                        int.Parse(tokens[4]),
                        int.Parse(tokens[5])
                    );
                    level.AddEnergyOrb(energyOrb);
                    break;
                case "light_switch":
                    var lightSwitch = new LightSwitch
                    (
                        int.Parse(tokens[1]),
                        int.Parse(tokens[2]),
                        int.Parse(tokens[3]),
                        int.Parse(tokens[4]),
                        int.Parse(tokens[5])
                    );
                    level.AddLightSwitch(lightSwitch);
                    break;
            }
        }
        return level;
    }
}
