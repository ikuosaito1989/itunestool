namespace ITunEsTooL
{
    partial class PicCheck
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PicCheck));
            this.Yes = new System.Windows.Forms.Button();
            this.No = new System.Windows.Forms.Button();
            this.lblSetumei = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.chkJikai = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnNextArtwork = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAlbum = new System.Windows.Forms.Label();
            this.lblArtist = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Yes
            // 
            this.Yes.BackColor = System.Drawing.Color.SkyBlue;
            this.Yes.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Yes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Yes.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Yes.ForeColor = System.Drawing.Color.White;
            this.Yes.Location = new System.Drawing.Point(12, 461);
            this.Yes.Name = "Yes";
            this.Yes.Size = new System.Drawing.Size(226, 27);
            this.Yes.TabIndex = 12;
            this.Yes.Text = "はい";
            this.Yes.UseVisualStyleBackColor = false;
            this.Yes.Click += new System.EventHandler(this.Yes_Click);
            // 
            // No
            // 
            this.No.BackColor = System.Drawing.Color.SkyBlue;
            this.No.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.No.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.No.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.No.ForeColor = System.Drawing.Color.White;
            this.No.Location = new System.Drawing.Point(250, 461);
            this.No.Name = "No";
            this.No.Size = new System.Drawing.Size(226, 27);
            this.No.TabIndex = 13;
            this.No.Text = "いいえ";
            this.No.UseVisualStyleBackColor = false;
            this.No.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblSetumei
            // 
            this.lblSetumei.BackColor = System.Drawing.Color.Transparent;
            this.lblSetumei.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSetumei.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSetumei.Location = new System.Drawing.Point(12, 3);
            this.lblSetumei.Name = "lblSetumei";
            this.lblSetumei.Size = new System.Drawing.Size(464, 22);
            this.lblSetumei.TabIndex = 14;
            this.lblSetumei.Text = "この画像を設定してもよろしいですか？";
            this.lblSetumei.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label43
            // 
            this.label43.BackColor = System.Drawing.Color.Transparent;
            this.label43.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label43.ForeColor = System.Drawing.Color.Black;
            this.label43.Location = new System.Drawing.Point(31, 438);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(111, 19);
            this.label43.TabIndex = 30;
            this.label43.Text = "次回から表示しない";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label43.Click += new System.EventHandler(this.label43_Click);
            // 
            // chkJikai
            // 
            this.chkJikai.AutoSize = true;
            this.chkJikai.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkJikai.ForeColor = System.Drawing.Color.DimGray;
            this.chkJikai.Location = new System.Drawing.Point(13, 441);
            this.chkJikai.Name = "chkJikai";
            this.chkJikai.Size = new System.Drawing.Size(15, 14);
            this.chkJikai.TabIndex = 29;
            this.chkJikai.UseVisualStyleBackColor = true;
            // 
            // btnNextArtwork
            // 
            this.btnNextArtwork.BackColor = System.Drawing.Color.SkyBlue;
            this.btnNextArtwork.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnNextArtwork.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextArtwork.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNextArtwork.ForeColor = System.Drawing.Color.White;
            this.btnNextArtwork.Location = new System.Drawing.Point(234, 396);
            this.btnNextArtwork.Name = "btnNextArtwork";
            this.btnNextArtwork.Size = new System.Drawing.Size(220, 27);
            this.btnNextArtwork.TabIndex = 31;
            this.btnNextArtwork.Text = "次のアルバムアートワークを検索する";
            this.btnNextArtwork.UseVisualStyleBackColor = false;
            this.btnNextArtwork.Click += new System.EventHandler(this.btnNextArtwork_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(13, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 12);
            this.label1.TabIndex = 32;
            this.label1.Text = "Album :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(13, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 12);
            this.label2.TabIndex = 33;
            this.label2.Text = "Artist  : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(13, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 12);
            this.label3.TabIndex = 34;
            this.label3.Text = "Name  : ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAlbum
            // 
            this.lblAlbum.AutoSize = true;
            this.lblAlbum.BackColor = System.Drawing.Color.Transparent;
            this.lblAlbum.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblAlbum.ForeColor = System.Drawing.Color.DimGray;
            this.lblAlbum.Location = new System.Drawing.Point(73, 29);
            this.lblAlbum.Name = "lblAlbum";
            this.lblAlbum.Size = new System.Drawing.Size(43, 12);
            this.lblAlbum.TabIndex = 35;
            this.lblAlbum.Text = "Album :";
            this.lblAlbum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.BackColor = System.Drawing.Color.Transparent;
            this.lblArtist.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblArtist.ForeColor = System.Drawing.Color.DimGray;
            this.lblArtist.Location = new System.Drawing.Point(73, 47);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(43, 12);
            this.lblArtist.TabIndex = 36;
            this.lblArtist.Text = "Album :";
            this.lblArtist.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Meiryo UI", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblName.ForeColor = System.Drawing.Color.DimGray;
            this.lblName.Location = new System.Drawing.Point(73, 65);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(43, 12);
            this.lblName.TabIndex = 37;
            this.lblName.Text = "Album :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(12, 87);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(464, 348);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // PicCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(488, 496);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblArtist);
            this.Controls.Add(this.lblAlbum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNextArtwork);
            this.Controls.Add(this.label43);
            this.Controls.Add(this.chkJikai);
            this.Controls.Add(this.lblSetumei);
            this.Controls.Add(this.No);
            this.Controls.Add(this.Yes);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PicCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "画像の設定";
            this.Load += new System.EventHandler(this.PicCheck_Load);
            this.Shown += new System.EventHandler(this.PicCheck_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Yes;
        private System.Windows.Forms.Button No;
        private System.Windows.Forms.Label lblSetumei;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.CheckBox chkJikai;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnNextArtwork;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAlbum;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.Label lblName;
    }
}