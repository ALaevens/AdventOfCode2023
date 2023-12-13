using AdventOfCode2023;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

internal class Day12 : BaseDay {
    public struct MemoArgs : IEquatable<MemoArgs> {
        public MemoArgs(string pattern, List<int> targetCounts) {
            this.pattern = pattern;
            this.targetCounts = targetCounts;
        }

        public string pattern;
        List<int> targetCounts;

        bool IEquatable<MemoArgs>.Equals(MemoArgs other) {
            return this.pattern == other.pattern && this.targetCounts.SequenceEqual(other.targetCounts);
        }
    }

    private long countPerms(string pattern, List<int> targetCounts) {
        MemoArgs memoArgs = new MemoArgs(pattern, targetCounts);
        if (memory.ContainsKey(memoArgs)) {
            return memory[memoArgs];
        }

        if (targetCounts.Count == 0 && pattern.Count(x => x == '#') == 0) {
            memory[memoArgs] = 1;
            return 1;
        } else if (targetCounts.Count == 0) {
            memory[memoArgs] = 0;
            return 0;
        }

        Regex r = new Regex(@"(?=((?<=[^#]|^)[#?]{" + targetCounts[0] + @"}(?=[^#]|$)))");

        long sum = 0;
        foreach (Match m in r.Matches(pattern)) {
            Group area = m.Groups[1];

            string processed = $"{pattern.Substring(0, area.Index).Replace('?', '.')}{new string('#', area.Length)}";
            string next = "";
            if (area.Index + area.Length + 1 < pattern.Length)
                next = pattern.Substring(area.Index + area.Length + 1);

            if (processed.Split('.', StringSplitOptions.RemoveEmptyEntries).Length > 1)
                break;

            sum += countPerms(next, targetCounts.Slice(1, targetCounts.Count-1));

        }
        memory[memoArgs] = sum;
        return sum;
    }

    public override void Part1(string[] inputLines) {
        Regex r = new Regex(@"([?#\.]+) (\d+(,\d+)+)");

        long sum = 0;
        foreach (string line in inputLines) {
            Match m = r.Match(line.Trim());

            string pattern = m.Groups[1].Value;
            List<int> counts = m.Groups[2].Value.Split(',').Select(x => Int32.Parse(x)).ToList();

            memory = new Dictionary<MemoArgs, long>();
            sum += countPerms(pattern, counts);
        }

        Console.WriteLine("SUM: " + sum);
    }

    public override void Part2(string[] inputLines) {
        Regex r = new Regex(@"([?#\.]+) (\d+(,\d+)+)");

        long sum = 0;
        int i = 0;
        foreach (string line in inputLines) {
            Match m = r.Match(line.Trim());

            string pattern = string.Join('?', Enumerable.Repeat(m.Groups[1].Value, 5));
            List<int> counts = string.Join(',', Enumerable.Repeat(m.Groups[2].Value, 5)).Split(',').Select(x => Int32.Parse(x)).ToList();

            memory = new Dictionary<MemoArgs, long>();
            long count = countPerms(pattern, counts);
            sum += count;

            Console.WriteLine($"{i+1} / {inputLines.Length}");
            i++;
        }

        Console.WriteLine("SUM: " + sum);
    }
    private Dictionary<MemoArgs, long> memory = new Dictionary<MemoArgs, long>();
}