using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities.Rules;

namespace PerfectStorm.Rules
{
    public class PerformRuleAction<T> : BaseRuleAction
    {
        private Action<T> _foo;

        public PerformRuleAction(Action<T> foo)
        {
            _foo = foo;
        }

        public override void Execute(RuleExecution context)
        {
            T p = (T)context.ThisObject;
            if (p != null)
            {
                _foo(p);
            }
        }
    }
}
