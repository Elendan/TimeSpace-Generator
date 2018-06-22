using System.Linq;
using TimeSpaceGenerator.Core.Handling;
using TimeSpaceGenerator.Enums;
using TimeSpaceGenerator.Helpers;
using TimeSpaceGenerator.Managers;
using TimeSpaceGenerator.Objects;
using TimeSpaceGenerator.Packets;

namespace TimeSpaceGenerator.Handlers
{
    internal class ScriptedInstancePacketHandler : IPacketHandler
    {
        public void RbrPacket(RbrPacket packet)
        {
            //Draw gifts
            ScriptManager.Instance.Script.Info.DrawGift.Add(new Item(packet.DrawGift1, packet.DrawGiftAmount1));
            ScriptManager.Instance.Script.Info.DrawGift.Add(new Item(packet.DrawGift2, packet.DrawGiftAmount2));
            ScriptManager.Instance.Script.Info.DrawGift.Add(new Item(packet.DrawGift3, packet.DrawGiftAmount3));
            ScriptManager.Instance.Script.Info.DrawGift.Add(new Item(packet.DrawGift4, packet.DrawGiftAmount4));
            ScriptManager.Instance.Script.Info.DrawGift.Add(new Item(packet.DrawGift5, packet.DrawGiftAmount5));

            //Special items
            ScriptManager.Instance.Script.Info.Special.Add(new Item(packet.SpecialItem1, packet.SpecialItemAmount1));
            ScriptManager.Instance.Script.Info.Special.Add(new Item(packet.SpecialItem2, packet.SpecialItemAmount2));

            //Bonus items
            ScriptManager.Instance.Script.Info.Bonus.Add(new Item(packet.BonusItem1, packet.BonusItemAmount1));
            ScriptManager.Instance.Script.Info.Bonus.Add(new Item(packet.BonusItem2, packet.BonusItemAmount2));
            ScriptManager.Instance.Script.Info.Bonus.Add(new Item(packet.BonusItem3, packet.BonusItemAmount3));

            //Title
            ScriptManager.Instance.Script.Info.Title = packet.TitleAndLabel;
            ScriptManager.Instance.FileName = $"Timespace - {packet.LevelMinimum}";

            // Level
            ScriptManager.Instance.Script.Info.LevelMinimum = packet.LevelMinimum;
            ScriptManager.Instance.Script.Info.LevelMax = packet.LevelMaximum;
        }

        public void WalkPacket(WalkPacket packet)
        {
            ScriptManager.Instance.PosX = packet.XCoordinate;
            ScriptManager.Instance.PosY = packet.YCoordinate;
            if (ScriptManager.Instance.Num3 == 1)
            {
                ScriptManager.Instance.Flag1 = false;
            }

            ++ScriptManager.Instance.Num3;
        }

        public void SuPacket(SuPacket packet)
        {
            if ((packet.EntityType == VisualType.Npc || packet.EntityType == VisualType.Character) && packet.Unknown7 == 0)
            {
                Monster monster3 = ScriptManager.Instance.MapMonsters.FirstOrDefault(s => s.MapMonsterId == packet.EntityId);
                if (monster3 != null)
                {
                    monster3.IsDead = true;
                    if (!monster3.IsTarget && ScriptManager.Instance.MapMonsters.All(s => s.IsDead))
                    {
                        ScriptManager.Instance.Monster1 = monster3;
                        ScriptManager.Instance.Target = ScriptManager.Instance.Map1;
                        ScriptManager.Instance.EventName = "OnMapClear";
                        return;
                    }

                    ScriptManager.Instance.Target = monster3;
                    ScriptManager.Instance.EventName = "OnDeath";
                }
            }
        }

        public void RsfnPacket(RsfnPacket packet)
        {
            ScriptManager.Instance.IndexX = packet.MapIndexX;
            ScriptManager.Instance.IndexY = packet.MapIndexY;
        }

        public void RsfmPacket(RsfmPacket packet)
        {
            /*
            ScriptManager.Instance.Data[0] = "rsfm 5 2 0 0";
            ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName, new Event(EventType.SendPacket, ScriptManager.Instance.Data));
            */
        }

        public void PreqPacket(PreqPacket packet)
        {
            ScriptManager.Instance.Portal1 = ScriptManager.Instance.Map1 != null
                ? ScriptManager.Instance.Map1.MapPortals.FirstOrDefault(s =>
                {
                    if (s.PosX != ScriptManager.Instance.PosX && s.PosX != (ScriptManager.Instance.PosX - 1) && s.PosX != (ScriptManager.Instance.PosX + 1))
                    {
                        return false;
                    }

                    if (s.PosY != ScriptManager.Instance.PosY && s.PosY != (ScriptManager.Instance.PosY - 1))
                    {
                        return s.PosY != (ScriptManager.Instance.PosY + 1);
                    }

                    return true;
                })
                : null;
            ScriptManager.Instance.Target = ScriptManager.Instance.Portal1;
        }

        public void OutPacket(OutPacket packet)
        {
            switch (packet.Type)
            {
                case VisualType.Monster:
                    if (!ScriptManager.Instance.Flag3)
                    {
                        ScriptManager.Instance.Flag3 = true;
                        ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName, new Event(EventType.ClearMonster, ScriptManager.Instance.Data));
                    }

                    break;
                case VisualType.MapObject:
                    ScriptManager.Instance.Target = ScriptManager.Instance.Map1?.MapButtons.FirstOrDefault(s => s.ButtonId == packet.EntityId);
                    break;
            }
        }

        public void NpcReqPacket(NpcReqPacket packet)
        {
            ScriptManager.Instance.Data[0] = packet.Dialog;
            ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName, new Event(EventType.NpcDialog, ScriptManager.Instance.Data, numericData: packet.Dialog));
        }

        public void MsgPacket(MsgPacket packet)
        {
            ScriptManager.Instance.Data[0] = packet.Message;
            ScriptManager.Instance.Data[1] = packet.Type;

            ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName, new Event(EventType.SendMsg, ScriptManager.Instance.Data, textData: packet.Message));
        }

        public void MinfoPacket(MinfoPacket packet)
        {
            if (ScriptManager.Instance.Script.Info.Lives == 0)
            {
                ScriptManager.Instance.Script.Info.Lives = (byte)(packet.Lives - 1);
            }
        }

        public void InPacket(InPacket packet)
        {
            switch (packet.EntityType)
            {
                case VisualType.Npc:
                    if (ScriptManager.Instance.Map1.MapNpcs.All(s => s.MapNpcId != packet.MateTransportId))
                    {
                        ScriptManager.Instance.Map1.MapNpcs.Add(new Npc(packet.NpcMonsterVnum, packet.MateTransportId, packet.PosX, packet.PosY));
                    }

                    break;
                case VisualType.Monster:
                    if (ScriptManager.Instance.Flag1)
                    {
                        if (ScriptManager.Instance.Map1.MapMonsters.All(s => s.MapMonsterId != packet.MateTransportId))
                        {
                            var monster3 = new Monster(packet.NpcMonsterVnum, packet.MateTransportId, packet.PosX, packet.PosY);
                            if (ScriptManager.Instance.Map1 != null)
                            {
                                ScriptManager.Instance.Map1.MapMonsters.Add(monster3);
                                ScriptManager.Instance.MapMonsters.Add(monster3);
                            }
                        }

                        return;
                    }

                    if (ScriptManager.Instance.EventName == "OnMapClear")
                    {
                        ScriptManager.Instance.Target = ScriptManager.Instance.Monster1;
                        ScriptManager.Instance.EventName = "OnDeath";
                    }

                    var monster4 = new Monster(packet.NpcMonsterVnum, packet.MateTransportId, packet.PosX, packet.PosY);
                    ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName, new Event(EventType.SummonMonster, null, monster4));
                    ScriptManager.Instance.MapMonsters.Add(monster4);

                    break;
                case VisualType.MapObject:
                    if (ScriptManager.Instance.Map1.MapButtons.All(s => s.ButtonId != packet.MateTransportId) && ScriptHelper.Instance.Buttons.Any(b => b == packet.NpcMonsterVnum) &&
                        ScriptManager.Instance.Map1 != null)
                    {
                        ScriptManager.Instance.Map1.MapButtons.Add(new Button(packet.NpcMonsterVnum, packet.MateTransportId, packet.PosX, packet.PosY));
                    }

                    break;
            }
        }

        public void GpPacket(GpPacket packet)
        {
            if (ScriptManager.Instance.Map1.MapPortals.All(s => s.PortalId != packet.PortalId))
            {
                var portal2 = new Portal(packet.SourceX, packet.SourceY, (byte)packet.PortalType, packet.PortalId, (short)ScriptManager.Instance.Script.Maps.Count);

                if (ScriptManager.Instance.Map1 != null)
                {
                    ScriptManager.Instance.Map1.MapPortals.Add(portal2);
                }

                if (ScriptManager.Instance.Portal1 != null && ScriptManager.Instance.Portal1.DestX == portal2.PosX && ScriptManager.Instance.Portal1.DestY == portal2.PosY)
                {
                    portal2.DestX = ScriptManager.Instance.Portal1.PosX;
                    portal2.DestY = ScriptManager.Instance.Portal1.PosY;
                    portal2.MapId = ScriptManager.Instance.Portal1.MapId;
                    ScriptManager.Instance.Portal1 = null;
                }

                return;
            }

            Portal portal3 = ScriptManager.Instance.Map1.MapPortals.FirstOrDefault(s => s.PortalId == packet.PortalId);

            if (packet.PortalType == 5)
            {
                ScriptManager.Instance.Data[0] = 0;
                ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName,
                    new Event(EventType.ChangePortalType, ScriptManager.Instance.Data, numericData: packet.PortalId, specialValue: true));
            }

            if (portal3 != null && portal3.PortalType != packet.PortalType)
            {
                ScriptManager.Instance.Data[0] = portal3.PortalId;
                ScriptManager.Instance.Data[1] = packet.PortalType;

                ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName,
                    new Event(EventType.ChangePortalType, ScriptManager.Instance.Data, numericData: packet.PortalId));
            }
        }

        public void EvntPacket(EvntPacket packet)
        {
            switch (packet.Type)
            {
                case ClockType.SimpleClock:
                    if (packet.BaseSecondsRemaining == packet.DeciSecondsRemaining)
                    {
                        if (ScriptManager.Instance.Flag1)
                        {
                            if (ScriptManager.Instance.Script.Maps.Count == 2)
                            {
                                ScriptManager.Instance.Map1.Clock = new Clock(ClockType.SimpleClock, packet.BaseSecondsRemaining);
                            }

                            break;
                        }

                        if (!ScriptManager.Instance.Flag2)
                        {
                            ScriptManager.Instance.Flag2 = true;
                            ScriptManager.Instance.Data[0] = packet.BaseSecondsRemaining - ScriptManager.Instance.Num2;
                            ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName, new Event(EventType.AddClockTime, ScriptManager.Instance.Data));
                        }

                        break;
                    }

                    ScriptManager.Instance.Num2 = packet.DeciSecondsRemaining;
                    break;
                case ClockType.MapClock:
                    if (packet.BaseSecondsRemaining == packet.DeciSecondsRemaining)
                    {
                        var clock2 = new Clock(ClockType.MapClock, packet.BaseSecondsRemaining);
                        if (ScriptManager.Instance.Flag1)
                        {
                            ScriptManager.Instance.Clock1 = clock2;
                            ScriptManager.Instance.Map1.MapClock = clock2;
                        }
                        else if (packet.BaseSecondsRemaining > 0)
                        {
                            ScriptManager.Instance.Data[0] = packet.BaseSecondsRemaining;
                            ScriptManager.Instance.Clock1 = clock2;
                            ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName, new Event(EventType.RemoveMapClock, ScriptManager.Instance.Data));
                        }
                        else if (ScriptManager.Instance.Clock1 != null)
                        {
                            ScriptManager.Instance.Clock1 = null;
                            ScriptManager.Instance.AddEvent(ScriptManager.Instance.Target, ScriptManager.Instance.EventName, new Event(EventType.RemoveMapClock, ScriptManager.Instance.Data));
                        }
                    }

                    break;
                case ClockType.TimeSpaceClock:
                    if (!ScriptManager.Instance.IsGenerated)
                    {
                        ScriptManager.Instance.ScriptData = ScriptManager.Instance.GenerateScript();
                        ScriptManager.Instance.IsGenerated = true;
                    }

                    break;
            }
        }

        public void EffPacket(EffPacket packet)
        {
            int num4 = packet.Id;

            switch (packet.EffectType)
            {
                case 3:
                    Monster monster2 = ScriptManager.Instance.MapMonsters.FirstOrDefault(s => s.MapMonsterId == packet.EntityId);

                    if (monster2 != null)
                    {
                        if (num4 == 824 && !monster2.IsTarget)
                        {
                            monster2.IsTarget = true;
                        }

                        if (num4 == 826 && !monster2.IsBonus)
                        {
                            monster2.IsBonus = true;
                        }
                    }

                    break;
            }
        }

        public void AtPacket(AtPacket packet)
        {
            ScriptManager.Instance.Map1 = new Map(packet.MapId, (byte)(ScriptManager.Instance.Script.Maps.Count + 1), ScriptManager.Instance.IndexX, ScriptManager.Instance.IndexY);

            ScriptManager.Instance.MapMonsters.Clear();
            ScriptManager.Instance.Flag1 = true;
            ScriptManager.Instance.Flag2 = false;
            ScriptManager.Instance.Flag3 = false;
            ScriptManager.Instance.Num3 = 0;
            ScriptManager.Instance.Target = ScriptManager.Instance.Map1;
            ScriptManager.Instance.EventName = "OnCharacterDiscoveringMap";

            if (!ScriptManager.Instance.Script.Maps.Any(s =>
            {
                if (s.IndexX == ScriptManager.Instance.IndexX)
                {
                    return s.IndexY == ScriptManager.Instance.IndexY;
                }

                return false;
            }))
            {
                if (ScriptManager.Instance.Map2 == null)
                {
                    ScriptManager.Instance.Map2 = ScriptManager.Instance.Map1;
                }
                else if (ScriptManager.Instance.Script.Maps.Count == 1)
                {
                    ScriptManager.Instance.Map2.IndexX = ScriptManager.Instance.IndexX;
                    ScriptManager.Instance.Map2.IndexY = ScriptManager.Instance.IndexY;
                }

                ScriptManager.Instance.Script.Maps.Add(ScriptManager.Instance.Map1);

                if (ScriptManager.Instance.Portal1 != null)
                {
                    ScriptManager.Instance.Portal1.DestX = packet.PositionX;
                    ScriptManager.Instance.Portal1.DestY = packet.PositionY;
                    ScriptManager.Instance.Portal1.DestMapId = ScriptManager.Instance.Script.Maps.Count;
                }
            }
        }
    }
}