namespace ITunEsTooL
{
    partial class CrashReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrashReport));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSyosai = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnNotSend = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(135, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "申し訳ありません。ITunEsTooLで内部エラーが発生しました。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(135, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(275, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "問題の原因特定と解決のためにご協力頂ける方は、";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(135, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "エラーメッセージの送信をお願い致します。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(135, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "※";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(155, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "個人情報";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(224, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "は含まれておりません。";
            // 
            // btnSyosai
            // 
            this.btnSyosai.BackColor = System.Drawing.Color.SkyBlue;
            this.btnSyosai.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnSyosai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyosai.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSyosai.ForeColor = System.Drawing.Color.White;
            this.btnSyosai.Location = new System.Drawing.Point(13, 240);
            this.btnSyosai.Name = "btnSyosai";
            this.btnSyosai.Size = new System.Drawing.Size(110, 27);
            this.btnSyosai.TabIndex = 12;
            this.btnSyosai.Text = "詳細を表示";
            this.btnSyosai.UseVisualStyleBackColor = false;
            this.btnSyosai.Click += new System.EventHandler(this.btnSyosai_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.SkyBlue;
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(370, 240);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(110, 27);
            this.btnSend.TabIndex = 13;
            this.btnSend.Text = "送信する";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnNotSend
            // 
            this.btnNotSend.BackColor = System.Drawing.Color.SkyBlue;
            this.btnNotSend.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnNotSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotSend.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNotSend.ForeColor = System.Drawing.Color.White;
            this.btnNotSend.Location = new System.Drawing.Point(254, 240);
            this.btnNotSend.Name = "btnNotSend";
            this.btnNotSend.Size = new System.Drawing.Size(110, 27);
            this.btnNotSend.TabIndex = 14;
            this.btnNotSend.Text = "送信しない";
            this.btnNotSend.UseVisualStyleBackColor = false;
            this.btnNotSend.Click += new System.EventHandler(this.btnNotSend_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ITunEsTooL.Properties.Resources.mark_warning01;
            this.pictureBox1.Location = new System.Drawing.Point(13, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(110, 113);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLabel2.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.linkLabel2.Location = new System.Drawing.Point(42, 199);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(401, 19);
            this.linkLabel2.TabIndex = 15;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "picpic - アルバムを検索してアルバムアートワークを見つけよう！";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(32, 178);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(423, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "ITunEsTooLで画像が設定できない場合は、picpicをで画像を検索してください。";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // CrashReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(496, 274);
            this.ControlBox = false;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.btnNotSend);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnSyosai);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CrashReport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Z";
            this.Load += new System.EventHandler(this.CrashReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSyosai;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnNotSend;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label9;
    }
}