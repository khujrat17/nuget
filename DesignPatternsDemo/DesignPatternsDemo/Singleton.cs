//1. Singleton Pattern
namespace DesignPatternsDemo
{

    // Singleton.cs
    public sealed class Singleton
    {
        private static readonly Singleton _instance = new Singleton();

        private Singleton() { }

        public static Singleton Instance => _instance;

        public string GetMessage() => "Hello from Singleton!";
    }




}


//2. Factory Method Pattern
// IProduct.cs
namespace DesignPatternsDemo
{
    public interface IProduct
    {
        string Operation();
    }

    public class ConcreteProductA : IProduct
    {
        public string Operation() => "Result of ConcreteProductA";
    }

    public class ConcreteProductB : IProduct
    {
        public string Operation() => "Result of ConcreteProductB";
    }

    public abstract class Creator
    {
        public abstract IProduct FactoryMethod();
    }

    public class ConcreteCreatorA : Creator
    {
        public override IProduct FactoryMethod() => new ConcreteProductA();
    }

    public class ConcreteCreatorB : Creator
    {
        public override IProduct FactoryMethod() => new ConcreteProductB();
    }
}


//3.Strategy Pattern
// IStrategy.cs
namespace DesignPatternsDemo
{
    public interface IStrategy
    {
        string Execute(int a, int b);
    }

    public class ConcreteStrategyAdd : IStrategy
    {
        public string Execute(int a, int b) => (a + b).ToString();
    }

    public class ConcreteStrategySubtract : IStrategy
    {
        public string Execute(int a, int b) => (a - b).ToString();
    }

    public class Context
    {
        private IStrategy _strategy;

        public void SetStrategy(IStrategy strategy) => _strategy = strategy;

        public string ExecuteStrategy(int a, int b) => _strategy.Execute(a, b);
    }
}

//4. Observer Pattern
// IObserver.cs
namespace DesignPatternsDemo
{
    public interface IObserver
    {
        void Update(string message);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public class ConcreteSubject : ISubject
    {
        private readonly List<IObserver> _observers = new List<IObserver>();
        private string _state;

        public string State
        {
            get => _state;
            set
            {
                _state = value;
                Notify();
            }
        }

        public void Attach(IObserver observer) => _observers.Add(observer);
        public void Detach(IObserver observer) => _observers.Remove(observer);
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_state);
            }
        }
    }

    public class ConcreteObserver : IObserver
    {
        private readonly string _name;

        public ConcreteObserver(string name) => _name = name;

        public void Update(string message) => Console.WriteLine($"{_name} received update: {message}");
    }
}