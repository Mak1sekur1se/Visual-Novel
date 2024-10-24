using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManagers 
{
    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
    {
        if (!filePath.StartsWith('/'))
            //相对路径改为绝对路径
            filePath = FilePaths.root + filePath;

        var lines = new List<string>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if(includeBlankLines || !string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError($"File not found: '{ e.FileName}'");
        }
        return lines;
    }

    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if (asset == null)
        {
            Debug.LogError($"Asset not found: '{filePath}'");
            return null;
        }

        return ReadTextAsset(asset, includeBlankLines);
    }

    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(asset.text))
        {
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if(includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    lines.Add(line);

            }
        }

        return lines;
    }
}
