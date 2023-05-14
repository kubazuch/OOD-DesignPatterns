using System.Collections.Generic;
using System.Linq;
using System.Text;
using BTM.Refraction;

namespace BTM
{
    public abstract class Bytebus : Vehicle
    {
        public new static readonly Dictionary<string, IField> AvailableFields = new List<IField>
        {
            new Field<int>("id"), new Field<string>("engineClass")
        }.ToDictionary(f => f.Name, f => f);

        public abstract List<Line> Lines { get; }
        public abstract string EngineClass { get; set; }

        protected Bytebus()
            : base(AvailableFields)
        {
            AssignSettersAndGetters();
        }

        public sealed override void AssignSettersAndGetters()
        {
            var id = (Field<int>)Fields["id"];
            id.SetSetter(value => Id = value);
            id.SetGetter(() => Id);

            var engineClass = (Field<string>)Fields["engineClass"];
            engineClass.SetSetter(value => EngineClass = value);
            engineClass.SetGetter(() => EngineClass);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("ByteBus #").Append(Id).Append(", engine class: ").AppendLine(EngineClass);
            builder.Append("\tLines: [").AppendJoin(", ", Lines.Select(x => x.NumberDec)).Append("]");
            return builder.ToString();
        }
    }
}