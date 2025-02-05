﻿using Gamer.Core;
using Gamer.Estate.Ultima.Data;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Gamer.Estate.Ultima.Resources
{
    /// <summary>
    /// Contains translation tables used for mapping body values to file subsets.
    /// <seealso cref="AnimationResource" />
    /// </summary>
    public class BodyConverter
    {
        static int[] _table1 = new int[0];
        static int[] _table2 = new int[0];
        static int[] _table3 = new int[0];
        static int[] _table4 = new int[0];

        // Mounts: ItemID , BodyID
        private static readonly int[][] _mountIDConv = {
            new int[]{0x3E94, 0xF3}, // Hiryu
            new int[]{0x3E97, 0xC3}, // Beetle
            new int[]{0x3E98, 0xC2}, // Swamp Dragon
            new int[]{0x3E9A, 0xC1}, // Ridgeback
            new int[]{0x3E9B, 0xC0}, // Unicorn
            new int[]{0x3E9C, 0xBF}, // Ki-Rin
            new int[]{0x3E9E, 0xBE}, // Fire Steed
            new int[]{0x3E9F, 0xC8}, // Horse
            new int[]{0x3EA0, 0xE2}, // Grey Horse
            new int[]{0x3EA1, 0xE4}, // Horse
            new int[]{0x3EA2, 0xCC}, // Brown Horse
            new int[]{0x3EA3, 0xD2}, // Zostrich
            new int[]{0x3EA4, 0xDA}, // Zostrich
            new int[]{0x3EA5, 0xDB}, // Zostrich
            new int[]{0x3EA6, 0xDC}, // Llama
            new int[]{0x3EA7, 0x74}, // Nightmare
            new int[]{0x3EA8, 0x75}, // Silver Steed
            new int[]{0x3EA9, 0x72}, // Nightmare
            new int[]{0x3EAA, 0x73}, // Ethereal Horse
            new int[]{0x3EAB, 0xAA}, // Ethereal Llama
            new int[]{0x3EAC, 0xAB}, // Ethereal Zostrich
            new int[]{0x3EAD, 0x84}, // Ki-Rin
            new int[]{0x3EAF, 0x78}, // Minax Warhorse
            new int[]{0x3EB0, 0x79}, // ShadowLords Warhorse
            new int[]{0x3EB1, 0x77}, // COM Warhorse
            new int[]{0x3EB2, 0x76}, // TrueBritannian Warhorse
            new int[]{0x3EB3, 0x90}, // Seahorse
            new int[]{0x3EB4, 0x7A}, // Unicorn
            new int[]{0x3EB5, 0xB1}, // Nightmare
            new int[]{0x3EB6, 0xB2}, // Nightmare
            new int[]{0x3EB7, 0xB3}, // Dark Nightmare
            new int[]{0x3EB8, 0xBC}, // Ridgeback
            new int[]{0x3EBA, 0xBB}, // Ridgeback
            new int[]{0x3EBB, 0x319}, // Undead Horse
            new int[]{0x3EBC, 0x317}, // Beetle
            new int[]{0x3EBD, 0x31A}, // Swamp Dragon
            new int[]{0x3EBE, 0x31F}, // Armored Swamp Dragon
            new int[]{0x3F6F, 0x9}    // Daemon
        };

        private BodyConverter() { }

        public static int DeathAnimationIndex(Body body)
        {
            switch (body.Type)
            {
                case BodyType.Human: return 21;
                case BodyType.Monster: return 2;
                case BodyType.Animal: return 8;
                default: return 2;
            }
        }

        public static int DeathAnimationFrameCount(Body body)
        {
            switch (body.Type)
            {
                case BodyType.Human: return 6;
                case BodyType.Monster: return 4;
                case BodyType.Animal: return 4;
                default: return 4;
            }
        }

        static BodyConverter()
        {
            var path = UltimaFileManager.GetFilePath("bodyconv.def");
            if (path == null)
                return;
            ArrayList list1 = new ArrayList(), list2 = new ArrayList(), list3 = new ArrayList(), list4 = new ArrayList();
            int max1 = 0, max2 = 0, max3 = 0, max4 = 0;
            using (var ip = new StreamReader(path))
            {
                string line;
                var totalDataRead = 0;
                while ((line = ip.ReadLine()) != null)
                {
                    totalDataRead += line.Length;
                    if ((line = line.Trim()).Length == 0 || line.StartsWith("#") || line.StartsWith("\"#"))
                        continue;
                    // string[] split = line.Split('\t');
                    var split = Regex.Split(line, @"\t|\s+", RegexOptions.IgnoreCase);
                    var original = System.Convert.ToInt32(split[0]);
                    var anim2 = System.Convert.ToInt32(split[1]);
                    if (split.Length < 3 || !int.TryParse(split[2], out int anim3))
                        anim3 = -1;
                    if (split.Length < 4 || !int.TryParse(split[3], out int anim4))
                        anim4 = -1;
                    if (split.Length < 5 || !int.TryParse(split[4], out int anim5))
                        anim5 = -1;
                    if (anim2 != -1)
                    {
                        if (anim2 == 68)
                            anim2 = 122;
                        if (original > max1)
                            max1 = original;
                        list1.Add(original);
                        list1.Add(anim2);
                    }
                    if (anim3 != -1)
                    {
                        if (original > max2)
                            max2 = original;
                        list2.Add(original);
                        list2.Add(anim3);
                    }
                    if (anim4 != -1)
                    {
                        if (original > max3)
                            max3 = original;
                        list3.Add(original);
                        list3.Add(anim4);
                    }
                    if (anim5 != -1)
                    {
                        if (original > max4)
                            max4 = original;
                        list4.Add(original);
                        list4.Add(anim5);
                    }
                }
                Metrics.ReportDataRead(totalDataRead);
            }
            _table1 = new int[max1 + 1];
            _table2 = new int[max2 + 1];
            _table3 = new int[max3 + 1];
            _table4 = new int[max4 + 1];
            for (var i = 0; i < _table1.Length; ++i)
                _table1[i] = -1;
            for (var i = 0; i < list1.Count; i += 2)
                _table1[(int)list1[i]] = (int)list1[i + 1];
            for (var i = 0; i < _table2.Length; ++i)
                _table2[i] = -1;
            for (var i = 0; i < list2.Count; i += 2)
                _table2[(int)list2[i]] = (int)list2[i + 1];
            for (var i = 0; i < _table3.Length; ++i)
                _table3[i] = -1;
            for (var i = 0; i < list3.Count; i += 2)
                _table3[(int)list3[i]] = (int)list3[i + 1];
            for (var i = 0; i < _table4.Length; ++i)
                _table4[i] = -1;
            for (var i = 0; i < list4.Count; i += 2)
                _table4[(int)list4[i]] = (int)list4[i + 1];
        }

        /// <summary>
        /// Checks to see if <paramref name="body" /> is contained within the mapping table.
        /// </summary>
        /// <returns>True if it is, false if not.</returns>
        public static bool Contains(int body)
        {
            if (_table1 != null && body >= 0 && body < _table1.Length && _table1[body] != -1)
                return true;
            if (_table2 != null && body >= 0 && body < _table2.Length && _table2[body] != -1)
                return true;
            if (_table3 != null && body >= 0 && body < _table3.Length && _table3[body] != -1)
                return true;
            if (_table4 != null && body >= 0 && body < _table4.Length && _table4[body] != -1)
                return true;
            return false;
        }

        /// <summary>
        /// Attempts to convert <paramref name="body" /> to a body index relative to a file subset, specified by the return value.
        /// </summary>
        /// <returns>A value indicating a file subset:
        /// <list type="table">
        /// <listheader>
        /// <term>Return Value</term>
        /// <description>File Subset</description>
        /// </listheader>
        /// <item>
        /// <term>1</term>
        /// <description>Anim.mul, Anim.idx (Standard)</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Anim2.mul, Anim2.idx (LBR)</description>
        /// </item>
        /// <item>
        /// <term>3</term>
        /// <description>Anim3.mul, Anim3.idx (AOS)</description>
        /// </item>
        /// </list>
        /// </returns>
        public static int Convert(ref int body)
        {
            // Converts MountItemID to BodyID
            if (body > 0x3E93)
                for (var i = 0; i < _mountIDConv.Length; ++i)
                {
                    var conv = _mountIDConv[i];
                    if (conv[0] == body)
                    {
                        body = conv[1];
                        break;
                    }
                }
            if (_table1 != null && body >= 0 && body < _table1.Length)
            {
                var val = _table1[body];
                if (val != -1)
                {
                    body = val;
                    return 2;
                }
            }
            if (_table2 != null && body >= 0 && body < _table2.Length)
            {
                var val = _table2[body];
                if (val != -1)
                {
                    body = val;
                    return 3;
                }
            }
            if (_table3 != null && body >= 0 && body < _table3.Length)
            {
                var val = _table3[body];
                if (val != -1)
                {
                    body = val;
                    return 4;
                }
            }
            if (_table4 != null && body >= 0 && body < _table4.Length)
            {
                var val = _table4[body];
                if (val != -1)
                {
                    body = val;
                    return 5;
                }
            }
            return 1;
        }

        /// <summary>
        /// Returns true if the parameter itemID corresponds to a mountable entity.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static bool CheckIfItemIsMount(ref int itemId)
        {
            if (itemId > 0x3E93)
                for (var i = 0; i < _mountIDConv.Length; ++i)
                {
                    var conv = _mountIDConv[i];
                    if (conv[0] == itemId)
                    {
                        itemId = conv[1];
                        return true;
                    }
                }
            return false;
        }
    }
}
