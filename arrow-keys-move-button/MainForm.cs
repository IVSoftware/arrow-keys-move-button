using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace arrow_keys_move_button
{
    public partial class MainForm : Form, IMessageFilter
    {
        public MainForm()
        {
            InitializeComponent();
            Application.AddMessageFilter(this);
            Disposed += (sender, e) => Application.RemoveMessageFilter(this);
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
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyData)
            {
                case Keys.Left: onMoveLeft(); e.Handled = true; break;
                case Keys.Up: onMoveUp(); e.Handled = true; break;
                case Keys.Right: onMoveRight(); e.Handled = true; break;
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
}