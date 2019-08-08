using System.Collections.Generic;
using FamilyTreeBuilder.ModelsGenerated;

namespace FamilyTreeBuilder.Models
{
    public class PersonViewModel
    {
        public Person Person { get; set; }
        public IEnumerable<Person> Children { get; set; }
    }

}
