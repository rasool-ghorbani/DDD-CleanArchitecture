namespace Domain.SeedWork.Rules
{
    public static class RuleChecker
    {
        public static async Task CheckRule(IBusinessRule rule)
        {
            if (!await rule.IsBrokenAsync())
                return;

            throw new BusinessRuleValidationException(rule);
        }
    }
}
