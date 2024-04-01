using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features;

/// <summary>
/// Various utilities for interacting with the game itself.
/// </summary>
public class Game
{
    public static void OpenDialogueBox(string content)
    {
        DialogueBox box = UnityEngine.Object.FindAnyObjectByType<DialogueBox>();
        box.Show(content);
    }
}
