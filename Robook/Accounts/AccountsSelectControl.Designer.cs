using System.ComponentModel;

namespace Robook.Accounts;

partial class AccountsSelectControl {
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
    
    #region User Designer Code
    
    /// <summary>
    /// Place your custom code here.
    /// </summary>
    private void UserDesign() {
        comboBox1.Dock = DockStyle.Fill;
    }
    
    #endregion
    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        comboBox1 = new ComboBox();
        SuspendLayout();
        // 
        // comboBox1
        // 
        comboBox1.FormattingEnabled = true;
        comboBox1.Location = new Point(0, 0);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new Size(194, 23);
        comboBox1.TabIndex = 0;
        // 
        // AccountsSelectControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(comboBox1);
        Name = "AccountsSelectControl";
        Size = new Size(290, 134);
        //
        // User Designer Code
        //
        UserDesign();
        ResumeLayout(false);
    }

    #endregion

    private ComboBox comboBox1;
}