namespace TauCode.Mq.EasyNetQ.LocalTests;

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