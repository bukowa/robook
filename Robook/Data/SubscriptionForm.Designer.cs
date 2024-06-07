using System.ComponentModel;

namespace Robook.Data;

partial class SubscriptionForm {
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
        dataGridView1 = new DataGridView();
        panel1 = new Panel();
        button1 = new Button();
        ((ISupportInitialize)dataGridView1).BeginInit();
        panel1.SuspendLayout();
        SuspendLayout();
        // 
        // dataGridView1
        // 
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Dock = DockStyle.Fill;
        dataGridView1.Location = new Point(0, 0);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new Size(800, 450);
        dataGridView1.TabIndex = 0;
        // 
        // panel1
        // 
        panel1.Controls.Add(button1);
        panel1.Dock = DockStyle.Bottom;
        panel1.Location = new Point(0, 350);
        panel1.Name = "panel1";
        panel1.Size = new Size(800, 100);
        panel1.TabIndex = 1;
        // 
        // button1
        // 
        button1.Dock = DockStyle.Top;
        button1.Location = new Point(0, 0);
        button1.Name = "button1";
        button1.Size = new Size(800, 23);
        button1.TabIndex = 0;
        button1.Text = "Save";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // SubscriptionForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(panel1);
        Controls.Add(dataGridView1);
        Name = "SubscriptionForm";
        Text = "SubscriiptionForm";
        ((ISupportInitialize)dataGridView1).EndInit();
        panel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private Panel panel1;
    private Button button1;
}