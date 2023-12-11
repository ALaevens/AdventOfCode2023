using AdventOfCode2023;

using Galaxy = System.Tuple<int, int>;
using GalaxyPair = System.Tuple<System.Tuple<int, int>, System.Tuple<int, int>>;
public class Day11 : BaseDay {
    private List<string> expand(List<string> universe) {
        List<string> expanded = new List<string>();
        foreach (string row in universe) {
            expanded.Add(row);
            if (row.Replace(".", "") == "") {
                expanded.Add(row);
            }
        }

        int colsAdded = 0;
        for (int x = 0; x < universe[0].Length; x++) {
            if (universe.Select(r => r[x] == '.').Aggregate((a, b) => a && b)) {
                for (int i = 0; i < expanded.Count; i++) {
                    expanded[i] = expanded[i].Insert(x + colsAdded, ".");
                }

                colsAdded+=1;
            }
        }

        return expanded;
    }

    private List<Galaxy> findGalaxies(List<string> universe) {
        List<Galaxy> galaxies = new List<Galaxy>();
        for (int y = 0; y < universe.Count; y++) {
            int x = 0;
            while (x < universe[y].Length) {
                x = universe[y].IndexOf('#', x);

                if (x < 0) {
                    break;
                }

                galaxies.Add(new Galaxy(x, y));

                x++;
            }
        }

        return galaxies;
    }

    private List<Galaxy> expandHugeAndFind(List<string> universe) {
        List<Galaxy> galaxies = findGalaxies(universe);
        int expandedSize = 1000000; // add 1 new row / col

        int rowsAdded = 0;
        for (int y = 0; y < universe.Count; y++) {
            if (universe[y].Replace(".", "") == "") { // empty row
                galaxies = galaxies.Select(g => {
                    if (g.Item2 > y + rowsAdded) {
                        return new Galaxy(g.Item1, g.Item2 + expandedSize - 1);
                    } else {
                        return g;
                    }
                }).ToList();

                rowsAdded += expandedSize - 1;
            }
        }

        int colsAdded = 0;
        for (int x = 0; x < universe[0].Length; x++) {
            if (universe.Select(r => r[x] == '.').Aggregate((a, b) => a && b)) { // empty column
                galaxies = galaxies.Select(g => {
                    if (g.Item1 > x + colsAdded) {
                        return new Galaxy(g.Item1 + expandedSize - 1, g.Item2);
                    } else {
                        return g;
                    }
                }).ToList();

                colsAdded += expandedSize - 1;
            }
        }

        return galaxies;
    }
    private HashSet<Tuple<Galaxy, Galaxy>> makePairs(ref List<Galaxy> galaxies) {
        HashSet<GalaxyPair> pairs = new HashSet<GalaxyPair>();

        int count = 0;

        foreach (Galaxy g1 in galaxies) {
            foreach (Galaxy g2 in galaxies) {
                if (!g1.Equals(g2)) {
                    GalaxyPair pair = Tuple.Create(g1, g2);
                    GalaxyPair pairReverse = Tuple.Create(g2, g1);

                    if (!pairs.Contains(pair) && !pairs.Contains(pairReverse)) {
                        pairs.Add(pair);
                        count++;
                    }
                }
            }
        }

        return pairs;
    }

    public override void Part1(string[] inputLines) {
        List<string> universe = inputLines.Select(x => x.Trim()).ToList();
        universe = expand(universe);

        List<Galaxy> galaxies = findGalaxies(universe);

        HashSet<GalaxyPair> pairs = makePairs(ref galaxies);

        int sum = 0;
        foreach (GalaxyPair pair in pairs) {
            Galaxy g1 = pair.Item1;
            Galaxy g2 = pair.Item2;

            int manhattan = Math.Abs(g1.Item1 - g2.Item1) + Math.Abs(g1.Item2 - g2.Item2);
            sum += manhattan;
        }

        Console.WriteLine("Manhattan sum: " + sum);

    }

    public override void Part2(string[] inputLines) {
        List<string> universe = inputLines.Select(x => x.Trim()).ToList();

        List<Galaxy> galaxies = expandHugeAndFind(universe);

        foreach (Galaxy g in galaxies) {
            Console.WriteLine($"GALAXY: ({g.Item1}, {g.Item2})");
        }
        HashSet<GalaxyPair> pairs = makePairs(ref galaxies);

        long sum = 0;
        foreach (GalaxyPair pair in pairs) {
            Galaxy g1 = pair.Item1;
            Galaxy g2 = pair.Item2;

            long manhattan = Math.Abs(g1.Item1 - g2.Item1) + Math.Abs(g1.Item2 - g2.Item2);
            sum += manhattan;
        }
        
        Console.WriteLine("Manhattan sum: " + sum);
    }
}