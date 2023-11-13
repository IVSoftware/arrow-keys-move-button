# Arrow Keys Move Button

Key messages go to the control that has focus and chances are that is 'not' the main `Form`. Subscribing to `Form1_KeyDown` is unlikely to be called and doubtful to move the picture box as you have observed. 

What will help here is intercepting the _unfiltered_ **Win32** [WM_KEYDOWN](https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keydown) message with `override WndProc()` to do it. Try something like this:

```
public partial class MainForm : Form
{
    public MainForm() => InitializeComponent();
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (m.Msg == WM_KEYDOWN)
        {
            var e = new KeyEventArgs(((Keys)m.WParam) | Control.ModifierKeys);
            OnHotKeyPreview(FromHandle(m.HWnd), e);
            if (e.Handled) m.Result = IntPtr.Zero;
        }
    }
    private void OnHotKeyPreview(Control? control, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.Left: onMoveLeft(); break;
            case Keys.Up: onMoveUp(); break;
            case Keys.Right:  onMoveRight(); break;
            case Keys.Down: onMoveDown(); break;
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