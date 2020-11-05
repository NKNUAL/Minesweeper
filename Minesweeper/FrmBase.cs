using Minesweeper.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class FrmBase : Form
    {
        public FrmBase()
        {
            InitializeComponent();
        }

        private void FrmBase_Load(object sender, EventArgs e)
        {
            LoadMinePanel(99, 16, 30);
        }

        MineRule _rule;

        private void LoadMinePanel(int mine_count, int height, int width)
        {
            _flagCount = mine_count;
            _rule = new MineRule
            {
                Height = height,
                Width = width,
                MineCount = mine_count
            };

            _rule.GenerateGame();

            flowPanel.Controls.Clear();

            for (int i = 0; i < _rule.Height * _rule.Width; i++)
            {
                Button btn = new Button
                {
                    Height = 18,
                    Width = 18,
                    Margin = new Padding(0, 0, 0, 0),
                    Padding = new Padding(0, 0, 0, 0),
                    Tag = i,
                    BackColor = Color.Gray,
                    Name = i.ToString(),
                    BackgroundImageLayout = ImageLayout.Zoom
                };
                btn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.but_Click);
                flowPanel.Controls.Add(btn);
            }

        }

        Dictionary<int, int> _ImgIndex = new Dictionary<int, int>();

        private void but_Click(object sender, MouseEventArgs e)
        {
            var btn = sender as Button;
            int i = (btn.Tag as int?) ?? 0;



            MouseEventArgs m_e = (MouseEventArgs)e;
            if (m_e.Button == MouseButtons.Left)
            {
                if (_ImgIndex.ContainsKey(i) && _ImgIndex[i] == 1)
                    return;
                int x = i / _rule.Width;
                int y = i % _rule.Width;
                if (_rule.Mine[x, y])
                {
                    MessageBox.Show("踩到雷了");
                }
                else
                {
                    Dictionary<int, int> index_mine = new Dictionary<int, int>();
                    List<int> weed = new List<int>();
                    _rule.GetAroundWithoutMine(ref index_mine, ref weed, x, y);
                    SetAroundSpaces(index_mine);
                }
            }
            else
            {
                if (!_ImgIndex.ContainsKey(i))
                {
                    btn.BackgroundImage = Resources.旗子;
                    btn.BackColor = Color.Green;
                    _ImgIndex.Add(i, 1);
                    SetFlagCount(-1);
                }
                else
                {
                    switch (_ImgIndex[i])
                    {
                        case 0:
                            btn.BackgroundImage = Resources.旗子;
                            btn.BackColor = Color.Green;
                            _ImgIndex[i] = 1;
                            SetFlagCount(-1);
                            break;
                        case 1:
                            btn.BackgroundImage = Resources.问号;
                            btn.BackColor = Color.Green;
                            _ImgIndex[i] = 2;
                            SetFlagCount(1);
                            break;
                        case 2:
                            btn.BackgroundImage = null;
                            btn.BackColor = Color.Gray;
                            _ImgIndex[i] = 0;
                            break;
                    }
                }

            }
        }

        int _flagCount = 0;
        private void SetFlagCount(int seed)
        {
            _flagCount += seed;
            tbCount.Text = _flagCount.ToString();
        }

        private void SetAroundSpaces(Dictionary<int, int> index_mine_count)
        {
            foreach (var item in index_mine_count)
            {
                var btn = flowPanel.Controls[item.Key];
                if (item.Value > 0)
                {
                    btn.Text = item.Value.ToString();
                    btn.Font = new Font("宋体", btn.Font.Size, FontStyle.Bold); ;
                }
                btn.Enabled = false;
                btn.BackColor = Color.White;
                btn.BackgroundImage = null;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTime.Text))
            {
                tbTime.Text = "0";
            }
            else
            {
                tbTime.Text = (int.Parse(tbTime.Text) + 1).ToString();
            }
        }
    }
}
