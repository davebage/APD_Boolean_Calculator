namespace APD_BooleanCalculator
{
    public class BooleanCalculator
    {
        public static bool Parse(string booleanValue)
        {
            bool result = false;

            if (bool.TryParse(booleanValue, out result)) return result;

            // TRUE OR TRUE
            if (booleanValue.Split(" OR ", 2, StringSplitOptions.TrimEntries).Length > 1)
            {
                var expressionOne = Parse(booleanValue.Split(" OR ", 2, StringSplitOptions.TrimEntries)[0]);
                var expressionTwo = Parse(booleanValue.Split(" OR ", 2, StringSplitOptions.TrimEntries)[1]);

                return expressionOne || expressionTwo;
            }

            // TRUE AND FALSE
            if (booleanValue.Split(" AND ", 2, StringSplitOptions.TrimEntries).Length > 1)
            {
                var expressionOne = Parse(booleanValue.Split(" AND ", 2, StringSplitOptions.TrimEntries)[0]);
                var expressionTwo = Parse(booleanValue.Split(" AND ", 2, StringSplitOptions.TrimEntries)[1]);

                return expressionOne && expressionTwo;
            }

            if (booleanValue.Split("NOT", 2, StringSplitOptions.TrimEntries).Length > 1)
            {
                return !Parse(booleanValue.Split("NOT", 2, StringSplitOptions.TrimEntries).Last());
            }

            return result;
        }
    }
}