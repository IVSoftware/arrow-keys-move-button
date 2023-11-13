# Arrow Keys Move Button

Key messages go to the control that has focus and chances are that is 'not' the main `Form`. Subscribing to `Form1_KeyDown` is unlikely to be called and doubtful to move the picture box. As you have observed **when buttons are introduced in the form** what changes is that one of those buttons will get the focus, and whichever one happens to have the focus will get the key events.

Try intercepting the **Win32** [WM_KEYDOWN](https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keydown) message with `override WndProc()` before it gets dispatched to the focused control as shown here. This should move the picture box as you intend.

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
        pictureBox1.Location = new Point(pictureBox1.Location.X + 15,  pictureBox1.Location.Y);
    private void onMoveDown() =>
        pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 15);
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_HOTKEY = 0x0312;
}
```
___
**Alternative**

Since key events on their own behave in a way that doesn't meet the objective, it's much cleaner to inherit from `PictureBox` to make a control that can focus and knows how to move _itself_ by handling key events that it would receive once it has focus. 

To use this control, go to the Designer.cs file and swap out instances of `PictureBox` for extended class `PictureBoxEx`. Here's the final, alternative solution with the `Form` class simplified as a result.

```
public partial class MainForm : Form
{
    public MainForm() => InitializeComponent();
}

class PictureBoxEx : PictureBox
{
    // PictureBox is not focusable by default so we do it ourselves.
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        Focus();
        foreach (var pb in Parent.Controls.OfType<PictureBoxEx>())
        {
            pb.BorderStyle = BorderStyle.None;
        }
        BorderStyle = BorderStyle.FixedSingle;   
    }
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        switch (e.KeyData)
        {
            case Keys.Left:
                Location = new Point(Location.X - 15, Location.Y);
                break;
            case Keys.Up:
                Location = new Point(Location.X, Location.Y - 15);
                break;
            case Keys.Right:
                Location = new Point(Location.X + 15, Location.Y);
                break;
            case Keys.Down:
                Location = new Point(Location.X, Location.Y + 15);
                break;
        }
    }
}


