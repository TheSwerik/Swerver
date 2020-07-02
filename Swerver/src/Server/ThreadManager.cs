using System;
using System.Collections.Generic;

namespace ServerLibrary.Server
{
    public static class ThreadManager
    {
        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> ExecuteCopiedOnMainThread = new List<Action>();
        private static bool _actionToExecuteOnMainThread;

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="action">The action to be executed on the main thread.</param>
        internal static void ExecuteOnMainThread(Action action)
        {
            if (action == null)
            {
                Console.WriteLine("No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(action);
                _actionToExecuteOnMainThread = true;
            }
        }

        /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
        internal static void UpdateMain()
        {
            if (!_actionToExecuteOnMainThread) return;
            ExecuteCopiedOnMainThread.Clear();
            lock (executeOnMainThread)
            {
                ExecuteCopiedOnMainThread.AddRange(executeOnMainThread);
                executeOnMainThread.Clear();
                _actionToExecuteOnMainThread = false;
            }

            foreach (var action in ExecuteCopiedOnMainThread) action();
        }
    }
}