using System;
using System.Collections.Generic;
using System.Workflow.Activities.Rules;

namespace PerfectStorm.Rules
{
    public abstract class BaseRuleCondition : RuleCondition
    {
        public override RuleCondition Clone()
        {
            return null;
        }

        public override ICollection<string> GetDependencies(RuleValidation validation)
        {
            return null;
        }

        public override string Name
        {
            get
            {
                return this.GetType().Name;
            }
            set
            {
                ;
            }
        }

        public override bool Validate(RuleValidation validation)
        {
            return true;
        }

    }
}
