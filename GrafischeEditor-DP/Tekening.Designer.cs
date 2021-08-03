
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tekening));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.Bestand = new System.Windows.Forms.ToolStripMenuItem();
            this.NieuwBestand = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenBestand = new System.Windows.Forms.ToolStripMenuItem();
            this.BestandOpslaan = new System.Windows.Forms.ToolStripMenuItem();
            this.bewerkenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ongedaanMakenCTRLYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opnieuwUitvoeren = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButtonResize = new System.Windows.Forms.Button();
            this.ButtonRemove = new System.Windows.Forms.Button();
            this.ButtonPointer = new System.Windows.Forms.Button();
            this.ButtonSquare = new System.Windows.Forms.Button();
            this.ButtonEllipse = new System.Windows.Forms.Button();
            this.DrawPanel = new System.Windows.Forms.Panel();
            this.bestandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Bestand,
            this.bewerkenToolStripMenuItem});
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
            this.bestandToolStripMenuItem});
            this.Bestand.Name = "Bestand";
            this.Bestand.Size = new System.Drawing.Size(61, 20);
            this.Bestand.Text = "Bestand";
            // 
            // NieuwBestand
            // 
            this.NieuwBestand.Name = "NieuwBestand";
            this.NieuwBestand.Size = new System.Drawing.Size(180, 22);
            this.NieuwBestand.Text = "Nieuw";
            this.NieuwBestand.Click += new System.EventHandler(this.NieuwBestand_Click);
            // 
            // OpenBestand
            // 
            this.OpenBestand.Name = "OpenBestand";
            this.OpenBestand.Size = new System.Drawing.Size(180, 22);
            this.OpenBestand.Text = "Openen";
            this.OpenBestand.Click += new System.EventHandler(this.OpenBestand_Click);
            // 
            // BestandOpslaan
            // 
            this.BestandOpslaan.Name = "BestandOpslaan";
            this.BestandOpslaan.Size = new System.Drawing.Size(180, 22);
            this.BestandOpslaan.Text = "Opslaan";
            this.BestandOpslaan.Click += new System.EventHandler(this.BestandOpslaan_Click);
            // 
            // bewerkenToolStripMenuItem
            // 
            this.bewerkenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ongedaanMakenCTRLYToolStripMenuItem,
            this.opnieuwUitvoeren});
            this.bewerkenToolStripMenuItem.Name = "bewerkenToolStripMenuItem";
            this.bewerkenToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.bewerkenToolStripMenuItem.Text = "Bewerken";
            // 
            // ongedaanMakenCTRLYToolStripMenuItem
            // 
            this.ongedaanMakenCTRLYToolStripMenuItem.Name = "ongedaanMakenCTRLYToolStripMenuItem";
            this.ongedaanMakenCTRLYToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.ongedaanMakenCTRLYToolStripMenuItem.Text = "Ongedaan maken (CTRL+ Z)";
            // 
            // opnieuwUitvoeren
            // 
            this.opnieuwUitvoeren.Name = "opnieuwUitvoeren";
            this.opnieuwUitvoeren.Size = new System.Drawing.Size(228, 22);
            this.opnieuwUitvoeren.Text = "Opnieuw uitvoeren (CTRL+Y)";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ButtonResize);
            this.panel1.Controls.Add(this.ButtonRemove);
            this.panel1.Controls.Add(this.ButtonPointer);
            this.panel1.Controls.Add(this.ButtonSquare);
            this.panel1.Controls.Add(this.ButtonEllipse);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(115, 602);
            this.panel1.TabIndex = 1;
            // 
            // ButtonResize
            // 
            this.ButtonResize.Font = new System.Drawing.Font("Webdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonResize.Image = global::GrafischeEditor_DP.Properties.Resources.resize_minimum_arrow_small_7_16467;
            this.ButtonResize.Location = new System.Drawing.Point(11, 227);
            this.ButtonResize.Name = "ButtonResize";
            this.ButtonResize.Size = new System.Drawing.Size(90, 65);
            this.ButtonResize.TabIndex = 4;
            this.ButtonResize.UseVisualStyleBackColor = true;
            this.ButtonResize.Click += new System.EventHandler(this.ButtonResize_Click);
            // 
            // ButtonRemove
            // 
            this.ButtonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonRemove.Image = global::GrafischeEditor_DP.Properties.Resources.filled_trash;
            this.ButtonRemove.Location = new System.Drawing.Point(11, 298);
            this.ButtonRemove.Name = "ButtonRemove";
            this.ButtonRemove.Size = new System.Drawing.Size(90, 65);
            this.ButtonRemove.TabIndex = 3;
            this.ButtonRemove.UseVisualStyleBackColor = true;
            this.ButtonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // ButtonPointer
            // 
            this.ButtonPointer.AccessibleDescription = "";
            this.ButtonPointer.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonPointer.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPointer.Image")));
            this.ButtonPointer.Location = new System.Drawing.Point(11, 12);
            this.ButtonPointer.Name = "ButtonPointer";
            this.ButtonPointer.Size = new System.Drawing.Size(90, 65);
            this.ButtonPointer.TabIndex = 2;
            this.ButtonPointer.UseVisualStyleBackColor = true;
            this.ButtonPointer.Click += new System.EventHandler(this.ButtonPointer_Click);
            // 
            // ButtonSquare
            // 
            this.ButtonSquare.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonSquare.Image = global::GrafischeEditor_DP.Properties.Resources._3766222;
            this.ButtonSquare.Location = new System.Drawing.Point(11, 154);
            this.ButtonSquare.Name = "ButtonSquare";
            this.ButtonSquare.Size = new System.Drawing.Size(90, 65);
            this.ButtonSquare.TabIndex = 1;
            this.ButtonSquare.UseVisualStyleBackColor = true;
            this.ButtonSquare.Click += new System.EventHandler(this.ButtonSquare_Click);
            // 
            // ButtonEllipse
            // 
            this.ButtonEllipse.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonEllipse.Image = global::GrafischeEditor_DP.Properties.Resources.ellipse_vector_format;
            this.ButtonEllipse.Location = new System.Drawing.Point(11, 83);
            this.ButtonEllipse.Name = "ButtonEllipse";
            this.ButtonEllipse.Size = new System.Drawing.Size(90, 65);
            this.ButtonEllipse.TabIndex = 0;
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
            this.DrawPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseDown);
            this.DrawPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseMove);
            this.DrawPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseUp);
            // 
            // bestandToolStripMenuItem
            // 
            this.bestandToolStripMenuItem.Name = "bestandToolStripMenuItem";
            this.bestandToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.bestandToolStripMenuItem.Text = "Bestand";
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ButtonPointer;
        private System.Windows.Forms.Button ButtonSquare;
        private System.Windows.Forms.Button ButtonEllipse;
        private System.Windows.Forms.Panel DrawPanel;
        private System.Windows.Forms.Button ButtonRemove;
        private System.Windows.Forms.Button ButtonResize;
        private System.Windows.Forms.ToolStripMenuItem bewerkenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ongedaanMakenCTRLYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opnieuwUitvoeren;
        private System.Windows.Forms.ToolStripMenuItem bestandToolStripMenuItem;
    }
}