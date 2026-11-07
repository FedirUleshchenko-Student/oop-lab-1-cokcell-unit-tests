using Microsoft.VisualStudio.TestPlatform.Utilities;
using oop_lab_1_cokcell.Classes.Cells;
using oop_lab_1_cokcell.Classes.ExpressionNodes;
using oop_lab_1_cokcell.Classes.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace oop_lab_1_cokcell_unit_tests
{
    public class SaverAndLoaderTester
    {
        private List<Expression?> expressions = new List<Expression?>();
        private List<List<CellId>> linkables = new List<List<CellId>>();
        private Table table = new Table(5, 5);

        private readonly ITestOutputHelper _output;

        public SaverAndLoaderTester(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void LocalEditAndSave()
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

            string filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "CokCell_Table.json");


            FileManager.SaveToFile(table, filePath);
            // Not working
            //Debug.Print($"Cache table location : '{filePath}'");
            // So:
            _output.WriteLine($"Cache table location : '{filePath}'");

            UsefulFuncs.ResetTable(table);
            table = FileManager.LoadFromFile(filePath);

            var it = table.GetEnummeratorCells;
            while (it.MoveNext())
            {
                expressions.Add(it.Current.Value.parsedExpression);
            }

            UsefulFuncs.CompareToAnswers(expressions, answers);
        }
    }
}
