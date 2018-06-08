using System.Collections.Generic;
using System.Linq;
using TimeSpaceGenerator.Enums;
using TimeSpaceGenerator.Errors;
using TimeSpaceGenerator.Helpers;

namespace TimeSpaceGenerator.Objects
{
    public class Event
    {
        public Event(EventType type, object[] data = null, Monster monster = null, int numericData = 0, string textData = null)
        {
            Type = type;
            Data = data;
            MonsterToSummon = monster;
            TextData = textData;
            NumericData = numericData;
        }

        public EventType Type { get; set; }

        public int NumericData { get; set; }

        public object[] Data { get; set; }

        public string TextData { get; set; }

        public Monster MonsterToSummon { get; set; }

        //Ik, this is ugly af
        public string SetEvent(byte space = 0)
        {
            if (Type == EventType.NpcDialog)
            {
                ErrorManager.Instance.Error.Add(new KeyValuePair<ErrorType, string>(ErrorType.WrongFormat, "Data : " + NumericData.ToString()));
            }
            switch (Type)
            {
                case EventType.SummonMonster:
                    if (MonsterToSummon == null)
                    {
                        return string.Empty;
                    }

                    if (!MonsterToSummon.OnDeathEvents.Any())
                    {
                        return string.Format("<SummonMonster VNum=\"{0}\" PositionX=\"{1}\" PositionY=\"{2}\" {3} {4}/>", MonsterToSummon.Vnum, MonsterToSummon.PosX, MonsterToSummon.PosY, MonsterToSummon.IsTarget ? "IsTarget=\"True\"" : "", MonsterToSummon.IsBonus ? "IsBonus=\"True\"" : "");
                    }

                    string concat = string.Format("<SummonMonster VNum=\"{0}\" PositionX=\"{1}\" PositionY=\"{2}\" {3} {4}>\r\n", MonsterToSummon.Vnum, MonsterToSummon.PosX, MonsterToSummon.PosY, MonsterToSummon.IsTarget ? "IsTarget=\"True\"" : "", MonsterToSummon.IsBonus ? "IsBonus=\"True\"" : "") + string.Format("{0}<OnDeath>\r\n", ScriptHelper.Instance.Space[space + 1]);
                    foreach (Event evt in MonsterToSummon.OnDeathEvents)
                    {
                        concat += string.Format("{0}{1}\r\n", ScriptHelper.Instance.Space[space + 2], evt.SetEvent((byte)(space + 2U)));
                    }

                    if (MonsterToSummon.OnDeathEvents.Any(s => s.Type == EventType.ChangePortalType))
                    {
                        concat += string.Format("{0}<RefreshMapItems/>\r\n", ScriptHelper.Instance.Space[space + 2]);
                    }
                    return concat + string.Format("{0}</OnDeath>\r\n", ScriptHelper.Instance.Space[space + 1]) + string.Format("{0}</SummonMonster>", ScriptHelper.Instance.Space[space]);
                case EventType.ClearMonster:
                    return "<ClearMapMonsters/>";
                case EventType.NpcDialog:
                    return string.Format("<NpcDialog Value=\"{0}\"/>", NumericData);
                case EventType.ChangePortalType:
                    return string.Format("<ChangePortalType IdOnMap=\"{0}\" Type=\"{1}\"/>", NumericData, 2); //TODO: Fix this
                case EventType.AddClockTime:
                    return string.Format("<AddClockTime Value=\"{0}\"/>", Data[0]);
                case EventType.MapClock:
                    return string.Format("<GenerateMapClock Value=\"{0}\"/>\r\n", Data[0]) + string.Format("<{0}StartMapClock/>", ScriptHelper.Instance.Space[space]);
                case EventType.SendPacket:
                    return string.Format("<SendPacket Value=\"{0}\"/>", Data[0]);
                case EventType.RemoveMapClock:
                    return "<StopMapClock/>";
                case EventType.SendMsg:
                    return string.Format("<SendMessage Value=\"{0}\" Type=\"{1}\"/>", TextData, Data[1]);
            }

            return string.Empty;
        }
    }
}
