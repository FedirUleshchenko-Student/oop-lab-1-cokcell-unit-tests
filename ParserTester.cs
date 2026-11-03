using oop_lab_1_cokcell.Classes.Cells;
using oop_lab_1_cokcell.Classes.ExpressionNodes;
using oop_lab_1_cokcell.Classes.Parser;
using System.Collections.Generic;
using System.Numerics;

namespace oop_lab_1_cokcell_unit_tests
{
    public class ParserTester
    {
        private List<Expression?> expressions = new List<Expression?>();
        private List<List<CellId>> linkables = new List<List<CellId>>();

        [Fact]
        public void NullOrErrorExceptions()
        {
            UsefulFuncs.UpdateAllTestMouses(expressions, linkables,
                new List<string> { "1   2", " /", "!", "bigger", "2 / fef", "KEK", "CHEBUREK", "(()", "(", "(()())" });



            foreach (var l in linkables)
                Assert.Empty(l);


            foreach (var e in expressions)
                Assert.True(e == null || !UsefulFuncs.isPossibleToCalculate(e));
        }

        [Fact]
        public void Numbers()
        {
            List<string> data = new List<string> { "1", "155555", "2626262", "45321849",
            "100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" };

            UsefulFuncs.UpdateAllTestMouses(expressions, linkables ,data);

            foreach (var l in linkables)
                Assert.Empty(l);

            int i = 0;
            foreach (var e in expressions)
            {
                Assert.Equal(e?.Calculate(), BigInteger.Parse(data[i]));
                i++;
            }
        }



        [Fact]
        public void SimpleDimpleOperations()
        {
            List<string> data = new List<string> { "1 + 2", "3 + 5", "3 + 4 - 5", "4 * 4", "1323 - 33 * 44", "5 / 2" };
            List<BigInteger?> answers = new List<BigInteger?> { 3, 8, 2, 16, -129, 2 };

            UsefulFuncs.UpdateAllTestMouses(expressions, linkables, data);

            foreach (var l in linkables)
                Assert.Empty(l);

            UsefulFuncs.CompareToAnswers(expressions, answers);
        }

        [Fact]
        public void ParenthesesArgsOperations()
        {
            List<string> data = new List<string> { "abs(3-5)", "nmax(1, 2, 45, -1487)", "nmin(-14)", "mod(5, 2)", "div(4, 2)", "(((3)))" };
            List<BigInteger?> answers = new List<BigInteger?> { 2, 45, -14, 1, 2, 3 };

            UsefulFuncs.UpdateAllTestMouses(expressions, linkables, data);

            foreach (var l in linkables)
                Assert.Empty(l);

            UsefulFuncs.CompareToAnswers(expressions, answers);
        }

        [Fact]
        public void ReferenceOperations()
        {
            //Table table = new Table();
            //List<string> data = new List<string> { "A1", "A3", "A4" };
            //List<BigInteger?> answers = new List<BigInteger?> { 2, 45, -14, 1, 2 };

            //UpdateAllTestMouses(data);

            //parsedExpression = ExpressionParser.Parse(expression, (id) =>
            //{
            //    var c = currTable.FindCell(id);
            //    if (c == null)
            //        //throw new Exception($"Cannot locate cell: {CellId.getStringSafe(id)}");
            //        throw new Exception($"Неможливо знайти клітинку: {CellId.getStringSafe(id)}");
            //    return c.GetValue(true);
            //}, out foundation);

            //foreach (var l in linkables)
            //    Assert.Empty(l);

            //CompareToAnswers(answers);
        }
    }
}
