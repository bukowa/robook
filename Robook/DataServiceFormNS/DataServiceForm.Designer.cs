using System.ComponentModel;

namespace Robook.DataServiceFormNS;

partial class DataServiceForm {
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
        tab1 = new TabControl();
        tabPage1 = new TabPage();
        accountsSelectControl1 = new Accounts.AccountsSelectControl();
        textBox4 = new TextBox();
        textBox3 = new TextBox();
        dateTimePicker2 = new DateTimePicker();
        dateTimePicker1 = new DateTimePicker();
        comboBox1 = new ComboBox();
        textBox2 = new TextBox();
        textBox1 = new TextBox();
        tabPage2 = new TabPage();
        tabPage3 = new TabPage();
        folderBrowserDialog1 = new FolderBrowserDialog();
        button1 = new Button();
        richTextBox1 = new RichTextBox();
        tab1.SuspendLayout();
        tabPage1.SuspendLayout();
        SuspendLayout();
        // 
        // tab1
        // 
        tab1.Controls.Add(tabPage1);
        tab1.Controls.Add(tabPage2);
        tab1.Controls.Add(tabPage3);
        tab1.Location = new Point(3, 12);
        tab1.Name = "tab1";
        tab1.SelectedIndex = 0;
        tab1.Size = new Size(911, 666);
        tab1.TabIndex = 0;
        // 
        // tabPage1
        // 
        tabPage1.Controls.Add(richTextBox1);
        tabPage1.Controls.Add(button1);
        tabPage1.Controls.Add(accountsSelectControl1);
        tabPage1.Controls.Add(textBox4);
        tabPage1.Controls.Add(textBox3);
        tabPage1.Controls.Add(dateTimePicker2);
        tabPage1.Controls.Add(dateTimePicker1);
        tabPage1.Controls.Add(comboBox1);
        tabPage1.Controls.Add(textBox2);
        tabPage1.Controls.Add(textBox1);
        tabPage1.Location = new Point(4, 24);
        tabPage1.Name = "tabPage1";
        tabPage1.Padding = new Padding(3);
        tabPage1.Size = new Size(903, 638);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "ReplayBars";
        tabPage1.UseVisualStyleBackColor = true;
        // 
        // accountsSelectControl1
        // 
        accountsSelectControl1.Location = new Point(25, 72);
        accountsSelectControl1.Name = "accountsSelectControl1";
        accountsSelectControl1.Size = new Size(292, 32);
        accountsSelectControl1.TabIndex = 7;
        // 
        // textBox4
        // 
        textBox4.Location = new Point(604, 103);
        textBox4.Name = "textBox4";
        textBox4.Size = new Size(155, 23);
        textBox4.TabIndex = 6;
        textBox4.Text = "filepath";
        textBox4.Click += textBox4_TextChanged;
        // 
        // textBox3
        // 
        textBox3.Location = new Point(359, 61);
        textBox3.Name = "textBox3";
        textBox3.Size = new Size(155, 23);
        textBox3.TabIndex = 5;
        textBox3.Text = "1";
        // 
        // dateTimePicker2
        // 
        dateTimePicker2.Location = new Point(556, 61);
        dateTimePicker2.Name = "dateTimePicker2";
        dateTimePicker2.Size = new Size(283, 23);
        dateTimePicker2.TabIndex = 4;
        // 
        // dateTimePicker1
        // 
        dateTimePicker1.Location = new Point(556, 32);
        dateTimePicker1.Name = "dateTimePicker1";
        dateTimePicker1.Size = new Size(283, 23);
        dateTimePicker1.TabIndex = 3;
        // 
        // comboBox1
        // 
        comboBox1.FormattingEnabled = true;
        comboBox1.Location = new Point(359, 32);
        comboBox1.Name = "comboBox1";
        comboBox1.Size = new Size(155, 23);
        comboBox1.TabIndex = 2;
        // 
        // textBox2
        // 
        textBox2.Location = new Point(184, 32);
        textBox2.Name = "textBox2";
        textBox2.Size = new Size(155, 23);
        textBox2.TabIndex = 1;
        textBox2.Text = "CME";
        // 
        // textBox1
        // 
        textBox1.Location = new Point(24, 32);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(154, 23);
        textBox1.TabIndex = 0;
        textBox1.Text = "NQH4";
        // 
        // tabPage2
        // 
        tabPage2.Location = new Point(4, 24);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new Padding(3);
        tabPage2.Size = new Size(903, 638);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "tabPage2";
        tabPage2.UseVisualStyleBackColor = true;
        // 
        // tabPage3
        // 
        tabPage3.Location = new Point(4, 24);
        tabPage3.Name = "tabPage3";
        tabPage3.Padding = new Padding(3);
        tabPage3.Size = new Size(903, 638);
        tabPage3.TabIndex = 2;
        tabPage3.Text = "tabPage3";
        tabPage3.UseVisualStyleBackColor = true;
        // 
        // button1
        // 
        button1.Location = new Point(86, 252);
        button1.Name = "button1";
        button1.Size = new Size(75, 23);
        button1.TabIndex = 8;
        button1.Text = "button1";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // richTextBox1
        // 
        richTextBox1.Location = new Point(233, 172);
        richTextBox1.Name = "richTextBox1";
        richTextBox1.Size = new Size(368, 185);
        richTextBox1.TabIndex = 9;
        richTextBox1.Text = "";
        // 
        // DataServiceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(917, 690);
        Controls.Add(tab1);
        Name = "DataServiceForm";
        Text = "DataServiceForm";
        tab1.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        tabPage1.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TabControl tab1;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private TabPage tabPage3;
    private TextBox textBox1;
    private TextBox textBox2;
    private ComboBox comboBox1;
    private DateTimePicker dateTimePicker2;
    private DateTimePicker dateTimePicker1;
    private TextBox textBox3;
    private TextBox textBox4;
    private FolderBrowserDialog folderBrowserDialog1;
    private Accounts.AccountsSelectControl accountsSelectControl1;
    private Button button1;
    private RichTextBox richTextBox1;
}