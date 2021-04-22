
public enum ToolType
{
    Preparing,
    Seeding,
    Watering,
    Harvesting,
}
public enum WeatherType
{
    Sunny = 1,
    Rainy,
    Windy,
}

public enum GroundType
{
    Grass,
    Path,
    FallowSoil,
    ArableSoil,
}

public enum MapPosition
{
    TopLeft,
    TopMiddle,
    TopRight,
    Left,
    Middle,
    Right,
    BottomLeft,
    BottomMiddle,
    BottomRight,
}

public enum PlantStage
{
    Seed,
    Vegetating,
    Budding,
    Flowering,
    Ripening,
    Decaying,

    Count,
}