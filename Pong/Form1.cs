/*
 * Description:     A basic PONG simulator
 * Author:    Valentina Montoya       
 * Date:     Feb 5 2023       
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);
        //SoundPlayer paddleHit = new SoundPlayer(Properties.Resources.);
        //SoundPlayer wallbounce = new SoundPlayer(Properties.Resources.wall bounce);

        //determines whether a key is being pressed or not
        Boolean wKeyDown, sKeyDown, upKeyDown, downKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball values
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        int BALL_SPEED = 4;
        const int BALL_WIDTH = 8;
        const int BALL_HEIGHT = 8;
        Rectangle ball;

        //player values
        const int PADDLE_SPEED = 4;
        const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            
        const int PADDLE_WIDTH = 10;
        const int PADDLE_HEIGHT = 40;
        Rectangle player1, player2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 2;  // number of points needed to win game

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.Escape:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                countDown.Visible = false;
                gameUpdateLoop.Start();
            }

            //player start positions
            player1 = new Rectangle(PADDLE_EDGE, this.Height / 2 - PADDLE_HEIGHT / 2, PADDLE_WIDTH, PADDLE_HEIGHT);
            player2 = new Rectangle(this.Width - PADDLE_EDGE - PADDLE_WIDTH, this.Height / 2 - PADDLE_HEIGHT / 2, PADDLE_WIDTH, PADDLE_HEIGHT);

            // TODO create a ball rectangle in the middle of screen
            ball = new Rectangle(this.Width / 2, this.Height / 2, BALL_WIDTH, BALL_HEIGHT);

        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>

        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {

            #region update ball position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if (ballMoveRight == false)
            {
                ball.X -= BALL_SPEED;
            }
            if (ballMoveRight == true)
            {
                ball.X += BALL_SPEED;
            }

            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED

            if (ballMoveDown == true)
            {
                ball.Y -= BALL_SPEED;
            }
            if (ballMoveDown == false)
            {
                ball.Y += BALL_SPEED;
            }
            #endregion

            #region update paddle positions
            //player 1 movement
            if (wKeyDown == true && player1.Y > 0)
            {
                player1.Y -= PADDLE_SPEED;
            }

            if (sKeyDown == true && player1.Y < this.Height - PADDLE_HEIGHT)
            {
                player1.Y += PADDLE_SPEED;
            }

            //player 2 movement
            if (downKeyDown == true && player2.Y < this.Height - PADDLE_HEIGHT)
            {
                player2.Y += PADDLE_SPEED;
            }

            if (upKeyDown == true && player2.Y > 0)
            {
                player2.Y -= PADDLE_SPEED;
            }

            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0) // if ball hits top line
            {
                if (ballMoveDown == true)
                {
                    ballMoveDown = false;
                    collisionSound.Play();
                }
                else
                {
                    ballMoveDown = true;
                    collisionSound.Play();
                }
            }
            // TODO In an else if statement check for collision with bottom line
            else if (ball.Y > this.Height - PADDLE_HEIGHT)
            {
                if (ballMoveDown == true)
                {
                    ballMoveDown = false;
                    collisionSound.Play();
                }
                else
                {
                    ballMoveDown = true;
                    collisionSound.Play();
                }
            }

            #endregion

            #region ball collision with paddles

            // TODO create if statment that checks if player1 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction

            if (player1.IntersectsWith(ball))
            {
                //paddleHit.Play();
                ballMoveRight = true;
                BALL_SPEED++;
            }

            // TODO create if statment that checks if player2 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction
            if (player2.IntersectsWith(ball))
            {
                //Play.()
                ballMoveRight = false;
                BALL_SPEED++;
            }

            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                BALL_SPEED = 4;
                player2Score++;
                player2ScoreLabel.Text = $"{player2Score}";

                //wait a few seconds / add count down
                countDown.Visible = true;
                Thread.Sleep(1000);
                countDown.Text = "3";
                Thread.Sleep(1000);
                countDown.Text = "2";
                Thread.Sleep(1000);
                countDown.Text = "1";
                Thread.Sleep(1000);
                countDown.Text = "";

                SetParameters();
            }
            if(player2Score == 4)
            {
                //player2 = winner;
            }

            if (ball.X == this.Width)  // ball hits right wall logic
            {
                BALL_SPEED = 4;
                player1Score++;
                player1ScoreLabel.Text = $"{player1Score}";

                //wait a few seconds / add count down
                countDown.Visible = true;
                Thread.Sleep(1000);
                countDown.Text = "3";
                Thread.Sleep(1000);
                countDown.Text = "2";
                Thread.Sleep(1000);
                countDown.Text = "1";
                Thread.Sleep(1000);
                countDown.Text = "";

                SetParameters();
            }
            if (player1Score == 4)
            {
                
            }
            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();

        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;

            gameUpdateLoop.Stop();

            //if (player1 = "winner")
            {

            }
                startLabel.Text = "Would you like to play again?";

            // TODO create game over logic
            // --- stop the gameUpdateLoop
            // --- show a message on the startLabel to indicate a winner, (may need to Refresh).
            // --- use the startLabel to ask the user if they want to play again

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw player2 using FillRectangle
            e.Graphics.FillRectangle(whiteBrush, player1);
            e.Graphics.FillRectangle(whiteBrush, player2);

            // TODO draw ball using FillRectangle
            e.Graphics.FillRectangle(whiteBrush, ball);
        }

    }
}
