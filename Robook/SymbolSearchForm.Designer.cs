using System.ComponentModel;

namespace Robook;

partial class SymbolSearchForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        exchangeTextBox = new TextBox();
        label1 = new Label();
        caseSensitiveCheckBox = new CheckBox();
        searchFieldComboBox = new ComboBox();
        searchOperatorComboBox = new ComboBox();
        searchTermTextBox = new TextBox();
        searchButton = new Button();
        searchGridView = new DataGridView();
        ((ISupportInitialize)searchGridView).BeginInit();
        SuspendLayout();
        // 
        // exchangeTextBox
        // 
        exchangeTextBox.Location = new Point(152, 73);
        exchangeTextBox.Name = "exchangeTextBox";
        exchangeTextBox.Size = new Size(100, 23);
        exchangeTextBox.TabIndex = 0;
        exchangeTextBox.Text = "CME";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(171, 55);
        label1.Name = "label1";
        label1.Size = new Size(58, 15);
        label1.TabIndex = 1;
        label1.Text = "Exchange";
        // 
        // caseSensitiveCheckBox
        // 
        caseSensitiveCheckBox.AutoSize = true;
        caseSensitiveCheckBox.Location = new Point(152, 160);
        caseSensitiveCheckBox.Name = "caseSensitiveCheckBox";
        caseSensitiveCheckBox.Size = new Size(100, 19);
        caseSensitiveCheckBox.TabIndex = 2;
        caseSensitiveCheckBox.Text = "Case Sensitive";
        caseSensitiveCheckBox.UseVisualStyleBackColor = true;
        // 
        // searchFieldComboBox
        // 
        searchFieldComboBox.FormattingEnabled = true;
        searchFieldComboBox.Location = new Point(143, 102);
        searchFieldComboBox.Name = "searchFieldComboBox";
        searchFieldComboBox.Size = new Size(121, 23);
        searchFieldComboBox.TabIndex = 3;
        // 
        // searchOperatorComboBox
        // 
        searchOperatorComboBox.FormattingEnabled = true;
        searchOperatorComboBox.Location = new Point(143, 131);
        searchOperatorComboBox.Name = "searchOperatorComboBox";
        searchOperatorComboBox.Size = new Size(121, 23);
        searchOperatorComboBox.TabIndex = 4;
        // 
        // searchTermTextBox
        // 
        searchTermTextBox.Location = new Point(152, 185);
        searchTermTextBox.Name = "searchTermTextBox";
        searchTermTextBox.PlaceholderText = "search term";
        searchTermTextBox.Size = new Size(100, 23);
        searchTermTextBox.TabIndex = 5;
        // 
        // searchButton
        // 
        searchButton.Location = new Point(154, 224);
        searchButton.Name = "searchButton";
        searchButton.Size = new Size(75, 23);
        searchButton.TabIndex = 6;
        searchButton.Text = "Search";
        searchButton.UseVisualStyleBackColor = true;
        searchButton.Click += searchButton_Click;
        // 
        // searchGridView
        // 
        searchGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        searchGridView.Location = new Point(12, 265);
        searchGridView.Name = "searchGridView";
        searchGridView.Size = new Size(1021, 315);
        searchGridView.TabIndex = 7;
        // 
        // SymbolSearchForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1045, 663);
        Controls.Add(searchGridView);
        Controls.Add(searchButton);
        Controls.Add(searchTermTextBox);
        Controls.Add(searchOperatorComboBox);
        Controls.Add(searchFieldComboBox);
        Controls.Add(caseSensitiveCheckBox);
        Controls.Add(label1);
        Controls.Add(exchangeTextBox);
        Name = "SymbolSearchForm";
        Text = "SymbolSearchForm";
        ((ISupportInitialize)searchGridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private TextBox exchangeTextBox;
    private Label label1;
    private CheckBox caseSensitiveCheckBox;
    private ComboBox searchFieldComboBox;
    private ComboBox searchOperatorComboBox;
    private TextBox searchTermTextBox;
    private Button searchButton;
    private DataGridView searchGridView;
}