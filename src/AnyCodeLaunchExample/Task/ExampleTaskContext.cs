using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCodeLaunchExample
{
    internal class ExampleTaskContext
    {
        public string FilePath { get; set; }
        public string Label { get; set; }
        public string MSBuildTask { get; set; }
        public Guid BuildContextType { get; set; }
    }
}
