using System.Collections.ObjectModel;

namespace Robook.Accounts;

public partial class AccountsSelectControl : UserControl {
    private Collection<Account> _accounts;

    public void Init(Collection<Account> accounts) {
        _accounts            = accounts;
        comboBox1.DataSource = _accounts;
    }

    public AccountsSelectControl() {
        InitializeComponent();
    }
    
    public Account SelectedAccount => (Account) comboBox1.SelectedItem;
}