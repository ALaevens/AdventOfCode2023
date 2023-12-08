using AdventOfCode2023;
using System.Numerics;
using System.Text.RegularExpressions;

public class Day8 : BaseDay {
    private int stepsToSolve(Dictionary<string, Tuple<string, string>> nodes, string route, string start, Regex goalFormat) {
        string currentNode = start;
        int step = 0;

        while (!goalFormat.Match(currentNode).Success) {
            char direction = route[step % route.Length];

            if (direction == 'L') {
                currentNode = nodes[currentNode].Item1;
            } else {
                currentNode = nodes[currentNode].Item2;
            }
            step++;
        }
        return step;
    }

    public override void Part1(string[] inputLines) {
        string route = inputLines[0].Trim();

        Dictionary<string, Tuple<string, string>> nodes = new Dictionary<string, Tuple<string, string>>();
        Regex rNode = new Regex(@"(\w{3}) = \((\w{3}), (\w{3})\)");
        for (int i = 2; i < inputLines.Length; i++) {
            Match m = rNode.Match(inputLines[i]);
            nodes.Add(m.Groups[1].Value, new Tuple<string, string>(m.Groups[2].Value, m.Groups[3].Value));
        }

        Console.WriteLine($"Steps: {stepsToSolve(nodes, route, "AAA", new Regex(@"ZZZ"))}");
    }

    public override void Part2(string[] inputLines) {
        string route = inputLines[0].Trim();
        List<string> startingNodes = new List<string>();

        Dictionary<string, Tuple<string, string>> nodes = new Dictionary<string, Tuple<string, string>>();
        Regex rNode = new Regex(@"(\w{3}) = \((\w{3}), (\w{3})\)");
        for (int i = 2; i < inputLines.Length; i++) {
            Match m = rNode.Match(inputLines[i]);
            nodes.Add(m.Groups[1].Value, new Tuple<string, string>(m.Groups[2].Value, m.Groups[3].Value));
        }

        startingNodes = nodes.Keys.ToList().Where(x => x[2] == 'A').ToList();

        List<BigInteger> individualSteps = new List<BigInteger>();
        foreach (string node in startingNodes) {
            individualSteps.Add(stepsToSolve(nodes, route, node, new Regex(@"\w{2}Z")));
        }

        BigInteger lcm = individualSteps.Aggregate((a, b) => {
            BigInteger gcd = BigInteger.GreatestCommonDivisor(a, b);
            return (a / gcd * b);
        });

        Console.WriteLine(lcm);
    }
}
