using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using ViewAnalysis.Enums;
using ViewAnalysis.Models;

namespace ViewAnalysis.Utilities
{
    internal static class ExportHelper
    {
        public static ExportResult Export(string fileName, IReadOnlyCollection<object> filteredObjects)
        {
            var exportResult = new ExportResult();

            try
            {
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        using (var worksheet = package.Workbook.Worksheets.Add("First Sheet"))
                        {
                            // Header Columns
                            worksheet.Cells[1, 1].Value = "Name";
                            worksheet.Cells[1, 2].Value = "Rule";
                            worksheet.Cells[1, 3].Value = "Resolution";
                            worksheet.Cells[1, 4].Value = "Fix Category";
                            worksheet.Cells[1, 5].Value = "Check Id";
                            worksheet.Cells[1, 6].Value = "Microsoft URL";
                            worksheet.Cells[1, 7].Value = "File";

                            var rowIndex = 2;

                            // Row Columns
                            foreach (var filteredObject in filteredObjects)
                            {
                                var issueModel = filteredObject as IssueModel;

                                worksheet.Cells[rowIndex, 1].Value = issueModel.Name;
                                worksheet.Cells[rowIndex, 2].Value = issueModel.MessageModel.Rule.Name;
                                worksheet.Cells[rowIndex, 3].Value = issueModel.Text;
                                worksheet.Cells[rowIndex, 4].Value = Enum.Parse(typeof(FixCategories), issueModel.FixCategory);
                                worksheet.Cells[rowIndex, 5].Value = issueModel.MessageModel.CheckId;
                                worksheet.Cells[rowIndex, 6].Value = issueModel.MessageModel.Rule.Url;

                                if (!string.IsNullOrWhiteSpace(issueModel.File))
                                {
                                    worksheet.Cells[rowIndex, 7].Value = $"file:///{(issueModel.Path + @"\" + issueModel.File).Replace("\\", "/").Replace(' ', (char)160)}";
                                }

                                ++rowIndex;
                            }

                            worksheet.Cells[1, 1, rowIndex, 1].AutoFitColumns();
                            worksheet.Cells[1, 2, rowIndex, 2].AutoFitColumns();
                            worksheet.Cells[1, 4, rowIndex, 4].AutoFitColumns();

                            worksheet.View.FreezePanes(2, 4);

                            worksheet.Cells[1, 1, rowIndex, 4].AutoFilter = true;

                            package.SaveAs(stream);
                        }
                    }
                }

                exportResult.Successful = true;
            }
            catch (Exception ex)
            {
                exportResult.Exception = ex;
                exportResult.Successful = false;
            }

            return exportResult;
        }
    }
}