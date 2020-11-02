using System;

namespace Vkimow.Unity.GOAP
{
    public struct GOAPState: IEquatable<GOAPState>
    {
        public readonly Type Type;
        public object Value => _value;

        private object _value;


        internal GOAPState(object value)
        {
            Type = value.GetType();
            _value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is GOAPState))
                return false;

            var goapState = (GOAPState)obj;

            return Equals(goapState);
        }

        public bool Equals(GOAPState other)
        {
            if (!ValueEquals(other._value))
                return false;

            return true;
        }

        internal bool ValueEquals(object otherValue)
        {
            if (Type != otherValue.GetType())
                throw new InvalidCastException();

            switch (_value)
            {
                case int number:
                    {
                        return number == (int)otherValue;
                    }
                case bool option:
                    {
                        return option == (bool)otherValue;
                    }
                case string line:
                    {
                        return line == (string)otherValue;
                    }
            }

            throw new ArgumentException();
        }

        internal void SetValue(object newValue)
        {
            if (Type != newValue.GetType())
                throw new InvalidCastException();

            _value = newValue;
        }

        internal void SetValue(GOAPState otherState)
        {
            SetValue(otherState.Value);
        }


        public override string ToString()
        {
            return $"{Value} {{{Type}}}";
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode() + Type.GetHashCode();
        }
    }
}
