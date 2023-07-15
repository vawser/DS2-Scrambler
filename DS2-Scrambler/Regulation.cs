using SoulsFormats.Util;
using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2_Scrambler
{
    public class Regulation
    {
        public string Path_Regulation;
        public string Path_Param_Folder;
        public string Path_PARAMDEF = AppContext.BaseDirectory + "\\Assets\\Paramdex\\DS2S\\Defs\\";

        public List<PARAMDEF> PARAMDEF_List = new List<PARAMDEF>();

        public IBinder? regulationBinder { get; set; }
        public bool isRegulationEncrypted { get; set; }
        public bool usingRegulation { get; set; }
        public List<ParamWrapper> regulationParamWrappers { get; set; }

        public Regulation(string path)
        {
            regulationParamWrappers = new List<ParamWrapper>();

            Path_Regulation = Path.Combine(path, "enc_regulation.bnd.dcx");
            Path_Param_Folder = Path.Combine(path, "Param");

            usingRegulation = true;

            if (!File.Exists(Path_Regulation))
            {
                Util.ShowError($"Regulation not found:\r\n{Path_Regulation}");
                usingRegulation = false;
            }

            BuildParamDefs();
        }

        public void BuildParamDefs()
        {
            PARAMDEF_List.Clear();
            var paramdef_dir = $@"Assets\\Paramdex\\DS2S\\Defs";

            foreach (string path in Directory.GetFiles(Path_PARAMDEF, "*.xml"))
            {
                string paramID = Path.GetFileNameWithoutExtension(path);

                try
                {
                    var paramdef = PARAMDEF.XmlDeserialize(path);

                    PARAMDEF_List.Add(paramdef);
                }
                catch (Exception ex)
                {
                    Util.ShowError($"Failed to load paramdef {paramID}.txt\r\n\r\n{ex}");
                }
            }
        }

        public bool LoadParams()
        {
            // Load regulation params (if they exist)
            if (usingRegulation)
            {
                try
                {
                    regulationBinder = SFUtil.DecryptDS2Regulation(Path_Regulation);
                    isRegulationEncrypted = true;
                }
                catch (Exception ex)
                {
                    Util.ShowError($"Failed to load regulation:\r\n{Path_Regulation}\r\n\r\n{ex}");

                    return false;
                }

                foreach (BinderFile file in regulationBinder.Files.Where(f => f.Name.EndsWith(".param")))
                {
                    string name = Path.GetFileNameWithoutExtension(file.Name);

                    try
                    {
                        PARAM param = PARAM.Read(file.Bytes);

                        foreach (PARAMDEF paramdef in PARAMDEF_List)
                        {
                            if (param.ParamType == paramdef.ParamType)
                                param.ApplyParamdef(paramdef);
                        }

                        var wrapper = new ParamWrapper(name, param, param.AppliedParamdef, false);
                        regulationParamWrappers.Add(wrapper);

                        //MessageBox.Show($"Load Regulation: {wrapper.Name}");
                    }
                    catch (Exception ex)
                    {
                        Util.ShowError($"Failed to load param file: {name}.param\r\n\r\n{ex}");
                    }
                }
            }

            regulationParamWrappers.Sort();

            return true;
        }

        public bool LoadLooseParams()
        {
            string[] paramFiles = Directory.GetFileSystemEntries(Path_Param_Folder, @"*.param");
            foreach(string filename in paramFiles)
            {
                string name = Path.GetFileNameWithoutExtension(filename);
                var paramBytes = File.ReadAllBytes(filename);

                try
                {
                    PARAM param = PARAM.Read(paramBytes);

                    foreach (PARAMDEF paramdef in PARAMDEF_List)
                    {
                        if (param.ParamType == paramdef.ParamType)
                            param.ApplyParamdef(paramdef);
                    }

                    var wrapper = new ParamWrapper(name, param, param.AppliedParamdef, true);
                    regulationParamWrappers.Add(wrapper);

                    //MessageBox.Show($"Load Loose: {wrapper.Name}");
                }
                catch (Exception ex)
                {
                    Util.ShowError($"Failed to load param file: {name}.param\r\n\r\n{ex}");
                }
            }

            return true;
        }

        public bool SaveParams()
        {
            // Save regulation params
            if (usingRegulation)
            {
                try
                {
                    foreach (BinderFile file in regulationBinder.Files)
                    {
                        foreach (ParamWrapper wrapper in regulationParamWrappers)
                        {
                            ParamWrapper paramFile = wrapper;

                            // Only save those that were in the regulation during load back into the regulation
                            if (!paramFile.isLoose)
                            {
                                if (Path.GetFileNameWithoutExtension(file.Name) == paramFile.Name)
                                {
                                    try
                                    {
                                        file.Bytes = paramFile.Param.Write();
                                        //MessageBox.Show($"{file.Name}");
                                    }
                                    catch
                                    {
                                        Util.ShowError($"Invalid data, failed to save {paramFile}. Data must be fixed before saving can complete.");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Util.ShowError($"Failed to save regulation file:\n{Path_Regulation}\n\n{ex}");
                    return false;
                }
            }

            if (regulationBinder is BND4 bnd4)
            {
                bnd4.Write(Path_Regulation);
            }

            return true;
        }

        public bool SaveLooseParams()
        {
            string[] paramFiles = Directory.GetFileSystemEntries(Path_Param_Folder, @"*.param");
            foreach (string filename in paramFiles)
            {
                string name = Path.GetFileNameWithoutExtension(filename);

                foreach (ParamWrapper wrapper in regulationParamWrappers)
                {
                    ParamWrapper paramFile = wrapper;

                    // Only save those that were in the regulation during load back into the regulation
                    if (paramFile.isLoose)
                    {
                        if (Path.GetFileNameWithoutExtension(name) == paramFile.Name)
                        {
                            try
                            {
                                var paramBytes = paramFile.Param.Write();
                                File.WriteAllBytes(filename, paramBytes);
                            }
                            catch
                            {
                                Util.ShowError($"Invalid data, failed to save {paramFile}. Data must be fixed before saving can complete.");
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        // Issues:
        // The map loose params do not seem to save correctly, causing DSMS to reject them.
        // Currently only the regulation params are written out as loose.

        public bool SaveAllParamsAsLoose()
        {
            // Save params from regulation
            foreach (BinderFile file in regulationBinder.Files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);

                foreach (ParamWrapper wrapper in regulationParamWrappers)
                {
                    ParamWrapper paramFile = wrapper;

                    if (Path.GetFileNameWithoutExtension(name) == paramFile.Name)
                    {
                        try
                        {
                            var param_path = Path.Combine(Path_Param_Folder, file.Name);
                            var paramBytes = paramFile.Param.Write();
                            File.WriteAllBytes(param_path, paramBytes);
                        }
                        catch
                        {
                            Util.ShowError($"Invalid data, failed to save {paramFile}. Data must be fixed before saving can complete.");
                            return false;
                        }
                    }
                }
            }

            // Save loose params
            string[] paramFiles = Directory.GetFileSystemEntries(Path_Param_Folder, @"*.param");
            foreach (string filename in paramFiles)
            {
                string name = Path.GetFileNameWithoutExtension(filename);

                foreach (ParamWrapper wrapper in regulationParamWrappers)
                {
                    ParamWrapper paramFile = wrapper;

                    if (Path.GetFileNameWithoutExtension(name) == paramFile.Name)
                    {
                        try
                        {
                            var param_path = Path.Combine(Path_Param_Folder, filename);
                            var paramBytes = paramFile.Param.Write();
                            File.WriteAllBytes(param_path, paramBytes);
                        }
                        catch
                        {
                            Util.ShowError($"Invalid data, failed to save {paramFile}. Data must be fixed before saving can complete.");
                            return false;
                        }
                    }
                }
            }

            // Empty regulation of params
            List<BinderFile> newFiles = new List<BinderFile>();
            foreach (var p in regulationBinder.Files)
            {
                if (!p.Name.ToUpper().Contains(".PARAM"))
                {
                    newFiles.Add(p);
                }
            }
            regulationBinder.Files = newFiles;

            if (regulationBinder is BND4 bnd4)
            {
                bnd4.Write(Path_Regulation);
            }

            return true;
        }
    }
}
