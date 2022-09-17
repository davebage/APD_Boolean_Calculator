using System.ComponentModel.Design;

namespace APD_BooleanCalculator
{
    public class BooleanCalculator
    {
        public static bool Parse(string booleanValue)
        {
            bool result = false;

            while (booleanValue.IndexOf("(") > -1)
            {
                // Process parentheses
                int openingParenthesisPosition = booleanValue.IndexOf("(");
                int closingParenthesisPosition = 0;

                int currentPosition = openingParenthesisPosition;
                int parenthesisCount = 0;

                foreach (char c in booleanValue.Substring(openingParenthesisPosition))
                {
                    if (c == '(') parenthesisCount++;
                    else if (c == ')') parenthesisCount--;

                    if (parenthesisCount == 0) break;
                    currentPosition++;
                }

                closingParenthesisPosition = currentPosition - openingParenthesisPosition;
                var expressionResult = Parse(booleanValue.Substring(openingParenthesisPosition + 1, closingParenthesisPosition - 1));

                booleanValue = booleanValue.Remove(openingParenthesisPosition,
                        closingParenthesisPosition + 1)
                    .Insert(openingParenthesisPosition, expressionResult.ToString().ToUpper());
            }

            if (bool.TryParse(booleanValue, out result)) return result;

            if (OperatorExists("OR", booleanValue))
            {
                var args = ProcessTwoArgumentOperator(booleanValue, "OR");

                return Parse(args[0]) || Parse(args[1]);
            }

            if (OperatorExists("AND", booleanValue))
            {
                var args = ProcessTwoArgumentOperator(booleanValue, "AND");

                return Parse(args[0]) && Parse(args[1]);
            }

            if (OperatorExists("NOT", booleanValue))
            {
                return !Parse(booleanValue.Split("NOT", 2, StringSplitOptions.TrimEntries).Last());
            }

            return result;
        }

        private static bool OperatorExists(string operatorName, string booleanValue)
        {
            return booleanValue.Split($"{operatorName}", 2, StringSplitOptions.TrimEntries).Length > 1;
        }

        private static List<string> ProcessTwoArgumentOperator(string booleanValue, string operatorName)
        {
            List<string> arguments = new List<string>();

            arguments.Add(booleanValue.Split($"{operatorName}", 2, StringSplitOptions.TrimEntries)[0]);
            arguments.Add(booleanValue.Split($"{operatorName}", 2, StringSplitOptions.TrimEntries)[1]);

            return arguments;
        }
    }
}