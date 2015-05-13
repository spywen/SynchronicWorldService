using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SynchronicWorldService.Models
{
    [DataContract]
    public class Person
    {
        public Person() { }
        public Person(DataAccess.Person person)
        {
            this.Id = person.Id;
            this.Name = person.Name;
            this.Nickname = person.Nickname;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Nickname { get; set; }
    }
}
