// <copyright file="Formula_PS2.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <summary>
//   <para>
//     This code is provides to start your assignment.  It was written
//     by Profs Joe, Danny, and Jim.  You should keep this attribution
//     at the top of your code where you have your header comment, along
//     with the other required information.
//   </para>
//   <para>
//     You should remove/add/adjust comments in your file as appropriate
//     to represent your work and any changes you make.
//   </para>
// <authors> Prachi Aswani </authors>
// <date> 20 September, 2024 </date>
// </summary>

namespace CS3500.Formula;

using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one ore more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    /// List to store formula tokens (operators, variables, numbers and parenthesis)
    /// </summary>
    private List<string> tokens;

    /// <summary>
    /// Stores the canonical string version of the formula (without spaces)
    /// </summary>
    private string canonicalString;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {
        // Check if the formula is empty or only whitespace
        if (formula.All(char.IsWhiteSpace))
        {
            throw new FormulaFormatException("The formula cannot be empty.");
        }

        // Get tokens from the formula string
        tokens = GetTokens(formula);

        // Balance to check for valid parentheses
        int parenBalance = 0;

        // Ensure correct operator/ operand structure
        bool expectOperand = true;

        var sb = new StringBuilder(); // StringBuilder for constructing canonical form

        // Loop through each token in the list
        for (int i = 0; i < tokens.Count; i++)
        {
            string token = tokens[i];

            // Check for empty parentheses
            // If the current token is ")" and the previous token is "(", this indicates empty parentheses
            if (i > 0 && token == ")" && tokens[i - 1] == "(")
            {
                throw new FormulaFormatException("Empty parentheses are not allowed.");
            }

            // Handle numbers
            // Attempt to parse the current token as a double
            if (double.TryParse(token, out double number))
            {
                token = number.ToString();

                // Ensure correct syntax (e.g., no consecutive numbers or misplaced operators)
                if (!expectOperand)
                {
                    throw new FormulaFormatException($"Unexpected operand: {token}");
                }

                sb.Append(token); // Add token to the canonical string
                expectOperand = false; // After a number, expect an operator
            }

            // Handle variables
            else if (IsVar(token))
            {
                // If a variable appears when not expected
                if (!expectOperand)
                {
                    throw new FormulaFormatException($"Unexpected variable: {token}");
                }

                // Normalize variables to uppercase
                tokens[i] = token.ToUpper();
                sb.Append(tokens[i]); // Add token to the canonical string
                expectOperand = false; // After a variable, expect an operator
            }

            // Handle opening parentheses
            else if (token == "(")
            {
                parenBalance++;
                expectOperand = true; // After "(", expect a number or variable
                sb.Append(token);
            }

            // Handle closing parentheses
            else if (token == ")")
            {
                parenBalance--;

                // If closing parentheses exceed opening ones, throw an error
                if (parenBalance < 0)
                {
                    throw new FormulaFormatException("Mismatched parentheses.");
                }

                sb.Append(token);
                expectOperand = false;
            }

            // Handle operators (+, -, *, /)
            else if ("+-*/".Contains(token))
            {
                // If an operator appears when an operand (number or variable) is expected, throw an error
                if (expectOperand)
                {
                    throw new FormulaFormatException($"Unexpected operator: {token}");
                }

                sb.Append(token);
                expectOperand = true; // After an operator, expect a number or variable
            }

            // Handle invalid tokens
            else
            {
                throw new FormulaFormatException($"Invalid token: {token}");
            }
        }

        // Check for unbalanced parentheses at the end of the formula
        if (parenBalance != 0)
        {
            throw new FormulaFormatException("Unbalanced parentheses.");
        }

        // Check if the formula ends with an operator, throw an exception
        if (expectOperand)
        {
            throw new FormulaFormatException("Formula ends with an operator.");
        }

        // Set the canonical string
        canonicalString = sb.ToString();
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
    ///   </remarks>
    ///   <para>
    ///     For example, if N is a method that converts all the letters in a string to upper case:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should enumerate "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should enumerate "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {
        var variables = new HashSet<string>();

        // Loop through each token in the list of tokens
        foreach (var token in tokens)
        {
            // Check if the current token is a valid variable
            if (IsVar(token))
            {
                // Add the variable to the HashSet
                variables.Add(token);
            }
        }

        // Return the set of unique variables
        return variables;
    }

    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f 
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All of the variables in the string will be normalized.  This
    ///     means capital letters.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + y1").ToString() should return "X1+Y1"
    ///       new("X1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This code should execute in O(1) time.
    ///   <para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public override string ToString()
    {
        return canonicalString;
    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
    public static bool operator ==(Formula f1, Formula f2)
    {
        // Since f1 and f2 cannot be null, we directly compare their values
        return f1.Equals(f2);
    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula f1, Formula f2)
    {
        // Not (==) operator
        return !(f1 == f2);
    }

    /// <summary>
    ///   <para>
    ///     Determines if two formula objects represent the same formula.
    ///   </para>
    ///   <para>
    ///     By definition, if the parameter is null or does not reference 
    ///     a Formula Object then return false.
    ///   </para>
    ///   <para>
    ///     Two Formulas are considered equal if their canonical string representations
    ///     (as defined by ToString) are equal.  
    ///   </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    ///   True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {
        // If the parameter is null, the objects cannot be equal
        if (obj is null)
            return false;

        // Check if the parameter is a Formula object
        if (obj is Formula other)
        {
            // Compare the canonical string representations of both formulas using ToString
            return this.ToString() == (other.ToString()); // Compare canonical strings
        }

        // If the object is not a Formula, return false
        return false;
    }

    /// <summary>
    ///   <para>
    ///     Evaluates this Formula, using the lookup delegate to determine the values of
    ///     variables.
    ///   </para>
    ///   <remarks>
    ///     When the lookup method is called, it will always be passed a normalized (capitalized)
    ///     variable name.  The lookup method will throw an ArgumentException if there is
    ///     not a definition for that variable token.
    ///   </remarks>
    ///   <para>
    ///     If no undefined variables or divisions by zero are encountered when evaluating
    ///     this Formula, the numeric value of the formula is returned.  Otherwise, a 
    ///     FormulaError is returned (with a meaningful explanation as the Reason property).
    ///   </para>
    ///   <para>
    ///     This method should never throw an exception.
    ///   </para>
    /// </summary>
    /// <param name="lookup">
    ///   <para>
    ///     Given a variable symbol as its parameter, lookup returns the variable's value
    ///     (if it has one) or throws an ArgumentException (otherwise).  This method will expect 
    ///     variable names to be normalized.
    ///   </para>
    /// </param>
    /// <returns> Either a double or a FormulaError, based on evaluating the formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        Stack<string> operatorStack = new Stack<string>();
        Stack<double> valueStack = new Stack<double>();

        //Using a try catch to catch a divide by 0 error, or an argument exception error from the lookup
        //If these exceptions are caught, it will return a formula error. 
        //If not, it will return a double. 
        try
        {
            foreach (var s in tokens)
            {
                if (IsLeftParenthesis(s))
                {
                    operatorStack.Push(s);
                }
                else if (IsRightParenthesis(s))
                {
                    // Check if the top of the operator stack is a plus or minus operator
                    if (IsPlusOrMinusOnStack(operatorStack))
                    {
                        double calculatedValue = PerformPlusMinusOperation(operatorStack, valueStack);
                        valueStack.Push(calculatedValue);
                    }
                    operatorStack.Pop(); // Pop the matching left parenthesis

                    if (IsMultiplyOrDivideOnStack(operatorStack))
                    {
                        double calculatedValue = PerformMultiplyDivideComputation(operatorStack, valueStack, 0, false);
                        valueStack.Push(calculatedValue);// Push the result onto valueStack
                    }
                }
                else if (IsMultiplyOrDivide(s))
                {
                    operatorStack.Push(s);
                }
                else if (IsPlusOrMinus(s))
                {
                    // Check if there's already a plus or minus operator on top of the stack
                    if (IsPlusOrMinusOnStack(operatorStack))
                    {
                        double calculatedValue = PerformPlusMinusOperation(operatorStack, valueStack);
                        valueStack.Push(calculatedValue);
                    }
                    operatorStack.Push(s);
                }
                else if (IsNumber(s))
                {
                    // Parse the token to a double
                    double value = double.Parse(s);
                    // Handle the number or variable
                    HandleNumOrVariable(operatorStack, valueStack, value);
                }
                else if (IsVar(s))
                {
                    // Retrieve the value of the variable using the lookup function
                    double value = lookup(s);
                    HandleNumOrVariable(operatorStack, valueStack, value);
                }
            }
            //End of for loop

            // Final evaluation step to return the computed value
            if (operatorStack.Count == 0)
                // Return the final computed value if no operators remain
                return valueStack.Pop();
            else
                return PerformPlusMinusOperation(operatorStack, valueStack);
        }
        catch (ArgumentException)
        {
            // Returns a formula error for invalid variables
            FormulaError error = new FormulaError("Invalid variable");
            return error;
        }
        catch (DivideByZeroException)
        {
            // Returns a formula error for division by zero
            FormulaError error = new FormulaError("Division by 0");
            return error;
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    ///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    ///     randomly-generated unequal Formulas have the same hash code should be extremely small.
    ///   </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {
        return canonicalString.GetHashCode(); // Use the hash code of the canonical string
    }

    // Private Helper methods for Evaluate

    /// <summary>
    /// Private helper method that determines whether the given token represents a valid number.
    /// This method correctly handles standard numeric formats as well as scientific notation.
    /// </summary>
    /// <param name="token">The token to evaluate for number validity.</param>
    /// <returns>True if the token is a valid number; otherwise, false.</returns>
    private bool IsNumber(string token)
    {
        // Attempt to parse the token as a double.
        double i = 0;
        // If successful, return true; otherwise, return false.
        return double.TryParse(token, out i);
    }

    /// <summary>
    /// Private helper method that checks if the given token is a left parenthesis.
    /// </summary>
    /// <param name="token">The token to evaluate.</param>
    /// <returns>True if the token is a left parenthesis; otherwise, false.</returns>
    private bool IsLeftParenthesis(string token)
    {
        // Check if the token is equal to the left parenthesis character.
        return token == "(";
    }

    /// <summary>
    /// Private helper method that checks if the given token is a right parenthesis.
    /// </summary>
    /// <param name="token">The token to evaluate.</param>
    /// <returns>True if the token is a right parenthesis; otherwise, false.</returns>
    private bool IsRightParenthesis(string token)
    {
        // Check if the token is equal to the right parenthesis character.
        return token == ")";
    }

    /// <summary>
    /// Private helper method that checks if the provided operator is either plus or minus.
    /// </summary>
    /// <param name="s">The operator to evaluate.</param>
    /// <returns>True if the operator is '+' or '-'; otherwise, false.</returns>
    private bool IsPlusOrMinus(string s)
    {
        // Check if the operator is equal to '+' or '-'.
        return s == "+" || s == "-";
    }

    /// <summary>
    /// Private helper method that checks if the given operator is either multiplication or division.
    /// </summary>
    /// <param name="s">The operator to evaluate.</param>
    /// <returns>True if the operator is '*' or '/'; otherwise, false.</returns>
    private bool IsMultiplyOrDivide(string s)
    {
        // Check if the operator is equal to '*' or '/'.
        return s == "*" || s == "/";
    }

    /// <summary>
    /// Private helper method that checks the top of the operator stack to see if it contains either '+' or '-'.
    /// </summary>
    /// <param name="operandStack">The operator stack to check.</param>
    /// <returns>True if the top of the stack is either '+' or '-'; otherwise, false.</returns>
    private bool IsPlusOrMinusOnStack(Stack<string> operandStack)
    {
        // If the stack is empty, return false.
        if (operandStack.Count == 0)
        {
            return false;
        }

        // Peek at the top of the stack and check if it is either '+' or '-'.
        string top = operandStack.Peek();
        return top == "+" || top == "-";
    }

    /// <summary>
    /// Private helper method that checks the top of the operator stack to see if it contains either '*' or '/'.
    /// </summary>
    /// <param name="operandStack">The operator stack to check.</param>
    /// <returns>True if the top of the stack is either '*' or '/'; otherwise, false.</returns>
    private bool IsMultiplyOrDivideOnStack(Stack<string> operandStack)
    {
        // If the stack is empty, return false.
        if (operandStack.Count == 0)
        {
            return false;
        }

        // Peek at the top of the stack and check if it is either '*' or '/'.
        string top = operandStack.Peek();
        return top == "*" || top == "/";
    }

    /// <summary>
    /// Performs basic arithmetic operations based on the provided values and operator.
    /// </summary>
    /// <param name="value1">The first value for computation.</param>
    /// <param name="value2">The second value for computation.</param>
    /// <param name="op">The operator to use for the computation.</param>
    /// <returns>The result of the computation. If an invalid operator is passed, the behavior is not defined.</returns>
    /// <remarks>
    /// This method will throw a DivideByZeroException if division by zero is attempted.
    /// </remarks>
    private double CalculateValue(double value1, double value2, string op)
    {
        // Perform the operation based on the specified operator.
        if (op == "+")
        {
            return value1 + value2;
        }
        else if (op == "-")
        {
            return value1 - value2;
        }
        else if (op == "/")
        {
            if (value2 == 0)
            {
                throw new DivideByZeroException();
            }
            return value1 / value2;
        }
        else
        {
            return value1 * value2;
        }
    }

    /// <summary>
    /// Private helper method that uses the CalculateValue method to perform a plus or minus computation.
    /// This is done by popping two values from the value stack
    /// and using the operator from the operator stack.
    /// Similar to PerformMultiplyDivideComputation.
    /// </summary>
    /// <param name="operatorStack">The stack containing the operators.</param>
    /// <param name="valueStack">The stack containing the values.</param>
    /// <returns>The calculated value from the plus or minus operation.</returns>
    /// <seealso cref="CalculateValue(double, double, string)"/>
    private double PerformPlusMinusOperation(Stack<string> operatorStack, Stack<double> valueStack)
    {
        // Pop the top two values from the value stack.
        double firstValue = valueStack.Pop();
        double secondValue = valueStack.Pop();

        // Pop the operator from the operator stack.
        string op = operatorStack.Pop();

        // Calculate the value using the operator and the popped values.
        double calculatedValue = CalculateValue(secondValue, firstValue, op);

        return calculatedValue;
    }

    /// <summary>
    /// Private helper method that uses the CalculateValue method to perform a multiplication or division computation.
    /// This can be done by either using two values from the value stack
    /// or by using one value from the stack and a provided value.
    /// Similar to PerformPlusMinusOperation.
    /// </summary>
    /// <param name="operatorStack">The stack containing the operators.</param>
    /// <param name="valueStack">The stack containing the values.</param>
    /// <param name="value">The additional value for computation, if needed.</param>
    /// <param name="isTokenDouble">If true, uses the provided value; otherwise, uses two values from the stack.</param>
    /// <returns>The calculated value from the multiplication or division operation.</returns>
    /// <seealso cref="CalculateValue(double, double, string)"/>
    private double PerformMultiplyDivideComputation(Stack<string> operatorStack, Stack<double> valueStack, double value, bool isTokenDouble)
    {
        // If using the provided value for computation, pop the top value from the stack.
        if (isTokenDouble)
        {
            double stackValue = valueStack.Pop();
            string op = operatorStack.Pop();
            double calculatedValue = CalculateValue(stackValue, value, op);
            return calculatedValue;
        }
        // If not using the provided value, pop two values from the stack and calculate.
        else
        {
            double firstValue = valueStack.Pop();
            double secondValue = valueStack.Pop();
            string op = operatorStack.Pop();
            double calculatedValue = CalculateValue(secondValue, firstValue, op);
            return calculatedValue;
        }
    }

    /// <summary>
    /// Private helper method that performs the same computation for both doubles and variables when given a value.
    /// This method checks the operator stack to determine if a multiplication or division operation should be performed.
    /// If so, it uses the provided value in the computation; otherwise, it simply pushes the value onto the value stack.
    /// </summary>
    /// <param name="operatorStack">The stack containing the operators.</param>
    /// <param name="valueStack">The stack containing the values.</param>
    /// <param name="value">The value used for the computation, which should come from either a double token or a variable token.</param>
    private void HandleNumOrVariable(Stack<string> operatorStack, Stack<double> valueStack, double value)
    {
        // Check if the top of the operator stack is a multiply or divide operator
        if (IsMultiplyOrDivideOnStack(operatorStack))
        {
            // Perform the multiplication or division computation with the provided value
            double calculatedValue = PerformMultiplyDivideComputation(operatorStack, valueStack, value, true);
            valueStack.Push(calculatedValue);
        }
        else
        {
            // If no multiplication or division is needed, push the value directly onto the value stack
            valueStack.Push(value);
        }
    }

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar(string token)
    {
        // notice the use of ^ and $ to denote that the entire string being matched is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch(token, standaloneVarPattern);
    }

    /// <summary>
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens(string formula)
    {
        List<string> results = [];

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(s);
            }
        }

        return results;
    }
}


/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError(string message)
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);
