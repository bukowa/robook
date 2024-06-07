using System.Reflection;

namespace Robook;

public partial class BaseForm : Form {
    public BaseForm() {
        InitializeComponent();
    }

    public void SetDoubleBuffered(bool value) {
        typeof(BaseForm).InvokeMember(
            "DoubleBuffered",
            BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.SetProperty,
            null,
            this,
            new object[] { value });
    }

    public Form Show() {
        base.Show();
        return this;
    }
    
    public Form Focus() {
        base.Focus();
        return this;
    }
}