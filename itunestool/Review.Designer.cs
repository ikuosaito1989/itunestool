namespace ITunEsTooL
{
    partial class Review
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Review));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtReview = new System.Windows.Forms.TextBox();
            this.btnNotSend = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.picReview = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chkJikai = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSyosai = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picReview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(189, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ITunEsTooLをご利用頂き、誠にありがとうございます！";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(189, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "ITunEsTooLの使い勝手はいかがでしょうか？";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(189, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(346, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "もしお時間がありましたら、皆様にレビューをして頂ければ幸いです。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(189, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(235, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "今後の開発時の参考にさせて頂きますので、";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(189, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "よろしくお願いします。";
            // 
            // txtReview
            // 
            this.txtReview.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtReview.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtReview.Location = new System.Drawing.Point(12, 292);
            this.txtReview.MaxLength = 800;
            this.txtReview.Multiline = true;
            this.txtReview.Name = "txtReview";
            this.txtReview.Size = new System.Drawing.Size(523, 85);
            this.txtReview.TabIndex = 2;
            this.txtReview.Text = resources.GetString("txtReview.Text");
            // 
            // btnNotSend
            // 
            this.btnNotSend.BackColor = System.Drawing.Color.SkyBlue;
            this.btnNotSend.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnNotSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotSend.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNotSend.ForeColor = System.Drawing.Color.White;
            this.btnNotSend.Location = new System.Drawing.Point(424, 407);
            this.btnNotSend.Name = "btnNotSend";
            this.btnNotSend.Size = new System.Drawing.Size(110, 27);
            this.btnNotSend.TabIndex = 5;
            this.btnNotSend.Text = "送信しない";
            this.btnNotSend.UseVisualStyleBackColor = false;
            this.btnNotSend.Click += new System.EventHandler(this.btnNotSend_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.SkyBlue;
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(297, 407);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(110, 27);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "送信する";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtFrom
            // 
            this.txtFrom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtFrom.Location = new System.Drawing.Point(12, 191);
            this.txtFrom.MaxLength = 800;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(523, 23);
            this.txtFrom.TabIndex = 1;
            this.txtFrom.Text = "あああ\r\nいいい\r\nううう\r\nえええ\r\nおおお\r\n";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(12, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(237, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "・Mail（半角）※未記入でも問題ありません。";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(12, 221);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 13);
            this.label7.TabIndex = 20;
            // 
            // picReview
            // 
            this.picReview.Image = global::ITunEsTooL.Properties.Resources._3;
            this.picReview.Location = new System.Drawing.Point(16, 20);
            this.picReview.Name = "picReview";
            this.picReview.Size = new System.Drawing.Size(76, 16);
            this.picReview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picReview.TabIndex = 19;
            this.picReview.TabStop = false;
            this.picReview.Click += new System.EventHandler(this.picReview_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ITunEsTooL.Properties.Resources.business_002b;
            this.pictureBox1.Location = new System.Drawing.Point(12, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(154, 147);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // chkJikai
            // 
            this.chkJikai.AutoSize = true;
            this.chkJikai.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkJikai.ForeColor = System.Drawing.Color.Black;
            this.chkJikai.Location = new System.Drawing.Point(11, 383);
            this.chkJikai.Name = "chkJikai";
            this.chkJikai.Size = new System.Drawing.Size(120, 18);
            this.chkJikai.TabIndex = 3;
            this.chkJikai.Text = "次回から表示しない。";
            this.chkJikai.UseVisualStyleBackColor = true;
            this.chkJikai.CheckedChanged += new System.EventHandler(this.chkJikai_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.picReview);
            this.groupBox1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(3, 219);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(541, 46);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "・評価をお願いします（星をクリックして評価を決めて下さい。）";
            // 
            // btnSyosai
            // 
            this.btnSyosai.BackColor = System.Drawing.Color.SkyBlue;
            this.btnSyosai.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnSyosai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyosai.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSyosai.ForeColor = System.Drawing.Color.White;
            this.btnSyosai.Location = new System.Drawing.Point(11, 407);
            this.btnSyosai.Name = "btnSyosai";
            this.btnSyosai.Size = new System.Drawing.Size(110, 27);
            this.btnSyosai.TabIndex = 22;
            this.btnSyosai.Text = "詳細を表示";
            this.btnSyosai.UseVisualStyleBackColor = false;
            this.btnSyosai.Click += new System.EventHandler(this.btnSyosai_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(10, 277);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(174, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "・こちらにご意見お願い致します。";
            // 
            // Review
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(547, 446);
            this.ControlBox = false;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnSyosai);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkJikai);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnNotSend);
            this.Controls.Add(this.txtReview);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Review";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ITunEsTooLレビュー";
            this.Load += new System.EventHandler(this.Review_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picReview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.TextBox txtReview;
        private System.Windows.Forms.Button btnNotSend;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picReview;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkJikai;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSyosai;
        private System.Windows.Forms.Label label8;
    }
}