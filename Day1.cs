using AdventOfCode2023;
using System.Text.RegularExpressions;

public class Day1 : BaseDay {
    public override void Part1(string[] inputLines) { }
    public override void Part2(string[] inputLines) {
        int sum = 0;
        Dictionary<string, string> wordTodigit = new Dictionary<string, string>() {
            {"one", "1"}, {"two", "2"}, {"three", "3"}, {"four", "4"}, {"five", "5"}, {"six", "6"}, {"seven", "7"}, {"eight", "8"}, {"nine", "9"}
        };

        foreach (string line in inputLines) {
            MatchCollection matches = Regex.Matches(line, @"(?=(one|two|three|four|five|six|seven|eight|nine|\d))");

            string first = "";
            string last = "";

            first = wordTodigit.TryGetValue(matches.First().Groups[1].Value, out first) ? first : matches.First().Groups[1].Value;
            last = wordTodigit.TryGetValue(matches.Last().Groups[1].Value, out last) ? last : matches.Last().Groups[1].Value;

            Console.WriteLine($"{line}: {first + last}");

            sum += Int32.Parse(first + last);
        }

        Console.WriteLine($"Sum: {sum}");
    }
}
