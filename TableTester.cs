using oop_lab_1_cokcell.Classes.Cells;
using oop_lab_1_cokcell.Classes.ExpressionNodes;
using System;
using System.Numerics;
using System.Text;

namespace oop_lab_1_cokcell_unit_tests
{
    public class TableTester
    {
        private List<Expression?> expressions = new List<Expression?>();
        private Table table = new Table(5,5);

        [Fact]
        public void SimpleCellsAsFunctions()
        {
            Dictionary<CellId, string> data = new Dictionary<CellId, string> {
                { new CellId("A1"), "3 + 5" },
                { new CellId("A2"), "3+++4" },
                { new CellId("A3"), "12/3" },
                { new CellId("A4"), "3 + abs(45)" },
                { new CellId("A5"), "nigger" },
            };
            List<BigInteger?> answers = new List<BigInteger?> { 8, 7, 4, 48, null };

            UsefulFuncs.SetCellsInTable(table, data);

            var it = table.GetEnummeratorCells;
            while (it.MoveNext())
            {
                expressions.Add(it.Current.Value.parsedExpression);
            }

            UsefulFuncs.CompareToAnswers(expressions, answers);
        }

        [Fact]
        public void ReferenceCellsAsFunctions()
        {
            Dictionary<CellId, string> data = new Dictionary<CellId, string> {
                { new CellId("A1"), "3" },
                { new CellId("A2"), "7+A1" },
                { new CellId("A3"), "2*A2 + A1" },
                { new CellId("A4"), "mod(A2, A1)" },
                { new CellId("A5"), "nmax(A1, A2, A3, A4)" },
            };
            List<BigInteger?> answers = new List<BigInteger?> { 3,10,23,1,23 };

            UsefulFuncs.SetCellsInTable(table, data);

            var it = table.GetEnummeratorCells;
            while (it.MoveNext())
            {
                expressions.Add(it.Current.Value.parsedExpression);
            }

            UsefulFuncs.CompareToAnswers(expressions, answers);
        }
    }
}
