using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuspiciousAPI.Features.Helpers.IL2CPP;

public static class TypeHelpers
{
    public static Dictionary<Il2CppSystem.Type, Type> Il2CppToManagedType = new Dictionary<Il2CppSystem.Type, Type>();

    /// <summary>
    /// Tries to fetch the managed version of a specified <see cref="Il2CppSystem.Type"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <returns><see cref="Type"/> if one is found, otherwise <see langword="null"/></returns>
    public static Type GetManagedType(this Il2CppSystem.Type type)
    {
        if (Il2CppToManagedType.ContainsKey(type))
            return Il2CppToManagedType[type];

        Type typeToReturn = null;
        Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly ass in allAssemblies) 
        {
            Type[] types; 
            
            try
            {
                types = ass.GetTypes();
            }
            catch (Exception ex)
            {
                continue;
            }

            foreach (Type t in types)
            {
                if (t.FullName == type.FullName)
                {
                    typeToReturn = t;

                    if (!Il2CppToManagedType.ContainsKey(type))
                        Il2CppToManagedType.Add(type, t);

                    break;
                }

                if (typeToReturn != null)
                    break;
            }
        }

        return typeToReturn;
    }
}
