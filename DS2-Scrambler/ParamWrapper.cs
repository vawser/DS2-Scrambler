using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoulsFormats;

namespace DS2_Scrambler
{
    public class ParamWrapper : IComparable<ParamWrapper>
    {
        public bool Error { get; }

        public string Name { get; }

        public string Description { get; }

        public PARAM Param;

        public PARAMDEF AppliedParamDef;

        public List<PARAM.Row> Rows => Param.Rows;

        public bool isLoose { get; set; }

        public ParamWrapper(string name, PARAM param, PARAMDEF paramdef, bool loose)
        {
            Name = name;
            Param = param;
            AppliedParamDef = paramdef;
            isLoose = loose;
        }

        public int CompareTo(ParamWrapper other) => Name.CompareTo(other.Name);
    }
}
