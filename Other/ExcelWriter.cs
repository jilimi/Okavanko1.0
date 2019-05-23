using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Text.RegularExpressions;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Utils;
/// <summary>
/// 该类用于型材，辅材，线材下料数据的写入以及csv数据生成Excel文件
/// </summary>
namespace CSCECDEC.Plugin.Util
{

    public static class ExcelWriter
    {
        static ExcelWriter() { }
        //写一行线材数据
        /*
        public static void WriteHeaderValue(List<string> DataInRows, int RowNum, ref ExcelWorksheet Sheet)
        {
            Sheet.Row(RowNum).Height = 20;
            Sheet.Row(RowNum).Style.Font.Bold = true;

            for (int Index = 0; Index < DataInRows.Count; Index++)
            {
                Sheet.Cells[RowNum, Index+1].Value = DataInRows[Index];
            }
        }*/
        public static void WriteBodyValue(List<string> DataInRows, ref ExcelWorksheet Sheet)
        {
           
            string Pattern = @",|，|；|;";
            for (int Index = 0; Index < DataInRows.Count; Index++)
            {
                string[] Str = Regex.Split(DataInRows[Index], Pattern);
                Sheet.Row(Index+1).Height = 21;
                for (int Index2 = 0; Index2 < Str.Length; Index2++)
                {
                    string tempContent = Str[Index2];
                    double Result;
                    //判断写入的值类型，如果是数字那么设定数字的格式
                    //其他的都以字符串的形式写入
                    bool IsSuccess = Double.TryParse(tempContent, out Result);
                    if (IsSuccess)
                    {
                        Sheet.Cells[Index+1, Index2 + 1].Value = Result;
                    }
                    else
                    {
                        Sheet.Cells[Index + 1, Index2 + 1].Value = Str[Index2];//.ToString();
                    }
                }
            }
            Sheet.Cells[1, 1, 100, 100].AutoFitColumns();
        }
        //obsolete
    }
}
