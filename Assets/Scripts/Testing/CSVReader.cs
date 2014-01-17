using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;

/// <summary>
/// A collection of static functions that return data from .csv files.
/// </summary>
public static class CSVReader
{
    /// <summary>
    /// Reads a CSV file into a 2D list.
    /// The file can't be open in another program or will throw sharing violation.
    /// </summary>
    /// <param name="filename">Path of .csv relative to Resources folder. </param>
    /// <returns>The contents of a .csv as a 2D list ([row][column])</returns>
    public static List<List<String>> ReadFile(string filename)
    {
        List<List<String>> filedata;
        var sr = File.OpenText("Assets/Resources/" + filename);
        filedata = sr.ReadToEnd().Split('\n').Select(s => s.Split(',').ToList()).ToList();
        sr.Close();
        return filedata;
    }
}