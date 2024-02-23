using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace Zoombee_game
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, goUp, goDown, gameOwer = false;
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 100;
        int zombiesSpeed = 1;
        Random randNum = new Random();
        int score;
        List<PictureBox> zoombiesList = new List<PictureBox>();


        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainTimeEvent(object sender, EventArgs e)
        {
            if(playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            }
            else
            {
                gameOwer = true;
                player.Image = Properties.Resources._74_745017_ive_found_probono_dead_as_a_fried_chicken;
                GameTime.Stop();
            }

            txtAmmo.Text = "Ammo: " + ammo;
            txtScore.Text = "Kills: " + score;
            if (score == 20) {
                goUp = false;
                goDown = false;
                goLeft = false;
                goRight = false;
                gameOwer = false;
                label3.Text = "OSMONDAGI BOLALARDAN \nKIM UYNAGAN BO'LSA HAM \n              YUTDINGIZ";
                healthBar.Value = 0;
                this.Controls.Remove(player);
                ((PictureBox)player).Dispose();
                ammo = 0;
            }
            if(goLeft == true && player.Left > 0)
            {
                player.Left -= speed;
            }
            if(goRight == true && player.Width < this.ClientSize.Width)
            {
                player.Left += speed;
            }
            if(goUp == true && player.Top > 44)
            {
                player.Top -= speed;
            }
            if(goDown == true && player.Height < this.ClientSize.Height)
            {
                player.Top += speed;
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 100;
                    }
                }




                if(x is PictureBox && (string)x.Tag == "zombie")
                {

                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                    }

                    if(x.Left > player.Left)
                    {
                        x.Left -= zombiesSpeed;
                        ((PictureBox)x).Image = Properties.Resources.ae7c0199c0103b36a78f11644bc72;
                    }
                    if (x.Left < player.Left)
                    {
                        x.Left += zombiesSpeed;
                        ((PictureBox)x).Image = Properties.Resources.ae7c0199c0103b3a6a78f11644bc72;
                    }
                    if (x.Top > player.Top)
                    {
                        x.Top -= zombiesSpeed;
                        ((PictureBox)x).Image = Properties.Resources.ae7c0199c0103b3a6a378f11644bc72;
                    }
                    if (x.Top < player.Top)
                    {
                        x.Top += zombiesSpeed;
                        ((PictureBox)x).Image = Properties.Resources.ae7c0199c0103b3a6a378f1644bc72;
                    }
                }
                foreach (Control j in this.Controls)
                {
                    if(j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;

                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zoombiesList.Remove(((PictureBox)x));
                            MakeZoombies();
                        }
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(gameOwer == true)
            {
                return;
            }
            if(e.KeyCode == Keys.A)
            {
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }
            if(e.KeyCode == Keys.D)
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right;
            }
            if(e.KeyCode == Keys.W)
            {
                goUp = true;
                facing = "up";
                player.Image = Properties.Resources.up;
            }
            if(e.KeyCode == Keys.S)
            {
                goDown = true;
                facing = "down";
                player.Image = Properties.Resources.down;
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.D)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.W)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.S)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && gameOwer == false)
            {
                ammo--;
                ShootBullet(facing);
                if(ammo < 1)
                {
                    DropAmmo();
                }
            }
            if(e.KeyCode == Keys.Enter == true)
            {
                RestartGame();
            }
        }

        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);
            shootBullet.bulletTop = player.Top + (player.Height / 2);
            shootBullet.MakeBullet(this);
        }

        private void MakeZoombies()
        {
            PictureBox zommbie = new PictureBox();
            zommbie.Tag = "zombie";
            zommbie.Image = Properties.Resources.ae7c0199c0103b3a6a378f1644bc72;
            zommbie.Left = randNum.Next(0, 900);
            zommbie.Top = randNum.Next(0,800);
            zommbie.SizeMode = PictureBoxSizeMode.Zoom;
            zoombiesList.Add(zommbie);
            this.Controls.Add(zommbie);
            player.BringToFront();
        }
        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.feat_2;
            ammo.SizeMode = PictureBoxSizeMode.Zoom;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(10, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();

        }

        private void RestartGame()
        {
            player.Image = Properties.Resources.up;
            foreach(PictureBox i in zoombiesList)
            {
                this.Controls.Remove(i);
            }

            zoombiesList.Clear();
            for (int i = 0; i < 5; i++)
            {
                MakeZoombies();
            }
            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOwer = false;
            playerHealth = 100;
            score = 0;
            ammo = 10;
            GameTime.Start();
            }
    }
}
