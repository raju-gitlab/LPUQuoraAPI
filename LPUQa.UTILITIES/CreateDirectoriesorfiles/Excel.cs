using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.UTILITIES.CreateDirectoriesorfiles
{
    public class Excel
    {

        #region CreateStudentAddmisssionDetailsExcelFIle
        public Excel(string FileName, dynamic DataFiels, string FilePath)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (ExcelPackage excel = new ExcelPackage())
                {

                    //Add Worksheets in Excel file
                    excel.Workbook.Worksheets.Add("TestSheet1");

                    //Create Excel file in Uploads folder of your project
                    string ExcelFileName = FilePath + FileName + ".xlsx";
                   /* FileInfo excelFile = new FileInfo(HttpContext.Current.Server.MapPath(ExcelFileName));*/

                    //Add header row columns name in string list array
                    var headerRow = new List<string[]>()
                    {
                        new string[] { "StudentName","Email","Qualification", "Board","TotalNumber","Percentage"}
                    };

                    // Get the header range
                    string Range = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // get the workSheet in which you want to create header
                    var worksheet = excel.Workbook.Worksheets["TestSheet1"];

                    // Popular header row data
                    worksheet.Cells[Range].LoadFromArrays(headerRow);

                    //show header cells with different style
                    worksheet.Cells[Range].Style.Font.Bold = true;
                    worksheet.Cells[Range].Style.Font.Size = 16;
                    worksheet.Cells[Range].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);

                    //Now add some data in rows for each column

                    /*var Data = new List<AddmissionExcelModel>();
                    for (int i = 0; i < DataFiels.QualiFication.Length; i++)
                    {
                        Data.Add(new AddmissionExcelModel
                        {
                            StudentName = DataFiels.FirstName + DataFiels.LastName,
                            Email = DataFiels.Email,
                            Qualification = DataFiels.QualiFication[i],
                            Board = DataFiels.Board[i],
                            TotalNumber = Convert.ToString(Convert.ToInt32(DataFiels.TotalNumber[i])),
                            Percentage = Convert.ToString(float.Parse(DataFiels.Percentage[i].ToString()))
                        });
                    }*/

                    //add the data in worksheet, here .Cells[2,1] 2 is rowNumber while 1 is column number
                   /* worksheet.Cells[2, 1].LoadFromCollection(Data);

                    //Save Excel file
                    excel.SaveAs(excelFile);*/
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region CreateExcelFIleForStoreRecordAboutStudentFessPaidDetails
        public static string CreateFessRecordExcelFile(string FileName, dynamic DataFiels, string FilePath, string CourseName, string SemesterName, string FESSType)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (ExcelPackage excel = new ExcelPackage())
                {

                    //Add Worksheets in Excel file
                    excel.Workbook.Worksheets.Add("TestSheet1");

                    //Create Excel file in Uploads folder of your project
                    string ExcelFileName = FilePath + FileName + ".xlsx";/*
                    FileInfo excelFile = new FileInfo(HttpContext.Current.Server.MapPath(ExcelFileName));*/

                    //Add header row columns name in string list array
                    var headerRow = new List<string[]>()
                    {
                        new string[] { "StudentName","CourseName","SemesterName", "FessType","FessOrFineDepositDate","PaidFessAmount","DueFessAmount","FineAmount", "DueFineAmount","PaidFineAmount" }
                    };

                    // Get the header range
                    string Range = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    // get the workSheet in which you want to create header
                    var worksheet = excel.Workbook.Worksheets["TestSheet1"];

                    // Popular header row data
                    worksheet.Cells[Range].LoadFromArrays(headerRow);

                    //show header cells with different style
                    worksheet.Cells[Range].Style.Font.Bold = true;
                    worksheet.Cells[Range].Style.Font.Size = 16;
                    worksheet.Cells[Range].Style.Font.Color.SetColor(System.Drawing.Color.DarkBlue);

                    #region Comment
                    //Now add some data in rows for each column
                    //var Data = new List<AddmissionExcelModel>();
                    //    Data.Add(new AddmissionExcelModel
                    //    {
                    //        StudentName = DataFiels.FirstName + DataFiels.LastName,
                    //        CourseName = CourseName,
                    //        SemesterName = SemesterName,
                    //        FessType = FESSType,
                    //        FessDepositDate = Convert.ToString(DateTime.Now),
                    //        FessAmount = DataFiels.Fess,
                    //        DueAmount = (1000 - Convert.ToInt32(DataFiels.Fess))
                    //    });

                    //add the data in worksheet, here .Cells[2,1] 2 is rowNumber while 1 is column number 
                    #endregion

                    worksheet.Cells[2, 1].Value = DataFiels.FirstName + DataFiels.LastName;
                    worksheet.Cells[2, 2].Value = CourseName;
                    worksheet.Cells[2, 3].Value = SemesterName;
                    worksheet.Cells[2, 4].Value = FESSType;
                    worksheet.Cells[2, 5].Value = Convert.ToString(DateTime.Now);
                    worksheet.Cells[2, 6].Value = DataFiels.Fess;
                    worksheet.Cells[2, 7].Value = (1000 - Convert.ToInt32(DataFiels.Fess));
                    worksheet.Cells[2, 8].Value = 0;
                    worksheet.Cells[2, 9].Value = 0;
                    worksheet.Cells[2, 10].Value = 0;

                    //Save Excel file
                    /*excel.SaveAs(excelFile);*/
                    return ExcelFileName;
                }
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }
        #endregion
    }
}
