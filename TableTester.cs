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
        // 5x5 table
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

        [Fact]
        public void ReferenceCellsRecursionWithUpdate()
        {
            Dictionary<CellId, string> data = new Dictionary<CellId, string> {
                { new CellId("A1"), "A2" },
                { new CellId("A2"), "A3" },
                { new CellId("A3"), "A1" },
            };
            List<BigInteger?> answers = new List<BigInteger?> { null, null, null };

            UsefulFuncs.SetCellsInTable(table, data);

            var it = table.GetEnummeratorCells;
            while (it.MoveNext())
            {
                expressions.Add(it.Current.Value.parsedExpression);
            }

            UsefulFuncs.CompareToAnswers(expressions, answers);


            answers = new List<BigInteger?> { 5, 5, 5 };

            table.PullCellSafe(new CellId("A2"))?.SetExpression("5", table);

            expressions.Clear();
            it = table.GetEnummeratorCells;
            while (it.MoveNext())
                expressions.Add(it.Current.Value.parsedExpression);

            UsefulFuncs.CompareToAnswers(expressions, answers);
        }

        private int GetCountOfExistingCells()
        {
            var it = table.GetEnummeratorCells;
            int ans = 0;
            while(it.MoveNext()) ans++;
            return ans;
        }

        private void ExpressionsAndAnswersUpdate(Dictionary<CellId, BigInteger?> answerData, List<BigInteger?> answers)
        {
            if(answers.Count != 0) answers.Clear();
            expressions.Clear();
            if (answerData.Count == GetCountOfExistingCells())
            {
                var it = table.GetEnummeratorCells;
                while (it.MoveNext())
                {
                    expressions.Add(it.Current.Value.parsedExpression);
                    answers.Add(answerData[it.Current.Value.Id]);
                }
            }
            else
            {
                foreach(var x in answerData)
                {
                    expressions.Add(table.PullCellSafe(x.Key)?.parsedExpression);
                    answers.Add(x.Value);
                }
            }
        }

        [Fact]
        public void SimpleSizeTest()
        {
            Dictionary<CellId, string> data = new Dictionary<CellId, string> {
                { new CellId("A5"), "5" },
                { new CellId("A4"), "4" },
            };
            Dictionary<CellId, BigInteger?> answerData = new Dictionary<CellId, BigInteger?> {
                { new CellId("A5"), 5 },
                { new CellId("A4"), 4 },
            };
            List<BigInteger?> answers = new List<BigInteger?>();

            UsefulFuncs.SetCellsInTable(table, data);
            ExpressionsAndAnswersUpdate(answerData, answers);
            UsefulFuncs.CompareToAnswers(expressions, answers);



            table.RemoveRow(table.Rows);
            answerData = new Dictionary<CellId, BigInteger?> {
                { new CellId("A5"), null },
                { new CellId("A4"), 4 },
            };
            ExpressionsAndAnswersUpdate(answerData, answers);
            Assert.Equal((uint)4, table.Rows);
            Assert.Equal((uint)5, table.Columns);
            UsefulFuncs.CompareToAnswers(expressions, answers);
        }

        [Fact]
        public void ReferenceToDeletedAndReturnedPosition()
        {
            Dictionary<CellId, string> data = new Dictionary<CellId, string> {
                { new CellId("A5"), "5" },
                { new CellId("A4"), "A5" },
            };
            Dictionary<CellId, BigInteger?> answerData = new Dictionary<CellId, BigInteger?> {
                { new CellId("A5"), 5 },
                { new CellId("A4"), 5 },
            };
            List<BigInteger?> answers = new List<BigInteger?>();

            UsefulFuncs.SetCellsInTable(table, data);
            ExpressionsAndAnswersUpdate(answerData, answers);
            UsefulFuncs.CompareToAnswers(expressions, answers);

            table.RemoveRow(table.Rows);
            answerData = new Dictionary<CellId, BigInteger?> {
                { new CellId("A5"), null },
                { new CellId("A4"), null },
            };
            ExpressionsAndAnswersUpdate(answerData, answers);
            Assert.Equal((uint)4, table.Rows);
            Assert.Equal((uint)5, table.Columns);
            UsefulFuncs.CompareToAnswers(expressions, answers);

            table.AddRow(table.Rows);
            answerData = new Dictionary<CellId, BigInteger?> {
                { new CellId("A5"), null },
                { new CellId("A4"), 0 },
            };
            ExpressionsAndAnswersUpdate(answerData, answers);
            Assert.Equal((uint)5, table.Rows);
            Assert.Equal((uint)5, table.Columns);
            UsefulFuncs.CompareToAnswers(expressions, answers);
        }
    }
}
