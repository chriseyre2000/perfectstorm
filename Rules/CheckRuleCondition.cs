using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities.Rules;

namespace PerfectStorm.Rules
{

    public delegate C Func<T, C>(T t);

    public class CheckRuleCondition<T> : BaseRuleCondition
    {
        private Func<T, bool> _validator;

        public CheckRuleCondition(Func<T, bool> validator)
        {
            _validator = validator;
        }

        public override bool Evaluate(RuleExecution execution)
        {
            bool result = false;

            T p = (T)execution.ThisObject;

            if (p != null)
            {
                result = _validator(p);
            }

            return result;
        }
    }

}
