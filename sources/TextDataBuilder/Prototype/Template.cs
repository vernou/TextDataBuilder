using System.Collections.Generic;
using System.Text;

namespace TextDataBuilder.Prototype
{
    public class Template : IPrototype
    {
        private readonly List<IPrototype> builds;

        public Template(List<IPrototype> builds)
        {
            this.builds = builds;
        }

        public string Build()
        {
            var output = new StringBuilder();
            foreach(var build in builds)
            {
                output.Append(build.Build());
            }
            return output.ToString();
        }
    }
}