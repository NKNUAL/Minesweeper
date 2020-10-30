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
            _rule = new MineRule
            {
                Height = height,
                Width = width,
                MineCount = mine_count
            };

            _rule.GenerateGame();

            flowPanel.Controls.Clear();

            for (int i = 0; i < _rule.Height * _rule.Width - 1; i++)
            {
                Button btn = new Button
                {
                    Height = 18,
                    Width = 18,
                    Margin = new Padding(0, 0, 0, 0),
                    Padding = new Padding(0, 0, 0, 0),
                    Tag = i
                };
                btn.Click += new System.EventHandler(this.but_Click);
                if (_rule.Mine[i / _rule.Width, i % _rule.Width])
                {
                    btn.BackColor = Color.Black;
                }
                flowPanel.Controls.Add(btn);
            }

        }


        private void but_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            int i = (btn.Tag as int?) ?? 0;
            int x = i / _rule.Width;
            int y = i % _rule.Width;
            if (_rule.Mine[x, y])
            {
                MessageBox.Show("踩到雷了");
            }
            else
            {
                btn.Text = _rule.GetAroundMines(x, y).ToString();
            }
        }





    }
}
