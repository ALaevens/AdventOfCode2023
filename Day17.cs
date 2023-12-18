
using AdventOfCode2023;
using System.Xml.Xsl;

internal class Day17 : BaseDay {
    private class Map {
        public struct Pos : IEquatable<Pos> {
            public Pos(int r, int c) {
                this.r = r; this.c = c;
            }

            public List<Pos> getNeighbours(int rows, int columns) {
                List<Pos> neighbours = new List<Pos>();

                foreach (char d in "rlud") {
                    int newR = r + dirCharToOffset[d][0];
                    int newC = c + dirCharToOffset[d][1];
                    if (newR >= 0 && newC >= 0 && newR < rows && newC < columns) {
                        neighbours.Add(new Pos(newR, newC));
                    }
                }

                return neighbours;
            }

            public bool Equals(Pos other) {
                return r == other.r && c == other.c;
            }

            public int r { get; private set; }
            public int c { get; private set; }
            static private Dictionary<char, int[]> dirCharToOffset = new Dictionary<char, int[]> {
                { 'r', new int[2]{0, 1} }, { 'l', new int[2]{0, -1} }, { 'u', new int[2]{-1,0} }, { 'd', new int[2]{1, 0} }
            };
            
        }
        public Map(string[] rowData) {
            //mapData = rowData.Select(x => x.Trim()).ToList();
            mapData = rowData.Select(line => line.Select(c => Int32.Parse(c.ToString())).ToList()).ToList();
            rows = mapData.Count;
            columns = mapData[0].Count;
        }

        public bool checkTooStraight(Dictionary<Pos, Pos> prev, Pos start) {
            int i = 0;

            int absdr = 0;
            int absdc = 0;
            Pos pos = start;
            while (prev.ContainsKey(pos) && i < 2) {
                Pos last = prev[pos];
                absdr += Math.Abs(last.r - pos.r);
                absdc += Math.Abs(last.c - pos.c);

                if (absdr > 0 && absdc > 0)
                    return false;

                pos = last;
                i++;
            }

            return i == 2;
        }

        public Dictionary<Pos, Pos> djikstra() {
            Pos start = new Pos(0, 0); // direction doesnt actually matter here
            Pos goal = new Pos(rows-1, columns-1); // or here

            HashSet<Pos> explored = new HashSet<Pos>();
            Dictionary<Pos, Pos> prev = new Dictionary<Pos, Pos>();
            PriorityQueue<Pos, int> queue = new PriorityQueue<Pos, int>();

            queue.Enqueue(start, 0);

            while (queue.Count > 0) {
                queue.TryDequeue(out Pos pos, out int priority);
                explored.Add(pos);
                Console.WriteLine($"EXPLORE: ({pos.r}, {pos.c}), P: {priority}");

                if (pos.Equals(goal)) {
                    Console.WriteLine("FOUND GOAL! Loss = " + priority);
                    printBackTrace(prev);
                    break;
                }

                foreach (Pos n in pos.getNeighbours(rows, columns)) {
                    Console.WriteLine($"  NEIGHBOUR: ({n.r}, {n.c}), P: {priority + mapData[n.r][n.c]}");
                    if (explored.Contains(n) || checkTooStraight(prev, n)) {
                        Console.WriteLine($"    SKIP");
                        continue;
                    }

                    //encountered.Add((n.r, n.c));

                    prev.Add(n, pos);
                    //prev[n] = pos;
                    queue.Enqueue(n, priority + mapData[n.r][n.c]);   
                }
            }

            return prev;
        }

        public void printNeighbours(int r, int c) {
            Pos pos = new Pos(r, c);
            foreach(Pos n in pos.getNeighbours(rows, columns)) {
                Console.WriteLine($"({n.r}, {n.c})");
            }
        }

        public void printBackTrace(Dictionary<Pos, Pos> prev) {
            HashSet<(int, int)> explored = new HashSet<(int, int)>();

            Pos? p = new Pos(12, 12);
            while (p != null) {
                explored.Add((p.Value.r, p.Value.c));
                Console.WriteLine($"EXPLORE BACK: ({p.Value.r}, {p.Value.c})");
                
                if (prev.ContainsKey(p.Value)) {
                    p = prev[p.Value];
                } else {
                    p = null;
                }
            }

            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < columns; c++) {
                    if (explored.Contains((r, c))) {
                        Console.Write("#");
                    } else {
                        Console.Write(mapData[r][c]);
                    }
                }
                Console.WriteLine();
            }
        }

        private List<List<int>> mapData;
        int rows, columns;
    }
    public override void Part1(string[] inputLines) {
        Map map = new Map(inputLines);
        map.djikstra();
    }

    public override void Part2(string[] inputLines) {
        throw new NotImplementedException();
    }
}

