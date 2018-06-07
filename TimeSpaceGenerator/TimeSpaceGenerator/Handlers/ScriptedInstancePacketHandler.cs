using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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
        public void InButtonPacket(InPacket packet)
        {
            MessageBox.Show($"entityType : {packet.EntityType.ToString()}\nNpcMonsterVnum {packet.NpcMonsterVnum}");
        }

        public void RbrPacket(RbrPacket packet)
        {
            MessageBox.Show($"{packet.LevelMinimum}\n{packet.Unknown1}\n{packet.Unknown2}\n{packet.Unknown3}\n{packet.Unknown4}\n{packet.Unknown5}\n");
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
                            //...
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
                    }
                    break;
                case ClockType.TimeSpaceClock:
                    //Todo: CreateXmlScript
                    break;
            }
        }

        public void EffPacket(EffPacket packet)
        {
            //Todo: Not quite sure for this one
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
                    //Todo: Not quite sure about this, maybe the unknown values actually are the Portal position
                    ScriptManager.Instance.Portal1.DestX = packet.PositionX;
                    ScriptManager.Instance.Portal1.DestY = packet.PositionY;

                }
            }
        }
    }
}
