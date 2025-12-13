using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Svarozhich.Views.Controls.Dialogs;

public record InputDialogResponse(
    string? Text,
    bool? FlagChecked
);

public record InputDialogConfigs(
    string Title,
    string Message,
    string DefaultValue = "",
    bool ShowBooleanFlag = false,
    string BooleanFlagLabel = "",
    bool BooleanFlagDefaultValue = false,
    bool BooleanFlagReadOnly = false
);

public partial class InputDialogView : Window
{
    
    public InputDialogView(InputDialogConfigs configs)
    {
        InitializeComponent();
        DataContext = this;
        Title = configs.Title;
        
        var caption = this.FindControl<TextBlock>("InputCaptionText");
        caption!.Text = configs.Message;
        
        var textInput = this.FindControl<TextBox>("InputBox");
        textInput!.Text = configs.DefaultValue;
        textInput.Focus();
        textInput.CaretIndex = textInput.Text?.Length ?? 0;

        var checkBox = this.FindControl<CheckBox>("InputCheckBox");
        checkBox.IsVisible = configs.ShowBooleanFlag;
        checkBox.Content = configs.BooleanFlagLabel;
        checkBox.IsChecked = configs.BooleanFlagDefaultValue;
        checkBox.IsEnabled = !configs.BooleanFlagReadOnly;
    }

    public void OkClick(object? sender, RoutedEventArgs e)
    {
        var dialogResult = new InputDialogResponse(this.FindControl<TextBox>("InputBox")!.Text,
            this.FindControl<CheckBox>("InputCheckBox")?.IsChecked);
        Close(dialogResult);
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