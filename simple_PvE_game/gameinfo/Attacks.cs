using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simple_PvE_game.gameinfo
{
    public class Attacks
    {
        private string _name;
        private string _move;
        private string _desc;
        private int _damage;
        private int _accuracy;
        private int _critical;
        private int _penetration;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Move
        {
            get { return _move; }
            set { _move = value; }
        }

        public string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public int Accuracy
        {
            get { return _accuracy; }
            set { _accuracy = value; }
        }

        public int Critical
        {
            get { return _critical; }
            set { _critical = value; }
        }

        public int Penetration
        {
            get { return _penetration; }
            set { _penetration = CalcPenetration(Damage); }
        }

        private static int CalcPenetration(int damage)
        {
            int penetration;
            if (damage >= 0 && damage <= 150)
            {
                penetration = 1;
            }
            else if (damage > 150 && damage <= 300)
            {
                penetration = 2;
            }
            else if (damage > 300 && damage <= 450)
            {
                penetration = 3;
            }
            else if (damage > 450 && damage <= 600)
            {
                penetration = 4;
            }
            else if (damage > 600 && damage <= 750)
            {
                penetration = 5;
            }
            else
            {
                penetration = 6;
            }
            return penetration;
        }

        public Attacks(string name, string move, string desc, int damage, int accuracy, int critical)
        {
            _name = name;
            _move = move;
            _desc = desc;
            _damage = damage;
            _accuracy = accuracy;
            _critical = critical;
            _penetration = CalcPenetration(damage);
        }
    }
}
