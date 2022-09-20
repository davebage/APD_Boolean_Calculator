using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace APD_BooleanCalculator
{

    public class BooleanCalculator
    {
        private const string NOT_OPERATOR = "NOT";
        private const string AND_OPERATOR = "AND";
        private const string OR_OPERATOR = "OR";
        private const char OPENING_PARENTHESIS = '(';
        private const char CLOSING_PARENTHESIS = ')';
        public static bool Solve(string booleanValue)
        {
            bool result = false;

            while (TokenOperatorExists(OPENING_PARENTHESIS.ToString(), booleanValue) && TokenOperatorExists(CLOSING_PARENTHESIS.ToString(), booleanValue))
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

            if (TokenOperatorExists(OR_OPERATOR, booleanValue))
            {
                var args = ProcessBinaryArgumentOperator(OR_OPERATOR, booleanValue);

                return Solve(args[0]) || Solve(args[1]);
            }

            if (TokenOperatorExists(AND_OPERATOR, booleanValue))
            {
                var args = ProcessBinaryArgumentOperator(AND_OPERATOR, booleanValue);

                return Solve(args[0]) && Solve(args[1]);
            }

            if (TokenOperatorExists(NOT_OPERATOR, booleanValue))
            {
                return !Solve(ProcessUnaryOperator(NOT_OPERATOR, booleanValue));
            }

            return result;
        }

        private static int GetOpeningParenthesisPosition(string booleanValue)
        {
            return booleanValue.IndexOf(OPENING_PARENTHESIS);
        }
        private static int GetClosingParenthesisPosition(string booleanValue, int openingParenthesisPosition)
        {
            int currentPosition = openingParenthesisPosition;
            int parenthesisCount = 0;

            foreach (char c in booleanValue.Substring(openingParenthesisPosition))
            {
                if (c == OPENING_PARENTHESIS) parenthesisCount++;
                else if (c == CLOSING_PARENTHESIS) parenthesisCount--;

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

        private static string ProcessUnaryOperator(string operatorName, string booleanValue)
        {
            return booleanValue.Split(operatorName, 2, StringSplitOptions.TrimEntries).Last();
        }

        private static List<string> ProcessBinaryArgumentOperator(string operatorName, string booleanValue)
        {
            List<string> arguments = new List<string>();

            arguments.Add(booleanValue.Split($"{operatorName}", 2, StringSplitOptions.TrimEntries)[0]);
            arguments.Add(booleanValue.Split($"{operatorName}", 2, StringSplitOptions.TrimEntries)[1]);

            return arguments;
        }
    }
}