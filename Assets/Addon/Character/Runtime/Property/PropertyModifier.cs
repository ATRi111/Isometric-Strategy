namespace Character
{
    public enum EModifyTiming
    {
        DirectAdd,
        DirectMultiply,
        FinalAdd,
        FinalMultiply,
    }

    public class PropertyModifier
    {
        private Property property;

        public float value;
        public FindPropertySO so;
        public EModifyTiming timing;

        public void Bind()
        {
            property = so.FindProperty();
        }
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

        private void Add(Property property)
        {
            property.Add(value);
        }

        private void Multiply(Property property)
        {
            property.Multiply(value);
        }
    }
}

