using System;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Tasks;

namespace SampleReSharperPlugin
{
    [ShellComponent]
    public class SolutionStateTracker : ISolutionStateTracker
    {
        public ISolution Solution { get; private set; }
        public ISignal<ISolution> AfterSolutionOpened { get; }
        public ISignal<ISolution> BeforeSolutionClosed { get; }

        public IProperty<string> SolutionName;

        public SolutionStateTracker([NotNull] Lifetime lifetime)
        {
            AfterSolutionOpened = new Signal<ISolution>(lifetime, "SolutionStateTracker.AfterSolutionOpened");
            BeforeSolutionClosed = new Signal<ISolution>(lifetime, "SolutionStateTracker.BeforeSolutionClosed");
            SolutionName = new Property<string>(lifetime, "SolutionStateTracker.SolutionName") { Value = "None" };
        }

        private void HandleSolutionOpened(ISolution solution)
        {
            Solution = solution;
            SolutionName.Value = solution.SolutionFile?.Name;
            AfterSolutionOpened.Fire(solution);
        }

        private void HandleSolutionClosed()
        {
            if (Solution == null)
                return;

            SolutionName.Value = "None";
            BeforeSolutionClosed.Fire(Solution);
            Solution = null;
        }


        [SolutionComponent]
        private class SolutionStateNotifier
        {
            public SolutionStateNotifier([NotNull] Lifetime lifetime,
                [NotNull] ISolution solution,
                [NotNull] ISolutionLoadTasksScheduler scheduler,
                [NotNull] SolutionStateTracker solutionStateTracker)
            {
                if (lifetime == null)
                    throw new ArgumentNullException("lifetime");
                if (solution == null)
                    throw new ArgumentNullException("solution");
                if (scheduler == null)
                    throw new ArgumentNullException("scheduler");
                if (solutionStateTracker == null)
                    throw new ArgumentNullException("solutionStateTracker");

                scheduler.EnqueueTask(new SolutionLoadTask("SolutionStateTracker",
                    SolutionLoadTaskKinds.Done, () => solutionStateTracker.HandleSolutionOpened(solution)));

                lifetime.AddAction(solutionStateTracker.HandleSolutionClosed);
            }
        }
    }

}