using System.ComponentModel;

namespace Robook;

partial class Main {
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
        components = new Container();
        contextMenuStrip1 = new ContextMenuStrip(components);
        menuStrip1 = new MenuStrip();
        contextMenuStrip2 = new ContextMenuStrip(components);
        menuStrip2 = new MenuStrip();
        contextMenuStrip3 = new ContextMenuStrip(components);
        menuStrip3 = new MenuStrip();
        mainToolStripMenuItem = new ToolStripMenuItem();
        accountsToolStripMenuItem = new ToolStripMenuItem();
        orderBookToolStripMenuItem = new ToolStripMenuItem();
        orderBookSimulationToolStripMenuItem = new ToolStripMenuItem();
        dataToolStripMenuItem = new ToolStripMenuItem();
        subscriptionsToolStripMenuItem = new ToolStripMenuItem();
        symbolsToolStripMenuItem = new ToolStripMenuItem();
        connectionsToolStripMenuItem = new ToolStripMenuItem();
        menuStrip3.SuspendLayout();
        SuspendLayout();
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.Size = new Size(61, 4);
        // 
        // menuStrip1
        // 
        menuStrip1.Location = new Point(0, 48);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(800, 24);
        menuStrip1.TabIndex = 1;
        menuStrip1.Text = "menuStrip1";
        // 
        // contextMenuStrip2
        // 
        contextMenuStrip2.Name = "contextMenuStrip2";
        contextMenuStrip2.Size = new Size(61, 4);
        // 
        // menuStrip2
        // 
        menuStrip2.Location = new Point(0, 24);
        menuStrip2.Name = "menuStrip2";
        menuStrip2.Size = new Size(800, 24);
        menuStrip2.TabIndex = 3;
        menuStrip2.Text = "menuStrip2";
        // 
        // contextMenuStrip3
        // 
        contextMenuStrip3.Name = "contextMenuStrip3";
        contextMenuStrip3.Size = new Size(61, 4);
        // 
        // menuStrip3
        // 
        menuStrip3.Items.AddRange(new ToolStripItem[] { mainToolStripMenuItem, accountsToolStripMenuItem, orderBookToolStripMenuItem, orderBookSimulationToolStripMenuItem, dataToolStripMenuItem, subscriptionsToolStripMenuItem, symbolsToolStripMenuItem, connectionsToolStripMenuItem });
        menuStrip3.Location = new Point(0, 0);
        menuStrip3.Name = "menuStrip3";
        menuStrip3.Size = new Size(800, 24);
        menuStrip3.TabIndex = 5;
        menuStrip3.Text = "menuStrip3";
        // 
        // mainToolStripMenuItem
        // 
        mainToolStripMenuItem.Name = "mainToolStripMenuItem";
        mainToolStripMenuItem.Size = new Size(46, 20);
        mainToolStripMenuItem.Text = "Main";
        // 
        // accountsToolStripMenuItem
        // 
        accountsToolStripMenuItem.Name = "accountsToolStripMenuItem";
        accountsToolStripMenuItem.Size = new Size(69, 20);
        accountsToolStripMenuItem.Text = "Accounts";
        accountsToolStripMenuItem.Click += accountsToolStripMenuItem_Click;
        // 
        // orderBookToolStripMenuItem
        // 
        orderBookToolStripMenuItem.Name = "orderBookToolStripMenuItem";
        orderBookToolStripMenuItem.Size = new Size(76, 20);
        orderBookToolStripMenuItem.Text = "OrderBook";
        orderBookToolStripMenuItem.Click += orderBookToolStripMenuItem_Click;
        // 
        // orderBookSimulationToolStripMenuItem
        // 
        orderBookSimulationToolStripMenuItem.Name = "orderBookSimulationToolStripMenuItem";
        orderBookSimulationToolStripMenuItem.Size = new Size(133, 20);
        orderBookSimulationToolStripMenuItem.Text = "OrderBookSimulation";
        orderBookSimulationToolStripMenuItem.Click += orderBookSimulationToolStripMenuItem_Click;
        // 
        // dataToolStripMenuItem
        // 
        dataToolStripMenuItem.Name = "dataToolStripMenuItem";
        dataToolStripMenuItem.Size = new Size(43, 20);
        dataToolStripMenuItem.Text = "Data";
        dataToolStripMenuItem.Click += dataToolStripMenuItem_Click;
        // 
        // subscriptionsToolStripMenuItem
        // 
        subscriptionsToolStripMenuItem.Name = "subscriptionsToolStripMenuItem";
        subscriptionsToolStripMenuItem.Size = new Size(90, 20);
        subscriptionsToolStripMenuItem.Text = "Subscriptions";
        subscriptionsToolStripMenuItem.Click += subscriptionsToolStripMenuItem_Click;
        // 
        // symbolsToolStripMenuItem
        // 
        symbolsToolStripMenuItem.Name = "symbolsToolStripMenuItem";
        symbolsToolStripMenuItem.Size = new Size(64, 20);
        symbolsToolStripMenuItem.Text = "Symbols";
        symbolsToolStripMenuItem.Click += symbolsToolStripMenuItem_Click;
        // 
        // connectionsToolStripMenuItem
        // 
        connectionsToolStripMenuItem.Name = "connectionsToolStripMenuItem";
        connectionsToolStripMenuItem.Size = new Size(86, 20);
        connectionsToolStripMenuItem.Text = "Connections";
        connectionsToolStripMenuItem.Click += connectionsToolStripMenuItem_Click;
        // 
        // Main
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(menuStrip1);
        Controls.Add(menuStrip2);
        Controls.Add(menuStrip3);
        MainMenuStrip = menuStrip1;
        Name = "Main";
        Text = "Main";
        menuStrip3.ResumeLayout(false);
        menuStrip3.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ContextMenuStrip contextMenuStrip1;
    private MenuStrip menuStrip1;
    private ContextMenuStrip contextMenuStrip2;
    private MenuStrip menuStrip2;
    private ContextMenuStrip contextMenuStrip3;
    private MenuStrip menuStrip3;
    private ToolStripMenuItem mainToolStripMenuItem;
    private ToolStripMenuItem accountsToolStripMenuItem;
    private ToolStripMenuItem orderBookToolStripMenuItem;
    private ToolStripMenuItem orderBookSimulationToolStripMenuItem;
    private ToolStripMenuItem dataToolStripMenuItem;
    private ToolStripMenuItem subscriptionsToolStripMenuItem;
    private ToolStripMenuItem symbolsToolStripMenuItem;
    private ToolStripMenuItem connectionsToolStripMenuItem;
}