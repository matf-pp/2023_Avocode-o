

using System.CodeDom.Compiler;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms.Design;
using Windows.Media.Playback;
using static System.Net.Mime.MediaTypeNames;

namespace Super_Mario
{
    public partial class Form1 : Form
    {
        public const int TILE = 48;
        public const int SCREENWIDTH = 32;
        // menjaj ako oces da vidis kako se ponasa kad ima dosta stvari na ekranu

        public Form1()
        {
            InitializeComponent();
        }

        // level je panel na kome se pravi nivo
        // world je jedan od 8 svetova
        // stage je jedan od 4 stajdza u svetu
        // player je igrac




        int speed = 8;
        int drawLevelPos = 0;
        int currentWorldPos = 0;

        int playerPosX = 0;
        int playerPosY = 0;

        int[,] currentWorld = new int[256, 16];
        bool genTile = true;
        bool moveLeft, moveRight, sprinting;
        PictureBox[,] drawLevel = new PictureBox[SCREENWIDTH + 2, 16];
        private void timerTick(object sender, EventArgs e)
        {
            if (sprinting) speed = 16; else speed = 8;
            if (moveLeft && player.Left > 0)
            {
                player.Left -= speed;
                playerPosX -= speed;
            }
            if (moveRight)
            {
                playerPosX += speed;
                if (player.Right < this.Width / 2)
                    player.Left += speed;

            }
            if (!moveLeft && !moveRight)
                player.Image = Properties.Resources.mario_small;

            if (player.Left >= this.Width / 2 - TILE && moveRight)
                foreach (Control x in level.Controls)
                {
                    if (x is PictureBox && x.Tag == "OnScreen")
                    {
                        x.Left -= speed;
                        if (x.Left <= -TILE && genTile)
                        {
                            Controls.Remove(x);
                            //renderLevel()
                        }
                    }
                }


            label1.Text = (playerPosX / TILE).ToString();
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;
                player.Image = Properties.Resources.mario_small_running_left;
            }

            if (e.KeyCode == Keys.Right)
            {
                moveRight = true;
                player.Image = Properties.Resources.mario_small_running;
            }
            if (e.KeyCode == Keys.ShiftKey)
                sprinting = true;
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                moveLeft = false;
            if (e.KeyCode == Keys.Right)
            {
                moveRight = false;
                timer2.Stop();
            }

            if (e.KeyCode == Keys.ShiftKey)
                sprinting = false;
        }

        private void importWorld(int world, int stage)
        {
            string fileName = "world" + world.ToString() + "_" + stage.ToString();
            string[] input = Properties.Resources.ResourceManager.GetObject(fileName, Properties.Resources.Culture).ToString().Split('\n');
            string[] lines = new string[input[0].Length];
            int i, j;

            for (j = 0; j < 16; j++)
            {
                lines = input[j].Split(' ');
                for (i = 0; i < lines.Length; i++)
                    currentWorld[i, j] = Convert.ToInt32(lines[i]);
            }
        }
        private void renderLevelStart()
        {
            for (; drawLevelPos < SCREENWIDTH + 2; drawLevelPos++)
                renderLevel();
            player.Left = 3 * TILE;
            player.Top = 13 * TILE;
        }

        private void renderLevel()
        {
            drawLevelPos = drawLevelPos % (SCREENWIDTH + 2);
            for (int i = 0; i < 16; i++)
            {
                if (currentWorld[currentWorldPos, i] != 0)
                {
                    drawLevel[drawLevelPos, i] = new PictureBox();
                    switch (currentWorld[currentWorldPos, i])
                    {
                        case 255: drawLevel[drawLevelPos, i].Image = Properties.Resources.solid; break;
                        case 254: drawLevel[drawLevelPos, i].Image = Properties.Resources.ground; break;
                        case 127: drawLevel[drawLevelPos, i].Image = Properties.Resources.loot; break;
                        case 100: drawLevel[drawLevelPos, i].Image = Properties.Resources.pipeTop; break;
                        case 64: drawLevel[drawLevelPos, i].Image = Properties.Resources.bricks; break;
                        case 5: drawLevel[drawLevelPos, i].Image = Properties.Resources.flag_flag; break;
                    }
                    drawLevel[drawLevelPos, i].Tag = "OnScreen";
                    drawLevel[drawLevelPos, i].SetBounds(TILE * drawLevelPos, TILE * i, TILE, TILE);
                    level.Controls.Add(drawLevel[drawLevelPos, i]);
                }

            }
            currentWorldPos++;
        }

        private void onStartup(object sender, EventArgs e)
        {
            importWorld(1, 1); // saljemo world i stage eventualno
            renderLevelStart();
            playerPosX = player.Left; // neiskorisceno jos
            playerPosY = player.Top; // neiskorisceno jos

            movementTimer.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // ne koriscen trenutno   

        }
    }
}