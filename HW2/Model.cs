using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Brushes
using System.Windows.Media;

// observable collections
using System.Collections.ObjectModel;

// INotifyPropertyChanged
using System.ComponentModel;


// debug output
using System.Diagnostics;

namespace HW2
{
    class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 9;
        private UInt32[] _buttonPresses = new UInt32[_numTiles];
        Random _randomNumber = new Random();

        int turn = 0;
        int[] scores = new int[8];
        bool[] winningScore = new bool[8];
        bool won = false;
        char winner;


        private String _statusText = "";
        public String StatusText

        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }

        public void zeroScores(int[] input)
        {
            for(int i = 0; i < input.Length; i++)
            {
                input[i] = 0;
                winningScore[i] = false;
            }
        }


        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
            TileCollection = new ObservableCollection<Tile>();
            for (int i = 0; i < _numTiles; i++)
            {
                TileCollection.Add(new Tile()
                {
                    TileBrush = Brushes.Black,
                    TileLabel = "",
                    TileName = i.ToString(),
                    TileBackground = Brushes.LightGray
                });
            }
        }

        /// <summary>
        /// processes all buttons. called from view when a button is clicked
        /// </summary>
        /// <param name="buttonSelected"></param>
        /// <returns></returns>
        public void UserSelection(String buttonSelected)
        {
            Debug.Write("Button selected was " + buttonSelected + "\n");

            int index = int.Parse(buttonSelected);

            if (!TileCollection[index].TileTaken)
            {
                TileCollection[index].TileTaken = true;

                if(turn % 2 == 0)
                {
                    TileCollection[index].TileLabel = "X";
                }
                else
                {
                    TileCollection[index].TileLabel = "O";
                    TileCollection[index].TileBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                }

                AddScores(index, turn);


                // check for win
                for (int i = 0; i < scores.Length; i++)
                {
                    if(scores[i] >= 3)//x wins
                    {
                        won = true;
                        winner = 'X';
                        winningScore[i] = true;
                    }
                    if(scores[i] <= -3)//o wins
                    {
                        won = true;
                        winner = 'O';
                        winningScore[i] = true;
                    }
                }

                // check for tie
                int takenCount = 0;
                
                for (int i = 0; i < _numTiles; i++)
                {
                    if (TileCollection[i].TileTaken)
                    {
                        takenCount++;
                    }
                }
                if (takenCount >= 9)
                {
                    StatusText = "Tie! Play again?";
                }

                if (won)
                {
                    highlightWin(winningScore);
                    StatusText = winner + " won! Play again?";
                }
                
                turn++;
            }
        }

        public void highlightWin(bool[] input)
        {
            int x;
            for(x = 0; x < input.Length; x++)
            {
                if (input[x] == true)
                    break;
            }
            switch (x)
            {
                case 0:
                    TileCollection[0].TileBackground = Brushes.Aqua;
                    TileCollection[1].TileBackground = Brushes.Aqua;
                    TileCollection[2].TileBackground = Brushes.Aqua;
                    break;
                case 1:
                    TileCollection[3].TileBackground = Brushes.Aqua;
                    TileCollection[4].TileBackground = Brushes.Aqua;
                    TileCollection[5].TileBackground = Brushes.Aqua;
                    break;
                case 2:
                    TileCollection[6].TileBackground = Brushes.Aqua;
                    TileCollection[7].TileBackground = Brushes.Aqua;
                    TileCollection[8].TileBackground = Brushes.Aqua;
                    break;
                case 3:
                    TileCollection[0].TileBackground = Brushes.Aqua;
                    TileCollection[3].TileBackground = Brushes.Aqua;
                    TileCollection[6].TileBackground = Brushes.Aqua;
                    break;
                case 4:
                    TileCollection[1].TileBackground = Brushes.Aqua;
                    TileCollection[4].TileBackground = Brushes.Aqua;
                    TileCollection[7].TileBackground = Brushes.Aqua;
                    break;
                case 5:
                    TileCollection[2].TileBackground = Brushes.Aqua;
                    TileCollection[5].TileBackground = Brushes.Aqua;
                    TileCollection[8].TileBackground = Brushes.Aqua;
                    break;
                case 6:
                    TileCollection[0].TileBackground = Brushes.Aqua;
                    TileCollection[4].TileBackground = Brushes.Aqua;
                    TileCollection[8].TileBackground = Brushes.Aqua;
                    break;
                case 7:
                    TileCollection[2].TileBackground = Brushes.Aqua;
                    TileCollection[4].TileBackground = Brushes.Aqua;
                    TileCollection[6].TileBackground = Brushes.Aqua;
                    break;
            }
        }

        /// <summary>
        /// Adds scores to correct values
        /// </summary>
        public void AddScores(int index, int turn)
        {

            // r1, r2, r3, c1, c2, c3, d1, d2
            //  0   1   2   3   4   5   6   7

            // 0   1   2
            //
            // 3   4   5
            //
            // 6   7   8
            int scoreMod;

            if(turn % 2 == 0)
            {
                scoreMod = 1;
            }
            else
            {
                scoreMod = -1;
            }

            switch (index)
            {
                case 0:
                    scores[0] += scoreMod;
                    scores[3] += scoreMod;
                    scores[6] += scoreMod;
                    break;
                case 1:
                    scores[0] += scoreMod;
                    scores[4] += scoreMod;
                    break;
                case 2:
                    scores[0] += scoreMod;
                    scores[5] += scoreMod;
                    scores[7] += scoreMod;
                    break;
                case 3:
                    scores[1] += scoreMod;
                    scores[3] += scoreMod;
                    break;
                case 4:
                    scores[1] += scoreMod;
                    scores[4] += scoreMod;
                    scores[6] += scoreMod;
                    scores[7] += scoreMod;
                    break;
                case 5:
                    scores[1] += scoreMod;
                    scores[5] += scoreMod;
                    break;
                case 6:
                    scores[2] += scoreMod;
                    scores[3] += scoreMod;
                    scores[7] += scoreMod;
                    break;
                case 7:
                    scores[2] += scoreMod;
                    scores[4] += scoreMod;
                    break;
                case 8:
                    scores[2] += scoreMod;
                    scores[5] += scoreMod;
                    scores[6] += scoreMod;
                    break;
            }
        }


        /// <summary>
        /// resets all buttons back to their starting point
        /// </summary>
        /// <param name></param>
        /// <returns></returns>
        public void Clear()
        {
            for (int x = 0; x < _numTiles; x++)
            {
                TileCollection[x].TileBrush = Brushes.Black;
                TileCollection[x].TileLabel = "";
                TileCollection[x].TileTaken = false;
                turn = 0;
                won = false;
                winner = ' ';
                zeroScores(scores);
                _buttonPresses[x] = 0;

            }

            StatusText = "";
        }
    }
}
