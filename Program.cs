﻿using AdventOfCode2023;
using System.Text.RegularExpressions;
class Program {
    static void Main(string[] args) {
        BaseDay problem = new Day17();
        string inputFile = @"D:\Libraries\Documents\Code\C#\AdventOfCode2023\input\day17.txt";

        if (!File.Exists(path: inputFile)) {
            Console.WriteLine("File not found.");
            return;
        }

        string[] lines = File.ReadAllLines(inputFile);

        problem.Part1(lines);
    }
}