namespace fs_32
{
    partial class Top_Menu
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.bottom_bar = new DevExpress.XtraEditors.PanelControl();
            this.right_bar = new DevExpress.XtraEditors.PanelControl();
            this.Left_bar = new DevExpress.XtraEditors.PanelControl();
            this.Title = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bottom_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.right_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Left_bar)).BeginInit();
            this.SuspendLayout();
            // 
            // bottom_bar
            // 
            this.bottom_bar.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(171)))), ((int)(((byte)(202)))));
            this.bottom_bar.Appearance.Options.UseBackColor = true;
            this.bottom_bar.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.bottom_bar.Location = new System.Drawing.Point(22, 31);
            this.bottom_bar.Name = "bottom_bar";
            this.bottom_bar.Size = new System.Drawing.Size(80, 1);
            this.bottom_bar.TabIndex = 11;
            this.bottom_bar.Visible = false;
            // 
            // right_bar
            // 
            this.right_bar.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(171)))), ((int)(((byte)(202)))));
            this.right_bar.Appearance.Options.UseBackColor = true;
            this.right_bar.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.right_bar.Location = new System.Drawing.Point(124, 9);
            this.right_bar.Name = "right_bar";
            this.right_bar.Size = new System.Drawing.Size(1, 20);
            this.right_bar.TabIndex = 10;
            // 
            // Left_bar
            // 
            this.Left_bar.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(144)))), ((int)(((byte)(185)))));
            this.Left_bar.Appearance.Options.UseBackColor = true;
            this.Left_bar.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.Left_bar.Location = new System.Drawing.Point(0, 9);
            this.Left_bar.Name = "Left_bar";
            this.Left_bar.Size = new System.Drawing.Size(1, 20);
            this.Left_bar.TabIndex = 9;
            // 
            // Title
            // 
            this.Title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Title.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Title.ForeColor = System.Drawing.Color.White;
            this.Title.Location = new System.Drawing.Point(0, 9);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(125, 19);
            this.Title.TabIndex = 12;
            this.Title.Text = "PC원클릭점검";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Title.Click += new System.EventHandler(this.Title_Click);
            this.Title.MouseEnter += new System.EventHandler(this.Top_Menu_MouseEnter);
            this.Title.MouseLeave += new System.EventHandler(this.Top_Menu_MouseLeave);
            // 
            // Top_Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(75)))), ((int)(((byte)(143)))));
            this.Controls.Add(this.bottom_bar);
            this.Controls.Add(this.right_bar);
            this.Controls.Add(this.Left_bar);
            this.Controls.Add(this.Title);
            this.Name = "Top_Menu";
            this.Size = new System.Drawing.Size(125, 40);
            this.Click += new System.EventHandler(this.Top_Menu_Click);
            this.MouseEnter += new System.EventHandler(this.Top_Menu_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Top_Menu_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.bottom_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.right_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Left_bar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl bottom_bar;
        private DevExpress.XtraEditors.PanelControl right_bar;
        private DevExpress.XtraEditors.PanelControl Left_bar;
        private System.Windows.Forms.Label Title;
    }
}
