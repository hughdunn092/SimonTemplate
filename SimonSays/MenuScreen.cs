using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimonSays
{
    public partial class MenuScreen : UserControl
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            //TODO: remove this screen and start the GameScreen
            Form f = this.FindForm();
            f.Controls.Remove(this);

            GameScreen gameScreen = new GameScreen();
            f.Controls.Add(gameScreen);

            gameScreen.Location = new Point((this.Width - gameScreen.Width) / 2, (this.Height - gameScreen.Height) / 2);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            //TODO: end the application
            Application.Exit();
        }
    }
}
