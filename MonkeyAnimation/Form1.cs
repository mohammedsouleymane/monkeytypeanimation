using MonkeyAnimation;

namespace MonkeyAnimation
{
    public partial class Form1 : Form
    {
        private bool dragging;
        private Point startPoint = new Point(0, 0);
        public Form1()
        {
            InitializeComponent();
            KeyLogger._pictureBox = pictureBox1;
            ShowInTaskbar = false;
            notifyIcon1.Text = "Monkey";
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:
                    ShowIcon = false;
                    notifyIcon1.Visible = true;
                    Hide();
                    break;
                case MouseButtons.Right:
                    Application.Exit();
                    break;
                default:
                    dragging = true;
                    startPoint = new Point(e.X, e.Y);
                    break;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {

            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Application.Exit();
            }
            ShowIcon = true;
            notifyIcon1.Visible = false;
            Show();
        }
    }
}