using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.CustomRPCs.Core;

/// <summary>
/// This will be used for various methods related to CustomRPCs
/// </summary>
public static class RpcManager
{
    public static byte GetTrueRpcId(byte Id)
    {
        return Id;
    }
}