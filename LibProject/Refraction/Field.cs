using System;

namespace BTM.Refraction
{
    public interface IField : ICloneable
    {
        string Name { get; }

        Type FieldType { get; }

        object? GetValue();
        
        void SetValue(object? value);

    }

    public class Field<T> : IField
    {
        private Func<T>? _getter;
        private Action<T>? _setter;

        public string Name { get; }

        public Type FieldType => typeof(T);

        public T Value
        {
            get => (_getter ?? throw new InvalidOperationException())();
            set => (_setter ?? throw new InvalidOperationException())(value);
        }

        public Field(string name)
        {
            Name = name;
        }

        public object? GetValue() => Value;

        public void SetValue(object? value)
        {
            Type t = Nullable.GetUnderlyingType(FieldType) ?? FieldType;
            Value = (T) (value == null ? null : Convert.ChangeType(value, t));
        }

        public void SetSetter(Action<T> setter) => _setter = setter;

        public void SetGetter(Func<T> getter) => _getter = getter;

        public object Clone() => new Field<T>(Name);
    }
}
