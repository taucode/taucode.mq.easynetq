namespace TauCode.Mq.EasyNetQ.Tests;

internal static class Helper
{
    internal static string ToCaption(this string? s)
    {
        if (s == null)
        {
            return "<null>";
        }

        return $"'{s}'";
    }
}