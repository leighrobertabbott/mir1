using Client.MirControls;
using Client.MirGraphics;
using Client.MirObjects;
using Client.MirSounds;
using S = ServerPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class InspectDialog : MirImageControl
    {
        public MirButton CloseButton;
        public MirLabel NameLabel, LevelLabel, ClassLabel;
        public MirLabel HPLabel, MPLabel, ACLabel, MACLabel, DCLabel, MCLabel, AccuracyLabel, AgilityLabel;

        public MirItemCell[] Grid;
        public static UserItem[] InspectEquipment = new UserItem[8]; // Static storage for inspected equipment
        private Stats InspectStats; // Store stats for the inspected player

        public InspectDialog()
        {
            Index = 137;
            Library = Libraries.Prguse;
            Location = new Point(Settings.ScreenWidth / 2 - Size.Width / 2, Settings.ScreenHeight / 2 - Size.Height / 2);
            Movable = true;
            Sort = true;

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(241, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            NameLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                NotControl = true,
                Location = new Point(120, 10),
                ForeColour = Color.White
            };

            LevelLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                NotControl = true,
                Location = new Point(120, 30),
                ForeColour = Color.White
            };

            ClassLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                NotControl = true,
                Location = new Point(120, 50),
                ForeColour = Color.White
            };

            // Stats labels - positioned below the character info
            HPLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(120, 75),
                Size = new Size(100, 14),
                ForeColour = Color.White
            };

            MPLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(120, 95),
                Size = new Size(100, 14),
                ForeColour = Color.White
            };

            ACLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(120, 115),
                Size = new Size(100, 14),
                ForeColour = Color.White
            };

            MACLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(120, 135),
                Size = new Size(100, 14),
                ForeColour = Color.White
            };

            DCLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(120, 155),
                Size = new Size(100, 14),
                ForeColour = Color.White
            };

            MCLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(120, 175),
                Size = new Size(100, 14),
                ForeColour = Color.White
            };

            AccuracyLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(120, 195),
                Size = new Size(100, 14),
                ForeColour = Color.White
            };

            AgilityLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(120, 215),
                Size = new Size(100, 14),
                ForeColour = Color.White
            };

            Grid = new MirItemCell[Enum.GetNames(typeof(EquipmentSlot)).Length];

            Grid[(int)EquipmentSlot.Weapon] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Weapon,
                GridType = MirGridType.Inspect,
                Parent = this,
                Location = new Point(293, 43),
                Size = new Size(60, 45)
            };

            Grid[(int)EquipmentSlot.Armour] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Armour,
                GridType = MirGridType.Inspect,
                Parent = this,
                Location = new Point(157, 69),
                Size = new Size(60, 45)
            };

            Grid[(int)EquipmentSlot.Helmet] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Helmet,
                GridType = MirGridType.Inspect,
                Parent = this,
                Location = new Point(157, 13),
                Size = new Size(60, 45)
            };

            Grid[(int)EquipmentSlot.Necklace] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Necklace,
                GridType = MirGridType.Inspect,
                Parent = this,
                Location = new Point(19, 43),
                Size = new Size(60, 45)
            };

            Grid[(int)EquipmentSlot.BraceletL] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.BraceletL,
                GridType = MirGridType.Inspect,
                Parent = this,
                Location = new Point(87, 13),
                Size = new Size(60, 45)
            };

            Grid[(int)EquipmentSlot.BraceletR] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.BraceletR,
                GridType = MirGridType.Inspect,
                Parent = this,
                Location = new Point(225, 13),
                Size = new Size(60, 45)
            };

            Grid[(int)EquipmentSlot.RingL] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.RingL,
                GridType = MirGridType.Inspect,
                Parent = this,
                Location = new Point(87, 69),
                Size = new Size(60, 45)
            };

            Grid[(int)EquipmentSlot.RingR] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.RingR,
                GridType = MirGridType.Inspect,
                Parent = this,
                Location = new Point(225, 69),
                Size = new Size(60, 45)
            };
        }

        public void ShowInspect(S.InspectPlayerResponse p)
        {
            NameLabel.Text = p.Name;
            LevelLabel.Text = $"Level: {p.Level}";
            ClassLabel.Text = $"Class: {p.Class}";

            // Store stats
            InspectStats = p.Stats ?? new Stats();

            // Update stat labels
            HPLabel.Text = $"HP: {InspectStats[Stat.HP]}";
            MPLabel.Text = $"MP: {InspectStats[Stat.MP]}";
            ACLabel.Text = $"AC: {InspectStats[Stat.MinAC]}-{InspectStats[Stat.MaxAC]}";
            MACLabel.Text = $"MAC: {InspectStats[Stat.MinMAC]}-{InspectStats[Stat.MaxMAC]}";
            DCLabel.Text = $"DC: {InspectStats[Stat.MinDC]}-{InspectStats[Stat.MaxDC]}";
            MCLabel.Text = $"MC: {InspectStats[Stat.MinMC]}-{InspectStats[Stat.MaxMC]}";
            AccuracyLabel.Text = $"Accuracy: {InspectStats[Stat.Accuracy]}";
            AgilityLabel.Text = $"Agility: {InspectStats[Stat.Agility]}";

            // Clear all equipment slots
            for (int i = 0; i < InspectEquipment.Length; i++)
            {
                InspectEquipment[i] = null;
            }

            // Set equipment items
            if (p.Equipment != null)
            {
                for (int i = 0; i < p.Equipment.Length && i < InspectEquipment.Length; i++)
                {
                    if (p.Equipment[i] != null)
                    {
                        // Get item info from the item list
                        p.Equipment[i].Info = GameScene.GetInfo(p.Equipment[i].ItemIndex);
                        if (p.Equipment[i].Info != null)
                        {
                            InspectEquipment[i] = p.Equipment[i];
                        }
                    }
                }
            }

            // Update grid cells - lock them to prevent interaction
            for (int i = 0; i < Grid.Length; i++)
            {
                Grid[i].Locked = true;
                Grid[i].Redraw();
            }

            Show();
        }

        public override void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public override void Hide()
        {
            base.Hide();
        }

        public MirItemCell GetCell(ulong id)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i].Item == null || Grid[i].Item.UniqueID != id) continue;
                return Grid[i];
            }
            return null;
        }
    }
}

