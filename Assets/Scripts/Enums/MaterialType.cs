public enum MaterialType
{
    Cobweb,  // 0
    Feather,  // 1
    Flower,   // 2
    Stones,   // 3
    Twig      // 4
}

public static class MaterialTypeHelper
{
    public static int Count => System.Enum.GetValues(typeof(MaterialType)).Length;
}