# Arrow Keys Move Button

Indeed, you are correct that as soon as there is a focused button on the form, it will monopolize Key messages and `Form1_KeyDown` will no longer be called or move the picture box as you have observed.

The way around this is to implement a custom message filter by implementing [IMessageFilter](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.application.addmessagefilter?view=windowsdesktop-7.0&devlangs=csharp&f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Windows.Forms.Application.AddMessageFilter)%3Bk(DevLang-csharp)%26rd%3Dtrue) so that we can choose to intercept **Win32** [WM_KEYDOWN](https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keydown) message unconditionally. Try something like this:
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
___
**Alternative**

The cleaner approach, however, would be to inherit from `PictureBox` and implement the `IMessageFilter` _there_, to make an extended `PictureBoxEx` control that knows how to move _itself_ by handling the intercepted key events. 

[![move either picture boxe][1]][1]

To use this control, go to the Designer.cs file and swap out instances of `PictureBox` for extended class `PictureBoxEx`. Here's the final, alternative solution with the `Form` class simplified as a result. The scheme here is to set the border of the picture box(es) to move.

```
public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        buttonDeselect.Click += (sender, e) =>
        {
            foreach (var pb in Controls.OfType<PictureBoxEx>())
            {
                pb.BorderStyle = BorderStyle.None;
            }
        };
    }
}
```

**PictureBoxEx**

```
class PictureBoxEx : PictureBox, IMessageFilter
{
    public PictureBoxEx()
    {
        Application.AddMessageFilter(this);
        Disposed += (sender, e) => Application.RemoveMessageFilter(this);
    }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (ModifierKeys != Keys.Control)   // Allow multiple select.
        {
            foreach (var pb in Parent.Controls.OfType<PictureBoxEx>())
            {
                pb.BorderStyle = BorderStyle.None;
            }
        }
        BorderStyle = BorderStyle.FixedSingle;
    }
    private const int WM_KEYDOWN = 0x0100;
    public bool PreFilterMessage(ref Message m)
    {
        if ((m.Msg == WM_KEYDOWN) && (BorderStyle == BorderStyle.FixedSingle))
        {

            var e = new KeyEventArgs(((Keys)m.WParam) | Control.ModifierKeys);
            OnKeyDown(e);
            return (e.Handled);
        }
        return false;
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
```


  [1]: https://i.stack.imgur.com/js8nL.png


