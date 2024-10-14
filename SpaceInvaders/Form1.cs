using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Form1 : Form
    {
        bool left, right;
        int playerSpeed = 12;
        int enemySpeed = 5;
        int score = 0;
        int enemyBulletTimer = 300;
        string start = " Press enter to start new game.";

        PictureBox[] invaders;
        bool shoot;
        bool isGameOver;

        public Form1()
        {
            InitializeComponent();
            gameSetup();
        }

        private void gameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            if (left)
            {
                player.Left -= playerSpeed;
            }

            if (right)
            {
                player.Left += playerSpeed;
            }

            enemyBulletTimer -= 10;
            if (enemyBulletTimer < 1)
            {
                enemyBulletTimer = 300;
                makeBullet("enemyBullet");
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "invader")
                {
                    x.Left += enemySpeed;
                    if (x.Left > 730)
                    {
                        x.Top += 65;
                        x.Left = -80;
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        gameOver("You lose!");
                    }

                    foreach (Control y in this.Controls)
                    {
                        if (y is PictureBox && (string)y.Tag == "bullet")
                        {
                            if (y.Bounds.IntersectsWith(x.Bounds)) //player hit the invader
                            {
                                this.Controls.Remove(x);
                                this.Controls.Remove(y);
                                score += 1;
                                shoot = false;
                            }
                        }
                    }
                }

                if (x is PictureBox && (string)x.Tag == "bullet")
                {
                    x.Top -= 20;
                    if (x.Top < 15) //bullet hit the top of the screen
                    {
                        this.Controls.Remove(x);
                        shoot = false;
                    }
                }

                if (x is PictureBox && (string)x.Tag == "enemyBullet")
                {
                    x.Top += 20;
                    if (x.Top > 620)
                    {
                        this.Controls.Remove(x);
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        this.Controls.Remove(x);
                        gameOver("Invader shot you!");
                    }
                }
            }

            if (score > 10)
            {
                enemySpeed = 8;
            }

            if (score == invaders.Length)
            {
                gameOver("You win!");
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) left = true;
            if (e.KeyCode == Keys.Right) right = true;
        }

        private void KetIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) left = false;
            if (e.KeyCode == Keys.Right) right = false;

            if (e.KeyCode == Keys.Space && shoot == false)
            {
                shoot = true;
                makeBullet("bullet");
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeAll();
                gameSetup();
            }
        }

        private void gameSetup()
        {
            txtScore.Text = "Score: 0";
            score = 0;
            isGameOver = false;
            enemyBulletTimer = 300;
            enemySpeed = 5;
            shoot = false;
            makeInvaders();
            gameTimer.Start();
        }

        private void gameOver(string text)
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Score: " + score + " " + text + start;
        }

        private void removeAll()
        {
            foreach (PictureBox i in invaders)
            {
                this.Controls.Remove(i);
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "enemyBullet" || (string)x.Tag == "invader")
                    {
                        this.Controls.Remove(x);
                    }
                }
            }
        }

        private void makeBullet(string tag)
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.bullet;
            bullet.Size = new Size(5, 20);
            bullet.Tag = tag;
            bullet.Left = player.Left + player.Width / 2;

            if ((string)bullet.Tag == "bullet")
            {
                bullet.Top = player.Top - 20;

            }
            else if ((string)bullet.Tag == "enemyBullet")
            {
                bullet.Top = -100;
            }

            this.Controls.Add(bullet);
            bullet.BringToFront();
        }

        private void makeInvaders()
        {
            invaders = new PictureBox[20];
            int left = 0;

            for (int i = 0; i < invaders.Length; i++)
            {
                invaders[i] = new PictureBox();
                invaders[i].Size = new Size(50, 40);
                invaders[i].Image = Properties.Resources.invader;
                invaders[i].Top = 5;
                invaders[i].Tag = "invader";
                invaders[i].Left = left;
                invaders[i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.Controls.Add(invaders[i]);
                left = left - 80;
            }
        }
    }
}
