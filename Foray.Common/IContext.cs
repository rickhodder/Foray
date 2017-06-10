using System.Collections.Generic;

namespace Foray.Common
{
    public interface IContext
    {
        Dictionary<string, object> Variables { get; set; }
        bool Finished { get; set; }
    }

    public interface IContext<TInput, TOutput> : IContext
    {
        TInput Input { get; set; }
        TOutput Output { get; set; }
        IState<TInput, TOutput> CurrentState { get; set; }
        void SetState(IState<TInput, TOutput> state);
        TOutput Execute();
        List<string> ErrorMessages { get; set; }
    }
}