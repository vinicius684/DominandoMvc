namespace AppSemTemplate.Services
{
    public class OperacaoService//Vai receber a Injeção de dependencia
    {
        public OperacaoService(
            IOperacaoTransient transient,
            IOperacaoScoped scoped,
            IOperacaoSingleton singleton,
            IOperacaoSingletonInstance singletonInstance)
        {
            Transient = transient;
            Scoped = scoped;
            Singleton = singleton;
            SingletonInstance = singletonInstance;
        }

        public IOperacaoTransient Transient { get; }//Disponibilizar as propriedades de cada tipo
        public IOperacaoScoped Scoped { get; }
        public IOperacaoSingleton Singleton { get; }
        public IOperacaoSingletonInstance SingletonInstance { get; }
    }
    
    //Demo Injeção de dependencia que virou Demo Cliclos de vida
    public class Operacao : IOperacaoTransient,
                            IOperacaoScoped,
                            IOperacaoSingleton,
                            IOperacaoSingletonInstance
    {
        public Operacao() : this(Guid.NewGuid())
        {
        }

        public Operacao(Guid id)
        {
            OperacaoId = id;
        }

        public Guid OperacaoId { get; private set; }
    }

    public interface IOperacao
    {
        Guid OperacaoId { get; }
    }
    public interface IOperacaoTransient : IOperacao
    {
    }

    public interface IOperacaoScoped : IOperacao
    {
    }

    public interface IOperacaoSingleton : IOperacao
    {
    }

    public interface IOperacaoSingletonInstance : IOperacao
    {
    }
}


