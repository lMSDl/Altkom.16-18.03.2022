using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TestObject : Entity
    {
        public TestObject TestObjectParent { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public bool SomeBool { get; set; }
        public int SomeInt { get; set; }

        public bool ShouldSerializeCreatedAt()
        {
            return SomeInt < 100;
        }

    }
}
