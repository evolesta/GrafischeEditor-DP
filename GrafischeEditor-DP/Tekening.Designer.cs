
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
            this.Bewerken = new System.Windows.Forms.ToolStripMenuItem();
            this.ongedaanMaken = new System.Windows.Forms.ToolStripMenuItem();
            this.opnieuwUitvoeren = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButtonResize = new System.Windows.Forms.Button();
            this.ButtonRemove = new System.Windows.Forms.Button();
            this.ButtonPointer = new System.Windows.Forms.Button();
            this.ButtonSquare = new System.Windows.Forms.Button();
            this.ButtonEllipse = new System.Windows.Forms.Button();
            this.DrawPanel = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.btnNieuweGroep = new System.Windows.Forms.Button();
            this.MenuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Bestand,
            this.Bewerken});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.MenuStrip.Size = new System.Drawing.Size(1431, 30);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip";
            // 
            // Bestand
            // 
            this.Bestand.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NieuwBestand,
            this.OpenBestand,
            this.BestandOpslaan});
            this.Bestand.Name = "Bestand";
            this.Bestand.Size = new System.Drawing.Size(76, 24);
            this.Bestand.Text = "Bestand";
            // 
            // NieuwBestand
            // 
            this.NieuwBestand.Name = "NieuwBestand";
            this.NieuwBestand.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.NieuwBestand.Size = new System.Drawing.Size(197, 26);
            this.NieuwBestand.Text = "Nieuw";
            this.NieuwBestand.Click += new System.EventHandler(this.NieuwBestand_Click);
            // 
            // OpenBestand
            // 
            this.OpenBestand.Name = "OpenBestand";
            this.OpenBestand.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenBestand.Size = new System.Drawing.Size(197, 26);
            this.OpenBestand.Text = "Openen";
            this.OpenBestand.Click += new System.EventHandler(this.OpenBestand_Click);
            // 
            // BestandOpslaan
            // 
            this.BestandOpslaan.Name = "BestandOpslaan";
            this.BestandOpslaan.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.BestandOpslaan.Size = new System.Drawing.Size(197, 26);
            this.BestandOpslaan.Text = "Opslaan";
            this.BestandOpslaan.Click += new System.EventHandler(this.BestandOpslaan_Click);
            // 
            // Bewerken
            // 
            this.Bewerken.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ongedaanMaken,
            this.opnieuwUitvoeren});
            this.Bewerken.Name = "Bewerken";
            this.Bewerken.Size = new System.Drawing.Size(87, 24);
            this.Bewerken.Text = "Bewerken";
            // 
            // ongedaanMaken
            // 
            this.ongedaanMaken.Name = "ongedaanMaken";
            this.ongedaanMaken.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.ongedaanMaken.Size = new System.Drawing.Size(267, 26);
            this.ongedaanMaken.Text = "Ongedaan maken";
            this.ongedaanMaken.Click += new System.EventHandler(this.ongedaanMaken_Click);
            // 
            // opnieuwUitvoeren
            // 
            this.opnieuwUitvoeren.Name = "opnieuwUitvoeren";
            this.opnieuwUitvoeren.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.opnieuwUitvoeren.Size = new System.Drawing.Size(267, 26);
            this.opnieuwUitvoeren.Text = "Opnieuw uitvoeren";
            this.opnieuwUitvoeren.Click += new System.EventHandler(this.opnieuwUitvoeren_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ButtonResize);
            this.panel1.Controls.Add(this.ButtonRemove);
            this.panel1.Controls.Add(this.ButtonPointer);
            this.panel1.Controls.Add(this.ButtonSquare);
            this.panel1.Controls.Add(this.ButtonEllipse);
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(131, 802);
            this.panel1.TabIndex = 1;
            // 
            // ButtonResize
            // 
            this.ButtonResize.Font = new System.Drawing.Font("Webdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonResize.Image = ((System.Drawing.Image)(resources.GetObject("ButtonResize.Image")));
            this.ButtonResize.Location = new System.Drawing.Point(13, 303);
            this.ButtonResize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonResize.Name = "ButtonResize";
            this.ButtonResize.Size = new System.Drawing.Size(103, 87);
            this.ButtonResize.TabIndex = 4;
            this.ButtonResize.UseVisualStyleBackColor = true;
            this.ButtonResize.Click += new System.EventHandler(this.ButtonResize_Click);
            // 
            // ButtonRemove
            // 
            this.ButtonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonRemove.Location = new System.Drawing.Point(13, 397);
            this.ButtonRemove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonRemove.Name = "ButtonRemove";
            this.ButtonRemove.Size = new System.Drawing.Size(103, 87);
            this.ButtonRemove.TabIndex = 3;
            this.ButtonRemove.UseVisualStyleBackColor = true;
            this.ButtonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // ButtonPointer
            // 
            this.ButtonPointer.AccessibleDescription = "";
            this.ButtonPointer.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonPointer.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPointer.Image")));
            this.ButtonPointer.Location = new System.Drawing.Point(13, 16);
            this.ButtonPointer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonPointer.Name = "ButtonPointer";
            this.ButtonPointer.Size = new System.Drawing.Size(103, 87);
            this.ButtonPointer.TabIndex = 2;
            this.ButtonPointer.UseVisualStyleBackColor = true;
            this.ButtonPointer.Click += new System.EventHandler(this.ButtonPointer_Click);
            // 
            // ButtonSquare
            // 
            this.ButtonSquare.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonSquare.Image = ((System.Drawing.Image)(resources.GetObject("ButtonSquare.Image")));
            this.ButtonSquare.Location = new System.Drawing.Point(13, 205);
            this.ButtonSquare.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonSquare.Name = "ButtonSquare";
            this.ButtonSquare.Size = new System.Drawing.Size(103, 87);
            this.ButtonSquare.TabIndex = 1;
            this.ButtonSquare.UseVisualStyleBackColor = true;
            this.ButtonSquare.Click += new System.EventHandler(this.ButtonSquare_Click);
            // 
            // ButtonEllipse
            // 
            this.ButtonEllipse.Font = new System.Drawing.Font("Wingdings", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ButtonEllipse.Image = ((System.Drawing.Image)(resources.GetObject("ButtonEllipse.Image")));
            this.ButtonEllipse.Location = new System.Drawing.Point(13, 111);
            this.ButtonEllipse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonEllipse.Name = "ButtonEllipse";
            this.ButtonEllipse.Size = new System.Drawing.Size(103, 87);
            this.ButtonEllipse.TabIndex = 0;
            this.ButtonEllipse.UseVisualStyleBackColor = true;
            this.ButtonEllipse.Click += new System.EventHandler(this.ButtonEllipse_Click);
            // 
            // DrawPanel
            // 
            this.DrawPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DrawPanel.Location = new System.Drawing.Point(138, 36);
            this.DrawPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DrawPanel.Name = "DrawPanel";
            this.DrawPanel.Size = new System.Drawing.Size(1041, 802);
            this.DrawPanel.TabIndex = 2;
            this.DrawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawPanel_Paint);
            this.DrawPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseDown);
            this.DrawPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseMove);
            this.DrawPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawPanel_MouseUp);
            // 
            // treeView
            // 
            this.treeView.LabelEdit = true;
            this.treeView.Location = new System.Drawing.Point(1186, 75);
            this.treeView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(230, 763);
            this.treeView.TabIndex = 3;
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeView_AfterLabelEdit);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            // 
            // btnNieuweGroep
            // 
            this.btnNieuweGroep.Location = new System.Drawing.Point(1187, 36);
            this.btnNieuweGroep.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNieuweGroep.Name = "btnNieuweGroep";
            this.btnNieuweGroep.Size = new System.Drawing.Size(230, 31);
            this.btnNieuweGroep.TabIndex = 4;
            this.btnNieuweGroep.Text = "Nieuwe groep";
            this.btnNieuweGroep.UseVisualStyleBackColor = true;
            this.btnNieuweGroep.Click += new System.EventHandler(this.btnNieuweGroep_Click);
            // 
            // Tekening
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1431, 855);
            this.Controls.Add(this.btnNieuweGroep);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.DrawPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
        private System.Windows.Forms.ToolStripMenuItem Bewerken;
        private System.Windows.Forms.ToolStripMenuItem ongedaanMaken;
        private System.Windows.Forms.ToolStripMenuItem opnieuwUitvoeren;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button btnNieuweGroep;
    }
}