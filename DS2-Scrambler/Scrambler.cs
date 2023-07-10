
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;
using System.Text.RegularExpressions;
using static SoulsFormats.PARAM;

namespace DS2_Scrambler
{
    static class Extensions
    {
        public static T GetRandom<T>(this List<T> list, Random rand)
        {
            return list[rand.Next(list.Count)];
        }

        public static T PopRandom<T>(this List<T> list, Random rand)
        {
            int index = rand.Next(list.Count);
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public static Match RegexMatch(this string input, string pattern, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            return Regex.Match(input, pattern, regexOptions);
        }
    }

    public class Scrambler
    {
        private readonly Random rand;
        public Regulation regulation;

        public string configPath = AppContext.BaseDirectory + "\\Assets\\Scramble\\";
        public string shufflePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\ShuffleType\\";
        public string generatePath = AppContext.BaseDirectory + "\\Assets\\Scramble\\GenerateType\\";

        List<string> KeyItems = new List<string>();
        List<string> ToolItems = new List<string>();
        List<string> GestureItems = new List<string>();

        Dictionary<string, List<string>> ShuffleParamFields = new Dictionary<string, List<string>>();

        Dictionary<string, List<string>> GenerateParamFields = new Dictionary<string, List<string>>();
        Dictionary<string, Dictionary<string, List<string>>> GenerateParamValues = new Dictionary<string, Dictionary<string, List<string>>>();

        public ParamWrapper ItemParam;

        #region Scramble - Core
        public Scrambler(string seed, Regulation reg)
        {
            regulation = reg;

            if (seed == string.Empty)
                rand = new Random();
            else
                rand = new Random(seed.GetHashCode());


            // Build param references
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == "ItemParam")
                {
                    ItemParam = wrapper;
                }
            }

            // Build Key Items list
            foreach (string line in File.ReadLines(configPath + "Key-Items.txt", Encoding.UTF8))
            {
                var list = line.Split(";");
                Console.WriteLine(list[0]);
                KeyItems.Add(list[0]);
            }

            // Build Tool Items list
            foreach (string line in File.ReadLines(configPath + "Tool-Items.txt", Encoding.UTF8))
            {
                var list = line.Split(";");
                Console.WriteLine(list[0]);
                ToolItems.Add(list[0]);
            }

            // Build Gesture Items list
            foreach (string line in File.ReadLines(configPath + "Gesture-Items.txt", Encoding.UTF8))
            {
                var list = line.Split(";");
                Console.WriteLine(list[0]);
                GestureItems.Add(list[0]);
            }

            // Build ShuffleParamFields dictionary
            foreach (string filepath in Directory.GetFiles(shufflePath))
            {
                var name = Path.GetFileNameWithoutExtension(filepath);
                var list = new List<string>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    list.Add(line);
                }

                ShuffleParamFields.Add(name, list);
            }

            // Build GenerateParamFields dictionary
            foreach (string filepath in Directory.GetFiles(generatePath + "Shuffle"))
            {
                var name = Path.GetFileNameWithoutExtension(filepath);
                var list = new List<string>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    list.Add(line);
                    Console.WriteLine(line);
                }

                GenerateParamFields.Add(name, list);
            }

            // Build GenerateParamValues dictionary
            foreach (string filepath in Directory.GetFiles(generatePath + "Generate"))
            {
                var name = Path.GetFileNameWithoutExtension(filepath);
                var dict = new Dictionary<string, List<string>>();

                foreach (string line in File.ReadLines(filepath, Encoding.UTF8))
                {
                    var list = line.Split(";");
                    var value_list = new List<string> { list[1], list[2], list[3] };

                    dict.Add(list[0], value_list);
                }

                GenerateParamValues.Add(name, dict);
            }
        }

        #endregion

        #region Scramble - Param
        public Regulation ScrambleParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if(useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - ItemLotParam
        public Regulation Scramble_ItemLotParam(string paramName, bool useGenerateType, bool ignoreKeyItems, bool ignoreToolItems, bool mapLootOnly = false)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (paramName == "ItemLotParam2_Chr")
                        param_rows = param_rows.Where(row => row.ID >= 1000000 && row.ID < 90000000).ToList();

                    if (paramName == "ItemLotParam2_Other")
                        param_rows = param_rows.Where(row => row.ID < 70000000).ToList();

                    if (paramName == "ItemLotParam2_SvrEvent")
                        param_rows = param_rows.Where(row => row.ID >= 1 && row.ID < 20000000).ToList();

                    // Map Loot
                    if(mapLootOnly)
                    {
                        var new_rows = new List<PARAM.Row>();

                        foreach (PARAM.Row row in param_rows)
                        {
                            bool keep_row = false;

                            // Vanilla
                            if (row.ID >= 10025010 && row.ID <= 40036000)
                                keep_row = true;

                            // DLC
                            if (row.ID >= 50355010 && row.ID <= 50376770)
                                keep_row = true;

                            if (keep_row)
                                new_rows.Add(row);
                        }

                        param_rows = new_rows;
                    }

                    if (ignoreKeyItems || ignoreToolItems)
                    {
                        var new_rows = new List<PARAM.Row>();

                        foreach(PARAM.Row row in param_rows)
                        {
                            var keep_row = true;

                            // If a key item is within the item lot, then remove it from the param row list
                            foreach(PARAM.Cell cell in row.Cells)
                            {
                                if (cell.Def.InternalName.Contains("item_lot"))
                                {
                                    if (ignoreKeyItems)
                                    {
                                        foreach (string entry in KeyItems)
                                        {
                                            if (cell.Value.ToString() == entry)
                                            {
                                                keep_row = false;
                                            }
                                        }
                                    }
                                    if (ignoreToolItems)
                                    {
                                        foreach (string entry in ToolItems)
                                        {
                                            if (cell.Value.ToString() == entry)
                                            {
                                                keep_row = false;
                                            }
                                        }
                                    }
                                }
                            }

                            if(keep_row)
                                new_rows.Add(row);
                        }

                        param_rows = new_rows;
                    }

                    var validItems = ItemParam.Param.Rows.Where(row => row.ID >= 1000000 && row.ID <= 69000000).ToList();

                    // Shuffle Type
                    if (!useGenerateType)
                    {
                        try
                        {
                            var fields = ShuffleParamFields[wrapper.Name];

                            RandomizeFromList(param_rows, fields);
                        }
                        catch (Exception ex)
                        {
                            Util.ShowError($"ShuffleType: failed to find fields for {wrapper.Name}.");
                        }
                    }
                    // Generate Type
                    else if (useGenerateType)
                    {
                        try
                        {
                            var dict = GenerateParamValues[wrapper.Name];

                            foreach (PARAM.Row row in param_rows)
                            {
                                foreach (PARAM.Cell cell in row.Cells)
                                {
                                    // Field is within the dictionary
                                    if (dict.ContainsKey(cell.Def.InternalName))
                                    {
                                        var list = dict[cell.Def.InternalName];

                                        // Weighted means that the result should be weighted to zero.
                                        bool isWeighted = list[2] == "WEIGHTED" ? true : false;

                                        // Is itemlot field
                                        if (cell.Def.InternalName.Contains("item_lot"))
                                        {
                                            var index = rand.Next(validItems.Count);
                                            var randomItem = validItems[index];

                                            cell.Value = randomItem.ID;
                                        }
                                        else
                                        {
                                            // Int32
                                            if (cell.Def.InternalType == "s32" || cell.Def.InternalType == "u32")
                                            {
                                                var min = int.Parse(list[0]);
                                                var max = int.Parse(list[1]);

                                                if (min != -1 || max != -1)
                                                {
                                                    var value = rand.Next(min, max);

                                                    if (isWeighted && rand.Next(100) < 50)
                                                        value = 0;

                                                    cell.Value = value;
                                                }
                                            }
                                            // Int16
                                            if (cell.Def.InternalType == "s16" || cell.Def.InternalType == "u16")
                                            {
                                                var min = short.Parse(list[0]);
                                                var max = short.Parse(list[1]);

                                                var value = rand.Next(min, max);

                                                if (isWeighted && rand.Next(100) < 50)
                                                    value = 0;

                                                cell.Value = value;
                                            }
                                            // Int8
                                            if (cell.Def.InternalType == "s8" || cell.Def.InternalType == "u8")
                                            {
                                                var min = byte.Parse(list[0]);
                                                var max = byte.Parse(list[1]);

                                                var value = rand.Next(min, max);

                                                if (isWeighted && rand.Next(100) < 50)
                                                    value = 0;

                                                cell.Value = value;
                                            }
                                            // Float
                                            if (cell.Def.InternalType == "f32")
                                            {
                                                var min = double.Parse(list[0]);
                                                var max = double.Parse(list[1]);

                                                var value = rand.NextDouble() * (max - min) + min;

                                                if (isWeighted && rand.Next(100) < 50)
                                                    value = 0;

                                                cell.Value = value;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Util.ShowError($"GenerateType: failed to find fields for {wrapper.Name}.");
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - ArmorParam
        public Regulation Scramble_ArmorParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    param_rows = param_rows.Where(row => row.ID >= 11010100 && row.ID <= 18000000).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - ItemParam
        public Regulation Scramble_ItemParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - ArrowParam
        public Regulation Scramble_ArrowParam(string paramName, bool useGenerateType)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    param_rows = param_rows.Where(row => row.ID >= 1).ToList();

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }

                    foreach (PARAM.Row row in param_rows)
                    {
                        bool enablePoison = false;
                        bool enableBleed = false;

                        foreach (PARAM.Cell cell in row.Cells)
                        {
                            if (cell.Def.InternalName == "damage_poison")
                            {
                                if ((byte)cell.Value > 0)
                                {
                                    enablePoison = true;
                                }
                            }
                            if (cell.Def.InternalName == "damage_bleed")
                            {
                                if ((byte)cell.Value > 0)
                                {
                                    enableBleed = true;
                                }
                            }

                            if (cell.Def.InternalName == "apply_poison")
                            {
                                if (enablePoison)
                                    cell.Value = 1;
                                else
                                    cell.Value = 0;
                            }

                            if (cell.Def.InternalName == "apply_bleed")
                            {
                                if (enableBleed)
                                    cell.Value = 1;
                                else
                                    cell.Value = 0;
                            }
                        }
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - WeaponActionCategoryParam
        public Regulation Scramble_WeaponActionCategoryParam(string paramName)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                }
            }

            return regulation;
        }
        #endregion

        #region Scramble - WeaponParam
        public Regulation Scramble_WeaponParam(string paramName, bool useGenerateType, bool ignoreFists)
        {
            foreach (ParamWrapper wrapper in regulation.regulationParamWrappers)
            {
                if (wrapper.Name == paramName)
                {
                    PARAM param = wrapper.Param;
                    var param_rows = param.Rows;

                    var new_rows = new List<PARAM.Row>();

                    foreach (PARAM.Row row in param_rows)
                    {
                        if (row.ID >= 1000000 && row.ID <= 6100000)
                            new_rows.Add(row);

                        if (row.ID >= 11000000 && row.ID <= 11840000)
                            new_rows.Add(row);

                        if(ignoreFists)
                        {
                            if (row.ID >= 3400000 && row.ID <= 3408000)
                                new_rows.Remove(row);
                        }
                    }

                    param_rows = new_rows;

                    if (!useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, ShuffleParamFields);
                    }
                    else if (useGenerateType)
                    {
                        ShuffleValuesForParam(wrapper, param_rows, GenerateParamFields);
                        GenerateValuesForParam(wrapper, param_rows);
                    }
                }
            }

            return regulation;
        }
        #endregion

        #region Util
        public void ShuffleValuesForParam(ParamWrapper wrapper, List<PARAM.Row> param_rows, Dictionary<string, List<string>> dict)
        {
            try
            {
                var fields = dict[wrapper.Name];

                RandomizeFromList(param_rows, fields);
            }
            catch (Exception ex)
            {
                Util.ShowError($"{ex}\n\nFailed to find fields for {wrapper.Name}.");
            }
        }

        public void GenerateValuesForParam(ParamWrapper wrapper, List<PARAM.Row> param_rows)
        {
            var cellType = "";

            try
            {
                var dict = GenerateParamValues[wrapper.Name];

                foreach (PARAM.Row row in param_rows)
                {
                    foreach (PARAM.Cell cell in row.Cells)
                    {
                        cellType = cell.Def.InternalType;

                        // Field is within the dictionary
                        if (dict.ContainsKey(cell.Def.InternalName))
                        {
                            var list = dict[cell.Def.InternalName];

                            // Weighted means that the result should be weighted to zero.
                            bool isWeighted = list[2] == "WEIGHTED" ? true : false;

                            // Int32
                            if (cell.Def.InternalType == "s32" || cell.Def.InternalType == "u32")
                            {
                                var min = int.Parse(list[0]);
                                var max = int.Parse(list[1]);

                                if (min != -1 || max != -1)
                                {
                                    var value = rand.Next(min, max);

                                    if (isWeighted && rand.Next(100) < 50)
                                        value = 0;

                                    cell.Value = value;
                                }
                            }
                            // Int16
                            if (cell.Def.InternalType == "s16" || cell.Def.InternalType == "u16")
                            {
                                var min = short.Parse(list[0]);
                                var max = short.Parse(list[1]);

                                var value = rand.Next(min, max);

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }
                            // Int8
                            if (cell.Def.InternalType == "s8" || cell.Def.InternalType == "u8")
                            {
                                var min = byte.Parse(list[0]);
                                var max = byte.Parse(list[1]);

                                var value = rand.Next(min, max);

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }
                            // Float
                            if (cell.Def.InternalType == "f32")
                            {
                                var min = double.Parse(list[0]);
                                var max = double.Parse(list[1]);

                                var value = rand.NextDouble() * (max - min) + min;

                                if (isWeighted && rand.Next(100) < 50)
                                    value = 0;

                                cell.Value = value;
                            }
                        }
                    }
                }
            }
            catch (OverflowException ex)
            {
                Util.ShowError($"{ex}\n\nAttempted to assign use GenerateType value that exceeds {cellType} capacity - {wrapper.Name}.");
            }
            catch (Exception ex)
            {
                Util.ShowError($"{ex}\n\nFailed to find values for {wrapper.Name}.");
            }
        }

        private void RandomizeOne<T>(IEnumerable<PARAM.Row> rows, string param, bool plusMode = false)
        {
            if (plusMode)
            {
                List<T> options = rows.Select(row => (T)row[param].Value).GroupBy(val => val).Select(group => group.First()).ToList();
                foreach (PARAM.Row row in rows)
                    row[param].Value = options.GetRandom(rand);
            }
            else
            {
                List<T> options = rows.Select(row => (T)row[param].Value).ToList();
                foreach (PARAM.Row row in rows)
                    row[param].Value = options.PopRandom(rand);
            }
        }

        private void RandomizePair<T1, T2>(IEnumerable<PARAM.Row> rows, string param1, string param2)
        {
            List<(T1, T2)> options = rows.Select(row => ((T1)row[param1].Value, (T2)row[param2].Value)).ToList();
            foreach (PARAM.Row row in rows)
            {
                (T1 val1, T2 val2) = options.PopRandom(rand);
                row[param1].Value = val1;
                row[param2].Value = val2;
            }
        }

        private void RandomizeTriple<T1, T2, T3>(IEnumerable<PARAM.Row> rows, string param1, string param2, string param3)
        {
            List<(T1, T2, T3)> options = rows.Select(row => ((T1)row[param1].Value, (T2)row[param2].Value, (T3)row[param3].Value)).ToList();
            foreach (PARAM.Row row in rows)
            {
                (T1 val1, T2 val2, T3 val3) = options.PopRandom(rand);
                row[param1].Value = val1;
                row[param2].Value = val2;
                row[param3].Value = val3;
            }
        }

        private void RandomizeAllExceptList(IEnumerable<PARAM.Row> rows, List<string> valid_fields)
        {
            foreach (PARAM.Cell cell in rows.First().Cells)
            {
                string cType = cell.Def.DisplayType.ToString();
                string cName = cell.Def.InternalName;

                bool modify = true;

                foreach (string field in valid_fields)
                {
                    if (cName == field)
                    {
                        modify = false;
                    }
                }

                if (modify)
                {
                    if (cType == "u8" || cType == "x8")
                        RandomizeOne<byte>(rows, cName);
                    else if (cType == "s8")
                        RandomizeOne<sbyte>(rows, cName);
                    else if (cType == "u16" || cType == "x16")
                        RandomizeOne<ushort>(rows, cName);
                    else if (cType == "s16")
                        RandomizeOne<short>(rows, cName);
                    else if (cType == "u32" || cType == "x32")
                        RandomizeOne<uint>(rows, cName);
                    else if (cType == "s32")
                        RandomizeOne<int>(rows, cName);
                    else if (cType == "f32")
                        RandomizeOne<float>(rows, cName);
                    else if (cType == "b8" || cType == "b32")
                        RandomizeOne<bool>(rows, cName);
                    else if (cType == "dummy8" || cType == "fixstr" || cType == "fixstrW")
                        Console.WriteLine($"Skipped {cName}");
                    else
                        throw null;
                }
            }
        }

        private void RandomizeFromList(IEnumerable<PARAM.Row> rows, List<string> valid_fields)
        {
            foreach (PARAM.Cell cell in rows.First().Cells)
            {
                string cType = cell.Def.DisplayType.ToString();
                string cName = cell.Def.InternalName;

                foreach (string field in valid_fields)
                {
                    if (cName == field)
                    {
                        if (cType == "u8" || cType == "x8")
                            RandomizeOne<byte>(rows, cName);
                        else if (cType == "s8")
                            RandomizeOne<sbyte>(rows, cName);
                        else if (cType == "u16" || cType == "x16")
                            RandomizeOne<ushort>(rows, cName);
                        else if (cType == "s16")
                            RandomizeOne<short>(rows, cName);
                        else if (cType == "u32" || cType == "x32")
                            RandomizeOne<uint>(rows, cName);
                        else if (cType == "s32")
                            RandomizeOne<int>(rows, cName);
                        else if (cType == "f32")
                            RandomizeOne<float>(rows, cName);
                        else if (cType == "b8" || cType == "b32")
                            RandomizeOne<bool>(rows, cName);
                        else if (cType == "dummy8" || cType == "fixstr" || cType == "fixstrW")
                            Console.WriteLine($"Skipped {cName}");
                        else
                            throw null;
                    }
                }
            }
        }

        private void RandomizeAll(IEnumerable<PARAM.Row> rows, bool plusMode = false)
        {
            foreach (PARAM.Cell cell in rows.First().Cells)
            {
                string cType = cell.Def.DisplayType.ToString();
                string cName = cell.Def.InternalName;

                //Console.WriteLine($"{cName} - {cType}");

                if (cType == "u8" || cType == "x8")
                    RandomizeOne<byte>(rows, cName, plusMode);
                else if (cType == "s8")
                    RandomizeOne<sbyte>(rows, cName, plusMode);
                else if (cType == "u16" || cType == "x16")
                    RandomizeOne<ushort>(rows, cName, plusMode);
                else if (cType == "s16")
                    RandomizeOne<short>(rows, cName, plusMode);
                else if (cType == "u32" || cType == "x32")
                    RandomizeOne<uint>(rows, cName, plusMode);
                else if (cType == "s32")
                    RandomizeOne<int>(rows, cName, plusMode);
                else if (cType == "f32")
                    RandomizeOne<float>(rows, cName, plusMode);
                else if (cType == "b8" || cType == "b32")
                    RandomizeOne<bool>(rows, cName, plusMode);
                else if (cType == "dummy8" || cType == "fixstr" || cType == "fixstrW")
                    Console.WriteLine($"Skipped {cName}");
                else
                    throw null;
            }
        }
        #endregion
    }
}
