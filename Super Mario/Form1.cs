

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
        public const int SCREENWIDTH = 28; // menja koliko tajlova je sirok ekran, veci broj -> sporije radi

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

        int[,] currentWorld = new int[256, 16]; // najveci nivo je 256 tajlova sirok
        bool moveLeft, moveRight, sprinting;
        PictureBox[,] drawLevel = new PictureBox[SCREENWIDTH, 16]; // ono sto se vidi
        private void timerTick(object sender, EventArgs e) // movementTimer event
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

            if (player.Left >= (this.Width / 2 - TILE) && moveRight)
                foreach (Control x in level.Controls)
                {
                    if (x.Tag == "OnScreen")
                    {
                        x.Left -= speed;
                        if (x.Left < -TILE)
                        {
                            x.Dispose();
                            renderLevelRTL();
                            drawLevelPos++;
                            currentWorldPos++;
                            break;
                        }
                    }
                }


            label1.Text = (playerPosX / TILE).ToString();
            label2.Text = (currentWorldPos).ToString();
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
                moveRight = false;
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
            for (; drawLevelPos < SCREENWIDTH; drawLevelPos++)
                renderLevelLTR();
            player.Left = 3 * TILE;
            player.Top = 13 * TILE;

        }
        /*
         0 Empty
         2 GroundBlock
         4 BrickBlock
         5 BrickHasCoin
         6 BrickHasStar
         8 SolidBlock
         
         10 PipeTopLeft
         11 PipeTopRight
         12 PipeLeftTop
         13 PipeLeftBottom
         14 PipeVerticalLeft
         15 PipeVerticalRight
         16 PipeHorizontalTop
         17 PipeHorizontalBottom
         18 PipeConnectorTop
         19 PipeConnectorBottom
         
         30 Goomba
         31 KoopaLand

         120 FlagPole
         121 FlagFlag
         122 FlagTip
         123 Castle
         
         127 QBlock
         128 QBlockHasMushroom
         130 QBlockUsed
         */
        private void setTileData(int i)
        {
            drawLevel[drawLevelPos, i] = new PictureBox();
            drawLevel[drawLevelPos, i].Tag = "OnScreen";
            switch (currentWorld[currentWorldPos, i])
            {
                case 2: drawLevel[drawLevelPos, i].Image = Properties.Resources.ground; break;
                case 4: drawLevel[drawLevelPos, i].Image = Properties.Resources.bricks; break;
                case 5: drawLevel[drawLevelPos, i].Image = Properties.Resources.bricks; break;
                case 6: drawLevel[drawLevelPos, i].Image = Properties.Resources.bricks; break;
                case 8: drawLevel[drawLevelPos, i].Image = Properties.Resources.solid; break;
                case 10: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeTopLeft; break;
                case 11: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeTopRight; break;
                case 12: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeLeftTop; break;
                case 13: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeLeftBottom; break;
                case 14: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeVerticalLeft; break;
                case 15: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeVerticalRight; break;
                case 16: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeHorizontalTop; break;
                case 17: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeHorizontalBottom; break;
                case 18: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeConnectorTop; break;
                case 19: drawLevel[drawLevelPos, i].Image = Properties.Resources.PipeConnectorBottom; break;
                case 30: drawLevel[drawLevelPos, i].Image = Properties.Resources.Goomba; break;
                case 31:
                    {
                        drawLevel[drawLevelPos, i].Image = Properties.Resources.KoopaLand;
                        drawLevel[drawLevelPos, i].SetBounds(TILE * drawLevelPos, TILE * i - TILE / 2, TILE + TILE / 2, TILE + TILE / 2);
                    }
                    break;
                case 127: drawLevel[drawLevelPos, i].Image = Properties.Resources.loot; break;
                case 128: drawLevel[drawLevelPos, i].Image = Properties.Resources.loot; break;
                case 120: drawLevel[drawLevelPos, i].Image = Properties.Resources.flag_pole; break;
                case 121: drawLevel[drawLevelPos, i].Image = Properties.Resources.flag_flag; break;
                case 122: drawLevel[drawLevelPos, i].Image = Properties.Resources.flag_tip; break;
                case 123:
                    {
                        drawLevel[drawLevelPos, i].SetBounds(TILE * drawLevelPos, TILE * i, TILE * 5, TILE * 5);
                        if (currentWorld[currentWorldPos - 1, i] != 123 && currentWorld[currentWorldPos, i - 1] != 123)
                            drawLevel[drawLevelPos, i].Image = Properties.Resources.castle_small;
                    }
                    break;
            }
        }
        private void renderLevelLTR()
        {
            drawLevelPos = drawLevelPos % (SCREENWIDTH);
            for (int i = 0; i < 16; i++)
            {
                if (currentWorld[currentWorldPos, i] != 0)
                {
                    setTileData(i);
                    if (currentWorld[currentWorldPos, i] != 123 && currentWorld[currentWorldPos, i] != 31)
                        drawLevel[drawLevelPos, i].SetBounds(TILE * drawLevelPos, TILE * i, TILE, TILE);
                    level.Controls.Add(drawLevel[drawLevelPos, i]);
                }

            }
            currentWorldPos++;
        } // za generisanje tokom kretanja

        private void renderLevelRTL() // za generisanje inicijalnog ekrana
        {
            drawLevelPos = drawLevelPos % (SCREENWIDTH);
            for (int i = 0; i < 16; i++)
            {
                if (currentWorld[currentWorldPos, i] != 0)
                {
                    setTileData(i);
                    level.Controls.Add(drawLevel[drawLevelPos, i]);
                    if (currentWorld[currentWorldPos, i] != 123)
                        drawLevel[drawLevelPos, i].SetBounds(TILE * (SCREENWIDTH - 1), TILE * i, TILE, TILE);
                }
            }
            
        }

        private void onStartup(object sender, EventArgs e)
        {
            importWorld(1, 1); // saljemo world i stage eventualno
            renderLevelStart();
            playerPosX = player.Left; // neiskorisceno jos
            playerPosY = player.Top; // neiskorisceno jos

            movementTimer.Start();
        }


    }
}