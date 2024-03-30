using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features;

/// <summary>
/// Represents the in-game player.
/// </summary>
public class Player
{
    private PlayerControl _playerControl;

    public Player(PlayerControl control)
    {
        _playerControl = control;
    }


}
