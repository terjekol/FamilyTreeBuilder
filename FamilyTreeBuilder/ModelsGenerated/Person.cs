using System;
using System.Collections.Generic;

namespace FamilyTreeBuilder.ModelsGenerated
{
    public partial class Person
    {
        public Person()
        {
            InverseFatherNavigation = new HashSet<Person>();
            InverseMotherNavigation = new HashSet<Person>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public bool? IsMale { get; set; }
        public int? Father { get; set; }
        public int? Mother { get; set; }
        public int? DataOwnerId { get; set; }

        public virtual Person FatherNavigation { get; set; }
        public virtual Person MotherNavigation { get; set; }
        public virtual ICollection<Person> InverseFatherNavigation { get; set; }
        public virtual ICollection<Person> InverseMotherNavigation { get; set; }
    }
}
