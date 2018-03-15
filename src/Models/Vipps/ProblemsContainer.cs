using System.Collections.Generic;
using System.Linq;

namespace PayEx.Client.Models.Vipps
{
    public class ProblemsContainer
    {
        public ProblemsContainer()
        {
            
        }

        public ProblemsContainer(string name, string description)
        {
            Problems.Add(new Problem { Name =  name, Description = description });
        }
        public List<Problem> Problems { get; set; } = new List<Problem>();

        public override string ToString()
        {
            return Problems.Select(p => p.ToString()).Aggregate((x, y) => x + "|" + y);
        }
    }

    public class Problem
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Name} : {Description}";
        }
    }
}
