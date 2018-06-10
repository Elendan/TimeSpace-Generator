using System.Linq;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Objects
{
    public class Event
    {
        public Event(EventType type, object[] data = null, Monster monster = null, int numericData = 0, string textData = null, bool specialValue = false)
        {
            Type = type;
            Data = data;
            MonsterToSummon = monster;
            TextData = textData;
            NumericData = numericData;
            SpecialValue = specialValue;
        }

        public EventType Type { get; set; }

        public int NumericData { get; set; }

        public object[] Data { get; set; }

        public string TextData { get; set; }

        public Monster MonsterToSummon { get; set; }

        public bool SpecialValue { get; set; }

        //Ik, this is ugly af
        public string SetEvent()
        {
            switch (Type)
            {
                case EventType.SummonMonster:
                    if (MonsterToSummon == null)
                    {
                        return string.Empty;
                    }

                    if (!MonsterToSummon.OnDeathEvents.Any())
                    {
                        return
                            $"<SummonMonster VNum=\"{MonsterToSummon.Vnum}\" PositionX=\"{MonsterToSummon.PosX}\" PositionY=\"{MonsterToSummon.PosY}\" {(MonsterToSummon.IsTarget ? "IsTarget=\"True\"" : "")} {(MonsterToSummon.IsBonus ? "IsBonus=\"True\"" : "")}/>";
                    }

                    string concat =
                        $"<SummonMonster VNum=\"{MonsterToSummon.Vnum}\" PositionX=\"{MonsterToSummon.PosX}\" PositionY=\"{MonsterToSummon.PosY}\" {(MonsterToSummon.IsTarget ? "IsTarget=\"True\"" : "")} {(MonsterToSummon.IsBonus ? "IsBonus=\"True\"" : "")}>\r\n" +
                        $"<OnDeath>\r\n";
                    foreach (Event evt in MonsterToSummon.OnDeathEvents)
                    {
                        concat += $"{evt.SetEvent()}\r\n";
                    }

                    if (MonsterToSummon.OnDeathEvents.Any(s => s.Type == EventType.ChangePortalType))
                    {
                        concat += $"<RefreshMapItems/>\r\n";
                    }

                    return concat + $"</OnDeath>\r\n" + $"</SummonMonster>";
                case EventType.ClearMonster:
                    return "<ClearMapMonsters/>";
                case EventType.NpcDialog:
                    return $"<NpcDialog Value=\"{NumericData}\"/>";
                case EventType.ChangePortalType:
                    return $"<ChangePortalType IdOnMap=\"{NumericData}\" Type=\"{(SpecialValue ? 4 : 2)}\"/>"; //TODO: Fix this
                case EventType.AddClockTime:
                    return $"<AddClockTime Value=\"{Data[0]}\"/>";
                case EventType.MapClock:
                    return $"<GenerateMapClock Value=\"{Data[0]}\"/>\r\n" + $"<StartMapClock/>";
                case EventType.SendPacket:
                    return $"<SendPacket Value=\"{Data[0]}\"/>";
                case EventType.RemoveMapClock:
                    return "<StopMapClock/>";
                case EventType.SendMsg:
                    return $"<SendMessage Value=\"{TextData}\" Type=\"{Data[1]}\"/>";
            }

            return string.Empty;
        }
    }
}