using simple_PvE_game.gameinfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simple_PvE_game.gameview
{
    public class Gamevm : ObservableObject
    {
        // ---------------------------------

        #region Gamestate

        private Gamestate _gamestate;

        public Gamestate Gamestate
        {
            get { return _gamestate; }
            set
            {
                _gamestate = value;
                OnPropertyChanged(nameof(Gamestate));
            }
        }

        private Modifiers _mods;

        public Modifiers Mods
        {
            get { return _mods; }
            set { _mods = value; }
        }


        #endregion

        // ---------------------------------

        #region Character Values

        private Character _player;
        private Character _enemy;

        public Character Player
        {
            get { return _player; }
            set 
            { 
                _player = value;
                OnPropertyChanged(nameof(Player));
            }
        }
        public Character Enemy
        {
            get { return _enemy; }
            set 
            { 
                _enemy = value;
                OnPropertyChanged(nameof(Enemy));
            }
        }

        #endregion

        // ---------------------------------

        #region Weapons
        public Attacks Slash { get; private set; }
        public Attacks Stone { get; private set; }
        public Attacks Axe { get; private set; }
        public Attacks Spear { get; private set; }
        public Attacks Kukri { get; private set; }
        public Attacks Sword { get; private set; }
        public Attacks Zwei { get; private set; }
        #endregion

        // ---------------------------------

        #region Enemy Types
        public Character Peasant { get; private set; } // Kukri
        public Character Knight1 { get; private set; } // Combat axe
        public Character Pikeman { get; private set; } // Zweihander
        public Character Knight2 { get; private set; } // Broadsword
        public Character Spearman { get; private set; } // Spear
        #endregion

        // ---------------------------------

        #region Customization
        private gameinfo.Customization _Customs;

        public gameinfo.Customization Customs
        {
            get { return _Customs; }
            set
            {
                _Customs = value;
                OnPropertyChanged(nameof(Customs));
            }
        }

        public Gamevm()
        {
            // These will be accessed when the Customization window is open
            InitPlayer();
            InitWeapons();
            InitCustomization();
        }

        private void InitCustomization()
        {
            Customs = new gameinfo.Customization(9);
            Customs.SkillDifference = Customs.SkillPtCap - Customs.UsedSkillPt;
        }

        private void InitPlayer()
        {
            Player = new Character("User", 1000, 0, 0, 0, 0, 0, Spear, Axe, Axe);
        }

        private void InitWeapons()
        {
            Slash = new Attacks("StdMov1", "Light Cut", "Standard attack move", 125, 85, 10);
            Stone = new Attacks("StdMov2", "Stone Bludgeon", "Standard attack move", 55, 100, 0);
            Axe = new Attacks("Combat Axe", "Powerful Chop", "From the Combat Axe", 185, 60, 20);
            Spear = new Attacks("Spear", "Thrust", "From the Spear", 105, 65, 50);
            Kukri = new Attacks("Kukri", "Quick Stab", "From the Kukri", 60, 100, 30);
            Sword = new Attacks("Broadsword", "Strong Slash", "From the Broadsword", 140, 75, 20);
            Zwei = new Attacks("Zweihänder", "Obliteration", "From the Zweihänder", 220, 30, 25);
        }

        public void UpdatePlayerSkills(string name, int value)
        {
            switch (name)
            {
                case "Strength":
                    Player.Strength = value;
                    break;
                case "Defense":
                    Player.Defense = value;
                    break;
                case "Precision":
                    Player.Precision = value;
                    break;
                case "Agility":
                    Player.Agility = value;
                    break;
                case "Luck":
                    Player.Luck = value;
                    break;
                default:
                    break;
            }
            CheckSkillsUsed();
        }

        public void UpdatePlayerWeapon(string name)
        {
            switch (name)
            {
                case nameof(Axe):
                    Player.Weapon = Axe;
                    break;
                case nameof(Spear):
                    Player.Weapon = Spear;
                    break;
                case nameof(Kukri):
                    Player.Weapon = Kukri;
                    break;
                case nameof(Sword):
                    Player.Weapon = Sword;
                    break;
                case nameof(Zwei):
                    Player.Weapon = Zwei;
                    break;
                default:
                    break;
            }
            Player.iWeapon = Stone;
            Player.iiWeapon = Slash;
        }

        public void CheckSkillsUsed()
        {
            int totalPoints = Player.Strength + Player.Defense + Player.Precision + Player.Agility + Player.Luck;
            Customs.SkillDifference = Customs.SkillPtCap - totalPoints;

            if (Customs.SkillDifference == 0)
            {
                // If the user has used the right amount of points...
                Customs.SkillUseInfo = "Continue forward!";
                Customs.PassesCustoms = true;
            }
            else if (Customs.SkillDifference > 0)
            {
                // If the user still has points leftover to use...
                Customs.SkillUseInfo = "Assign the remaining skill points to the categories of your choice.";
                Customs.PassesCustoms = false;
            }
            else
            {
                // If the user has used too many points...
                Customs.SkillUseInfo = "Too many points have been assigned. Remove skill points from one or multiple categories.";
                Customs.PassesCustoms = false;
            }
        }
        #endregion

        // ---------------------------------

        #region AI Scripting

        private void AI_AttackProcedure()
        {
            // Ensure the user cannot attack, and that the enemy will be displayed in UI
            Gamestate.CanPlayerAttack = false;
            Gamestate.WhoseMove = Enemy;

            // Check which weapon would be most viable to use
            AI_DetermineViableWeapon();

            // Attack the player with the weapon and modifications
            Player = ProcessAttack(Enemy, Player);

            // Checks to see if player has survived, and responds accordingly
            PlayerSurvival();
        }

        private void AI_DetermineViableWeapon()
        {
            // Flavors represents the AI likelihood to make a decision
            // For 'laststand', they are more reckless with less health
            // They will have a higher propensity to use high damage, high crit, low accuracy moves
            // This is so that they can somehow ensure a victory despite poor circumstances
            int flavor_laststand = 0;
            int flavor_luck = 0;

            Attacks attack = Stone;

            if (Enemy.Health == 1000)
            {
                flavor_laststand = 20;
            }
            else if (Enemy.Health >= 800 && Enemy.Health < 1000)
            {
                flavor_laststand = 0;
            }
            else if (Enemy.Health >= 600 && Enemy.Health < 800)
            {
                flavor_laststand = 0;
            }
            else if (Enemy.Health >= 400 && Enemy.Health < 600)
            {
                flavor_laststand = 20;
            }
            else if (Enemy.Health >= 200 && Enemy.Health < 400)
            {
                flavor_laststand = 40;
            }
            else // If AI health is less than 200...
            {
                flavor_laststand = 98;
            }

            flavor_luck = AI_ConsiderLuck();
            attack = AI_ConsiderPlayerHealth(flavor_laststand, flavor_luck);

            Enemy.ChosenAttack = attack;
        }

        private int AI_ConsiderLuck()
        {
            int luck = Enemy.Luck;
            int flavor_luck = 0;
            switch (luck)
            {
                case 0:
                    flavor_luck = 0;
                    break;
                case 1:
                    flavor_luck = 10;
                    break;
                case 2:
                    flavor_luck = 20;
                    break;
                case 3:
                    flavor_luck = 45;
                    break;
                case 4:
                    flavor_luck = 70;
                    break;
                case 5:
                    flavor_luck = 80;
                    break;
                default:
                    break;
            }
            return flavor_luck;
        }

        private Attacks AI_ConsiderPlayerHealth(int flavor_laststand, int flavor_luck)
        {
            Attacks attack = Stone;
            if (Player.Health < 200)
            {
                // This value ignores 'laststand' as it is still possible to win
                attack = AI_CheckForAttackAccuracy(85);
            }
            else
            {
                int flavor_health = 0;
                if (Player.Health >= 200 && Player.Health < 400)
                {
                    flavor_health = 90;
                }
                else if (Player.Health >= 400 && Player.Health < 600)
                {
                    flavor_health = 50;
                }
                else if (Player.Health >= 600 && Player.Health < 800)
                {
                    flavor_health = 15;
                }
                else // If the player has between 800 to 1000 health
                {
                    flavor_health = 5;
                }

                attack = AI_WeighAllFlavors(flavor_laststand, flavor_luck, flavor_health);
            }

            return attack;
        }

        private Attacks AI_WeighAllFlavors(int flavor_laststand, int flavor_luck, int flavor_health)
        {
            Attacks attack = Stone;

            if (flavor_health >= RandNum(Gamestate.RandObj, 100))
            {
                attack = AI_CheckForAttackAccuracy(75);
            }
            else
            {
                if (flavor_laststand > flavor_health)
                {
                    attack = AI_CheckForHighestDamage(flavor_luck);
                }
                else
                {
                    if (flavor_laststand >= RandNum(Gamestate.RandObj, 100))
                    {
                        attack = AI_CheckForHighestDamage(flavor_laststand);
                    }
                    else
                    {
                        attack = AI_CheckForHighestDamage(0);
                    }
                }
            }

            return attack;
        }

        private Attacks AI_CheckForHighestDamage(int flavor_luck)
        {
            Attacks attack = Stone;
            int weapon_dmg = AI_PotentialWeaponDamage(Enemy.Weapon);
            int attack1_dmg = AI_PotentialWeaponDamage(Enemy.iWeapon);
            int attack2_dmg = AI_PotentialWeaponDamage(Enemy.iiWeapon);

            // If luck is valued more, then search for the weapon with the highest critical chance
            if (flavor_luck >= RandNum(Gamestate.RandObj, 100))
            {
                if (Enemy.Weapon.Critical > Enemy.iWeapon.Critical)
                {
                    if (Enemy.Weapon.Critical > Enemy.iiWeapon.Critical)
                    {
                        attack = Enemy.Weapon;
                    }
                    else
                    {
                        attack = Enemy.iiWeapon;
                    }
                }
                else
                {
                    if (Enemy.iWeapon.Critical > Enemy.iiWeapon.Critical)
                    {
                        attack = Enemy.iWeapon;
                    }
                    else
                    {
                        attack = Enemy.iiWeapon;
                    }
                }
            }
            else
            {
                attack = Enemy.Weapon;
                if (attack1_dmg > weapon_dmg)
                {
                    attack = Enemy.iWeapon;
                }
                if (attack2_dmg > attack.Damage)
                {
                    attack = Enemy.iiWeapon;
                }
            }

            return attack;
        }

        private Attacks AI_CheckForAttackAccuracy(int desiredAcc)
        {
            int netPrecision = (int)(Math.Floor(Enemy.Precision * Mods.mPrecision * 100));
            int accuracy_Weapon = netPrecision + Enemy.ChosenAttack.Accuracy;
            int accuracy_Attack1 = netPrecision + Enemy.iWeapon.Accuracy;
            int accuracy_Attack2 = netPrecision + Enemy.iiWeapon.Accuracy;

            bool weaponFound = false;
            Attacks attack = Stone;

            if (accuracy_Weapon >= desiredAcc)
            {
                if (weaponFound == true)
                {
                    attack = AI_CompareTwoWeapons(attack, Enemy.Weapon);
                }
                else
                {
                    weaponFound = true;
                    attack = Enemy.Weapon;
                }
            }
            if (accuracy_Attack1 >= desiredAcc)
            {
                if (weaponFound == true)
                {
                    attack = AI_CompareTwoWeapons(attack, Enemy.iWeapon);
                }
                else
                {
                    weaponFound = true;
                    attack = Enemy.iWeapon;
                }
            }
            if (accuracy_Attack2 >= desiredAcc)
            {
                if (weaponFound == true)
                {
                    attack = AI_CompareTwoWeapons(attack, Enemy.iiWeapon);
                }
                else
                {
                    weaponFound = true;
                    attack = Enemy.iiWeapon;
                }
            }

            return attack;
        }

        private Attacks AI_CompareTwoWeapons(Attacks attack1, Attacks attack2)
        {
            // Retrieves the raw damage even against the player's defense value
            int attack1_dmg = AI_PotentialWeaponDamage(attack1);
            int attack2_dmg = AI_PotentialWeaponDamage(attack2);

            // The previous attack entered is given priority over the new weapon that also meets the criteria
            Attacks desiredAtk = attack1;

            // Check to see if the second attack will have the ability to knock the opponent out
            if (attack2_dmg >= Player.Health)
            {
                // If the damage is higher than the health, attack2 is valued more
                desiredAtk = attack2;
            }
            else
            {
                // If the damage isn't higher vs health, then raw damage will be the next valued attribute
                if (attack1_dmg > attack2_dmg)
                {
                    desiredAtk = attack1;
                }
                else if (attack2_dmg > attack1_dmg)
                {
                    desiredAtk = attack2;
                }
                else
                {
                    // If somehow the damages are equal, whichever has the highest critical hit chance is more important
                    if (attack1.Critical > attack2.Critical)
                    {
                        desiredAtk = attack1;
                    }
                    else if (attack2.Critical > attack1.Critical)
                    {
                        desiredAtk = attack2;
                    }
                    else
                    {
                        // The first attack entered is always given priority
                        desiredAtk = attack1;
                    }
                }
            }

            return desiredAtk;
        }

        private int AI_PotentialWeaponDamage(Attacks weapon)
        {
            int damage = DamageBeforeDefense(Enemy);
            bool canDefend = IsDefenderDefensive(Enemy, Player);
            if (canDefend == true)
            {
                damage = ReducedDamageFromDefense(damage);
            }
            return damage;
        }

        #endregion

        // ---------------------------------

        #region Game Initiation

        public void BeginGame(Gamevm viewModel)
        {
            // Initializes the Gamestate and Gameview objects
            Game gameView = new Game(viewModel);
            Gamestate = new Gamestate();
            Mods = new Modifiers();
            Gamestate.Turns = 1;

            // Initializes enemy types and creates one
            InitEnemyTypes();
            CreateEnemy();

            // Checks if the enemy moves prior to the user
            // If so, their move will occur before the window opens
            bool enemyMoveFirst = CheckSpeedTie();
            if (enemyMoveFirst == true)
            {
                AI_AttackProcedure();
            }
            else
            {
                Gamestate.CanPlayerAttack = true;
                Gamestate.WhoseMove = Player;
            }

            // Displays the Gameview window
            gameView.Show();
        }

        private void InitEnemyTypes()
        {
            Peasant = new Character("Peasant", 1000, 0, 0, 1, 2, 2, Kukri, Stone, Slash);
            Knight1 = new Character("Knight", 1000, 2, 1, 1, 3, 0, Axe, Stone, Slash);
            Pikeman = new Character("Pikeman", 1000, 2, 3, 3, 1, 2, Zwei, Stone, Slash);
            Knight2 = new Character("Knight", 1000, 1, 2, 1, 4, 0, Sword, Stone, Slash);
            Spearman = new Character("Spearman", 1000, 3, 1, 4, 0, 3, Spear, Stone, Slash);
        }

        private void CreateEnemy()
        {
            // Refreshes the enemy character
            Enemy = new Character("Enemy", 1000, 0, 0, 0, 0, 0, Stone, Stone, Stone);

            // Sets the new enemy to one of the enemy template types
            ApplyEnemyType();

            // Determines the Gamestate difficulty to see the difficulty level buffs for enemies
            SetDifficulty();

            // Randomly applies new skill points to the current enemy's skills based on the difficulty
            UpdateEnemyStats();
        }

        private void ApplyEnemyType()
        {
            // The array of characters must be updated when new characters are added to the game
            Character[] enemies = new Character[] { Peasant, Knight1, Knight2, Pikeman, Spearman };

            int enemies_index = RandNum(Gamestate.RandObj, 5);

            // Updates the enemy to the type selected
            Enemy = enemies[enemies_index];
        }

        private int RandNum(Random random, int value)
        {
            int randNew = random.Next(0, value);
            return randNew;
        }

        private void SetDifficulty()
        {
            // Every "x" number of turns, the difficulty will be increased
            // For instance, if TurnDiffChanged was equal to 7, the difficulty would be increased every 7 turns
            // i.e Difficulty is 1 @ 7 turns, difficulty 2 @ 14 turns, etc.

            double vBase = Gamestate.Turns / Gamestate.TurnDiffChangedOn;
            double vFloor = Math.Floor(vBase);
            int vInt = (int)vFloor;
            Gamestate.Difficulty = vInt;
        }

        private void UpdateEnemyStats()
        {
            Enemy.Strength += RandNum(Gamestate.RandObj, Gamestate.Difficulty);
            Enemy.Defense += RandNum(Gamestate.RandObj, Gamestate.Difficulty);
            Enemy.Agility += RandNum(Gamestate.RandObj, Gamestate.Difficulty);
            Enemy.Precision += RandNum(Gamestate.RandObj, Gamestate.Difficulty);
            Enemy.Luck += RandNum(Gamestate.RandObj, Gamestate.Difficulty);
        }

        #endregion

        // ---------------------------------

        #region Battle Methods

        public void Player_DetMove(string name)
        {
            switch (name)
            {
                case "StdMov1":
                    Player.ChosenAttack = Slash;
                    break;
                case "StdMov2":
                    Player.ChosenAttack = Stone;
                    break;
                case "Combat Axe":
                    Player.ChosenAttack = Axe;
                    break;
                case "Spear":
                    Player.ChosenAttack = Spear;
                    break;
                case "Kukri":
                    Player.ChosenAttack = Kukri;
                    break;
                case "Broadsword":
                    Player.ChosenAttack = Sword;
                    break;
                case "Zweihänder":
                    Player.ChosenAttack = Zwei;
                    break;
                default:
                    break;
            }
            Player_AttackProcedure();
        }

        private void Player_AttackProcedure()
        {
            Gamestate.GameMessage += "\n";
            Enemy = ProcessAttack(Player, Enemy);
            Gamestate.GameMessage += "\n";
            EnemySurvival();
        }

        private void EnemySurvival()
        {
            if (Enemy.Health <= 0)
            {
                Player.Health = 1000;
                Gamestate.Turns += 1;
                Gamestate.GameMessage = "";
                CreateEnemy();
                Enemy.Health = 1000;
                bool enemyMoveFirst = CheckSpeedTie();
                if (enemyMoveFirst == true)
                {
                    AI_AttackProcedure();
                }
            }
            else
            {
                Gamestate.CanPlayerAttack = false;
                Gamestate.WhoseMove = Enemy;
                AI_AttackProcedure();
            }
        }

        private Character ProcessAttack(Character attacker, Character defender)
        {
            // Tests the attacker's Precision and weapon accuracy to see if a hit lands
            bool attackLands = WillAttackLand(attacker);

            if (attackLands == true)
            {
                // Obtain the damage prior to Luck and Defense being factored in
                int damage = DamageBeforeDefense(attacker);

                // Tests to see if a critical hit has landed
                bool critLanded = DoesCritLand(attacker);

                // If a critical hit has landed, more damage is added in
                if (critLanded == true)
                {
                    damage = DamageFromCrit(attacker, damage);
                }

                // See if the attacker's weapon penetration is better than the defender's armor
                bool penetrationNoPass = IsDefenderDefensive(attacker, defender);

                // If the defender's armor is too strong, the damage is reduced
                if (penetrationNoPass == true)
                {
                    damage = ReducedDamageFromDefense(damage);
                }

                // The defender now finally loses health from the damage of the attacker
                defender = ApplyDamageToDefender(attacker, defender, damage);
            }
            else
            {
                Msg_MoveMissed(attacker);
            }

            // Updates the defender to the calling method, specifically their health
            return defender;
        }

        private Character ApplyDamageToDefender(Character attacker, Character defender, int damage)
        {
            defender.Health -= damage;
            Msg_DamageDone(attacker, defender, damage);
            return defender;
        }

        private int ReducedDamageFromDefense(int damage)
        {
            int subtractor = (int)(Math.Floor(damage * Mods.mDefense));
            damage -= subtractor;
            return damage;
        }

        private bool IsDefenderDefensive(Character attacker, Character defender)
        {
            bool defensive = false;
            int penetration = attacker.ChosenAttack.Penetration;
            int defense = defender.Defense;

            if (penetration > defense)
            {
                defensive = false;
            }
            else
            {
                defensive = true;
            }

            return defensive;
        }

        private int DamageBeforeDefense(Character attacker)
        {
            double netStrengthMod = attacker.Strength * Mods.mStrength;
            int bonusFromStrength = (int)(Math.Floor(netStrengthMod * attacker.ChosenAttack.Damage));
            int fullStrength = attacker.ChosenAttack.Damage + bonusFromStrength;
            return fullStrength;
        }

        private bool DoesCritLand(Character attacker)
        {
            bool critLanded = false;
            int netLuck = (int)(Math.Floor(attacker.Luck * Mods.mLuck * 100));
            int vComp = RandNum(Gamestate.RandObj, 100);
            if (netLuck >= vComp)
            {
                critLanded = true;
                Msg_CritLanded(attacker);
            }
            else
            {
                critLanded = false;
            }
            return critLanded;
        }

        private int DamageFromCrit(Character attacker, int damage)
        {
            int totalDamage;

            int critDmg = (int)(Math.Floor(damage * Mods.mCrit));

            totalDamage = critDmg + damage;

            return totalDamage;
        }

        private bool WillAttackLand(Character attacker)
        {
            bool attackLands = true;
            int netPrecision = (int)(Math.Floor(attacker.Precision * Mods.mPrecision * 100));
            int fullAccuracy = netPrecision + attacker.ChosenAttack.Accuracy;

            if (fullAccuracy >= 100)
            {
                attackLands = true;
            }
            else 
            {
                int vComp = RandNum(Gamestate.RandObj, 100);
                if (fullAccuracy >= vComp)
                {
                    attackLands = true;
                }
                else
                {
                    attackLands = false;
                }
            }

            return attackLands;
        }

        private void PlayerSurvival()
        {
            if (Player.Health <= 0)
            {
                Gamestate.CanPlayerAttack = false;
                Gamestate.GameEnded = true;
                Gamestate.GameMessage += "\n\n";
                Gamestate.GameMessage += "You lost!";
            }
            else
            {
                Gamestate.CanPlayerAttack = true;
                Gamestate.WhoseMove = Player;
            }
        }

        private bool CheckSpeedTie()
        {
            bool enemyMoveFirst = false;
            if (Enemy.Agility >= Player.Agility)
            {
                enemyMoveFirst = true;
            } 
            else
            {
                enemyMoveFirst = false;
            }
            return enemyMoveFirst;
        }

        #endregion

        // ---------------------------------

        #region Game Messages

        private void Msg_MoveMissed(Character attacker)
        {
            Gamestate.GameMessage += $"\nThe {attacker.Name}'s {attacker.ChosenAttack.Move} missed!";
        }

        private void Msg_CritLanded(Character attacker)
        {
            Gamestate.GameMessage += $"\n{attacker.Name} landed a critical hit!";
        }

        private void Msg_DamageDone(Character attacker, Character defender, int damage)
        {
            Gamestate.GameMessage += $"\n{attacker.Name} used {attacker.ChosenAttack.Move}!";
            Gamestate.GameMessage += $"\n{defender.Name} lost {damage} health.";
        }

        #endregion

        // ---------------------------------

    }
}
