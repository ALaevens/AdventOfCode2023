using AdventOfCode2023;
using System.Text.RegularExpressions;

public class Day3 : BaseDay {
    public override void Part1(string[] inputLines) {
        inputLines = inputLines.Select(x => x.Trim()).ToArray();

        Regex rNumbers = new Regex(@"\d+");
        int sum = 0;

        for (int i = 0;  i < inputLines.Length; i++) {
            MatchCollection numberMatches = rNumbers.Matches(inputLines[i]);

            foreach (Match match in numberMatches) {
                int left = Math.Max(match.Index - 1, 0);
                int right = Math.Min(match.Index + match.Length + 1, inputLines[i].Length);
                int top = Math.Max(i - 1, 0);
                int bottom = Math.Min(i + 1, inputLines.Length-1);

                Regex rSymbols = new Regex(@"[^0-9.]");
                bool isAdjacent = false;

                for (int j = top; j <= bottom; j++) {
                    string part = inputLines[j].Substring(left, right - left);
                    Match m = rSymbols.Match(part);

                    if (m.Success) {
                        isAdjacent = true;
                        break;
                    }
                }

                if (isAdjacent) {
                    Console.WriteLine($"{match.Value} is adjacent");
                    sum += Int32.Parse(match.Value);
                } else {
                    Console.WriteLine($"{match.Value} is NOT adjacent");
                }
            }
        }

        Console.WriteLine($"SUM: {sum}");
    }

    public override void Part2(string[] inputLines) {
        inputLines = inputLines.Select(x => x.Trim()).ToArray();

        Regex rNumbers = new Regex(@"\d+");
        int sum = 0;

        Dictionary<Tuple<int,int>, List<int>> gears = new Dictionary<Tuple<int, int>, List<int>>();

        for (int i = 0; i < inputLines.Length; i++) {
            MatchCollection numberMatches = rNumbers.Matches(inputLines[i]);

            foreach (Match numberMatch in numberMatches) {
                int left = Math.Max(numberMatch.Index - 1, 0);
                int right = Math.Min(numberMatch.Index + numberMatch.Length + 1, inputLines[i].Length);
                int top = Math.Max(i - 1, 0);
                int bottom = Math.Min(i + 1, inputLines.Length - 1);
                int number = Int32.Parse(numberMatch.Value);

                Regex rGears = new Regex(@"\*");

                for (int j = top; j <= bottom; j++) {
                    string part = inputLines[j].Substring(left, right - left);
                    MatchCollection adjacentGearMatches = rGears.Matches(part);
                    
                    foreach (Match gearMatch in adjacentGearMatches) {
                        Tuple<int, int> pos = new Tuple<int, int>(j, gearMatch.Index + left);
                        Console.WriteLine($"Gear @ [{pos.Item1}, {pos.Item2}] adjacent to {number}");
                        if (gears.ContainsKey(pos)) {
                            gears[pos].Add(number);
                        } else {
                            gears.Add(pos, new List<int>{ number });
                        }
                    }
                }
            }
        }

        foreach (List<int> numberList in gears.Values) {
            if (numberList.Count == 2) {
                sum += (numberList[0] * numberList[1]);
            }
        }

        Console.WriteLine($"SUM: {sum}");
    }
}
