using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities.Rules;

namespace PerfectStorm.Rules
{
    public abstract class BaseRuleAction : RuleAction
    {
        public override RuleAction Clone()
        {
            return null;
        }

        public override ICollection<string> GetSideEffects(RuleValidation validation)
        {
            return null;
        }

        public override bool Validate(RuleValidation validator)
        {
            return true;
        }

    }
}
