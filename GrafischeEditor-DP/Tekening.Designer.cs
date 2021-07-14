
namespace GrafischeEditor_DP
{
    partial class Tekening
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.Bestand = new System.Windows.Forms.ToolStripMenuItem();
            this.NieuwBestand = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenBestand = new System.Windows.Forms.ToolStripMenuItem();
            this.BestandOpslaan = new System.Windows.Forms.ToolStripMenuItem();
            this.BestandOpslaanAls = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButtonPointer = new System.Windows.Forms.Button();
            this.ButtonSquare = new System.Windows.Forms.Button();
            this.ButtonEllipse = new System.Windows.Forms.Button();
            this.DrawPanel = new System.Windows.Forms.Panel();
            this.ButtonRemove = new System.Windows.Forms.Button();
            this.MenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Bestand});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(1044, 24);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip";
            // 
            // Bestand
            // 
            this.Bestand.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NieuwBestand,
            this.OpenBestand,
            this.BestandOpslaan,
            this.BestandOpslaanAls});
            this.Bestand.Name = "Bestand";
            this.Bestand.Size = new System.Drawing.Size(61, 20);
            this.Bestand.Text = "Bestand";
            // 
            // NieuwBestand
            // 
            this.NieuwBestand.Name = "NieuwBestand";
            this.NieuwBestand.Size = new System.Drawing.Size(134, 22);
            this.NieuwBestand.Text = "Nieuw";
            // 
            // OpenBestand
            // 
            this.OpenBestand.Name = "OpenBestand";
            this.OpenBestand.Size = new System.Drawing.Size(134, 22);
            this.OpenBestand.Text = "Openen";
            // 
            // BestandOpslaan
            // 
            this.BestandOpslaan.Name = "BestandOpslaan";
            this.BestandOpslaan.Size = new System.Drawing.Size(134, 22);
            this.BestandOpslaan.Text = "Opslaan";
            // 
            // BestandOpslaanAls
            // 
            this.BestandOpslaanAls.Name = "BestandOpslaanAls";
            this.BestandOpslaanAls.Size = new System.Drawing.Size(134, 22);
            this.BestandOpslaanAls.Text = "Opslaan als";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ButtonRemove);
            this.panel1.Controls.Add(this.ButtonPointer);
            this.panel1.Controls.Add(this.ButtonSquare);
            this.panel1.Controls.Add(this.ButtonEllipse);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(115, 602);
            this.panel1.TabIndex = 1;
            // 
            // ButtonPointer
            // 
            this.ButtonPointer.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonPointer.Location = new System.Drawing.Point(11, 12);
            this.ButtonPointer.Name = "ButtonPointer";
            this.ButtonPointer.Size = new System.Drawing.Size(90, 65);
            this.ButtonPointer.TabIndex = 2;
            this.ButtonPointer.Text = "8";
            this.ButtonPointer.UseVisualStyleBackColor = true;
            this.ButtonPointer.Click += new System.EventHandler(this.ButtonPointer_Click);
            // 
            // ButtonSquare
            // 
            this.ButtonSquare.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonSquare.Location = new System.Drawing.Point(11, 154);
            this.ButtonSquare.Name = "ButtonSquare";
            this.ButtonSquare.Size = new System.Drawing.Size(90, 65);
            this.ButtonSquare.TabIndex = 1;
            this.ButtonSquare.Text = "o";
            this.ButtonSquare.UseVisualStyleBackColor = true;
            this.ButtonSquare.Click += new System.EventHandler(this.ButtonSquare_Click);
            // 
            // ButtonEllipse
            // 
            this.ButtonEllipse.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonEllipse.Location = new System.Drawing.Point(11, 83);
            this.ButtonEllipse.Name = "ButtonEllipse";
            this.ButtonEllipse.Size = new System.Drawing.Size(90, 65);
            this.ButtonEllipse.TabIndex = 0;
            this.ButtonEllipse.Text = "m";
            this.ButtonEllipse.UseVisualStyleBackColor = true;
            this.ButtonEllipse.Click += new System.EventHandler(this.ButtonEllipse_Click);
            // 
            // DrawPanel
            // 
            this.DrawPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DrawPanel.Location = new System.Drawing.Point(121, 27);
            this.DrawPanel.Name = "DrawPanel";
            this.DrawPanel.Size = new System.Drawing.Size(911, 602);
            this.DrawPanel.TabIndex = 2;
            this.DrawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawPanel_Paint);
            this.DrawPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_Click);
            this.DrawPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseDown);
            this.DrawPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseMove);
            this.DrawPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseUp);
            // 
            // ButtonRemove
            // 
            this.ButtonRemove.Font = new System.Drawing.Font("Wingdings 2", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonRemove.Location = new System.Drawing.Point(11, 225);
            this.ButtonRemove.Name = "ButtonRemove";
            this.ButtonRemove.Size = new System.Drawing.Size(90, 65);
            this.ButtonRemove.TabIndex = 3;
            this.ButtonRemove.Text = "3";
            this.ButtonRemove.UseVisualStyleBackColor = true;
            this.ButtonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // Tekening
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 641);
            this.Controls.Add(this.DrawPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "Tekening";
            this.Text = "Grafische Editor";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Bestand;
        private System.Windows.Forms.ToolStripMenuItem NieuwBestand;
        private System.Windows.Forms.ToolStripMenuItem OpenBestand;
        private System.Windows.Forms.ToolStripMenuItem BestandOpslaan;
        private System.Windows.Forms.ToolStripMenuItem BestandOpslaanAls;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ButtonPointer;
        private System.Windows.Forms.Button ButtonSquare;
        private System.Windows.Forms.Button ButtonEllipse;
        private System.Windows.Forms.Panel DrawPanel;
        private System.Windows.Forms.Button ButtonRemove;
    }
}