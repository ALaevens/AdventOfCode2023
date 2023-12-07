using AdventOfCode2023;
using System.Linq;
using System.Text.RegularExpressions;

/*
 * Let d: distance travelled by contestant, t: time pressing button, l: time limit, r record distance
 * 
 * d = t(l - t) <==> d = lt - t^2
 * 
 * solve d = r ==> lt - t^2 = r ==> -(t^2) + lt - r = 0
 * 
 * Use quadratic formula to solve for t:
 * t = (l +/- sqrt(l^2  - 4R)) / 2
 * 
 * ceil lower t, floor upper t
 * 
 */
public class Day6 : BaseDay {
    private long waysToWin(long l, long r) {
        double root = Math.Sqrt(l * l - 4 * r);

        // using +1 and floor instead of ceil removes the case where the intersection is exactly on a whole millisecond (which is only a tie)
        long lowerBound = (long)Math.Floor((l - root) / 2 + 1);
        long upperBound = (long)Math.Ceiling((l + root) / 2 - 1);

        Console.WriteLine($"Range: {lowerBound}, {upperBound}");
        return upperBound - lowerBound + 1;
    }
    public override void Part1(string[] inputLines) {
        Regex rNumbers = new Regex(@"\d+");
        int[] timeLimits = rNumbers.Matches(inputLines[0]).Select(x => Int32.Parse(x.Value)).ToArray();
        int[] distances = rNumbers.Matches(inputLines[1]).Select(x => Int32.Parse(x.Value)).ToArray();

        int total = 1;
        for (int i = 0; i < timeLimits.Length; i++) {
            int l = timeLimits[i];
            int r = distances[i];

            total *= (int)waysToWin(l, r);
        }

        Console.WriteLine(total);
    }

    public override void Part2(string[] inputLines) {
        Regex rNumbers = new Regex(@"\d+");
        long l = Int64.Parse(
            string.Concat(
                rNumbers.Matches(inputLines[0]).Select(x => x.Value).ToArray()
        ));

        long r = Int64.Parse(
            string.Concat(
                rNumbers.Matches(inputLines[1]).Select(x => x.Value).ToArray()
        ));

        Console.WriteLine($"Ways to win: {waysToWin(l, r)}");
    }
}