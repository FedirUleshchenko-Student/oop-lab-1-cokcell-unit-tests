using oop_lab_1_cokcell.Classes.Cells;
using oop_lab_1_cokcell.Classes.ExpressionNodes;
using oop_lab_1_cokcell.Classes.Parser;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace oop_lab_1_cokcell_unit_tests
{
    internal static class UsefulFuncs
    {
        internal static Expression? ParseExprSafe(string s, TableAction cellReferenceInit, out List<CellId> linkables)
        {
            Expression? ans;
            try
            {
                ans = ExpressionParser.Parse(s, cellReferenceInit, out linkables);
            }
            catch
            {
                ans = null;
                linkables = new List<CellId> { };
            }
            return ans;
        }

        internal static void UpdateAllTestMouses(
            List<Expression?> expressions, List<List<CellId>> linkables,
            List<string> exprString, TableAction? cellReferenceInit = null)
        {

            linkables.Clear();
            expressions.Clear();

            List<CellId> temp_linkables = new List<CellId>();
            foreach (string s in exprString)
            {
                expressions.Add(ParseExprSafe(s,
                    cellReferenceInit == null ? (x => 0) : cellReferenceInit,
                    out temp_linkables));
                linkables.Add(temp_linkables);
            }
        }

        internal static bool isPossibleToCalculate(Expression? x)
        {
            if (x == null) return false;
            try
            {
                x.Calculate();
                return true;
            }
            catch { return false; }
        }

        internal static void CompareToAnswers(List<Expression?> expressions, List<BigInteger?> answers)
        {
            if (answers.Count != expressions.Count)
                throw new Exception("Answers count != Expressions count");

            int i = 0;
            foreach (var e in expressions)
            {
                if (e == null || !isPossibleToCalculate(e))
                    Assert.Null(answers[i]);
                else
                    Assert.Equal(e.Calculate(), answers[i]);
                i++;
            }
        }

        internal static void ResetTable(Table table, uint rows = 5, uint columns = 5)
        {
            table = new Table(rows, columns);
        }

        internal static void SetCellsInTable(Table table, Dictionary<CellId, string> cells)
        {
            foreach (var cell in cells)
            {
                table.PullCell(cell.Key).SetExpression(cell.Value, table);
            }
        }
    }
}
