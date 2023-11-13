using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace arrow_keys_move_button
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
    }
    class PictureBoxEx : PictureBox
    {
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
}