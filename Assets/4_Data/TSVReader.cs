using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

public static class TSVReader
{
    public static List<T> Read<T>(string fileName) where T : new()
    {
        List<T> list = new List<T>();

        // [변경 1] 확장자를 .tsv로 변경
        string path = Path.Combine(Application.streamingAssetsPath, fileName + ".tsv");

        if (!File.Exists(path))
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {path}");
            return list;
        }

        string fileContent = File.ReadAllText(path);
        string[] lines = Regex.Split(fileContent, @"\r\n|\n|\r");
        
        if (lines.Length <= 1) return list;

        // [변경 2] 헤더를 쉼표가 아닌 탭('\t')으로 분리
        string[] header = lines[0].Split('\t');

        for (int i = 1; i < lines.Length; i++)
        {
            // [변경 3] 데이터도 탭('\t')으로 분리
            string[] values = lines[i].Split('\t');
            
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            T entry = new T();
            
            for (int j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                string fieldName = header[j].Trim();
                FieldInfo field = typeof(T).GetField(fieldName);
                
                if (field != null)
                {
                    try 
                    {
                        object convertedValue = Convert.ChangeType(value, field.FieldType);
                        field.SetValue(entry, convertedValue);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"파싱 에러: {fieldName} / {e.Message}");
                    }
                }
            }
            list.Add(entry);
        }

        return list;
    }
}