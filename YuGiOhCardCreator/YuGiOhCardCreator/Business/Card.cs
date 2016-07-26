using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace YuGiOhCardCreator.Business
{
    public class Card
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Id { get; set; }
        public CardType CardType { get; set; }
        public SpecificCardType SpecificCardType { get; set; }
    }

    public class CardType
    {
        private string _cardtype;
        public static readonly CardType Spell = new CardType() {_cardtype = "Spell"};
        public static readonly CardType Monster = new CardType() { _cardtype = "Monster" };
        public static readonly CardType Trap = new CardType() { _cardtype = "Trap" };
        private CardType(){}

        public override string ToString()
        {
            return _cardtype;
        }
    }

    public class SpecificCardType
    {
        public CardType Parent { get; private set; }
        private string _specificCardType;
        private string _layoutName;
        public List<SpecificCardType> Subtypes { get; private set; }


        public string GetLayoutName()
        {
            return _layoutName;
        }
        #region Monsters
        public static readonly SpecificCardType NormalMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Normal",
            _layoutName = "card-normal.png"
        };

        public static readonly SpecificCardType EffectMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Effect",
            _layoutName = "card-effect.png"
        };

        public static readonly SpecificCardType FusionMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Fusion",
            _layoutName = "card-fusion.png"
        };

        public static readonly SpecificCardType RitualMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Ritual",
            _layoutName = "card-ritual.png"
        };

        private static readonly SpecificCardType SynchroLMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Synchro",
            _layoutName = "card-synchro.png"
        };

        private static readonly SpecificCardType SynchroDarkMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Dark Synchro",
            _layoutName = "card-dsynchro.png",
        };

        public static readonly SpecificCardType SynchroMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Synchro",
            Subtypes = new List<SpecificCardType>()
            {
                {SynchroLMonster },
                {SynchroDarkMonster }
            }
        };

        public static readonly SpecificCardType XyzMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "XYZ",
            _layoutName = "card-xyz.png"
        };

        private static readonly SpecificCardType PendulumNormalMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Normal Pendulum",
            _layoutName = "card-pendulum-n.png"
        };

        private static readonly SpecificCardType PendulumEffectMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Effect Pendulum",
            _layoutName = "card-pendulum-e.png"
        };

        private static readonly SpecificCardType PendulumFusionMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Fusion Pendulum",
            _layoutName = "card-pendulum-f.png"
        };

        private static readonly SpecificCardType PendulumRitualMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Ritual Pendulum",
            _layoutName = "card-pendulum-r.png"
        };


        public static readonly SpecificCardType PendulumMonster = new SpecificCardType
        {
            Parent = CardType.Monster,
            _specificCardType = "Pendulum",
            Subtypes =
                new List<SpecificCardType>
                {
                    PendulumNormalMonster,
                    PendulumEffectMonster,
                    PendulumFusionMonster,
                    PendulumRitualMonster
                }

        };
        #endregion
        #region Spell
        public static readonly SpecificCardType NormalSpell = new SpecificCardType
        {
            Parent = CardType.Spell,
            _specificCardType = "Normal"
        };

        public static readonly SpecificCardType SpeedSpell = new SpecificCardType
        {
            Parent = CardType.Spell,
            _specificCardType = "Speed"
        };

        public static readonly SpecificCardType RitualSpell = new SpecificCardType
        {
            Parent = CardType.Spell,
            _specificCardType = "Ritual"
        };

        public static readonly SpecificCardType ContinuousSpell = new SpecificCardType
        {
            Parent = CardType.Spell,
            _specificCardType = "Continuous"
        };

        public static readonly SpecificCardType EquipmentSpell = new SpecificCardType
        {
            Parent = CardType.Spell,
            _specificCardType = "Equipement"
        };

        public static readonly SpecificCardType FieldSpell = new SpecificCardType()
        {
            Parent = CardType.Spell,
            _specificCardType = "Field"
        };
        #endregion
        #region Trap
        public static readonly SpecificCardType NormalTrap = new SpecificCardType
        {
            Parent = CardType.Trap,
            _specificCardType = "Normal"
        };

        public static readonly SpecificCardType ContinuousTrap = new SpecificCardType
        {
            Parent = CardType.Trap,
            _specificCardType = "Continuous"
        };

        public static readonly SpecificCardType CounterTrap = new SpecificCardType
        {
            Parent = CardType.Trap,
            _specificCardType = "Counter"
        };
#endregion

        private SpecificCardType()
        {
        }

        public override string ToString()
        {
            return _specificCardType;
        }
    }

    public class SpecificLayout
    {
        // TODO changer la methode de modif des row de la carte avec les noms des rows
        public List<RowDefinition> GridCardRows { get; set; }
        public Orientation LevelPanelOrientation { get; set; }
    }
}
