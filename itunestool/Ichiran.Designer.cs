namespace ITunEsTooL
{
    partial class Ichiran
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ichiran));
            this.lstIchiran = new System.Windows.Forms.ListBox();
            this.OK = new System.Windows.Forms.Button();
            this.lblSetumei = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSerch = new System.Windows.Forms.TextBox();
            this.btnSerch = new System.Windows.Forms.Button();
            this.treeIchiran = new System.Windows.Forms.TreeView();
            this.chkSentaku = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lstIchiran
            // 
            this.lstIchiran.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstIchiran.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstIchiran.FormattingEnabled = true;
            this.lstIchiran.HorizontalScrollbar = true;
            this.lstIchiran.ItemHeight = 15;
            this.lstIchiran.Location = new System.Drawing.Point(7, 86);
            this.lstIchiran.Name = "lstIchiran";
            this.lstIchiran.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstIchiran.Size = new System.Drawing.Size(401, 229);
            this.lstIchiran.TabIndex = 0;
            this.lstIchiran.Click += new System.EventHandler(this.lstIchiran_Click);
            // 
            // OK
            // 
            this.OK.BackColor = System.Drawing.Color.SkyBlue;
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OK.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.OK.Location = new System.Drawing.Point(233, 334);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(85, 27);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = false;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // lblSetumei
            // 
            this.lblSetumei.BackColor = System.Drawing.Color.SteelBlue;
            this.lblSetumei.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSetumei.ForeColor = System.Drawing.Color.White;
            this.lblSetumei.Location = new System.Drawing.Point(7, 4);
            this.lblSetumei.Name = "lblSetumei";
            this.lblSetumei.Size = new System.Drawing.Size(401, 33);
            this.lblSetumei.TabIndex = 2;
            this.lblSetumei.Text = "説明";
            this.lblSetumei.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSetumei.Click += new System.EventHandler(this.lblSetumei_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.SkyBlue;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDelete.Location = new System.Drawing.Point(7, 333);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 28);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.button1_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SkyBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(324, 333);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 27);
            this.button1.TabIndex = 4;
            this.button1.Text = "キャンセル";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // txtSerch
            // 
            this.txtSerch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtSerch.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSerch.Location = new System.Drawing.Point(7, 57);
            this.txtSerch.Name = "txtSerch";
            this.txtSerch.Size = new System.Drawing.Size(320, 21);
            this.txtSerch.TabIndex = 5;
            this.txtSerch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSerch_KeyDown);
            this.txtSerch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSerch_KeyPress);
            // 
            // btnSerch
            // 
            this.btnSerch.BackColor = System.Drawing.Color.SkyBlue;
            this.btnSerch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerch.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSerch.Location = new System.Drawing.Point(334, 58);
            this.btnSerch.Name = "btnSerch";
            this.btnSerch.Size = new System.Drawing.Size(75, 23);
            this.btnSerch.TabIndex = 6;
            this.btnSerch.Text = "検索";
            this.btnSerch.UseVisualStyleBackColor = false;
            this.btnSerch.Click += new System.EventHandler(this.btnSerch_Click);
            // 
            // treeIchiran
            // 
            this.treeIchiran.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.treeIchiran.Location = new System.Drawing.Point(108, 341);
            this.treeIchiran.Name = "treeIchiran";
            this.treeIchiran.Size = new System.Drawing.Size(34, 19);
            this.treeIchiran.TabIndex = 7;
            this.treeIchiran.Visible = false;
            // 
            // chkSentaku
            // 
            this.chkSentaku.AutoSize = true;
            this.chkSentaku.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkSentaku.Location = new System.Drawing.Point(7, 38);
            this.chkSentaku.Name = "chkSentaku";
            this.chkSentaku.Size = new System.Drawing.Size(179, 18);
            this.chkSentaku.TabIndex = 8;
            this.chkSentaku.Text = "検索する際に選択状態を保持する";
            this.chkSentaku.UseVisualStyleBackColor = true;
            // 
            // Ichiran
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(417, 366);
            this.ControlBox = false;
            this.Controls.Add(this.chkSentaku);
            this.Controls.Add(this.treeIchiran);
            this.Controls.Add(this.btnSerch);
            this.Controls.Add(this.txtSerch);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lblSetumei);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.lstIchiran);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("HG創英角ﾎﾟｯﾌﾟ体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Ichiran";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Ichiran_FormClosed);
            this.Load += new System.EventHandler(this.Ichiran_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstIchiran;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label lblSetumei;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSerch;
        private System.Windows.Forms.Button btnSerch;
        private System.Windows.Forms.TreeView treeIchiran;
        private System.Windows.Forms.CheckBox chkSentaku;
    }
}