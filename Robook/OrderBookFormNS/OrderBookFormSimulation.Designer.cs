using System.ComponentModel;

namespace Robook.OrderBookFormNS;

partial class OrderBookFormSimulation {
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
        buttonsPanel = new Panel();
        orderBookPanel = new Panel();
        addButton = new Button();
        removeButton = new Button();
        buttonsPanel.SuspendLayout();
        SuspendLayout();
        // 
        // buttonsPanel
        // 
        buttonsPanel.Controls.Add(removeButton);
        buttonsPanel.Controls.Add(addButton);
        buttonsPanel.Dock = DockStyle.Top;
        buttonsPanel.Location = new Point(0, 0);
        buttonsPanel.Name = "buttonsPanel";
        buttonsPanel.Size = new Size(800, 61);
        buttonsPanel.TabIndex = 0;
        // 
        // orderBookPanel
        // 
        orderBookPanel.Dock = DockStyle.Fill;
        orderBookPanel.Location = new Point(0, 61);
        orderBookPanel.Name = "orderBookPanel";
        orderBookPanel.Size = new Size(800, 389);
        orderBookPanel.TabIndex = 1;
        // 
        // addButton
        // 
        addButton.Location                =  new Point(30, 17);
        addButton.Name                    =  "addButton";
        addButton.Size                    =  new Size(116, 23);
        addButton.TabIndex                =  2;
        addButton.Text                    =  "Add column";
        addButton.UseVisualStyleBackColor =  true;
        addButton.Click                   += addButton_Click;
        // 
        // removeButton
        // 
        removeButton.Location = new Point(152, 17);
        removeButton.Name = "removeButton";
        removeButton.Size = new Size(129, 23);
        removeButton.TabIndex = 3;
        removeButton.Text = "Remove column";
        removeButton.UseVisualStyleBackColor = true;
        removeButton.Click += removeButton_Click;
        // 
        // OrderBookFormSimulation
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(orderBookPanel);
        Controls.Add(buttonsPanel);
        Name = "OrderBookFormSimulation";
        Text = "OrderBookFormSimulation";
        buttonsPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private Panel buttonsPanel;
    private Button addButton;
    private Panel orderBookPanel;
    private Button removeButton;
}