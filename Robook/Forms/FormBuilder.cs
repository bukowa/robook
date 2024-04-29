namespace Robook.Forms;

/// <summary>
/// Form builder.
/// </summary>
public class FormBuilder {
    private readonly Lazy<Form> _lazyForm;
    private readonly Form       _form;
    
    public Form Form => _lazyForm.Value ?? _form;
    
    /// <summary>
    /// Creates a new instance of the <see cref="FormBuilder"/> class.
    /// </summary>
    public FormBuilder(Form form) {
        _form = form;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="FormBuilder"/> class.
    public FormBuilder(Func<Form> formFactory) {
        _lazyForm = new Lazy<Form>(formFactory);
    }

    /// <summary>
    /// Builds the form.
    /// </summary>
    public Form Build() {
        return Form;
    }

    /// <summary>
    /// Sets the form to be hidden when closed.
    /// </summary>
    public FormBuilder SetHiddenOnClose() {
        Form.FormClosing += (sender, args) => {
            Form.Hide();
            args.Cancel = true;
        };
        return this;
    }
}