using System.ComponentModel;

namespace Robook.SymbolNS;

partial class SymbolTextBoxControl {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        textBox1 = new TextBox();
        SuspendLayout();
        // 
        // textBox1
        // 
        textBox1.Location = new Point(3, 3);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(100, 23);
        textBox1.TabIndex = 0;
        // 
        // SymbolTextBoxControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(textBox1);
        Name = "SymbolTextBoxControl";
        Size = new Size(107, 28);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox textBox1;
}