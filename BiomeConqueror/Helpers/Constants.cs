using System.Collections.Generic;

namespace BiomeConqueror.Helpers
{
    public class Constants
    {
        public static readonly string EIKTHYR_DEFEATED_MESSAGE_KEY = "$event_boss01_end";
        public static readonly string ELDER_DEFEATED_MESSAGE_KEY = "$event_boss02_end";
        public static readonly string BONEMASS_DEFEATED_MESSAGE_KEY = "$event_boss03_end";
        public static readonly string MODER_DEFEATED_MESSAGE_KEY = "$event_boss04_end";
        public static readonly string YAGLUTH_DEFEATED_MESSAGE_KEY = "$event_boss05_end";
        public static readonly string QUEEN_DEFEATED_MESSAGE_KEY = "$enemy_boss_queen_deathmessage";
        public static readonly string FADER_DEFEATED_MESSAGE_KEY = "$enemy_boss_fader_deathmessage";
        public static readonly string TROPHY_EIKTHYR = "TrophyEikthyr";
        public static readonly string TROPHY_ELDER = "TrophyTheElder";
        public static readonly string TROPHY_BONEMASS = "TrophyBonemass";
        public static readonly string TROPHY_MODER = "TrophyDragonQueen";
        public static readonly string TROPHY_YAGLUTH = "TrophyGoblinKing";
        public static readonly string TROPHY_QUEEN = "TrophySeekerQueen";
        public static readonly string TROPHY_FADER = "TrophyFader";
        public static readonly string EIKTHYR_DEFEATED_PLAYER_KEY = "EikthyrDefeated_BC";
        public static readonly string ELDER_DEFEATED_PLAYER_KEY = "ElderDefeated_BC";
        public static readonly string BONEMASS_DEFEATED_PLAYER_KEY = "BonemassDefeated_BC";
        public static readonly string MODER_DEFEATED_PLAYER_KEY = "ModerDefeated_BC";
        public static readonly string YAGLUTH_DEFEATED_PLAYER_KEY = "YagluthDefeated_BC";
        public static readonly string QUEEN_DEFEATED_PLAYER_KEY = "QueenDefeated_BC";
        public static readonly string FADER_DEFEATED_PLAYER_KEY = "FaderDefeated_BC";
        
        public static readonly List<string> benefitDefeatedMessageKeys = new List<string> {
            EIKTHYR_DEFEATED_MESSAGE_KEY,
            ELDER_DEFEATED_MESSAGE_KEY,
            BONEMASS_DEFEATED_MESSAGE_KEY,
            MODER_DEFEATED_MESSAGE_KEY,
            YAGLUTH_DEFEATED_MESSAGE_KEY,
            QUEEN_DEFEATED_MESSAGE_KEY,
            FADER_DEFEATED_MESSAGE_KEY
        };
        public static readonly List<string> benefitDefeatedPlayerOldKeys = new List<string> {
            "EikthyrDefeated",
            "ElderDefeated",
            "BonemassDefeated",
            "ModerDefeated",
            "YagluthDefeated",
            "QueenDefeated",
            "FaderDefeated"
        };
    }
}
