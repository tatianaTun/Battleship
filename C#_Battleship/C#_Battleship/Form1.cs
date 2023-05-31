using System;
using System.Diagnostics.Metrics;
using System.Web;

namespace C__Battleship
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PictureBox[,] battleshipPlayer1 = new PictureBox[10, 10];
        PictureBox[,] battleshipPlayer2 = new PictureBox[10, 10];
        int Player1Hit = 0;
        int Player2Hit = 0;

        //Hashset - unordered collection of unique elements
        HashSet<int> PlayerLocations = new HashSet<int>();
        HashSet<int> ComputerLocations = new HashSet<int>();
        List<int> CompLocations = new List<int>(); //initiated here so we have access to it in the PictureboxClick event
        private void PrintBoard()
        {//Player 1
            int xpos = 45;
            int ypos = 45;
            for (int i = 0; i < battleshipPlayer1.GetLength(0); i++)
            {
                for (int j = 0; j < battleshipPlayer1.GetLength(1); j++)
                {
                    battleshipPlayer1[i, j] = new PictureBox
                    {
                        Name = "picturebox" + i.ToString() + j.ToString(),
                        Location = new Point(xpos, ypos),
                        ImageLocation = "./sea.jpg",
                        Size = new Size(43, 43),
                        Visible = true,
                        BackColor = Color.Black
                    };
                    this.Controls.Add(battleshipPlayer1[i, j]);
                    //battleshipPlayer1[i, j].Click += new EventHandler(PictureBoxClick); -------- I don't need this here because we are building a 1 player game
                    xpos += 45;
                }
                xpos = 45;
                ypos += 45;
            }
            //Player 2 - computer
            xpos = 550;
            ypos = 45;
            for (int i = 0; i < battleshipPlayer2.GetLength(0); i++)
            {
                for (int j = 0; j < battleshipPlayer2.GetLength(1); j++)
                {
                    battleshipPlayer2[i, j] = new PictureBox
                    {
                        Name = "picturebox" + i.ToString() + j.ToString(),
                        Location = new Point(xpos, ypos),
                        ImageLocation = "./sea.jpg",
                        Size = new Size(43, 43),
                        Visible = true,
                        BackColor = Color.Black
                    };
                    this.Controls.Add(battleshipPlayer2[i, j]);
                    battleshipPlayer2[i, j].Click += new EventHandler(Player1Click);
                    xpos += 45;
                }
                xpos = 550;
                ypos += 45;
            }
            textBox1.Text = $"Player 1 score: {Player1Hit}";
            textBox2.Text = $"Computer score: {Player2Hit}";
        }
        void TurnAssignment()
        {
            Random random = new Random();
            int TurnPlayer1or2 = random.Next();
            if (TurnPlayer1or2 % 2 == 0)
            {
                MessageBox.Show("Player's Turn");
            }
            else
            {
                MessageBox.Show("Computer's Turn");
                ComputerTurn();
            }
        }
        void RandomAssignment()
        {
            Random rnd = new Random();
            while (PlayerLocations.Count < 8)
            {
                //here we are creating x and y locations and store them separately in hashset
                int xLocation = rnd.Next(10);
                PlayerLocations.Add(xLocation);
                int yLocation = rnd.Next(10);
                PlayerLocations.Add(yLocation);
            }
            while (ComputerLocations.Count < 8)
            {
                int xLocation = rnd.Next(10);
                ComputerLocations.Add(xLocation);
                int yLocation = rnd.Next(10);
                ComputerLocations.Add(yLocation);
            }
        }
        void DisplayShips()
        {
            List<int> MyPlace = PlayerLocations.ToList(); //list is like an array, you can access it like an array. The reason we are using the list is because of how easy it is to convert the hashset to list by adding .ToList(). You cannot access hashset by index,
            battleshipPlayer1[MyPlace[0], MyPlace[1]].ImageLocation = "./ship1.png";
            battleshipPlayer1[MyPlace[2], MyPlace[3]].ImageLocation = "./ship1.png";
            battleshipPlayer1[MyPlace[4], MyPlace[5]].ImageLocation = "./ship1.png";
            battleshipPlayer1[MyPlace[6], MyPlace[7]].ImageLocation = "./ship1.png";

            CompLocations = ComputerLocations.ToList();
            battleshipPlayer2[CompLocations[0], CompLocations[1]].ImageLocation = "./sea.jpg"; //"./sea.jpg"
            battleshipPlayer2[CompLocations[2], CompLocations[3]].ImageLocation = "./sea.jpg";
            battleshipPlayer2[CompLocations[4], CompLocations[5]].ImageLocation = "./sea.jpg";
            battleshipPlayer2[CompLocations[6], CompLocations[7]].ImageLocation = "./sea.jpg";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrintBoard();
            RandomAssignment();
            DisplayShips();
            TurnAssignment();
        }

        private void Player1Click(object sender, EventArgs e)
        {
            PictureBox player1click = (PictureBox)sender;
            int xAttack = Convert.ToInt32(player1click.Name.Substring(10, 1));
            int yAttack = Convert.ToInt32(player1click.Name.Substring(11));

            if ((CompLocations[0] == xAttack && CompLocations[1] == yAttack) || (CompLocations[2] == xAttack && CompLocations[3] == yAttack) || (CompLocations[4] == xAttack && CompLocations[5] == yAttack) || (CompLocations[6] == xAttack && CompLocations[7] == yAttack))
            {
                player1click.ImageLocation = "./hit.png";
                MessageBox.Show($"You hit row {xAttack + 1}, column {yAttack + 1}. Now computer turn");
                Player1Hit++;
                textBox1.Text = $"Player 1 score: {Player1Hit}";
            }
            else if (player1click.ImageLocation == "./sea.jpg")
            {
                player1click.ImageLocation = "./miss.png";
                MessageBox.Show($"You miss row {xAttack + 1}, column {yAttack + 1}. Now computer turn");
            }
            else
            {
                MessageBox.Show("Already pressed before, please choose another location");
                return;

            }
            bool checkEndGame = WinCheck();
            if (checkEndGame == false)
            {
                ComputerTurn();
            }
        }
        void ComputerTurn()
        {
            Random rnd = new Random();
            int xAttack = rnd.Next(10);
            int yAttack = rnd.Next(10);
            if (battleshipPlayer1[xAttack, yAttack].ImageLocation == "./ship1.png")
            {
                battleshipPlayer1[xAttack, yAttack].ImageLocation = "./hit.png";
                MessageBox.Show($"Computer hit row {xAttack + 1}, column {yAttack + 1}. Now player turn");
                Player2Hit++;
                textBox2.Text = $"Computer score: {Player2Hit}";
            }
            else if (battleshipPlayer1[xAttack, yAttack].ImageLocation == "./sea.jpg")
            {
                battleshipPlayer1[xAttack, yAttack].ImageLocation = "./miss.png";
                MessageBox.Show($"Computer misses row {xAttack + 1}, column {yAttack + 1}. Now player turn");
            }
            else
            {
                ComputerTurn();
            }
            WinCheck();
        }
        bool WinCheck()
        {
            if (Player1Hit == 4)
            {
                MessageBox.Show("Congratulations, Player 1! You won!");
                Reset();
                return true;
            }
            else if (Player2Hit == 4)
            {
                MessageBox.Show("Player 2(Computer) wins");
                Reset();
                return true;
            }
            return false;
        }
        void Reset()
        {
            Player1Hit = 0;
            Player2Hit = 0;
            textBox1.Text = $"Player 1 score: {Player1Hit}";
            textBox2.Text = $"Computer score: {Player2Hit}";
            PlayerLocations.Clear();
            ComputerLocations.Clear();
            MessageBox.Show("Start a new game");
            //PrintBoard();
            for (int i = 0; i < battleshipPlayer1.GetLength(0); i++)
            {
                for (int j = 0; j < battleshipPlayer1.GetLength(1); j++)
                {
                    battleshipPlayer1[i, j].ImageLocation = "./sea.jpg";
                    battleshipPlayer2[i, j].ImageLocation = "./sea.jpg";
                }
            }
            RandomAssignment();
            DisplayShips();
            TurnAssignment();
        }
    }
}