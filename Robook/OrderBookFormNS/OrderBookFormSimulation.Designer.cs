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
        fontSizeNumericUpDown = new NumericUpDown();
        italicCheckBox = new CheckBox();
        boldCheckBox = new CheckBox();
        fontFamilyComboBox = new ComboBox();
        removeButton = new Button();
        addButton = new Button();
        orderBookPanel = new Panel();
        buttonsPanel.SuspendLayout();
        ((ISupportInitialize)fontSizeNumericUpDown).BeginInit();
        SuspendLayout();
        // 
        // buttonsPanel
        // 
        buttonsPanel.Controls.Add(fontSizeNumericUpDown);
        buttonsPanel.Controls.Add(italicCheckBox);
        buttonsPanel.Controls.Add(boldCheckBox);
        buttonsPanel.Controls.Add(fontFamilyComboBox);
        buttonsPanel.Controls.Add(removeButton);
        buttonsPanel.Controls.Add(addButton);
        buttonsPanel.Dock = DockStyle.Top;
        buttonsPanel.Location = new Point(0, 0);
        buttonsPanel.Name = "buttonsPanel";
        buttonsPanel.Size = new Size(800, 61);
        buttonsPanel.TabIndex = 0;
        // 
        // fontSizeNumericUpDown
        // 
        fontSizeNumericUpDown.Location = new Point(439, 18);
        fontSizeNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        fontSizeNumericUpDown.Name = "fontSizeNumericUpDown";
        fontSizeNumericUpDown.Size = new Size(64, 23);
        fontSizeNumericUpDown.TabIndex = 8;
        fontSizeNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // italicCheckBox
        // 
        italicCheckBox.AutoSize = true;
        italicCheckBox.Location = new Point(597, 20);
        italicCheckBox.Name = "italicCheckBox";
        italicCheckBox.Size = new Size(74, 19);
        italicCheckBox.TabIndex = 7;
        italicCheckBox.Text = "Italic text";
        italicCheckBox.UseVisualStyleBackColor = true;
        // 
        // boldCheckBox
        // 
        boldCheckBox.AutoSize = true;
        boldCheckBox.FlatStyle = FlatStyle.Flat;
        boldCheckBox.Location = new Point(518, 21);
        boldCheckBox.Name = "boldCheckBox";
        boldCheckBox.Size = new Size(70, 19);
        boldCheckBox.TabIndex = 6;
        boldCheckBox.Text = "Bold text";
        boldCheckBox.UseVisualStyleBackColor = true;
        // 
        // fontFamilyComboBox
        // 
        fontFamilyComboBox.Location = new Point(296, 17);
        fontFamilyComboBox.Name = "fontFamilyComboBox";
        fontFamilyComboBox.Size = new Size(121, 23);
        fontFamilyComboBox.TabIndex = 4;
        fontFamilyComboBox.Text = "Font family";
        // 
        // removeButton
        // 
        removeButton.FlatStyle = FlatStyle.Popup;
        removeButton.Location = new Point(152, 17);
        removeButton.Name = "removeButton";
        removeButton.Size = new Size(129, 23);
        removeButton.TabIndex = 3;
        removeButton.Text = "Remove column";
        removeButton.UseVisualStyleBackColor = true;
        removeButton.Click += removeButton_Click;
        // 
        // addButton
        // 
        addButton.Location = new Point(30, 17);
        addButton.Name = "addButton";
        addButton.Size = new Size(116, 23);
        addButton.TabIndex = 2;
        addButton.Text = "Add column";
        addButton.UseVisualStyleBackColor = true;
        addButton.Click += addButton_Click;
        // 
        // orderBookPanel
        // 
        orderBookPanel.Dock = DockStyle.Fill;
        orderBookPanel.Location = new Point(0, 61);
        orderBookPanel.Name = "orderBookPanel";
        orderBookPanel.Size = new Size(800, 389);
        orderBookPanel.TabIndex = 1;
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
        buttonsPanel.PerformLayout();
        ((ISupportInitialize)fontSizeNumericUpDown).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel buttonsPanel;
    private Button addButton;
    private Panel orderBookPanel;
    private Button removeButton;
    private ComboBox fontFamilyComboBox;
    private CheckBox italicCheckBox;
    private CheckBox boldCheckBox;
    private NumericUpDown fontSizeNumericUpDown;
}