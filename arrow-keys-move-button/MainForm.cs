using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace arrow_keys_move_button
{
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
}