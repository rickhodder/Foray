namespace Foray.Common
{
    public interface IState<TInput, TOutput>
    {
        void Handle();
    }
}