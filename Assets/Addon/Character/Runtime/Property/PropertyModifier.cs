namespace Character
{
    public enum EModifyTiming
    {
        DirectAdd,
        DirectMultiply,
        FinalAdd,
        FinalMultiply,
    }

    public abstract class PropertyModifier<T> where T : struct
    {
        public Property<T> property;
        public EModifyTiming timing;
        public T value;

        public void Register()
        {
            switch (timing)
            {
                case EModifyTiming.DirectAdd:
                    property.DirectAdd += Add;
                    break;
                case EModifyTiming.DirectMultiply:
                    property.DirectMultiply += Multiply;
                    break;
                case EModifyTiming.FinalAdd:
                    property.FinalAdd += Add;
                    break;
                case EModifyTiming.FinalMultiply:
                    property.FinalMultiply += Multiply;
                    break;
            }

        }

        public void Unregister()
        {
            switch (timing)
            {
                case EModifyTiming.DirectAdd:
                    property.DirectAdd -= Add;
                    break;
                case EModifyTiming.DirectMultiply:
                    property.DirectMultiply -= Multiply;
                    break;
                case EModifyTiming.FinalAdd:
                    property.FinalAdd -= Add;
                    break;
                case EModifyTiming.FinalMultiply:
                    property.FinalMultiply -= Multiply;
                    break;
            }
        }

        protected void Add(Property<T> property)
        {
            property.Add(value);
        }
        protected void Multiply(Property<T> property)
        {
            property.Multiply(value);
        }
    }
}

