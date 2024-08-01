using System.ComponentModel;

namespace Robook.OrderBookFormNS;

partial class OrderBookForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        accountsSelectControl1 = new Accounts.AccountsSelectControl();
        symbolTextBoxControl1 = new SymbolNS.SymbolTextBoxControl();
        button1 = new Button();
        panelSymbol = new Panel();
        panelOrderBook = new Panel();
        panelMenu = new Panel();
        menuStrip1 = new MenuStrip();
        settingsToolStripMenuItem = new ToolStripMenuItem();
        columnsToolStripMenuItem = new ToolStripMenuItem();
        addToolStripMenuItem = new ToolStripMenuItem();
        toolStripMenuItem1 = new ToolStripMenuItem();
        panelSymbol.SuspendLayout();
        panelMenu.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // accountsSelectControl1
        // 
        accountsSelectControl1.Location = new Point(14, 12);
        accountsSelectControl1.Name = "accountsSelectControl1";
        accountsSelectControl1.Size = new Size(290, 21);
        accountsSelectControl1.TabIndex = 0;
        // 
        // symbolTextBoxControl1
        // 
        symbolTextBoxControl1.Location = new Point(319, 12);
        symbolTextBoxControl1.Name = "symbolTextBoxControl1";
        symbolTextBoxControl1.Size = new Size(165, 21);
        symbolTextBoxControl1.TabIndex = 1;
        // 
        // button1
        // 
        button1.Location = new Point(500, 10);
        button1.Name = "button1";
        button1.Size = new Size(75, 23);
        button1.TabIndex = 2;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // panelSymbol
        // 
        panelSymbol.Controls.Add(accountsSelectControl1);
        panelSymbol.Controls.Add(button1);
        panelSymbol.Controls.Add(symbolTextBoxControl1);
        panelSymbol.Dock = DockStyle.Top;
        panelSymbol.Location = new Point(0, 24);
        panelSymbol.Name = "panelSymbol";
        panelSymbol.Size = new Size(800, 54);
        panelSymbol.TabIndex = 3;
        // 
        // panelOrderBook
        // 
        panelOrderBook.Dock = DockStyle.Fill;
        panelOrderBook.Location = new Point(0, 78);
        panelOrderBook.Name = "panelOrderBook";
        panelOrderBook.Size = new Size(800, 372);
        panelOrderBook.TabIndex = 4;
        // 
        // panelMenu
        // 
        panelMenu.Controls.Add(menuStrip1);
        panelMenu.Dock = DockStyle.Top;
        panelMenu.Location = new Point(0, 0);
        panelMenu.Name = "panelMenu";
        panelMenu.Size = new Size(800, 24);
        panelMenu.TabIndex = 5;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(800, 24);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";
        // 
        // settingsToolStripMenuItem
        // 
        settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { columnsToolStripMenuItem });
        settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
        settingsToolStripMenuItem.Size = new Size(61, 20);
        settingsToolStripMenuItem.Text = "Settings";
        // 
        // columnsToolStripMenuItem
        // 
        columnsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem, toolStripMenuItem1 });
        columnsToolStripMenuItem.Name = "columnsToolStripMenuItem";
        columnsToolStripMenuItem.Size = new Size(180, 22);
        columnsToolStripMenuItem.Text = "Columns";
        // 
        // addToolStripMenuItem
        // 
        addToolStripMenuItem.Name = "addToolStripMenuItem";
        addToolStripMenuItem.Size = new Size(180, 22);
        addToolStripMenuItem.Text = "Add";
        // addToolStripMenuItem.Click += addToolStripMenuItem_Click;
        // 
        // toolStripMenuItem1
        // 
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(180, 22);
        toolStripMenuItem1.Text = "toolStripMenuItem1";
        // 
        // OrderBookForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(panelOrderBook);
        Controls.Add(panelSymbol);
        Controls.Add(panelMenu);
        MainMenuStrip = menuStrip1;
        Name = "OrderBookForm";
        Text = "OrderBookForm";
        panelSymbol.ResumeLayout(false);
        panelMenu.ResumeLayout(false);
        panelMenu.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Accounts.AccountsSelectControl accountsSelectControl1;
    private SymbolNS.SymbolTextBoxControl symbolTextBoxControl1;
    private Button button1;
    private Panel panelSymbol;
    private Panel panelOrderBook;
    private Panel panelMenu;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem settingsToolStripMenuItem;
    private ToolStripMenuItem columnsToolStripMenuItem;
    private ToolStripMenuItem addToolStripMenuItem;
    private ToolStripMenuItem toolStripMenuItem1;
}