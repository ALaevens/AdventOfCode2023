using AdventOfCode2023;
using System.Text.RegularExpressions;

public class Day9Iterative : BaseDay {
    private void iterDown(ref List<List<int>> history) {
        bool allZero;
        int depth = 0;

        do {
            allZero = true;
            history.Add(new List<int>());

            for (int i = 0; i < history[depth].Count - 1; i++) {
                int difference = history[depth][i + 1] - history[depth][i];
                history[depth + 1].Add(difference);

                if (difference != 0) {
                    allZero = false;
                }
            }
            depth++;

        } while (!allZero);
    }

    private void expandUp(ref List<List<int>> history) {
        for (int i = history.Count - 2; i >=0; i--) {
            history[i].Add(history[i].Last() + history[i+1].Last());
        }
    }

    public override void Part1(string[] inputLines) {
        Regex rNumbers = new Regex(@"-?\d+");

        long sum = 0;
        foreach (string line in inputLines) {
            List<List<int>> history = new List<List<int>>();
            history.Add(rNumbers.Matches(line).Select(x => Int32.Parse(x.Value)).ToList());

            iterDown(ref history);
            expandUp(ref history);

            sum += history[0].Last();
        }

        Console.WriteLine(sum);
    }

    public override void Part2(string[] inputLines) {
        Regex rNumbers = new Regex(@"-?\d+");

        long sum = 0;
        foreach (string line in inputLines) {
            List<List<int>> history = new List<List<int>>();
            history.Add(rNumbers.Matches(line).Select(x => Int32.Parse(x.Value)).ToList());
            history.Last().Reverse();

            iterDown(ref history);
            expandUp(ref history);

            sum += history[0].Last();
        }

        Console.WriteLine("SUM: " + sum);
    }
}

public class Day9Recursive : BaseDay {
    private int predict(List<int> sequence) {
        if (sequence.All(x => x == 0)) {
            return 0;
        }

        List<int> next = new List<int>();
        for (int i = 0; i < sequence.Count - 1; i++) {
            next.Add(sequence[i+1] - sequence[i]);
        }

        return sequence.Last() + predict(next);
    }

    public override void Part1(string[] inputLines) {
        Regex rNumbers = new Regex(@"-?\d+");

        long sum = 0;
        foreach (string line in inputLines) {
            List<int> sequence = rNumbers.Matches(line).Select(x => Int32.Parse(x.Value)).ToList();
            sum += predict(sequence);
        }

        Console.WriteLine($"SUM: {sum}");
    }

    public override void Part2(string[] inputLines) {
        Regex rNumbers = new Regex(@"-?\d+");

        long sum = 0;
        foreach (string line in inputLines) {
            List<int> sequence = rNumbers.Matches(line).Select(x => Int32.Parse(x.Value)).ToList();
            sequence.Reverse();
            sum += predict(sequence);
        }

        Console.WriteLine($"SUM: {sum}");
    }
}