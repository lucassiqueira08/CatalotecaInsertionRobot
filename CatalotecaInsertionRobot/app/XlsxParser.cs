using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Parsers
{
  /// <summary>
  /// Object used as template for Spreadsheet Header
  /// </summary>
  internal class Header
  {
    internal string Name { get; set; }
    internal int Position { get; set; }
  }


  public class XlsxParser
  {
    private string _filePath;
    private Stream _fileStream;
    public XlsxParser(string filePath) => _filePath = filePath;
    public XlsxParser(Stream fileStream) => _fileStream = fileStream;


    /// <summary>
    /// Function responsible for parseing the spreadsheet
    /// </summary>
    /// <returns>A list of dictionaries representing each row, key equals header and value equals column value.</returns>
    public List<IDictionary<string, string>> Parse()
    {
      if (_filePath != null)
        return ReadFromFilePath(_filePath);

      return ReadFromStream(_fileStream);
    }

    private List<IDictionary<string, string>> ReadFromFilePath(string filePath)
    {
      using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
      {
        return ParseExcelPackage(package);
      }

    }

    private List<IDictionary<string, string>> ReadFromStream(Stream fileStream)
    {
      using (ExcelPackage package = new ExcelPackage(fileStream))
      {
        return ParseExcelPackage(package);
      }

    }

    private List<IDictionary<string, string>> ParseExcelPackage(ExcelPackage package)
    {
      ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
      int rows = worksheet.Dimension.End.Row;
      int cols = worksheet.Dimension.End.Column;

      List<IDictionary<string, string>> result = new List<IDictionary<string, string>>();
      List<Header> headers = new List<Header>();

      for (int row = 1; row <= rows; row++)
      {
        if (row == 1)
        {
          headers = GetHeadersList(cols, row, worksheet);
        }
        else
        {
          var register = GetRegister(row, headers, worksheet);
          if (register == null || register.Count == 0) break;

          result.Add(register);
        }
      }
      return result;
    }

    /// <summary>
    /// Return register from worksheet row
    /// </summary>
    /// <param name="row"></param>
    /// <param name="headers"></param>
    /// <param name="worksheet"></param>
    /// <returns></returns>
    private Dictionary<string, string> GetRegister(int row, List<Header> headers, ExcelWorksheet worksheet)
    {
      var colDict = new Dictionary<string, string>();
      int colPosition;
      string colHeader;
      string colValue;
      for (int col = 1; col <= headers.Count; col++)
      {
        try
        {
          colPosition = headers[col - 1].Position;
          colHeader = headers[col - 1].Name;
          var valueCell = worksheet.Cells[row, colPosition];
          if (valueCell == null || valueCell.Value == null)
            continue;

          colValue = valueCell.Value.ToString();
          colDict.Add(colHeader, colValue);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.StackTrace);
          Console.WriteLine(ex.Message);

          throw;
        }
      }
      return colDict;
    }

    /// <summary>
    /// Function responsible for returning a list of all headers present in the spreadsheet.
    /// </summary>
    /// <param name="cols"></param>
    /// <param name="row"></param>
    /// <param name="worksheet"></param>
    /// <returns>Returns a list of all headers present in the spreadsheet.</returns>
    private List<Header> GetHeadersList(int cols, int row, ExcelWorksheet worksheet)
    {
      List<Header> headers = new List<Header>();
      string name;
      int pos;

      for (int col = 1; col <= cols; col++)
      {
        try
        {
          var headerCell = worksheet.Cells[row, col];
          if (headerCell != null && headerCell.Value != null)
          {
            name = headerCell.Value.ToString();
            pos = worksheet.Column(col).ColumnMax;

            if (name.Length > 0)
            {
              var header = new Header()
              {
                Name = name,
                Position = pos
              };
              headers.Add(header);
            }
          }
        }
        catch (System.Exception)
        {
          continue;
        }
      }
      return headers;
    }

  }
}
