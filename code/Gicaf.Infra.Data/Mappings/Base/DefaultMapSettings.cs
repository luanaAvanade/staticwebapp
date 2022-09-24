using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisAgua.Infra.Mappings
{
    public class DefaultMapSettings
    {
        public int DefaultStringSize { get; set; } = 500;
        public string DefaultStringType { get; set; } = "varchar({0})";
        public bool ValueGeneratedOnAdd = true;
        public string DefaultStringDescription => String.Format(DefaultStringType, DefaultStringSize == 0 ? "max" : DefaultStringSize.ToString());
    }
}
