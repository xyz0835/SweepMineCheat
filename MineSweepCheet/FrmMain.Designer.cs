namespace MineSweepCheet
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.picZone = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.picMine = new System.Windows.Forms.PictureBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picZone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMine)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "重置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // picZone
            // 
            this.picZone.Location = new System.Drawing.Point(12, 138);
            this.picZone.Name = "picZone";
            this.picZone.Size = new System.Drawing.Size(542, 290);
            this.picZone.TabIndex = 1;
            this.picZone.TabStop = false;
            this.picZone.Click += new System.EventHandler(this.picZone_Click);
            this.picZone.DoubleClick += new System.EventHandler(this.picZone_DoubleClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "获取图片";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "X:";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(51, 71);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(25, 21);
            this.txtX.TabIndex = 4;
            this.txtX.Text = "0";
            this.txtX.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(111, 71);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(25, 21);
            this.txtY.TabIndex = 6;
            this.txtY.Text = "0";
            this.txtY.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Y:";
            // 
            // picMine
            // 
            this.picMine.Location = new System.Drawing.Point(144, 68);
            this.picMine.Name = "picMine";
            this.picMine.Size = new System.Drawing.Size(44, 34);
            this.picMine.TabIndex = 7;
            this.picMine.TabStop = false;
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(196, 53);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(358, 79);
            this.txtResult.TabIndex = 8;
            this.txtResult.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtResult_KeyDown);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(174, 24);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "扫一次";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(255, 24);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "扫雷";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 444);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.picMine);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.picZone);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "扫雷外挂测试";
            ((System.ComponentModel.ISupportInitialize)(this.picZone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox picZone;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picMine;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

