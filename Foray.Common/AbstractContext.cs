using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Foray.Common
{
    

    public abstract class AbstractContext<TInput, TOutput> : IContext<TInput, TOutput>
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();

        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, Object>();
        public bool Finished { get; set; }
        public TInput Input { get; set; }
        public TOutput Output { get; set; }
        public IState<TInput, TOutput> CurrentState { get; set; }

        public void SetState(IState<TInput, TOutput> state)
        {
            CurrentState = state;
        }

        public TOutput Execute()
        {
            while (!Finished)
            {
                CurrentState.Handle();
            }

            return Output;
        }
    }
}