using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace APD_BooleanCalculator
{
    public class BooleanCalculator
    {
        public static bool Solve(string booleanValue)
        {
            bool result = false;

            while (TokenOperatorExists("(", booleanValue) && TokenOperatorExists(")", booleanValue))
            {
                // Process parentheses
                int openingParenthesisPosition = GetOpeningParenthesisPosition(booleanValue);
                int closingParenthesisPosition = GetClosingParenthesisPosition(booleanValue, openingParenthesisPosition);
                var expressionResult = Solve(
                    ExtractExpressionFromParentheses(booleanValue, openingParenthesisPosition, closingParenthesisPosition));

                booleanValue = ReplaceParenthesesWithResult(booleanValue, expressionResult, openingParenthesisPosition,
                    closingParenthesisPosition); 
            }

            if (bool.TryParse(booleanValue, out result)) return result;

            if (TokenOperatorExists("OR", booleanValue))
            {
                var args = ProcessTwoArgumentOperator(booleanValue, "OR");

                return Solve(args[0]) || Solve(args[1]);
            }

            if (TokenOperatorExists("AND", booleanValue))
            {
                var args = ProcessTwoArgumentOperator(booleanValue, "AND");

                return Solve(args[0]) && Solve(args[1]);
            }

            if (TokenOperatorExists("NOT", booleanValue))
            {
                return !Solve(booleanValue.Split("NOT", 2, StringSplitOptions.TrimEntries).Last());
            }

            return result;
        }

        private static int GetOpeningParenthesisPosition(string booleanValue)
        {
            return booleanValue.IndexOf("(");
        }
        private static int GetClosingParenthesisPosition(string booleanValue, int openingParenthesisPosition)
        {
            int currentPosition = openingParenthesisPosition;
            int parenthesisCount = 0;

            foreach (char c in booleanValue.Substring(openingParenthesisPosition))
            {
                if (c == '(') parenthesisCount++;
                else if (c == ')') parenthesisCount--;

                if (parenthesisCount == 0) break;
                currentPosition++;
            }

            return currentPosition - openingParenthesisPosition;
        }


        private static string ExtractExpressionFromParentheses(string booleanValue, int openingParenthesisPosition, int closingParenthesisPosition)
        {
            return booleanValue.Substring(openingParenthesisPosition + 1, closingParenthesisPosition - 1);
        }

        private static string ReplaceParenthesesWithResult(string booleanValue, bool expressionResult,
            int openingParenthesisPosition, int closingParenthesisPosition)
        {
            return booleanValue.Remove(openingParenthesisPosition,
                    closingParenthesisPosition + 1)
                .Insert(openingParenthesisPosition, expressionResult.ToString().ToUpper());
        }

        private static bool TokenOperatorExists(string operatorName, string booleanValue)
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