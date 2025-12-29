using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Data
{
    /// <summary>
    /// Sophisticated attribute-to-stat mapping system that replaces class-based stat calculations.
    /// Primary attributes (Fire/Water/Electric/Ground/Good/Evil/Yin/Yang) determine base stat contributions.
    /// Secondary attributes (Health/Mana/Attack/Magic/Agility/Defence) provide direct stat bonuses.
    /// </summary>
    public static class AttributeStatCalculator
    {
        // Primary Attribute Contributions to Stats
        // These determine the "class-like" role based on attribute distribution
        
        /// <summary>
        /// Calculates HP contribution from attributes
        /// Primary: Ground (defensive), Good/Evil (alignment), Yin/Yang (balance)
        /// Secondary: Health (direct bonus)
        /// </summary>
        public static int CalculateHP(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            // Base HP for all characters
            int baseHP = 14;
            
            // Primary attribute contributions
            float groundContribution = GetAttributeValue(attributes, Attribute.Ground) * 0.8f; // Ground = defensive, more HP
            float goodEvilContribution = (GetAttributeValue(attributes, Attribute.Good) + GetAttributeValue(attributes, Attribute.Evil)) * 0.3f;
            float yinYangContribution = (GetAttributeValue(attributes, Attribute.Yin) + GetAttributeValue(attributes, Attribute.Yang)) * 0.2f;
            
            // Level scaling - Ground-based characters scale better with level
            float levelMultiplier = 1.0f + (GetAttributeValue(attributes, Attribute.Ground) * 0.05f);
            float levelGain = (level / 4.0f + 4.5f) * level * levelMultiplier;
            
            // Secondary attribute direct bonus
            float healthBonus = GetAttributeValue(attributes, Attribute.Health) * 2.5f;
            
            int totalHP = (int)(baseHP + groundContribution + goodEvilContribution + yinYangContribution + levelGain + healthBonus);
            return Math.Max(1, totalHP);
        }
        
        /// <summary>
        /// Calculates MP contribution from attributes
        /// Primary: Fire (magical), Water (flow), Electric (energy), Good/Evil (spiritual)
        /// Secondary: Mana (direct bonus)
        /// </summary>
        public static int CalculateMP(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            int baseMP = 11;
            
            // Primary attribute contributions - magical attributes boost MP
            float fireContribution = GetAttributeValue(attributes, Attribute.Fire) * 0.6f;
            float waterContribution = GetAttributeValue(attributes, Attribute.Water) * 0.5f;
            float electricContribution = GetAttributeValue(attributes, Attribute.Electric) * 0.4f;
            float goodEvilContribution = (GetAttributeValue(attributes, Attribute.Good) + GetAttributeValue(attributes, Attribute.Evil)) * 0.4f;
            
            // Level scaling - Fire-based characters scale better
            float levelMultiplier = 1.0f + (GetAttributeValue(attributes, Attribute.Fire) * 0.08f);
            float levelGain = level * 3.5f * levelMultiplier;
            
            // Special scaling for high Fire (Wizard-like)
            if (GetAttributeValue(attributes, Attribute.Fire) > 50)
            {
                levelGain = ((level / 5.0f + 2.0f) * 2.2f * level) + (level * 0.5f);
            }
            
            // Secondary attribute direct bonus
            float manaBonus = GetAttributeValue(attributes, Attribute.Mana) * 3.0f;
            
            int totalMP = (int)(baseMP + fireContribution + waterContribution + electricContribution + goodEvilContribution + levelGain + manaBonus);
            return Math.Max(1, totalMP);
        }
        
        /// <summary>
        /// Calculates AC (Armor Class) contribution from attributes
        /// Primary: Ground (defensive), Yang (strength)
        /// Secondary: Defence (direct bonus)
        /// </summary>
        public static int CalculateMinAC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            return 0; // Min AC typically starts at 0
        }
        
        public static int CalculateMaxAC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            // Primary attribute contributions
            float groundContribution = GetAttributeValue(attributes, Attribute.Ground) * 0.12f;
            float yangContribution = GetAttributeValue(attributes, Attribute.Yang) * 0.08f;
            
            // Level scaling
            float levelGain = level * 7.0f * (1.0f + GetAttributeValue(attributes, Attribute.Ground) * 0.01f);
            
            // Secondary attribute direct bonus
            float defenceBonus = GetAttributeValue(attributes, Attribute.Defence) * 0.15f;
            
            int totalAC = (int)(groundContribution + yangContribution + levelGain + defenceBonus);
            return Math.Max(0, totalAC);
        }
        
        /// <summary>
        /// Calculates MAC (Magic Armor Class) contribution from attributes
        /// Primary: Water (flow), Yin (balance), Good/Evil (spiritual defense)
        /// Secondary: Defence (direct bonus)
        /// </summary>
        public static int CalculateMinMAC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            // Primary attribute contributions
            float waterContribution = GetAttributeValue(attributes, Attribute.Water) * 0.15f;
            float yinContribution = GetAttributeValue(attributes, Attribute.Yin) * 0.10f;
            
            // Level scaling
            float levelGain = level * 12.0f * (1.0f + GetAttributeValue(attributes, Attribute.Water) * 0.01f);
            
            // Secondary attribute direct bonus
            float defenceBonus = GetAttributeValue(attributes, Attribute.Defence) * 0.12f;
            
            int totalMAC = (int)(waterContribution + yinContribution + levelGain + defenceBonus);
            return Math.Max(0, totalMAC);
        }
        
        public static int CalculateMaxMAC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            // Primary attribute contributions
            float waterContribution = GetAttributeValue(attributes, Attribute.Water) * 0.18f;
            float yinContribution = GetAttributeValue(attributes, Attribute.Yin) * 0.12f;
            float goodEvilContribution = (GetAttributeValue(attributes, Attribute.Good) + GetAttributeValue(attributes, Attribute.Evil)) * 0.10f;
            
            // Level scaling
            float levelGain = level * 6.0f * (1.0f + GetAttributeValue(attributes, Attribute.Water) * 0.01f);
            
            // Secondary attribute direct bonus
            float defenceBonus = GetAttributeValue(attributes, Attribute.Defence) * 0.15f;
            
            int totalMAC = (int)(waterContribution + yinContribution + goodEvilContribution + levelGain + defenceBonus);
            return Math.Max(0, totalMAC);
        }
        
        /// <summary>
        /// Calculates DC (Destructive Power/Physical Attack) contribution from attributes
        /// Primary: Ground (physical strength), Yang (force), Electric (power)
        /// Secondary: Attack (direct bonus)
        /// </summary>
        public static int CalculateMinDC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            return 0; // Min DC typically starts at 0
        }
        
        public static int CalculateMaxDC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            // Primary attribute contributions
            float groundContribution = GetAttributeValue(attributes, Attribute.Ground) * 0.10f;
            float yangContribution = GetAttributeValue(attributes, Attribute.Yang) * 0.08f;
            float electricContribution = GetAttributeValue(attributes, Attribute.Electric) * 0.06f;
            
            // Level scaling
            float levelGain = level * 5.0f * (1.0f + GetAttributeValue(attributes, Attribute.Ground) * 0.01f);
            
            // Secondary attribute direct bonus
            float attackBonus = GetAttributeValue(attributes, Attribute.Attack) * 0.20f;
            
            int totalDC = (int)(groundContribution + yangContribution + electricContribution + levelGain + attackBonus);
            return Math.Max(0, totalDC);
        }
        
        /// <summary>
        /// Calculates MC (Magic Attack) contribution from attributes
        /// Primary: Fire (magical power), Water (flow), Electric (energy)
        /// Secondary: Magic (direct bonus)
        /// </summary>
        public static int CalculateMinMC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            return 0; // Min MC typically starts at 0
        }
        
        public static int CalculateMaxMC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            // Primary attribute contributions
            float fireContribution = GetAttributeValue(attributes, Attribute.Fire) * 0.12f;
            float waterContribution = GetAttributeValue(attributes, Attribute.Water) * 0.08f;
            float electricContribution = GetAttributeValue(attributes, Attribute.Electric) * 0.10f;
            
            // Level scaling
            float levelGain = level * 7.0f * (1.0f + GetAttributeValue(attributes, Attribute.Fire) * 0.01f);
            
            // Secondary attribute direct bonus
            float magicBonus = GetAttributeValue(attributes, Attribute.Magic) * 0.25f;
            
            int totalMC = (int)(fireContribution + waterContribution + electricContribution + levelGain + magicBonus);
            return Math.Max(0, totalMC);
        }
        
        /// <summary>
        /// Calculates SC (Spirit/Spell Attack) contribution from attributes
        /// Primary: Good/Evil (spiritual), Yin/Yang (balance), Water (flow)
        /// Secondary: Magic (direct bonus)
        /// </summary>
        public static int CalculateMinSC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            return 0; // Min SC typically starts at 0
        }
        
        public static int CalculateMaxSC(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            // Primary attribute contributions
            float goodEvilContribution = (GetAttributeValue(attributes, Attribute.Good) + GetAttributeValue(attributes, Attribute.Evil)) * 0.10f;
            float yinYangContribution = (GetAttributeValue(attributes, Attribute.Yin) + GetAttributeValue(attributes, Attribute.Yang)) * 0.08f;
            float waterContribution = GetAttributeValue(attributes, Attribute.Water) * 0.06f;
            
            // Level scaling
            float levelGain = level * 7.0f * (1.0f + (GetAttributeValue(attributes, Attribute.Good) + GetAttributeValue(attributes, Attribute.Evil)) * 0.005f);
            
            // Secondary attribute direct bonus
            float magicBonus = GetAttributeValue(attributes, Attribute.Magic) * 0.22f;
            
            int totalSC = (int)(goodEvilContribution + yinYangContribution + waterContribution + levelGain + magicBonus);
            return Math.Max(0, totalSC);
        }
        
        /// <summary>
        /// Calculates Accuracy contribution from attributes
        /// Primary: Electric (precision), Yang (focus)
        /// Secondary: Agility (direct bonus)
        /// </summary>
        public static int CalculateAccuracy(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            int baseAccuracy = 5;
            
            // Primary attribute contributions
            float electricContribution = GetAttributeValue(attributes, Attribute.Electric) * 0.05f;
            float yangContribution = GetAttributeValue(attributes, Attribute.Yang) * 0.03f;
            
            // Secondary attribute direct bonus
            float agilityBonus = GetAttributeValue(attributes, Attribute.Agility) * 0.10f;
            
            int totalAccuracy = (int)(baseAccuracy + electricContribution + yangContribution + agilityBonus);
            return Math.Max(0, totalAccuracy);
        }
        
        /// <summary>
        /// Calculates Agility contribution from attributes
        /// Primary: Electric (speed), Yin (balance), Water (flow)
        /// Secondary: Agility (direct bonus)
        /// </summary>
        public static int CalculateAgility(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            int baseAgility = 15;
            
            // Primary attribute contributions
            float electricContribution = GetAttributeValue(attributes, Attribute.Electric) * 0.08f;
            float yinContribution = GetAttributeValue(attributes, Attribute.Yin) * 0.05f;
            float waterContribution = GetAttributeValue(attributes, Attribute.Water) * 0.04f;
            
            // Secondary attribute direct bonus
            float agilityBonus = GetAttributeValue(attributes, Attribute.Agility) * 0.15f;
            
            int totalAgility = (int)(baseAgility + electricContribution + yinContribution + waterContribution + agilityBonus);
            return Math.Max(0, totalAgility);
        }
        
        /// <summary>
        /// Calculates Bag Weight capacity from attributes
        /// Primary: Ground (strength), Yang (power)
        /// </summary>
        public static int CalculateBagWeight(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            int baseWeight = 50;
            
            // Primary attribute contributions
            float groundContribution = GetAttributeValue(attributes, Attribute.Ground) * 0.5f;
            float yangContribution = GetAttributeValue(attributes, Attribute.Yang) * 0.3f;
            
            // Level scaling
            float levelGain = ((level / 3.0f) * level);
            
            int totalWeight = (int)(baseWeight + groundContribution + yangContribution + levelGain);
            return Math.Max(0, totalWeight);
        }
        
        /// <summary>
        /// Calculates Wear Weight capacity from attributes
        /// Primary: Ground (strength), Yang (power)
        /// </summary>
        public static int CalculateWearWeight(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            int baseWeight = 15;
            
            // Primary attribute contributions
            float groundContribution = GetAttributeValue(attributes, Attribute.Ground) * 0.4f;
            float yangContribution = GetAttributeValue(attributes, Attribute.Yang) * 0.25f;
            
            // Level scaling - varies based on primary attribute
            float levelMultiplier = 1.0f;
            if (GetAttributeValue(attributes, Attribute.Ground) > 30)
                levelMultiplier = 20.0f; // Warrior-like
            else if (GetAttributeValue(attributes, Attribute.Fire) > 30)
                levelMultiplier = 100.0f; // Wizard-like (low weight capacity)
            else if (GetAttributeValue(attributes, Attribute.Good) + GetAttributeValue(attributes, Attribute.Evil) > 30)
                levelMultiplier = 50.0f; // Taoist-like
            
            float levelGain = ((level / levelMultiplier) * level);
            
            int totalWeight = (int)(baseWeight + groundContribution + yangContribution + levelGain);
            return Math.Max(0, totalWeight);
        }
        
        /// <summary>
        /// Calculates Hand Weight capacity from attributes
        /// Primary: Ground (strength), Yang (power)
        /// </summary>
        public static int CalculateHandWeight(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            int baseWeight = 12;
            
            // Primary attribute contributions
            float groundContribution = GetAttributeValue(attributes, Attribute.Ground) * 0.35f;
            float yangContribution = GetAttributeValue(attributes, Attribute.Yang) * 0.20f;
            
            // Level scaling - varies based on primary attribute
            float levelMultiplier = 1.0f;
            if (GetAttributeValue(attributes, Attribute.Ground) > 30)
                levelMultiplier = 13.0f; // Warrior-like
            else if (GetAttributeValue(attributes, Attribute.Fire) > 30)
                levelMultiplier = 90.0f; // Wizard-like (low weight capacity)
            else if (GetAttributeValue(attributes, Attribute.Good) + GetAttributeValue(attributes, Attribute.Evil) > 30)
                levelMultiplier = 42.0f; // Taoist-like
            
            float levelGain = ((level / levelMultiplier) * level);
            
            int totalWeight = (int)(baseWeight + groundContribution + yangContribution + levelGain);
            return Math.Max(0, totalWeight);
        }
        
        /// <summary>
        /// Gets the effective value of an attribute (Level + Points)
        /// This represents the total investment in an attribute
        /// </summary>
        private static float GetAttributeValue(Dictionary<Attribute, UserAttribute> attributes, Attribute attribute)
        {
            if (attributes == null || !attributes.ContainsKey(attribute))
                return 0;
            
            UserAttribute attr = attributes[attribute];
            // Level provides base scaling, Points provide direct bonuses
            // Higher level = more effective per point
            float levelMultiplier = 1.0f + (attr.Level * 0.1f);
            return attr.Points * levelMultiplier + (attr.Level * 2.0f);
        }
        
        /// <summary>
        /// Calculates all base stats from attributes
        /// </summary>
        public static Stats CalculateAllStats(Dictionary<Attribute, UserAttribute> attributes, int level)
        {
            Stats stats = new Stats();
            
            stats[Stat.HP] = CalculateHP(attributes, level);
            stats[Stat.MP] = CalculateMP(attributes, level);
            stats[Stat.MinAC] = CalculateMinAC(attributes, level);
            stats[Stat.MaxAC] = CalculateMaxAC(attributes, level);
            stats[Stat.MinMAC] = CalculateMinMAC(attributes, level);
            stats[Stat.MaxMAC] = CalculateMaxMAC(attributes, level);
            stats[Stat.MinDC] = CalculateMinDC(attributes, level);
            stats[Stat.MaxDC] = CalculateMaxDC(attributes, level);
            stats[Stat.MinMC] = CalculateMinMC(attributes, level);
            stats[Stat.MaxMC] = CalculateMaxMC(attributes, level);
            stats[Stat.MinSC] = CalculateMinSC(attributes, level);
            stats[Stat.MaxSC] = CalculateMaxSC(attributes, level);
            stats[Stat.Accuracy] = CalculateAccuracy(attributes, level);
            stats[Stat.Agility] = CalculateAgility(attributes, level);
            stats[Stat.BagWeight] = CalculateBagWeight(attributes, level);
            stats[Stat.WearWeight] = CalculateWearWeight(attributes, level);
            stats[Stat.HandWeight] = CalculateHandWeight(attributes, level);
            
            return stats;
        }
        
        /// <summary>
        /// Determines a "class-like" role based on attribute distribution
        /// Used for display purposes and backward compatibility
        /// </summary>
        public static MirClass DetermineClassRole(Dictionary<Attribute, UserAttribute> attributes)
        {
            if (attributes == null) return MirClass.Warrior;
            
            float groundValue = GetAttributeValue(attributes, Attribute.Ground);
            float fireValue = GetAttributeValue(attributes, Attribute.Fire);
            float goodEvilValue = GetAttributeValue(attributes, Attribute.Good) + GetAttributeValue(attributes, Attribute.Evil);
            
            // Determine primary role
            if (groundValue > fireValue && groundValue > goodEvilValue)
                return MirClass.Warrior; // Ground-based = Warrior-like
            else if (fireValue > goodEvilValue)
                return MirClass.Wizard; // Fire-based = Wizard-like
            else
                return MirClass.Taoist; // Good/Evil-based = Taoist-like
        }
    }
}

