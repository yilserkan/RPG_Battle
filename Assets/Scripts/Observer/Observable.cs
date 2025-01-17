using System;

namespace RPGGame.Observer
{
    public class Observable<T>
    {
        private T _value;
        public event Action<T> onValueChanged;

        public T Value
        {
            get => _value;
            set => Set(value);
        }

        public Observable(T value, IObserver<T> action = null)
        {
            AddListener(action);
            _value = value;
            Notify();
        }

        private void Set(T value)
        {
            if (Equals(_value, value)) { return; }

            _value = value;
            Notify();
        }

        private void Notify()
        {
            onValueChanged?.Invoke(_value);
        }

        public void AddListener(IObserver<T> action)
        {
            if (action == null) { return; }

            onValueChanged += action.Notify;
        }

        public void RemoveListener(IObserver<T> action)
        {
            if (action == null) { return; }

            onValueChanged -= action.Notify;
        }
    }

}

