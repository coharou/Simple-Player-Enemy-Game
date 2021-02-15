using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simple_PvE_game.gameinfo
{
    public class Customization : gameview.ObservableObject
    {
        private int _skillPts;
        private bool _inPlay;
        private int _skillCap;

        public int UsedSkillPt
        {
            get { return _skillPts; }
            set { 
                _skillPts = value;
                OnPropertyChanged(nameof(UsedSkillPt));
            }
        }

        public int SkillPtCap
        {
            get { return _skillCap; }
            set { _skillCap = value;
                OnPropertyChanged(nameof(SkillPtCap));
            }
        }

        private int _difference;

        public int SkillDifference
        {
            get { return _difference; }
            set { _difference = value; 
                OnPropertyChanged(nameof(SkillDifference)); }
        } 

        public bool InPlay
        {
            get { return _inPlay; }
            set { _inPlay = value; }
        }

        private string _skillUseInfo;

        public string SkillUseInfo
        {
            get { return _skillUseInfo; }
            set 
            { 
                _skillUseInfo = value;
                OnPropertyChanged(nameof(SkillUseInfo));
            }
        }

        private bool _passCustoms;

        public bool PassesCustoms
        {
            get { return _passCustoms; }
            set 
            { 
                _passCustoms = value;
                OnPropertyChanged(nameof(PassesCustoms));
            }
        }

        public Customization(int skillPtCap)
        {
            _skillPts = 0;
            _skillCap = skillPtCap;
            _skillUseInfo = "Use all skill points provided. You may not proceed with points leftover, or with too many applied.";
            _passCustoms = false;
        }
    }
}
