# Arrow Keys Move Button

Indeed, you are correct that as soon as there is a focused button on the form, it will monopolize Key messages and `Form1_KeyDown` will no longer be called or move the picture box as you have observed.

Since the Key messages we need have been filtered out, the way around this is to implement our own message filter by implementing [IMessageFilter](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.application.addmessagefilter?view=windowsdesktop-7.0&devlangs=csharp&f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Windows.Forms.Application.AddMessageFilter)%3Bk(DevLang-csharp)%26rd%3Dtrue) so that we can choose to intercept **Win32** [WM_KEYDOWN](https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keydown) message unconditionally. Try something like this:
```
public partial class MainForm : Form, IMessageFilter
{
    public MainForm()
    {
        InitializeComponent();
        Application.AddMessageFilter(this);
        Disposed += (sender, e) =>Application.RemoveMessageFilter(this);
    }
    public bool PreFilterMessage(ref Message m)
    {
        if (m.Msg == WM_KEYDOWN)
        {
            var e = new KeyEventArgs(((Keys)m.WParam) | Control.ModifierKeys);
            OnHotKeyPreview(FromHandle(m.HWnd), e);
            return (e.Handled);
        }
        return false;
    }
    private void OnHotKeyPreview(Control? control, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.Left: onMoveLeft(); e.Handled = true; break;
            case Keys.Up: onMoveUp(); e.Handled = true; break;
            case Keys.Right:  onMoveRight(); e.Handled = true; break;
            case Keys.Down: onMoveDown(); e.Handled = true; break;
        }
    }
    private void onMoveLeft() =>
        pictureBox1.Location = new Point(pictureBox1.Location.X - 15, pictureBox1.Location.Y);
    private void onMoveUp() => 
        pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 15);
    private void onMoveRight() =>
        pictureBox1.Location = new Point(pictureBox1.Location.X + 15,
            pictureBox1.Location.Y);
    private void onMoveDown() =>
        pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 15);
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_HOTKEY = 0x0312;
}
```