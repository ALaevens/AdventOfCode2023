using AdventOfCode2023;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

internal class Day16 : BaseDay {
    private class Map {
        private class PosComparer : IEqualityComparer<Pos> {
            public bool Equals(Pos? x, Pos? y) {
                if (x == null || y == null) {
                    return false;
                }

                return x.r == y.r && x.c == y.c && x.dir == y.dir;
            }

            public int GetHashCode([DisallowNull] Pos obj) {
                return obj.r.GetHashCode() ^ obj.c.GetHashCode() ^ obj.dir.GetHashCode();
            }
        }
        public class Pos {
            public Pos(int r, int c, char dir) {
                this.r = r; this.c = c;
                this.dir = dir;
            }

            public Pos inc() {
                int dr = 0; int dc = 0;
                switch (dir) {
                    case 'r':
                        dc = 1;
                        break;
                    case 'l':
                        dc = -1;
                        break;
                    case 'u':
                        dr = -1;
                        break;
                    case 'd':
                        dr = 1;
                        break;
                }

                return new Pos(r + dr, c + dc, dir);
            }

            public Pos reflect(char type) {
                List<Pos> result = new List<Pos>();
                Dictionary<(char, char), char> bounceDirection = new Dictionary<(char, char), char> {
                    {('/', 'r'), 'u'}, {('/', 'l'), 'd'}, {('/', 'u'), 'r'}, {('/', 'd'), 'l'},
                    {('\\', 'r'), 'd'}, {('\\', 'l'), 'u'}, {('\\', 'u'), 'l'}, {('\\', 'd'), 'r'},
                };

                dir = bounceDirection[(type, dir)];
                return inc();
            }

            public List<Pos> split(char type) {
                Dictionary<char, char> opposite = new Dictionary<char, char> {
                    {'r', 'l'}, {'l', 'r'}, {'u', 'd'}, {'d', 'u'}
                };

                List<Pos> result = new List<Pos>();
                if (type == '-' && (dir == 'r' || dir == 'l')) {
                    //Console.WriteLine("Not affected");
                    result.Add(inc());
                } 
                else if (type == '|' && (dir == 'u' || dir == 'd')) {
                    //Console.WriteLine("Not affected");
                    result.Add(inc());
                }

                else if (type == '-' && (dir == 'u' || dir == 'd')) {
                    //Console.WriteLine("Split horizontal");
                    dir = 'l';
                    result.Add(inc());
                    dir = 'r';
                    result.Add(inc());
                } else if (type == '|' && (dir == 'r' || dir == 'l')) {
                    //Console.WriteLine("Split vertical");
                    dir = 'u';
                    result.Add(inc());
                    dir = 'd';
                    result.Add(inc());
                }

                return result;
            }

            public int r { get; private set; }
            public int c { get; private set; }
            public char dir { get; private set; }
        }

        public Map(string[] mapLines) {
            mapData = mapLines.Select(x => x.Trim()).ToList();
            rows = mapData.Count;
            columns = mapData[0].Length;
        }

        public int explore(Pos start) {
            List<Pos> frontier = new List<Pos>();
            HashSet<Pos> explored = new HashSet<Pos>(new PosComparer());

            frontier.Add(start);

            int i = 0;
            while (frontier.Count > 0) {
                Pos pos = frontier[0];
                frontier.RemoveAt(0);

                if (!explored.Contains(pos)) {
                    explored.Add(pos);
                } else {
                    continue;
                }
                    
                //Console.WriteLine($"Step {i}: Explore [{pos.r}, {pos.c}], # Explored: {explored.Count}");

                List<Pos> nextPos = takeAction(pos);
                //nextPos.ForEach(x => Console.WriteLine($"    Next: [{x.r}, {x.c}]"));
                nextPos.ForEach(x => frontier.Add(x));
                i++;
            }

            return explored.DistinctBy(pos => (pos.r, pos.c)).ToList().Count;
        }

        private List<Pos> takeAction(Pos pos) {
            List<Pos> result = new List<Pos>();
            

            char tile = mapData[pos.r][pos.c];
            if (tile == '.') {
                //Console.WriteLine("  Continue");
                result.Add(pos.inc());
            } else if (tile == '/' || tile == '\\') {
                //Console.WriteLine("  Reflect");
                result.Add(pos.reflect(tile));
            } else if (tile == '-' || tile == '|') {
                //Console.WriteLine("  Split");
                pos.split(tile).ForEach(x => result.Add(x));
            }
            
            result = result.Where(x => x.r >= 0 && x.r < rows && x.c >= 0 && x.c < columns).ToList();

            return result;
        }

        private List<string> mapData;
        public int rows { get; private set; }
        public int columns { get; private set; }
    }
    public override void Part1(string[] inputLines) {
        Map map = new Map(inputLines);
        Console.WriteLine(map.explore(new Map.Pos(0, 0, 'r')));

    }

    public override void Part2(string[] inputLines) {
        Map map = new Map(inputLines);
        map.explore(new Map.Pos(0, 0, 'r'));

        int best = 0;
        for (int r = 0; r < map.rows; r++) {
            best = Math.Max(best, map.explore(new Map.Pos(r, 0, 'r')));
            best = Math.Max(best, map.explore(new Map.Pos(r, map.columns-1, 'l')));
        }

        for (int c = 0; c < map.columns; c++) {
            best = Math.Max(best, map.explore(new Map.Pos(0, c, 'd')));
            best = Math.Max(best, map.explore(new Map.Pos(map.rows-1, c, 'u')));
        }

        Console.WriteLine(best);
    }
}
