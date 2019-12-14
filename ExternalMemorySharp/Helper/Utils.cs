﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExternalMemory.Helper
{
    public static class Utils
    {
        /// <summary>
        /// Alternative version of <see cref="Type.IsSubclassOf"/> that supports raw generic types (generic types without any type parameters).
        /// </summary>
        /// <param name="baseType">The base type class for which the check is made.</param>
        /// <param name="toCheck">To type to determine for whether it derives from <paramref name="baseType"/>.</param>
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type baseType)
        {
            while (toCheck != typeof(object))
            {
                Type cur = toCheck != null && toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (baseType == cur)
                    return true;

                if (toCheck != null)
                    toCheck = toCheck.BaseType;
            }

            return false;
        }
        internal static int GetDependenciesSize(ExternalOffset dependency, IEnumerable<ExternalOffset> offsets)
        {
            List<ExternalOffset> unrealOffsets = offsets.Where(f => f != null).ToList();

            // If Empty Then It's Usually Dynamic Pointer (Like `Data` Member In `TArray`)
            List<ExternalOffset> d = unrealOffsets.Where(off => off.Dependency == dependency).ToList();
            if (!d.Any())
                return 0;

            // Get Biggest Offset
            int biggestOffset = d.Max(unrealOffset => unrealOffset.Offset);

            // Get Offset
            ExternalOffset offset = unrealOffsets.Find(off => off.Dependency == dependency && off.Offset == biggestOffset);

            // Get Size Of Data
            int valueSize = offset.Value.Length;

            return biggestOffset + valueSize;
        }
        public static byte[] StringToBytes(string str, bool isUnicode)
        {
            return isUnicode ? Encoding.Unicode.GetBytes(str) : Encoding.ASCII.GetBytes(str);
        }
        public static string BytesToString(byte[] strBytes, bool isUnicode)
        {
            return isUnicode ? Encoding.Unicode.GetString(strBytes) : Encoding.ASCII.GetString(strBytes);
        }
    }
}