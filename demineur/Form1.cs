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
        int nbr_mine = 0, x = 0, y = 0, bomba = 0, desamorce=0, antminerestante=0;
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

        private void perdu(object sender)
        {
            Button bt = sender as Button;
            MessageBox.Show("et hop, une jambe de moins", "Perdu");
            table1.Enabled = false;
            btn_fire.Enabled = true;
            label2.Visible = true;
            bt.Text = "X";
            loose = true;

            //---------------------------------------------------
            //on affiche un X sur chaque bombe de la map
            //---------------------------------------------------
            Button b;
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                {
                    if (carte[x, y] == "X")
                    {
                        b = table1.GetControlFromPosition(x, y) as Button;
                        b.Text = "X";
                    }
                }           
            
        }
        private void Arshess_Load(object sender, EventArgs e)
        {
            gengrille();
            table1.Enabled = false;
        }

        private void verif_cases(object sender, int xx, int yy,Button bt)
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
            bt.Text = bomba.ToString();
           
            bomba = 0;

        }
        private void Bt_Click(object sender, MouseEventArgs e)
        {
            Button bt = sender as Button;
            string[] test = bt.Name.Split('-');
            int xx = Convert.ToInt32(test[1]);
            int yy = Convert.ToInt32(test[2]);
            if (e.Button == MouseButtons.Left)
            {
                //Console.WriteLine("{0} {1}", test[1], test[2]);
                if (carte[xx, yy] == "X")
                { 
                    perdu(sender);
                }
                else
                {
                    verif_cases(sender,xx,yy,bt);
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
                //Console.WriteLine("x = {0} y = {1}",x,y);
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
                    table1.Controls.Clear();
                    gengrille();
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
