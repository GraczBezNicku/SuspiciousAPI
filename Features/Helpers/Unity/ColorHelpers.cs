using UnityEngine;

namespace SuspiciousAPI.Features.Helpers.Unity;

public static class ColorHelpers
{
    public static string ColorToHex(this Color color) => ColorUtility.ToHtmlStringRGBA(color);
}
