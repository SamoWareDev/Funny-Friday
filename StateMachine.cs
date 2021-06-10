using System;
using System.Collections.Generic;
using System.Text;

namespace FunnyFriday
{
    class StateMachine
    {
        private Stack<GameState> stateStack = new Stack<GameState>();
        private GameState currentState;

        public void AddStack(GameState state)
        {
            stateStack.Push(state);
            currentState = stateStack.Peek();
        }

        public void RemoveStack(GameState state)
        {
            stateStack.Pop();
            currentState = stateStack.Peek();
        }

        public void ChangeStack(GameState state)
        {
            stateStack.Pop();
            stateStack.Push(state);
            currentState = stateStack.Peek();
        }

        public GameState GetActiveStack()
        {
            return stateStack.Peek();
        }

    }
}
