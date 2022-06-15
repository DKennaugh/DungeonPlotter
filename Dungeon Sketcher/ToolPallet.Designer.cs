namespace Dungeon_Sketcher
{
    partial class ToolPallet
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelect = new System.Windows.Forms.RadioButton();
            this.btnSquare = new System.Windows.Forms.RadioButton();
            this.btnCircle = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnSelect
            // 
            this.btnSelect.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSelect.Checked = true;
            this.btnSelect.Location = new System.Drawing.Point(3, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 24);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.TabStop = true;
            this.btnSelect.Text = "Select";
            this.btnSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.CheckedChanged += new System.EventHandler(this.BtnSelect_CheckedChanged);
            // 
            // btnSquare
            // 
            this.btnSquare.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSquare.Location = new System.Drawing.Point(84, 3);
            this.btnSquare.Name = "btnSquare";
            this.btnSquare.Size = new System.Drawing.Size(75, 24);
            this.btnSquare.TabIndex = 1;
            this.btnSquare.Text = "Rectangle";
            this.btnSquare.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSquare.UseVisualStyleBackColor = true;
            this.btnSquare.CheckedChanged += new System.EventHandler(this.BtnSquare_CheckedChanged);
            // 
            // btnCircle
            // 
            this.btnCircle.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnCircle.Location = new System.Drawing.Point(3, 33);
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(75, 24);
            this.btnCircle.TabIndex = 2;
            this.btnCircle.Text = "Ellipse";
            this.btnCircle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCircle.UseVisualStyleBackColor = true;
            this.btnCircle.CheckedChanged += new System.EventHandler(this.BtnCircle_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton4.Location = new System.Drawing.Point(84, 33);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(75, 24);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "radioButton4";
            this.radioButton4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // ToolPallet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.btnCircle);
            this.Controls.Add(this.btnSquare);
            this.Controls.Add(this.btnSelect);
            this.Name = "ToolPallet";
            this.Size = new System.Drawing.Size(162, 274);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton btnSelect;
        private System.Windows.Forms.RadioButton btnSquare;
        private System.Windows.Forms.RadioButton btnCircle;
        private System.Windows.Forms.RadioButton radioButton4;
    }
}
