namespace SuspiciousAPI.Features.Helpers.Unity;

public static class ObjectHelpers
{
    public static List<UnityEngine.Object> ObjectsToBeSaved = new List<UnityEngine.Object>();

    public static void SaveObjectFromDestruction(this UnityEngine.Object obj)
    {
        if (ObjectsToBeSaved.Contains(obj))
            return;

        ObjectsToBeSaved.Add(obj);
    }

    public static void AllowObjectDestruction(this UnityEngine.Object obj)
    {
        if (!ObjectsToBeSaved.Contains(obj))
            return;

        ObjectsToBeSaved.Remove(obj);
    }
}
