using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simple_PvE_game.gameinfo
{
    public class Character : gameview.ObservableObject
    {
        private string _name;
        private int _health;
        private int _strength;
        private int _defense;
        private int _precision;
        private int _agility;
        private int _luck;
        private gameinfo.Attacks _weapon;
        private gameinfo.Attacks _attack1;
        private gameinfo.Attacks _attack2;

        public string Name
        {
            get { return _name; }
            set { 
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Health
        {
            get { return _health; }
            set { 
                _health = value;
                OnPropertyChanged(nameof(Health));
            }
        }

        public int Strength
        {
            get { return _strength; }
            set { 
                _strength = value;
                OnPropertyChanged(nameof(Strength));
            }
        }

        public int Defense
        {
            get { return _defense; }
            set { 
                _defense = value;
                OnPropertyChanged(nameof(Defense));
            }
        }

        public int Precision
        {
            get { return _precision; }
            set { 
                _precision = value;
                OnPropertyChanged(nameof(Precision));
            }
        }

        public int Agility
        {
            get { return _agility; }
            set { 
                _agility = value;
                OnPropertyChanged(nameof(Agility));
            }
        }

        public int Luck
        {
            get { return _luck; }
            set { 
                _luck = value;
                OnPropertyChanged(nameof(Luck));
            }
        }

        public gameinfo.Attacks Weapon
        {
            get { return _weapon; }
            set { 
                _weapon = value;
                OnPropertyChanged(nameof(Weapon));
            }
        }

        public gameinfo.Attacks iWeapon
        {
            get { return _attack1; }
            set { 
                _attack1 = value;
                OnPropertyChanged(nameof(iWeapon));
            }
        }

        public gameinfo.Attacks iiWeapon
        {
            get { return _attack2; }
            set { 
                _attack2 = value;
                OnPropertyChanged(nameof(iiWeapon));
            }
        }

        private gameinfo.Attacks _chosenAtk;

        public gameinfo.Attacks ChosenAttack
        {
            get { return _chosenAtk; }
            set 
            {
                OnPropertyChanged(nameof(ChosenAttack));
                _chosenAtk = value; 
            }
        }


        public Character(string name, int health, int strength, int defense, int precision, int agility, int luck, gameinfo.Attacks weaponAtk, gameinfo.Attacks stanAtk1, gameinfo.Attacks stanAtk2)
        {
            _name = name;
            _health = health;
            _strength = strength;
            _defense = defense;
            _precision = precision;
            _agility = agility;
            _luck = luck;
            _weapon = weaponAtk;
            _attack1 = stanAtk1;
            _attack2 = stanAtk2;
            _chosenAtk = stanAtk1;
        }
    }
}
