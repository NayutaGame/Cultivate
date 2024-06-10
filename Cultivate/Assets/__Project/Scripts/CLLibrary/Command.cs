
using System;
using System.Threading.Tasks;
using DG.Tweening;

namespace CLLibrary
{
    public class Command
    {
        protected Action _execute;
        protected Func<Task> _asyncExecute;
        protected Action _complete;

        protected Command()
        {
            
        }

        protected Command(Action execute, Func<Task> asyncExecute, Action complete)
        {
            _execute = execute;
            _asyncExecute = asyncExecute;
            _complete = complete;
        }

        public void Execute() => _execute();
        public async Task AsyncExecute() => await _asyncExecute();
        public void Complete() => _complete();
    }

    public class CommandFromTween : Command
    {
        private Func<Tween> _getHandle;
        private Action<Tween> _setHandle;
        private Tween _tween;
        
        public CommandFromTween(Func<Tween> getHandle, Action<Tween> setHandle, Tween tween)
        {
            _getHandle = getHandle;
            _setHandle = setHandle;
            _tween = tween;
            _execute = _tween.Complete;
            _asyncExecute = DefaultAsyncExecute;
            _complete = getHandle().Complete;
        }
        
        private async Task DefaultAsyncExecute()
        {
            _getHandle()?.Kill();
            _setHandle(_tween);
            _getHandle().Restart();
            await _getHandle().AsyncWaitForCompletion();
        }
    }
}
