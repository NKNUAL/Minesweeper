using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class MineRule
    {
        public int MineCount { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool[,] Mine { get; set; }

        public void GenerateGame()
        {
            Mine = new bool[Height, Width];
            //填充雷
            for (int i = 0; i < MineCount; i++)
                Mine[i / Width, i % Width] = true;

            Random r = new Random();
            //随机布雷
            for (int i = 0; i < Height * Width - 1; i++)
            {
                var index = r.Next(0, i + 1);
                Swap(Mine, i / Width, i % Width, index / Width, index % Width);
            }
        }

        private void Swap(bool[,] mines, int x1, int y1, int x2, int y2)
        {
            bool temp = mines[x1, y1];
            mines[x1, y1] = mines[x2, y2];
            mines[x2, y2] = temp;
        }

        public int GetAroundMines(int x, int y, out List<Tuple<int, int>> aroundSpace)
        {
            aroundSpace = new List<Tuple<int, int>>();
            List<int> list_x = new List<int> { x - 1, x, x + 1 };
            List<int> list_y = new List<int> { y - 1, y, y + 1 };

            int mine_count = 0;

            foreach (var item_x in list_x)
            {
                if (item_x < 0 || item_x > Height - 1)
                    continue;

                foreach (var item_y in list_y)
                {
                    if (item_y < 0 || item_y > Width - 1)
                        continue;

                    if (item_x == x && item_y == y)
                        continue;

                    aroundSpace.Add(new Tuple<int, int>(item_x, item_y));

                    if (Mine[item_x, item_y])
                        mine_count++;
                }
            }
            return mine_count;
        }


        public void GetAroundWithoutMine(ref Dictionary<int, int> index_mine, ref List<int> weed, int x, int y)
        {
            int key = x * Width + y;
            if (weed.Contains(key))
                return;

            weed.Add(key);

            int mine_count = GetAroundMines(x, y, out var aroundIndex);

            if (!index_mine.ContainsKey(key))
                index_mine.Add(key, mine_count);
            if (mine_count == 0)
            {
                foreach (var item in aroundIndex)
                {
                    GetAroundWithoutMine(ref index_mine, ref weed, item.Item1, item.Item2);
                    weed.Add(item.Item1 * Width + item.Item2);
                }
            }
        }
    }
}
