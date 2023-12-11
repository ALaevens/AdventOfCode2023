using AdventOfCode2023;
using System.Text.RegularExpressions;

public class Day10 : BaseDay {
    public struct Position : IEquatable<Position> {
        public Position (int y, int x) {
            this.y = y; this.x = x;
        }

        public int y; public int x;

        public bool Equals(Position other) {
            return this.y == other.y && this.x == other.x;
        }
    }

    private Dictionary<char, int[][]> pipeToNeighbours = new Dictionary<char, int[][]> {
        { '┃', new int[][] { new int[] { -1, 0 }, new int[]{ 1, 0 } } },
        { '━', new int[][] { new int[] { 0, -1 }, new int[] { 0, 1 } } },
        { '┗', new int[][] { new int[] { -1, 0 }, new int[] { 0, 1 } } },
        { '┛', new int[][] { new int[] { -1, 0 }, new int[] { 0, -1 } } },
        { '┓', new int[][] { new int[] { 1, 0 }, new int[] { 0, -1 } } },
        { '┏', new int[][] { new int[] { 1, 0 }, new int[] { 0, 1 } } },
        { '.', new int[][] { new int[] { 0, 0 }, new int[] { 0, 0 } } }
    };

    private int[][] startNeighbourOffsets(Position pos, List<string> world) {
        int[][] offsets = new int[2][];
        int found = 0;
        for (int yShift = -1; yShift <=1; yShift++) {
            for (int xShift = -1; xShift <=1; xShift++) {

                if (xShift != 0 ^ yShift != 0) {
                    foreach (int[] neighbourOffset in pipeToNeighbours[world[pos.y + yShift][pos.x + xShift]]){
                        if (yShift + neighbourOffset[0] == 0 && xShift + neighbourOffset[1] == 0) {
                            offsets[found] = new int[] { yShift, xShift };
                            found++;
                        }
                    }
                }

            }
        }

        return offsets;
    }

    private int BFS(ref HashSet<Position> explored, ref List<Tuple<Position, int>> frontier, ref List<string> world, Position start) {
        frontier.Add(new Tuple<Position, int>(start, 0));

        int maxDepth = 0;

        while (frontier.Count > 0) {
            Position pos = frontier[0].Item1;
            int depth = frontier[0].Item2;
            frontier.RemoveAt(0);

            if (!explored.Contains(pos)) {
                explored.Add(pos);
                maxDepth = Math.Max(maxDepth, depth);

                int[][] offsets;
                if (world[pos.y][pos.x] == 'S') {
                    offsets = startNeighbourOffsets(pos, world);
                } else {
                    offsets = pipeToNeighbours[world[pos.y][pos.x]];
                }

                foreach (int[] offset in offsets) {
                    frontier.Add(new Tuple<Position, int>(new Position(pos.y + offset[0], pos.x + offset[1]), depth + 1));
                }
            }
        }

        return maxDepth;
    }

    private List<string> prepareWorld(string[] lines) {
        List<string> world = new List<string>();
        foreach (string l in lines) {
            string line = l.Trim();
            line = line.Replace('|', '┃').Replace('-', '━').Replace('L', '┗').Replace('J', '┛').Replace('7', '┓').Replace('F', '┏');
            world.Add(line);
            Console.WriteLine(line);
        }

        return world;
    }

    private List<string> cleanWorld(ref HashSet<Position> explored, List<string> world) {
        List<string> clean = new List<string>();

        for (int y = 0; y < world.Count; y++) {
            clean.Add("");
            for (int x = 0; x < world[y].Length; x++) {
                if (explored.Contains(new Position(y, x))) {
                    clean[y] += world[y][x];
                } else {
                    clean[y] += ".";
                }
            }
        }

        return clean;
    }

    private int countEnclosed(ref HashSet<Position> explored, ref List<string> world) {
        int count = 0;
        Regex rHorizontal = new Regex(@"┏━*┓|┗━*┛");
        Regex rVertical = new Regex(@"┏━*┛|┗━*┓");
        
        foreach (string l in world) {
            string line = l;

            // Find start and replace with pipe shape
            if (line.Contains('S')) {
                int[][] startOffsets = startNeighbourOffsets(new Position(world.IndexOf(line), line.IndexOf('S')), world);

                foreach (char shape in pipeToNeighbours.Keys) {
                    bool match = true;
                    foreach (int[] offset in pipeToNeighbours[shape]) {
                        if (!Enumerable.SequenceEqual(offset, startOffsets[0]) && !Enumerable.SequenceEqual(offset, startOffsets[1])) {
                            match = false;
                            break;
                        } 
                    }

                    if (match) {
                        line = line.Replace('S', shape); break;
                    }
                }
            }

            line = rHorizontal.Replace(line, "━");
            line = rVertical.Replace(line, "┃");
            line = line.Replace("━", "");
           

            int pipesCrossed = 0;
            foreach (char c in line) {
                if (c != '.' && c != ' ') {
                    pipesCrossed++;
                } else if (pipesCrossed %2 == 1 && c == '.'){
                    count++;
                } else {
                }
            }
            Console.WriteLine(line);
        }

        return count;
    }
    public override void Part1(string[] inputLines) {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<string> world = prepareWorld(inputLines);

        int startY = world.IndexOf(world.Find(x => x.Contains('S')));
        int startX = world[startY].IndexOf('S');

        HashSet<Position> explored = new HashSet<Position>();
        List<Tuple<Position, int>> frontier = new List<Tuple<Position, int>>();
        int maxDepth = BFS(ref explored, ref frontier, ref world, new Position(startY, startX));

        Console.WriteLine("Max Depth: " + maxDepth);
    }

    public override void Part2(string[] inputLines) {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<string> world = prepareWorld(inputLines);

        int startY = world.IndexOf(world.Find(x => x.Contains('S')));
        int startX = world[startY].IndexOf('S');

        HashSet<Position> explored = new HashSet<Position>();
        List<Tuple<Position, int>> frontier = new List<Tuple<Position, int>>();
        int maxDepth = BFS(ref explored, ref frontier, ref world, new Position(startY, startX));

        world = cleanWorld(ref explored, world);

        Console.WriteLine("\n\n\nCleaned:");
        foreach (string line in world) {
            Console.WriteLine(line);
        }

        Console.WriteLine("\n\n\nCount:");
        int enclosed = countEnclosed(ref explored, ref world);

        Console.WriteLine("Num Enclosed: " + enclosed);
    }
}