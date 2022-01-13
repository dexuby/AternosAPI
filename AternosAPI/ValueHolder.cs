namespace AternosAPI
{
    public abstract class ValueHolder<T>
    {
        private readonly T _value;

        protected ValueHolder(T value)
        {
            _value = value;
        }

        public T GetValue() => _value;
    }
}