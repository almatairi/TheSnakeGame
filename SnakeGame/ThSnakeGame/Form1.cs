using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; //compress the bitmat screenshot to a jpg file
using Timer = System.Timers.Timer;
using Timer1 = System.Timers.Timer;
namespace TheSnakeGame
{   
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>(); //list of of objects from the Circle class
        private Circle food = new Circle();              //food that increases score by one point
        private Circle doubleFood = new Circle();        //food that increases score by two points
        private Circle shrinkFood = new Circle();             //food that decreases the score by two points

        int maxWidth; //the maximum width that the Snake is allowed to travel
        int maxHeight;//the maximum height that the Snake is allowed to travel

        int score;   
        int highScore; 

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;

        public Form1()
        {
            InitializeComponent();
            new Settings();
        }

        //make sure the snake doesent go to the oposite direction
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }
        }

        //reset the keys back to false
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        //press Start button
        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        //press Take Snapshot button
        private void TakeSnapshot(object sender, EventArgs e)
        {   //create image label
            Label caption = new Label();
            caption.Text = "I scored: " + score + " and my Highscore is " + highScore + " on the Snake Game";
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.Green;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            //create save dialog box
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game SnapShot";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;// check if the image name is a valid name

            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height); 
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            // setting the directions
            if (goLeft)
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }

            for (int i = Snake.Count - 1; i >= 0; i--) 
            {
                if (i == 0)
                {
                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].X--; //move left on the X axis
                            break;
                        case "right":
                            Snake[i].X++; //move right on the X axis
                            break;
                        case "down":
                            Snake[i].Y++; //move up on the Y axis
                            break;
                        case "up":
                            Snake[i].Y--; //move down on the Y axis
                            break;
                    }

                    //appear to the other side
                    if (Snake[i].X < 0)   
                    {
                        Snake[i].X = maxWidth;
                    }
                    if (Snake[i].X > maxWidth)
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }

                    
                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();
                    }

                    if (Snake[i].X == doubleFood.X && Snake[i].Y == doubleFood.Y)
                    {
                        EatDoubleFood();
                    }

                    if (Snake[i].X == shrinkFood.X && Snake[i].Y == shrinkFood.Y)
                    {
                        Shrink();
                    }
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        //check of the snake's head has hit a part of its body OR the score has decreased below 0
                        //i->snake head j->snake body
                        if ((Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)||score<0)
                        {
                            GameOver();
                        }
                        
                    }
                }
                else
                {  //make the body follow the head
                    Snake[i].X = Snake[i - 1].X; 
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
            picCanvas.Invalidate();//redraw with every tick 
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            if (Settings.GameOver == false)
            {
                
                Graphics canvas = e.Graphics;

                Brush snakeColour;

                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        snakeColour = Brushes.Black; //color the head
                    }
                    else
                    {
                        snakeColour = Brushes.DarkGreen; //color the body
                    }

                    canvas.FillEllipse(snakeColour, new Rectangle
                        (
                        Snake[i].X * Settings.Width,
                        Snake[i].Y * Settings.Height,
                        Settings.Width, Settings.Height
                        ));
                }

                //color the food
                canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                (
                food.X * Settings.Width,
                food.Y * Settings.Height,
                Settings.Width, Settings.Height
                ));

                canvas.FillEllipse(Brushes.Gold, new Rectangle
               (
               doubleFood.X * Settings.Width,
               doubleFood.Y * Settings.Height,
               Settings.Width, Settings.Height
               ));

                canvas.FillEllipse(Brushes.Purple, new Rectangle
              (
              shrinkFood.X * Settings.Width,
              shrinkFood.Y * Settings.Height,
              Settings.Width, Settings.Height
              ));

            }
        }
        //function for restasting a new game
        private void RestartGame()
        {
            
            maxWidth = picCanvas.Width / Settings.Width - 1;  //to make sure that the snake doesnt go too close to the edges
            maxHeight = picCanvas.Height / Settings.Height - 1;

            Snake.Clear(); // check if there are elements in the list and clear it

            startButton.Enabled = false; 
            snapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;

            Circle head = new Circle { X = 10, Y = 5 }; //placement of the snake
            Snake.Add(head); // adding the head OF the snake to the list 

            for (int i = 0; i < 5; i++) //add five body parts to the snake
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) }; //generate food in a random position(not to close to the edges)
            doubleFood = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            Timer x = new Timer(10000);
            x.AutoReset = true;
            x.Elapsed += new System.Timers.ElapsedEventHandler(myTimer);

            shrinkFood = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            Timer1 x1 = new Timer(10000);
            x1.AutoReset = true;
            x1.Elapsed += new System.Timers.ElapsedEventHandler(myTimer2);
            
            gameTimer.Start();   //start the game timer

            x.Start();
            x1.Start();
        }

        //function for "eating" the red food
        private void EatFood()
        {
            score += 1;
           txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {//add to the last index of the snake
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };//create new food on a random position
        }

        //function for "eating" the yellow food
        private void EatDoubleFood()
        {
            score += 2;
            txtScore.Text = "Score: " + score;
           
            for(int i = 0; i < 2; i++)
            {
                Circle body = new Circle
                {//add to the last index of the snake
                    X = Snake[Snake.Count - 1].X,
                    Y = Snake[Snake.Count - 1].Y
                };
                Snake.Add(body);
            }
            doubleFood = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
        }

        ////function for "eating" the purple food
        private void Shrink()
        {
            score -= 2;
            txtScore.Text = "Score: " + score;
            
            for (int i = 0; i < 2; i++)
            {
                //remove at the last index of the snake
                Snake.RemoveAt(Snake.Count - 1);
            }

            shrinkFood = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
        }


        private void GameOver()
        {
            gameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;

            if (score > highScore)
            {
                highScore = score;
                txtHighScore.Text = "High Score: " + Environment.NewLine + highScore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
        //timer for the yellow food
        public void myTimer(object sender,System.Timers.ElapsedEventArgs e)
        {
            doubleFood = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
        }
        //timer for the purple food
        public void myTimer2(object sender, System.Timers.ElapsedEventArgs e)
        {
           shrinkFood = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
        }
    }
}
