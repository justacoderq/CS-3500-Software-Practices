// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> Prachi Aswani </authors>
// <date> 19 September, 2024 </date>

namespace FormulaTests;

using CS3500.Formula;

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNoTokens_Invalid()
    {
        _ = new Formula(string.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNoTokens_Invalid1()
    {
        _ = new Formula("");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a single numeric token.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOneTokenNumber_Valid()
    {
        _ = new Formula("1");
    }

    /// <summary>
    ///   <para>
    ///    This test ensures that a valid formula is created when
    ///     the input string consists of a single variable token.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOneTokenVariable_Valid()
    {
        _ = new Formula("Re41");
    }

    // --- Tests for Valid Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of numeric tokens with a decimal point.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestDecimalNumberToken_Valid()
    {
        _ = new Formula("3.14 + 4.5");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of lower case exponent tokens.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNumberELowerCaseToken_Valid()
    {
        _ = new Formula("2e5 * 23");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a valid formula is created when
    ///     the input string consists of scientific notation with negative exponents.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNumberENegativeToken_Valid()
    {
        _ = new Formula("3.5e-6 - 6");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of uppercase exponenet tokens.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNumberUpperCaseEToken_Valid()
    {
        _ = new Formula("3.5E6 / 2");
    }

    /// <summary>
    ///   <para>
    ///    This test ensures that a FormulaFormatException is thrown when
    ///     the input string starts with a negative number token, which is invalid.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNegativeNumberToken_Invalid()
    {
        _ = new Formula("-5 + 6");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of simple addition operator using numbers.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestAdditionOperatorToken_Valid()
    {
        _ = new Formula("4 + 8");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of simple subtraction.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestSubtractionOperatorToken_Valid()
    {
        _ = new Formula("10 - 3");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a valid formula is created when
    ///     the input string consists of simple multiplication operator using numbers.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestMultiplicationOperatorToken_Valid()
    {
        _ = new Formula("6 * 4");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a valid formula is created when
    ///     the input string consists of simple division.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestDivisionOperatorToken_Valid()
    {
        _ = new Formula("4 / 2");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string contains parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestParenthesisOperatorToken_Valid()
    {
        _ = new Formula("(3 - 2)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a simple variable (one letter
    ///     followed by one digit) and a number token.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestSimpleVariableToken_Valid()
    {
        _ = new Formula("a1 - 1");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a variable with multiple letters
    ///     and digits.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestComplexVariableToken_Valid()
    {
        _ = new Formula("AAab1 / 2");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a variable with letters and digits
    ///     followed by a multiplication operation.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestComplexVariableToken1_Valid()
    {
        _ = new Formula("abc123 * 3");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a valid formula is created when
    ///     the input string consists of a variable followed by an addition operation.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestComplexVariableToken2_Valid()
    {
        _ = new Formula("abc2 + 3");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a variable followed by a subtraction operation.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestComplexVariableToken3_Valid()
    {
        _ = new Formula("ab14 - 9");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a variable followed by an addition operation
    ///     with a number.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestComplexVariableToken4_Valid()
    {
        _ = new Formula("abc123 + 23");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a variable followed by a division operation
    ///     with a number.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestComplexVariableToken5_Valid()
    {
        _ = new Formula("adc92 / 3");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a complex variable token followed by
    ///     a multiplication operation.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestVariableTokenComplex_Valid()
    {
        _ = new Formula("Lkj54 * 90");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a FormulaFormatException is thrown when
    ///     the input string consists of a variable that does not conform
    ///     to the expected format (e.g., a single letter followed by a digit).
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestVariableToken_Invalid()
    {
        _ = new Formula("a + 0");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string consists of a variable that starts with a digit
    ///     followed by a letter.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestVariableToken1_Invalid()
    {
        _ = new Formula("1a -2");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string consists of a variable with a letter followed by a
    ///     digit followed by letter again.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestVariableToken2_Invalid()
    {
        _ = new Formula("a1a * 2");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string contains an invalid token such as a special character.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSymbolToken_Invalid()
    {
        _ = new Formula("45 + $");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a valid expression with variables
    ///     and numbers, including operations enclosed in parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestValidTokenComplex_Valid()
    {
        _ = new Formula("3 + (x1 * 4.5)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a valid formula is created when
    ///     the input string consists of a valid expression without spaces
    ///     between numbers and operators.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestWNoWhiteSpaceToken_Valid()
    {
        _ = new Formula("123-456");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string contains a complex invalid expression with
    ///     improper variable formats and special characters.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestValidTokenComplex_Invalid()
    {
        _ = new Formula("1abc2a-x$2");
    }

    // --- Tests for Closing Parenthesis Rule

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string contains an extra closing parenthesis after
    ///     an operator.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesisGreaterThanOpeningParethesis_Valid()
    {
        _ = new Formula("(1+)4)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string contains multiple unmatched closing
    ///     parentheses at the end.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesisGreaterThanOpeningParethesis1_Valid()
    {
        _ = new Formula("(1+4))");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string contains an extra closing parenthesis
    ///     after a complete expression.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesisGreaterThanOpeningParethesis2_Valid()
    {
        _ = new Formula("(1)+4)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string contains multiple unmatched closing
    ///     parentheses in various parts of the formula.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesisGreaterThanOpeningParethesisMixed_Valid()
    {
        _ = new Formula("(1)+)4))");
    }

    // --- Tests for Balanced Parentheses Rule

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with balanced parentheses
    ///     and a valid expression is correctly created without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalancedParanthesis_Valid()
    {
        _ = new Formula("(23*6)");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a formula with nested balanced
    ///      parentheses and a valid expression is correctly created
    ///      without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalancedParanthesisNested_Valid()
    {
        _ = new Formula("(2 + (23*6))");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a formula with multiple levels
    ///      of nested balanced parentheses and a valid expression 
    ///      is correctly created without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalancedParanthesisNestedMultiple_Valid()
    {
        _ = new Formula("(2 + (23-(6/2)))");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with balanced parentheses
    ///     in different sections and a valid expression is correctly 
    ///     created without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestBalancedParanthesisComplex_Valid()
    {
        _ = new Formula("(3 + 2) * (x1 - 5)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     the input string contains unbalanced parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestBalancedParanthesisUnbalanced_Invalid()
    {
        _ = new Formula("(29 / (23*6)");
    }

    // --- Tests for First Token Rule

    /// <summary>
    ///   <para>
    ///     Make sure a simple well formed formula is accepted by the constructor (the constructor
    ///     should not throw an exception).
    ///   </para>
    ///   <remarks>
    ///     This is an example of a test that is not expected to throw an exception, i.e., it succeeds.
    ///     In other words, the formula "1+1" is a valid formula which should not cause any errors.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenNumber_Valid()
    {
        _ = new Formula("1+1");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula starting with a variable as the first token
    ///     is correctly accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenVariable_Valid()
    {
        _ = new Formula("c1 + 2");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula starting with an
    ///     opening parenthesis as the first token is correctly
    ///     accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenOpeningParenthesis_Valid()
    {
        _ = new Formula("(c1 + 2) / 2");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is
    ///     thrown when a formula starts with an operator as the first token,
    ///     which is not allowed.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenOperator_Invalid()
    {
        _ = new Formula("+ 3 * x1");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown
    ///     when a formula starts with a closing parenthesis as the first
    ///     token, which is not allowed.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenClosingParenthesis_Invalid()
    {
        _ = new Formula(") + 6");
    }

    // --- Tests for  Last Token Rule ---

    /// <summary>
    ///   <para>
    ///      This test ensures that a formula ending with a
    ///      number as the last token is correctly accepted
    ///      by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLastTokenNumber_Valid()
    {
        _ = new Formula("28 / 4");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula ending with
    ///     a variable as the last token is correctly accepted
    ///     by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLastTokenVariable_Valid()
    {
        _ = new Formula("43 - EE2");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula ending with
    ///     a closing parenthesis as the last token is correctly 
    ///     accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLastTokenClosingParenthesis_Valid()
    {
        _ = new Formula("(5 / 2)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown
    ///     when a formula ends with an opening parenthesis as the
    ///     last token, which is not allowed.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenOpeningParenthesis_Invalid()
    {
        _ = new Formula("5 * (");
    }

    /// <summary>
    ///   <para>
    ///    This test ensures that a FormulaFormatException is
    ///    thrown when a formula ends with an operator as the
    ///    last token, which is not allowed.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenOperator_Invalid()
    {
        _ = new Formula("67 +");
    }

    // --- Tests for Parentheses/Operator Following Rule ---

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula where an opening parenthesis
    ///     is immediately followed by a number is correctly accepted
    ///     by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByNumber_Valid()
    {
        _ = new Formula("(9 - 7)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula where an opening parenthesis
    ///     is immediately followed by a variable is correctly accepted
    ///     by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByVariable_Valid()
    {
        _ = new Formula("(aAb1 * 2)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula where an opening parenthesis
    ///     is immediately followed by another opening parenthesis is
    ///     correctly accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByOpeningParenthesis_Valid()
    {
        _ = new Formula("( (2 / 4) - 3 )");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     an opening parenthesis is immediately followed by an operator,
    ///     which is not allowed.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByOperator_Invalid()
    {
        _ = new Formula("( + 3)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     a formula has empty parentheses, which is not allowed.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpeningParenthesisFollowedByClosingParenthesis_Invalid()
    {
        _ = new Formula("4 * ()");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     a formula within parentheses contains an invalid symbol, which
    ///     is not allowed.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpeningParenthesisFollowedBySymbol_Invalid()
    {
        _ = new Formula("( $  + 1)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with an operator followed by
    ///     a valid number is correctly accepted by the constructor without
    ///     throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByNumber_Valid()
    {
        _ = new Formula("3 + 25");
    }


    /// <summary>
    ///   <para>
    ///    This test ensures that a formula with an operator followed by
    ///     variable is correctly accepted by the constructor without
    ///     throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByVariable_Valid()
    {
        _ = new Formula("28 * a1");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with an operator followed by
    ///     an opening parenthesis is correctly accepted by the constructor
    ///     without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowedByOpeningParenthesis_Valid()
    {
        _ = new Formula("2 + (10 - 6)");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a FormulaFormatException is thrown when
    ///     an operator is followed by another operator.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorFollowedByOperator_Invalid()
    {
        _ = new Formula("5 + + 5)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     an operator is directly followed by a closing parenthesis.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorFollowedByClosingParenthesis_Invalid()
    {
        _ = new Formula(" 3 * )");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a FormulaFormatException is thrown when
    ///     an operator is followed by an invalid symbol.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorFollowedBySymbol_Invalid()
    {
        _ = new Formula(" 19 / @ ");
    }

    // --- Tests for Extra Following Rule ---

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a number followed by an operator
    ///     is correctly accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtraFollowingNumberIsOperator_Valid()
    {
        _ = new Formula("72 / 2");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a formula with a number
    ///      followed by a closing parenthesis is correctly
    ///      accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtraFollowingNumberIsClosingParenthesis_Valid()
    {
        _ = new Formula("(a1 + 2)");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a formula with a number followed
    ///      by another number results in a FormulaFormatException, as
    ///      this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingNumberIsNumber_Invalid()
    {
        _ = new Formula("23 2 + dF3");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a number followed by a variable
    ///     results in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingNumberIsVariable_Invalid()
    {
        _ = new Formula("345 RE34");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a number followed by an opening parenthesis
    ///     results in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingNumberIsOpeningParenthesis_Invalid()
    {
        _ = new Formula("43 + 789(");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a number followed by an invalid symbol
    ///     results in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingNumberIsSymbol_Invalid()
    {
        _ = new Formula("a1 + 879$");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a variable followed by an operator
    ///     is correctly accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtraFollowingVariableIsOperator_Valid()
    {
        _ = new Formula("Av6 - 5");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a variable
    ///     followed by a closing parenthesis is correctly accepted
    ///     by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtraFollowingVariableIsClosingParenthesis_Valid()
    {
        _ = new Formula("(Df2)");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a variable followed by a number
    ///     results in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingVariableIsNumber_Invalid()
    {
        _ = new Formula("Er1 3 + 3");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a formula with a variable followed by another variable
    ///     results in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingVariableIsVariable_Invalid()
    {
        _ = new Formula("AD4 Ji1 / 2");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a variable followed by an opening parenthesis
    ///     results in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingVariableIsOpeningParenthesis_Invalid()
    {
        _ = new Formula("1 + SFd3(");
    }

    /// <summary>
    ///   <para>
    ///    This test ensures that a formula with a variable followed by an invalid symbol
    ///     results in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingVariableIsSymbol_Invalid()
    {
        _ = new Formula(" 2 + KMn45#");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a 
    ///     closing parenthesis followed by an operator is correctly
    ///     accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtraFollowingClosingParenthesisIsOperator_Valid()
    {
        _ = new Formula("(9 - 3) + 2");
    }

    /// <summary>
    ///   <para>
    ///      This test ensures that a formula with a closing
    ///      parenthesis followed by another closing parenthesis is
    ///      correctly accepted by the constructor without throwing exceptions.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestExtraFollowingClosingParenthesisIsClosingParenthesis_Valid()
    {
        _ = new Formula("( 3 + (9+8) )");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a
    ///     closing parenthesis followed by a number results
    ///     in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingClosingParenthesisIsNumber_Invalid()
    {
        _ = new Formula("(24 - 4)3");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a closing
    ///     parenthesis followed by a variable results
    ///     in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingClosingParenthesisIsVariable_Invalid()
    {
        _ = new Formula("(4 + 3)Ji1");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a closing
    ///     parenthesis followed by an opening parenthesis
    ///     results in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingClosingParenthesisIsOpeningParenthesis_Invalid()
    {
        _ = new Formula("(1 * 45)(");
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with a closing
    ///     parenthesis followed by an invalid symbol results
    ///     in a FormulaFormatException, as this is an invalid token sequence.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestExtraFollowingClosingParenthesisIsSymbol_Invalid()
    {
        _ = new Formula("(24/2)#");
    }

    // Tests for ToString() method

    /// <summary>
    ///   <para>
    ///   This test ensures that a simple numeric formula 
    ///   is represented correctly as a string, with no modifications 
    ///   to the number.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestNumber_Valid()
    {
        Formula num = new Formula("23");
        Assert.AreEqual("23", num.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that a formula with
    ///   spaces between numbers and operators is returned
    ///   as a string with no spaces.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestWhitespace_Valid()
    {
        Formula num = new Formula("3 + 5");
        Assert.AreEqual("3+5", num.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that a formula containing both
    ///   numbers and operators is returned as a string with
    ///   no spaces, maintaining the correct order of operations.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestNumberAndOperator_Valid()
    {
        Formula num = new Formula("3 + 5 - 2");
        Assert.AreEqual("3+5-2", num.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that a formula with parentheses is returned 
    ///   as a string in a canonical form with no spaces.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestParenthesis_Valid()
    {
        Formula num = new Formula("(3 + 5) * 2");
        Assert.AreEqual("(3+5)*2", num.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that a formula with lowercase variable names 
    ///   is returned with variables normalized to uppercase.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestVariableLowerCase_Valid()
    {
        Formula variable = new Formula("x1 - y24");
        Assert.AreEqual("X1-Y24", variable.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that a formula with a mix of uppercase and lowercase 
    ///   variable names is normalized to uppercase in the string representation.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestVariableMixedCase_Valid()
    {
        Formula variable = new Formula("x1 * Y23");
        Assert.AreEqual("X1*Y23", variable.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that a formula containing decimal
    ///   numbers is represented correctly in the string, preserving decimal points.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestDecimal_Valid()
    {
        Formula variable = new Formula("3.14 + 2.71");
        Assert.AreEqual("3.14+2.71", variable.ToString());
    }

    /// <summary>
    ///   <para>
    ///     This test ensures that a formula with an integer
    ///     decimal is reduced to its canonical integer form.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestDecimalInteger_Valid()
    {
        Formula variable = new Formula("30.00");
        Assert.AreEqual("30", variable.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that a formula using scientific notation 
    ///   (with lowercase 'e') is properly expanded and normalized.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestScientificNotationLowerCase_Valid()
    {
        Formula variable = new Formula("4e2* 9");
        Assert.AreEqual("400*9", variable.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that a formula using scientific notation 
    ///   (with uppercase 'E') is properly expanded and normalized.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestScientificNotationUpperCase_Valid()
    {
        Formula variable = new Formula("9E3 +3");
        Assert.AreEqual("9000+3", variable.ToString());
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that a complex formula with variables, numbers, 
    ///   and operations is correctly represented in canonical form, 
    ///   with no spaces and variables in uppercase.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void ToString_TestComplexFormula_Valid()
    {
        Formula variable = new Formula("ga22*a1 + 4E3- 22.0 / Ga22");
        Assert.AreEqual("GA22*A1+4000-22/GA22", variable.ToString());
    }

    // Tests for GetVariables() method

    /// <summary>
    ///   <para>
    ///   This test checks that a formula with a single variable returns 
    ///   the correct variable in uppercase in the set of variables.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_SingleVariable_ReturnsCorrectVariable()
    {
        var formula = new Formula("x1 + 2");
        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.Contains("X1"));
        Assert.AreEqual(1, variables.Count);
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that a formula with multiple variables returns 
    ///   a set containing all unique variables in uppercase.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_MultipleVariables_ReturnsUniqueVariables()
    {
        var formula = new Formula("x1 + y1 * z1");
        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.Contains("X1"));
        Assert.IsTrue(variables.Contains("Y1"));
        Assert.IsTrue(variables.Contains("Z1"));
        Assert.AreEqual(3, variables.Count);
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that a formula containing both variables and numbers 
    ///   returns the correct set of variables in uppercase.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_VariablesAndNumbers_ReturnsCorrectVariables()
    {
        var formula = new Formula("x1 + 5.0 - y2");
        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.Contains("X1"));
        Assert.IsTrue(variables.Contains("Y2"));
        Assert.AreEqual(2, variables.Count);
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that a formula with no variables returns 
    ///   an empty set of variables.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_FormulaWithNoVariables_ReturnsEmptySet()
    {
        var formula = new Formula("3 + 4 * 5");
        ISet<string> variables = formula.GetVariables();

        Assert.AreEqual(0, variables.Count);
    }

    /// <summary>
    ///   <para>
    ///   This test checks that a formula with variables in different cases 
    ///   returns all variables normalized to uppercase.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_VariablesWithDifferentCases_ReturnsUpperCaseVariables()
    {
        var formula = new Formula("a1 + B2 + c3");
        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.Contains("A1"));
        Assert.IsTrue(variables.Contains("B2"));
        Assert.IsTrue(variables.Contains("C3"));
        Assert.AreEqual(3, variables.Count);
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that a complex formula with multiple variables 
    ///   returns the correct set of unique variables in uppercase.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void GetVariables_FormulaComplex_ReturnsCorrectly()
    {
        var formula = new Formula("ga22* a1 + 4E3- 22.0 / Ga22");
        ISet<string> variables = formula.GetVariables();

        Assert.IsTrue(variables.Contains("GA22"));
        Assert.IsTrue(variables.Contains("A1"));
        Assert.AreEqual(2, variables.Count);
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that an invalid formula (e.g., one containing 
    ///   invalid tokens) throws a FormulaFormatException when 
    ///   attempting to retrieve variables.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void GetVariables_FormulaWithInvalidTokens_Throws()
    {
        var formula = new Formula("# + invalid");
        _ = formula.GetVariables();
    }

    // Tests for Evaluate() method

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with a single number 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Evaluate_SingleNumber_ReturnsCorrectValue()
    {
        Formula formula = new Formula("5");
        double expectedValue = 5;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with a single variable 
    ///   returns the correct value using the lookup function.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_SingleVariable_ReturnsCorrectValue()
    {
        Formula formula = new Formula("X5");
        double expectedValue = 13;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 13));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with an addition operation 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_Addition_ReturnsCorrectValue()
    {
        Formula formula = new Formula("5 + 3");
        double expectedValue = 8;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with a subtraction operation 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_Subtraction_ReturnsCorrectValue()
    {
        Formula formula = new Formula("18 - 10");
        double expectedValue = 8;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with a multiplication operation 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_Multiplication_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 * 4");
        double expectedValue = 8;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with a division operation 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_Division_ReturnsCorrectValue()
    {
        Formula formula = new Formula("16 / 2");
        double expectedValue = 8;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with both arithmetic operations 
    ///   and variables returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_ArithmeticWithVariable_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 + X1");
        double expectedValue = 6;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 4));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula follows left-to-right precedence 
    ///   when no parentheses are involved.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_LeftToRight_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 * 6 + 3");
        double expectedValue = 15;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula respects the order of operations 
    ///   (multiplication before addition).
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_OrderOfOperations_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 + 6 * 3");
        double expectedValue = 20;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with parentheses prioritizes 
    ///   the operations inside parentheses before other operations.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_ParenthesesTimes_ReturnsCorrectValue()
    {
        Formula formula = new Formula("(2 + 6) * 3");
        double expectedValue = 24;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with multiplication and parentheses 
    ///   gives precedence to the operations inside the parentheses.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_TimesParentheses_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 * (3 + 5)");
        double expectedValue = 16;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with addition and parentheses 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_PlusParentheses_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 + (3 + 5)");
        double expectedValue = 10;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a more complex formula with both addition 
    ///   and multiplication inside parentheses returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_PlusComplex_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 + (3 + 5 * 9)");
        double expectedValue = 50;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a complex formula with both addition, 
    ///   multiplication, and parentheses returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_ComplexTimesParentheses_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 + 3 * (3 + 5)");
        double expectedValue = 26;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a complex formula with deeply nested 
    ///   parentheses and multiple operations returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_ComplexAndParentheses_ReturnsCorrectValue()
    {
        Formula formula = new Formula("2 + 3 * 5 + (3 + 4 * 8) * 5 + 2");
        double expectedValue = 194;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with right-nested parentheses 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_ComplexNestedParensRight_ReturnsCorrectValue()
    {
        Formula formula = new Formula("X1 + (X2 + (X3 + (X4 + (X5 + X6))))");
        double expectedValue = 6;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 1));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with left-nested parentheses 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_ComplexNestedParensLeft_ReturnsCorrectValue()
    {
        Formula formula = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
        double expectedValue = 12;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 2));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with repeated variables 
    ///   returns the correct value.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_RepeatedVariable_ReturnsCorrectValue()
    {
        Formula formula = new Formula("a4-a4*a4/a4");
        double expectedValue = 0;
        Assert.AreEqual(expectedValue, formula.Evaluate(s => 3));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with an unknown variable 
    ///   returns a FormulaError.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_UnknownVariable_ReturnsFormulaError()
    {
        Formula formula = new Formula("2 + x1");
        object error = formula.Evaluate(s => { throw new ArgumentException("Unknown variable"); });
        Assert.IsInstanceOfType(error, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with a division by zero operation 
    ///   returns a FormulaError.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_DivideByZero_ReturnsFormulaError()
    {
        Formula formula = new Formula("5 / 0");
        object error = formula.Evaluate(s => 0);
        Assert.IsInstanceOfType(error, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a formula with a computed value that results 
    ///   in a division by zero returns a FormulaError.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_DivideByZeroComputedValue_ReturnsFormulaError()
    {
        Formula formula = new Formula("5 / (5 - 5)");
        object error = formula.Evaluate(s => 0);
        Assert.IsInstanceOfType(error, typeof(FormulaError));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that evaluating a simple arithmetic formula returns the correct result.
    ///   Specifically, it checks that the formula "5.6 - 3.6" evaluates to 2.0.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEvaluate_DoubleSubtraction_ReturnsCorrectValue()
    {
        Formula formula = new Formula("5.6 - 3.6");
        Assert.AreEqual(2.0, (double)formula.Evaluate(s => 0), 1e-9);
    }

    // Tests for operator == method

    /// <summary>
    ///   <para>
    ///   This test verifies that the == operator returns true for two formulas 
    ///   that are equivalent, and false for the != operator with the same formulas.
    ///   </para>
    /// </summary>
    [TestMethod()]
    public void TestOperatorEquals_EqualFormula_Valid()
    {
        Formula t = new Formula("x1+y1");
        Formula r = new Formula("x1+y1");

        Assert.IsTrue(t == r);

        Assert.IsFalse(t != r);
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the != operator returns true and the == operator 
    ///   returns false for two formulas that are not equal.
    ///   </para>
    /// </summary>
    [TestMethod()]
    public void TestOperatorEquals_NotEqualFormula_Valid()
    {
        Formula t = new Formula("x1+Y1");
        Formula r = new Formula("x1-y1");

        Assert.IsTrue(t != r);

        Assert.IsFalse(t == r);
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the == operator returns true for two formulas 
    ///   with identical variable names, regardless of letter casing.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestOperatorEquals_IdenticalVariableFormulas_ReturnsTrue()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula("a1 + B1");

        Assert.IsTrue(formula1 == formula2);
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the == operator returns false for two different formulas 
    ///   with different variable names or constants.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestOperatorEquals_DifferentFormulas_ReturnsFalse()
    {
        Formula formula1 = new Formula("a1 + B1");
        Formula formula2 = new Formula("A1 + b2");

        Assert.IsFalse(formula1 == formula2);
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the == operator returns true when comparing the same 
    ///   instance of a formula object.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestOperatorEquals_SameInstance_ReturnsTrue()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = formula1; // Same reference

        Assert.IsTrue(formula1 == formula2);
    }

    // Tests for operator != method

    /// <summary>
    ///   <para>
    ///   This test verifies that the != operator returns true for two formulas 
    ///   that are different.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestOperatorNotEquals_DifferentFormulas_ReturnsTrue()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula("A1 + B2");
        Assert.IsTrue(formula1 != formula2); // Different formulas
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the != operator returns false for two formulas 
    ///   that are the same.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestOperatorNotEquals_SameFormulas_ReturnsFalse()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula("A1 + B1");
        Assert.IsFalse(formula1 != formula2); // Same formulas
    }

    // Tests for Equals() method

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns false when comparing a formula 
    ///   with null, as they cannot be equal.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEquals_Null_ReturnsFalse()
    {
        Formula formula = new Formula("A1 + B1");
        Assert.IsFalse(formula.Equals(null)); // Comparing to null
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns false when comparing a formula 
    ///   to an object of a different type, as they are not equal.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEquals_DifferentType_ReturnsFalse()
    {
        Formula formula = new Formula("A1 + B1");
        object obj = new object();
        Assert.IsFalse(formula.Equals(obj)); // Comparing to a different type
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns true when comparing two 
    ///   formulas that are the same.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEquals_SameFormula_ReturnsTrue()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula("A1 + B1");
        Assert.IsTrue(formula1.Equals(formula2)); // Same formulas
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns false when comparing two 
    ///   different formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEquals_DifferentFormulas_ReturnsFalse()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula("A1 + B2");
        Assert.IsFalse(formula1.Equals(formula2)); // Different formulas
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns true when comparing 
    ///   the same formula instance.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEquals_SameInstance_ReturnsTrue()
    {
        Formula formula = new Formula("A1 + B1");
        Assert.IsTrue(formula.Equals(formula)); // Same instance
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns true even if the formulas 
    ///   have variables in different letter cases.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestEquals_DifferentCaseFormulas_ReturnsTrue()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula("a1 + b1"); // Different case
        Assert.IsTrue(formula1.Equals(formula2));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns false when comparing a formula 
    ///   to a string object.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_DifferentType_ReturnsFalse()
    {
        Formula formula1 = new Formula("A1 + B1");
        object obj = "A1 + B1"; // String type

        Assert.IsFalse(formula1.Equals(obj));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns true when comparing two formulas 
    ///   with the same values but different amounts of whitespace.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void Equals_SameFormulaDifferentWhitespace_ReturnsTrue()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula(" A1 + B1 "); // Extra spaces

        Assert.IsTrue(formula1.Equals(formula2));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns true for two formulas that 
    ///   represent the same values but with different levels of numeric precision.
    ///   </para>
    /// </summary>
    [TestMethod()]
    public void TestEquals_SameValueDifferentPrecision_ReturnsTrue()
    {
        Formula t = new Formula("2.0 + x7");
        Formula r = new Formula("2.0000 + x7");

        Assert.IsTrue(t.Equals(r));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns false when comparing a formula 
    ///   to an object of a different type (in this case, an object).
    ///   </para>
    /// </summary>
    [TestMethod()]
    public void TestEquals_DifferentTypePrecision_ReturnsFalse()
    {
        Formula t = new Formula("2.0 + x7");
        object r = new object();

        Assert.IsFalse(t.Equals(r));
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the Equals method returns true for two formulas that 
    ///   use different representations for the same number, such as scientific notation 
    ///   and standard notation.
    ///   </para>
    /// </summary>
    [TestMethod()]
    public void TestEquals_ScientificNotationEquivalent_ReturnsTrue()
    {
        Formula t = new Formula("10e1 + x7");
        Formula r = new Formula("100 + x7");
        Assert.IsTrue(t.Equals(r));
    }

    // Tests for GetHashCode() method

    /// <summary>
    ///   <para>
    ///   This test verifies that two identical formulas return the same hash code, 
    ///   ensuring consistent behavior for equal formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestGetHashCode_IdenticalFormulas_ReturnsSameHash()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula("A1 + B1");
        Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that two different formulas return different hash codes, 
    ///   particularly when the order of operations is different.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestGetHashCode_DifferentFormulas_ReturnsDifferentHash()
    {
        Formula formula1 = new Formula("A1 + B1");
        Formula formula2 = new Formula("B1 + A1"); // Different order, should have different hash
        Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that the hash code for a formula containing a single variable 
    ///   remains consistent across multiple calls.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestGetHashCode_SingleVariable_ReturnsConsistentHash()
    {
        Formula formula = new Formula("A1");
        Assert.AreEqual(formula.GetHashCode(), formula.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that the hash code generated for a formula is consistent with 
    ///   the hash code of its canonical string representation.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestGetHashCode_CanonicalFormHashConsistency()
    {
        Formula formula = new Formula("A1 + B1");
        string canonicalForm = formula.ToString();
        Assert.AreEqual(canonicalForm.GetHashCode(), formula.GetHashCode(), "Hash code should match the hash of the canonical string.");
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that two complex formulas with identical structure return 
    ///   the same hash code, ensuring consistent behavior for equal complex formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestGetHashCode_IdenticalComplexFormulas_ReturnsSameHash()
    {
        Formula formula1 = new Formula("(A1 + B1) * (C1 - D1)");
        Formula formula2 = new Formula("(A1 + B1) * (C1 - D1)");
        Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that two complex formulas with different structures and variable 
    ///   orders return different hash codes, reflecting their inequality.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestGetHashCode_DifferentComplexFormulas_ReturnsDifferentHash()
    {
        Formula formula1 = new Formula("(A1 + B1) * (C1 - D1)");
        Formula formula2 = new Formula("(D1 - C1) * (B1 + A1)"); // Different order and structure
        Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///   This test verifies that formulas with identical structure but different amounts 
    ///   of whitespace return the same hash code, as whitespace should not affect the result.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestGetHashCode_ComplexFormulaWithSpaces_ReturnsSameHash()
    {
        Formula formula1 = new Formula(" ( A1 + B1 ) * ( C1 - D1 ) ");
        Formula formula2 = new Formula("(A1 + B1) * (C1 - D1)"); // Different spacing, but should be equal
        Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///   This test ensures that two formulas with nested parentheses and different variable 
    ///   orders return different hash codes, reflecting their structural differences.
    ///   </para>
    /// </summary>
    [TestMethod]
    public void TestGetHashCode_NestedParentheses_ReturnsSameHash()
    {
        Formula formula1 = new Formula("((A1 + B1) * C1) - D1");
        Formula formula2 = new Formula("((B1 + A1) * C1) - D1"); // Same variables, different order
        Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
    }
}