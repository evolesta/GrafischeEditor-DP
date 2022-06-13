namespace GrafischeEditor_DP.StrategyPattern
{
    public static class Strategies
    {
        public static EllipseStrategy EllipseStrategy { get; } = new();
        public static RectangleStrategy RectangleStrategy { get; } = new();
    }
}
