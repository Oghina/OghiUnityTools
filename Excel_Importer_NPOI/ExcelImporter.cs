using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

namespace OghiUnityTools.Excel_Importer_NPOI
{
    public class ExcelImporter<T> where T : ScriptableObject
    {
        private string excelFilePath = "Assets/ExcelImporter/ItemDetails.xlsx";
        private string outputPath = "Assets/ExcelImporter/ScriptableObjects";
        
        private List<Dictionary<string, object>> ImportedItemsData { get; } = new();
        private List<T> ImportedItems { get; } = new();
        
        public void SetExcelFilePath(string excelFilePath) => this.excelFilePath = excelFilePath;
        
        public void SetOutputPath(string outputPath) => this.outputPath = outputPath;
        
        public List<T> ImportItemsFromExcel(ScriptableObject scriptableObjectTemplate)
        {
            if (!File.Exists(excelFilePath))
            {
                Debug.LogError($"Excel file not found at path: {excelFilePath}");
                return null;
            }

            if (!Directory.Exists(outputPath))
            {
                Debug.Log("Output directory for excel importer not found .. creating one");
                Directory.CreateDirectory(outputPath);
            }

            IWorkbook workbook;
            using (FileStream file = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(file);
            }

            ISheet sheet = workbook.GetSheetAt(0);

            // Read header
            IRow headerRow = sheet.GetRow(0);
            List<string> headers = new List<string>();
            for (int col = 0; col < headerRow.LastCellNum; col++)
            {
                headers.Add(headerRow.GetCell(col).StringCellValue);
            }

            // Read data rows
            for (int row = 1; row <= sheet.LastRowNum; row++) // assuming the first row is the header
            {
                IRow excelRow = sheet.GetRow(row);
                if (excelRow == null) continue;

                Dictionary<string, object> fields = new Dictionary<string, object>();
                for (int col = 0; col < headers.Count; col++)
                {
                    ICell cell = excelRow.GetCell(col);
                    if (cell == null) continue;

                    object value;
                    switch (cell.CellType)
                    {
                        case CellType.String:
                            value = cell.StringCellValue;
                            break;
                        case CellType.Numeric:
                            value = cell.NumericCellValue;
                            break;
                        case CellType.Boolean:
                            value = cell.BooleanCellValue;
                            break;
                        default:
                            value = cell.ToString();
                            break;
                    }

                    fields.Add(headers[col], value);
                }

                ImportedItemsData.Add(fields);
                var item = CreateScriptableObject(fields, scriptableObjectTemplate, row);
                ImportedItems.Add(item);
            }

            AssetDatabase.SaveAssets();

            Debug.Log($"{ImportedItems.Count} ItemDetails assets created successfully.");
            return ImportedItems;
        }

        private T CreateScriptableObject(Dictionary<string, object> fields, ScriptableObject scriptableObjectTemplate, int index)
        {
            string itemName = fields.TryGetValue("itemName", out var field1) ? $"{field1.ToString()}_{index}" : $"NewItem_{index}";
            string assetPath = Path.Combine(outputPath, $"{itemName}_{index}.asset");

            var itemDetails = ScriptableObject.CreateInstance(scriptableObjectTemplate.GetType());

            // Populate fields using reflection
            FieldInfo[] templateFields = scriptableObjectTemplate.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in templateFields)
            {
                if (fields.ContainsKey(field.Name))
                {
                    field.SetValue(itemDetails, fields[field.Name]);
                }
            }

            AssetDatabase.CreateAsset(itemDetails, assetPath);

            return itemDetails as T;
        }
    }
}