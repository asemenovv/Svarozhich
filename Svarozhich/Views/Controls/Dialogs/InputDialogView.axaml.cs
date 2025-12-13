using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Svarozhich.Views.Controls.Dialogs;

public record InputDialogConfigs(
    string Title,
    string Message,
    string DefaultValue = ""
);

public partial class InputDialogView : Window
{
    
    public InputDialogView(InputDialogConfigs configs)
    {
        InitializeComponent();
        DataContext = this;
        Title = configs.Title;
        
        var caption = this.FindControl<TextBlock>("CaptionText");
        caption!.Text = configs.Message;
        
        var textInput = this.FindControl<TextBox>("InputBox");
        textInput!.Text = configs.DefaultValue;
        textInput.Focus();
        textInput.CaretIndex = textInput.Text?.Length ?? 0;
    }

    public void OkClick(object? sender, RoutedEventArgs e)
    {
        Close(this.FindControl<TextBox>("InputBox")!.Text);
    }

    public void CancelClick(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                Close(this.FindControl<TextBox>("InputBox")!.Text);
                break;
            case Key.Escape:
                Close(null);
                break;
        }
        base.OnKeyDown(e);
    }
}