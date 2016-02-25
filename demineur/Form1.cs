using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace demineur
{
    public partial class demineur : Form
    {
        int nbr_mine = 0, x = 0, y = 0, bomba = 0, desamorce = 0, antminerestante = 0;
        // matrice de 9 sur 9
        string[,] carte = new string[10, 10];
        bool loose = false;
        public demineur()
        {
            InitializeComponent();
        }
        private void gengrille()
        {
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                {
                    Button bt = new Button();
                    bt.Name = "bt-" + x + "-" + y;
                    bt.MouseDown += Bt_Click;
                    table1.Controls.Add(bt, x, y);
                }
            table1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        }
        private void cleangrille()
        {
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                {
                    Button b = table1.GetControlFromPosition(x, y) as Button;
                    b.Text = "";
                }
        }
        private void perdu()
        {
            MessageBox.Show("Et hop, une jambe de moins", "Perdu");
            table1.Enabled = false;
            btn_fire.Enabled = true;
            label2.Text = "Bien joué, tu retentes ?";
            label2.Visible = true;
            loose = true;
            //---------------------------------------------------
            //on affiche un X sur chaque bombe sur la map
            //---------------------------------------------------
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                {
                    if (carte[x, y] == "X")
                    {
                        Button b = table1.GetControlFromPosition(x, y) as Button;
                        b.Text = "X";
                    }
                }
        }
        private void Arshess_Load(object sender, EventArgs e)
        {
            gengrille();
            table1.Enabled = false;
        }
        private void verif_cases(int xx, int yy)
        {
            // les verticales ou l'inverse j'ai un doute
            if (xx < 9)
            {
                if (carte[xx + 1, yy] == "X")
                    bomba++;
            }
            if (xx > 0)
            {
                if (carte[xx - 1, yy] == "X")
                    bomba++;
            }
            // les horizontales  ou l'inverse j'ai un doute
            if (yy < 9)
            {
                if (carte[xx, yy + 1] == "X")
                    bomba++;
            }
            if (yy > 0)
            {
                if (carte[xx, yy - 1] == "X")
                    bomba++;
            }
            //les diagonales, ça c'est sûr
            if (xx < 9 && yy < 9)
            {
                if (carte[xx + 1, yy + 1] == "X")
                    bomba++;
            }
            if (xx > 0 && yy > 0)
            {
                if (carte[xx - 1, yy - 1] == "X")
                    bomba++;
            }
            if (xx < 9 && yy > 0)
            {
                if (carte[xx + 1, yy - 1] == "X")
                    bomba++;
            }
            if (xx > 0 && yy < 9)
            {
                if (carte[xx - 1, yy + 1] == "X")
                    bomba++;
            }
            Button bt = table1.GetControlFromPosition(xx, yy) as Button;
            bt.Text = bomba.ToString();
            //if (bomba == 0)
            //{
            //    verif_cases(xx + 1, yy);
            //    verif_cases(xx - 1, yy);
            //    verif_cases(xx, yy + 1);
            //    verif_cases(xx, yy - 1);
            //}
            bomba = 0;

        }
        private void Bt_Click(object sender, MouseEventArgs e)
        {
            Button bt = sender as Button;
            string[] bt_pos = bt.Name.Split('-');
            int xx = Convert.ToInt32(bt_pos[1]);
            int yy = Convert.ToInt32(bt_pos[2]);
            if (e.Button == MouseButtons.Left)
            {
                if (carte[xx, yy] == "X")
                {
                    perdu();
                }
                else
                {
                    verif_cases(xx, yy);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (bt.Text == "M")
                {
                    bt.Text = "";
                    antminerestante++;
                }
                else
                {
                    if (carte[xx, yy] == "X" && antminerestante > 0)
                    {
                        bt.Text = "M";
                        desamorce++;
                        antminerestante--;
                    }
                    else
                    {
                        bt.Text = "M";
                        antminerestante--;
                    }
                    if (desamorce == nbr_mine)
                    {
                        label2.Text = "You are Winner ";
                        label2.Visible = true;
                        table1.Enabled = false;
                        btn_fire.Enabled = true;
                        loose = true;
                    }
                }
                lb_nb_bombe.Text = antminerestante.ToString();
            }
        }
        private void placement()
        {
            Random pos = new Random();
            for (int i = 0; i < nbr_mine; i++)
            {
                do // tant que la case au rand contient dejà une mine on reroll
                {
                    x = pos.Next(0, 10);
                    y = pos.Next(0, 10);
                } while (carte[x, y] == "X");

                carte[x, y] = "X";
            }
        }
        private void btn_fire_Click(object sender, EventArgs e)
        {
            if (Regex.IsMatch(Tb_nbr_mine.Text, "^[0-9]{1,2}$"))
            {
                Array.Clear(carte, 0, carte.Length);
                nbr_mine = Convert.ToInt32(Tb_nbr_mine.Text);
                placement();
                btn_fire.Enabled = false;
                table1.Enabled = true;
                if (loose)
                {
                    cleangrille();
                }
                antminerestante = nbr_mine;
                lb_nb_bombe.Visible = true;
                label3.Visible = true;
                lb_nb_bombe.Text = Tb_nbr_mine.Text;
                label2.Visible = false;
            }
        }
    }
}
