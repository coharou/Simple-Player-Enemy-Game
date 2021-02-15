using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simple_PvE_game.gameinfo
{
    public class Gamestate : gameview.ObservableObject
    {
        private int _turns;

        public int Turns
        {
            get { return _turns; }
            set 
            { 
                _turns = value;
                OnPropertyChanged(nameof(Turns));
            }
        }

        public Random RandObj = ObtainRandObj();

        private static Random ObtainRandObj()
        {
            long tick = DateTime.Now.Ticks;
            int iTick = (int)tick;
            iTick = Math.Abs(iTick);
            Random _random = new Random(iTick);
            return _random;
        }

        private int _diff;

        public int TurnDiffChangedOn = 5;

        public int Difficulty
        {
            get { return _diff; }
            set 
            { 
                _diff = value;
                OnPropertyChanged(nameof(Difficulty));
            }
        }

        private Character _whoseMove;

        public Character WhoseMove
        {
            get { return _whoseMove; }
            set 
            { 
                _whoseMove = value;
                OnPropertyChanged(nameof(WhoseMove));
            }
        }

        private bool _canPlayerAttack;

        public bool CanPlayerAttack
        {
            get { return _canPlayerAttack; }
            set 
            { 
                _canPlayerAttack = value;
                OnPropertyChanged(nameof(CanPlayerAttack));
            }
        }

        private bool _gameEnded;

        public bool GameEnded
        {
            get { return _gameEnded; }
            set 
            { 
                _gameEnded = value;
                OnPropertyChanged(nameof(GameEnded));
            }
        }

        private string _gameMsg;

        public string GameMessage
        {
            get { return _gameMsg; }
            set 
            { 
                _gameMsg = value;
                OnPropertyChanged(nameof(GameMessage));
            }
        }


    }
}
