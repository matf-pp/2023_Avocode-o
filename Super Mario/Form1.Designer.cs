namespace Super_Mario
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            movementTimer = new System.Windows.Forms.Timer(components);
            player = new PictureBox();
            level = new Panel();
            label2 = new Label();
            label1 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            timer2 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)player).BeginInit();
            level.SuspendLayout();
            SuspendLayout();
            // 
            // movementTimer
            // 
            movementTimer.Interval = 20;
            movementTimer.Tick += timerTick;
            // 
            // player
            // 
            player.BackColor = Color.Transparent;
            player.Image = Properties.Resources.mario_small;
            player.Location = new Point(607, 314);
            player.Name = "player";
            player.Size = new Size(48, 48);
            player.TabIndex = 0;
            player.TabStop = false;
            // 
            // level
            // 
            level.Controls.Add(label2);
            level.Controls.Add(label1);
            level.Controls.Add(player);
            level.Location = new Point(0, 0);
            level.Margin = new Padding(0);
            level.Name = "level";
            level.Size = new Size(1280, 768);
            level.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 60);
            label2.Name = "label2";
            label2.Size = new Size(50, 20);
            label2.TabIndex = 5;
            label2.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 28);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 4;
            label1.Tag = "";
            label1.Text = "label1";
            // 
            // timer2
            // 
            timer2.Tick += timer2_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 768);
            Controls.Add(level);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Super Mario";
            Load += onStartup;
            KeyDown += keyDown;
            KeyUp += keyUp;
            ((System.ComponentModel.ISupportInitialize)player).EndInit();
            level.ResumeLayout(false);
            level.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer movementTimer;
        public PictureBox player;
        public Panel level;
        private Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private Label label2;
    }
}