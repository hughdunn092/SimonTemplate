using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Security.Cryptography;
using System.Xml.Schema;

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        int arrayIndex;
        bool computerTurn = true;
        bool playerTurn = false;
        int counter = 0;

        //TODO: create an int guess variable to track what part of the pattern the user is at
        int guess;

        const int delayTime = 30;

        //arrays
        Color[] secondColorArray = {Color.LawnGreen, Color.Red, Color.Blue, Color.Yellow};
        Color[] firstColorArray = {Color.ForestGreen, Color.DarkRed, Color.DarkBlue, Color.Goldenrod};
        SoundPlayer[] soundArray = {new SoundPlayer(Properties.Resources.green), new SoundPlayer(Properties.Resources.red), new SoundPlayer(Properties.Resources.blue), new SoundPlayer(Properties.Resources.yellow), new SoundPlayer(Properties.Resources.mistake)};
        Button[] buttonArray = new Button[4];
        int[] blinkTimer = {0, 0, 0, 0};

        public GameScreen()
        {
            InitializeComponent();

            buttonArray[0] = greenButton;
            buttonArray[1] = redButton;
            buttonArray[2] = blueButton;
            buttonArray[3] = yellowButton;
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            //button regions for shape
            GraphicsPath buttonPath = new GraphicsPath();
            buttonPath.AddArc(5, 5, 200, 200, 180, 90);
            buttonPath.AddArc(75, 75, 62, 62, 270, -90);

            for (int i = 0; i < buttonArray.Length; i++)
            {
                buttonArray[i].Region = new Region(buttonPath);

                Matrix transformMatrix = new Matrix();
                transformMatrix.RotateAt(90, new PointF(55, 55));
                buttonPath.Transform(transformMatrix);
            }

            //TODO: clear the pattern list from form1
            Form1.pattern.Clear();
            //TODO: refresh
            Refresh();
            //TODO: pause for a bit
            Thread.Sleep(delayTime);
            //TODO: run ComputerTurn()
            ComputerTurn();
        }

        private void ComputerTurn()
        {

            //TODO: get rand num between 0 and 4  (0, 1, 2, 3) and add to pattern list. Each number represents a button. For example, 0 may be green, 1 may be blue, etc.
            Random randGen = new Random();
            int randNum = randGen.Next(0, 4);
            Form1.pattern.Add(randNum);

            //TODO: set guess value back to 0
            guess = 0;
        }

        public void GameOver()
        {
            //TODO: Play a game over sound
            soundArray[4].Play();
            //TODO: close this screen and open the GameOverScreen
            Form1.ChangeScreen(this, new GameOverScreen());
        }

        public void ClickEvent(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            //find which button was clicked and store it
            for (int i = 0; i < buttonArray.Length; i++)
            {
                if (buttonArray[i] == button)
                {
                    arrayIndex = i;
                }
            }

            //if button that flashed is the button that was pressed
            if (Form1.pattern[guess] == arrayIndex)
            {
                //blink the button and play a sound
                soundArray[arrayIndex].Play();
                buttonArray[arrayIndex].BackColor = secondColorArray[arrayIndex];
                guess++;
            }
            //else lose
            else
            {
                GameOver();
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //player turn
            if (playerTurn == true)
            {
                for (int i = 0; i < blinkTimer.Length; i++)
                {
                    //if button is lit up increase its timer
                    if (buttonArray[i].BackColor == secondColorArray[i])
                    {
                        blinkTimer[i]++;
                    }

                    //if buttons timer is above limit change it back to original colour
                    if (blinkTimer[i] >= delayTime)
                    {
                        buttonArray[i].BackColor = firstColorArray[i];
                        blinkTimer[i] = 0;
                    }

                    //if blink timer is reset and the guess number and pattern size are equal go to computer turn
                    if (guess == Form1.pattern.Count && blinkTimer[i] == 0)
                    {
                        //blinking lights to show round over
                        for (int g = 0; g < 4; g++)
                        {
                            for (int f = 0; f < buttonArray.Length; f++)
                            {
                                buttonArray[f].BackColor = secondColorArray[f];
                            }
                            this.Refresh();
                            Thread.Sleep(300);
                            for (int f = 0; f < buttonArray.Length; f++)
                            {
                                buttonArray[f].BackColor = firstColorArray[f];
                            }
                            this.Refresh();
                            Thread.Sleep(300);
                        }

                        //reset and change to computer turn
                        ComputerTurn();
                        counter = 0;

                        computerTurn = true;
                        playerTurn = false;
                    }
                }
            }

            //computer turn
            if (computerTurn == true)
            {
                //go one by one through the pattern lighting up buttons

                arrayIndex = Form1.pattern[counter];

                //play sound only once
                if (blinkTimer[arrayIndex] == 0)
                {
                    soundArray[arrayIndex].Play(); ;
                }
                //wait while light is on
                if (blinkTimer[arrayIndex] < delayTime)
                {
                    buttonArray[arrayIndex].BackColor = secondColorArray[arrayIndex];
                    blinkTimer[arrayIndex]++;
                }
                //wait while light is off
                else if (blinkTimer[arrayIndex] < delayTime * 2)
                {
                    buttonArray[arrayIndex].BackColor = firstColorArray[arrayIndex];
                    blinkTimer[arrayIndex]++;
                }
                //reset timer and move on to next part of the pattern
                else
                {
                    blinkTimer[arrayIndex] = 0;
                    counter++;
                }

                //if at the end of the pattern switch to player turn
                if (counter == Form1.pattern.Count)
                {
                    computerTurn = false;
                    playerTurn = true;
                }
            }

            this.Refresh();
        }
    }
}
