namespace Svarozhich.Models.Nodes;

public readonly struct ColorRgb(float r, float g, float b)
{
    public float R { get; } = r;
    public float G { get; } = g;
    public float B { get; } = b;

    public static ColorRgb operator *(ColorRgb c, float k)
        => new(c.R * k, c.G * k, c.B * k);

    public override string ToString()
    {
        return $"({R}, {G}, {B})";
    }
}